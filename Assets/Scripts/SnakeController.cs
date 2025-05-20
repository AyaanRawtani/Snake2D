using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private Vector2 dir = Vector2.right;        //default direction is set to right 

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

    private void FixedUpdate()                                          //using fixed timestep in project settings for speed
    {
        this.transform.position = new Vector3(
           Mathf.Round (this.transform.position.x + dir.x),             //rounding up/down so we can have grid based movement
           Mathf.Round (this.transform.position.y + dir.y),
            0.0f);

       
    }

}
