using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private Vector2 dir = Vector2.right;                                                          //default direction is set to right 
    
    public float wrapOffset = 1f;                                               
    
    public GameObject leftBound;
    public GameObject rightBound;
    public GameObject topBound;
    public GameObject bottomBound;

    public GameObject segmentPrefab;
    private List<Transform> segments = new List<Transform>();

    public FoodController foodController;

    private void Start()
    {
        segments.Add(this.transform);
    }

    private void Update()
    {
        HandleInput();  
    }

    private void HandleInput()                                                                     //transform euler angles so sprite rotates according to the dir
    {
        if (Input.GetKeyDown(KeyCode.W) && dir != Vector2.down)
        {
            dir = Vector2.up;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.A) && dir != Vector2.right)
        {
            dir = Vector2.left;
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
        if (Input.GetKeyDown(KeyCode.S) && dir != Vector2.up)
        {
            dir = Vector2.down;
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        if (Input.GetKeyDown(KeyCode.D) && dir != Vector2.left)
        {
            dir = Vector2.right;
            transform.eulerAngles = new Vector3(0, 0, 270);
        }
    }

    private void FixedUpdate()                                                                       
    {
        Move();
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
        for (int i = segments.Count - 1; i >0; i--)
        {
            segments[i].position = segments[i - 1].position;                          //Moving each segment to the one in front of it (before it in list) hence -- 
        }

        Vector3 newPosition = transform.position + new Vector3(dir.x, dir.y, 0);
        newPosition = WrapPosition(newPosition);                                                         //checks if any bounds have been crossed 
        transform.position = new Vector3(Mathf.Round(newPosition.x), Mathf.Round(newPosition.y), 0f);
    }

    private void Grow()
    {
        GameObject newSegment = Instantiate(segmentPrefab);
        newSegment.transform.position = segments[segments.Count - 1].position;
        segments.Add(newSegment.transform);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Grow();
            Destroy(other.gameObject);                  //growing the snake and then destroying the food obj and then spawning new food prefab

            foodController.SpawnFood(); 
        }
    }


}


