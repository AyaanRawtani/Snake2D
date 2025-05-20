using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private Vector2 dir = Vector2.right;                                        //default direction is set to right 
    
    public float wrapOffset = 1f;
    
    public GameObject leftBound;
    public GameObject rightBound;
    public GameObject topBound;
    public GameObject bottomBound;

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

    private void FixedUpdate()                                                                   //using fixedtimestep in proj settings for speed instead of float speed 
    {
        Vector3 newPosition = transform.position + new Vector3(dir.x, dir.y, 0);
        newPosition = WrapPosition(newPosition);
        transform.position = new Vector3(Mathf.Round(newPosition.x), Mathf.Round(newPosition.y), 0f);
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


}


