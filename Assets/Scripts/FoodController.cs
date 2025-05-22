using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public GameObject foodPrefab;

    private void Start()
    {
        SpawnFood();
    }
    public void SpawnFood()
    {
        Bounds bounds = gridArea.bounds;

        float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

        Instantiate(foodPrefab, new Vector3(x,y,0f),Quaternion.identity);
        
    }
}
