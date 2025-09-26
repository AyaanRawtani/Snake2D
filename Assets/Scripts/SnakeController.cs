using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum PlayerId { Player1, Player2}
public class SnakeController : MonoBehaviour
{
    public PlayerId playerId;

 
    private Vector2 dir = Vector2.right;
   private  Vector2 currentDir = Vector2.right;

    public float moveInterval = 0.1f;

    public float wrapOffset = 1f;                                               
    
    public GameObject leftBound;
    public GameObject rightBound;
    public GameObject topBound;
    public GameObject bottomBound;

    public GameObject segmentPrefab;
    private List<Transform> segments = new List<Transform>();

    public FoodController foodController;

    private bool isGameOver = false;


    private bool hasShield = false;

    public GameOverController gameOverController;

    

    private void Start()
    {
        segments.Add(this.transform);
        StartCoroutine(MoveCoroutine());
    }

    private void Update()
    {
        if ((!isGameOver))
        {
            HandleInput();
        }
        
    }

    private void HandleInput()                                                                     //transform euler angles so sprite rotates according to the dir
    {
        if (playerId == PlayerId.Player1)
        {


            if (Input.GetKeyDown(KeyCode.W) && dir != Vector2.down)
            {
                dir = Vector2.up;

               // transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.A) && dir != Vector2.right)
            {
                dir = Vector2.left;
                //transform.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (Input.GetKeyDown(KeyCode.S) && dir != Vector2.up)
            {
                dir = Vector2.down;
               // transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else if (Input.GetKeyDown(KeyCode.D) && dir != Vector2.left)
            {
                dir = Vector2.right;
              //  transform.eulerAngles = new Vector3(0, 0, 270);

            }
        }
        else if (playerId == PlayerId.Player2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && dir != Vector2.down)
            {
                dir = Vector2.up;

               // transform.eulerAngles = new Vector3(0, 0, 0);
            }
           else  if (Input.GetKeyDown(KeyCode.LeftArrow) && dir != Vector2.right)
            {
                dir = Vector2.left;
               // transform.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && dir != Vector2.up)
            {
                dir = Vector2.down;
               // transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && dir != Vector2.left)
            {
                dir = Vector2.right;
               // transform.eulerAngles = new Vector3(0, 0, 270);

            }

        }
    }

    private IEnumerator MoveCoroutine()
    {
        while (true)
        {
            Move();
            yield return new WaitForSeconds(moveInterval);
        }
    }

    

    private Vector3 WrapPosition(Vector3 position)
    {
        if (position.x < leftBound.transform.position.x)
        {
            position.x = rightBound.transform.position.x - wrapOffset;
        }
        else if (position.x > rightBound.transform.position.x)
        {
            position.x = leftBound.transform.position.x + wrapOffset;
        }

        if (position.y < bottomBound.transform.position.y)
        {
            position.y = topBound.transform.position.y - wrapOffset;
        }
        else if (position.y > topBound.transform.position.y)
        {
            position.y = bottomBound.transform.position.y +wrapOffset;
        }

        return position;
    }

     private void Move()
 {

     if (currentDir != (dir * -1))                                               //ensuring snake can't turn 180 degrees
     {
         currentDir = dir;

         if (currentDir == Vector2.up)                                              
             transform.eulerAngles = new Vector3(0, 0, 0);
         else if (currentDir == Vector2.left)
             transform.eulerAngles = new Vector3(0, 0, 90);
         else if (currentDir == Vector2.down)
             transform.eulerAngles = new Vector3(0, 0, 180);
         else if (currentDir == Vector2.right)
             transform.eulerAngles = new Vector3(0, 0, 270);
     }

     for (int i = segments.Count - 1; i >0; i--)
     {
         segments[i].position = segments[i - 1].position;                          //Moving each segment to the one in front of it  hence -- 
     }

     Vector3 newPosition = transform.position + new Vector3(currentDir.x, currentDir.y, 0);
     newPosition = WrapPosition(newPosition);                                                         //checks if any bounds have been crossed 
     transform.position = new Vector3(Mathf.Round(newPosition.x), Mathf.Round(newPosition.y), 0f);

 }
    public void Grow(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newSegment = Instantiate(segmentPrefab);

            newSegment.transform.position = segments[segments.Count -1 ].position ;
            segments.Add(newSegment.transform);

            Collider2D col = newSegment.GetComponent<Collider2D>();
            col.enabled = false;
            StartCoroutine(EnableColliderAfterDelay(col, 0.5f));


        }
    }

    private IEnumerator EnableColliderAfterDelay(Collider2D col, float delay)
    {
        yield return new WaitForSeconds(delay);
        col.enabled = true;
    }


    public void Shrink(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (segments.Count > 1) 
            {
                Transform lastSegment = segments[segments.Count - 1];
                segments.RemoveAt(segments.Count - 1);
                Destroy(lastSegment.gameObject);
            }
        }
    }

    public void ActivateShield()
    {
        hasShield = true;
        Debug.Log("Shield Active");
    }

    public void DeactivateShield()
    {
        hasShield = false;
        Debug.Log("Shield Inactive");
    }

    public bool isShieldActive()
    {
        return hasShield;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            FoodController foodController = FindObjectOfType<FoodController>();

            if (foodController.ContainsFood(other.gameObject))
            {
                FoodType type = foodController.GetFoodType(other.gameObject);

                
                foodController.Eat(this, type);

                Destroy(other.gameObject);
                foodController.RemoveFoodReference(other.gameObject);
                foodController.SpawnFood();
            }
        }

        else if (other.gameObject.CompareTag("Body"))
        {
          Debug.Log("Snake collided with itself.");

           PlayerDie();    
        }
    }

    public void PlayerDie()

    {
        if (isShieldActive())
        {
            Debug.Log("Shield saved you!");
            DeactivateShield();
        }

        else
        {
            Debug.Log("Game Over");
            isGameOver = true;

            gameOverController.PlayerDied();

        }
    }
}


