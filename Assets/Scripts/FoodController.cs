using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FoodEntry
{
    public GameObject foodObj;
    public FoodType foodType;

    public FoodEntry(GameObject obj, FoodType type)
    {
        foodObj = obj;
        foodType = type;
    }
}

public class FoodController : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public GameObject regularfoodPrefab;
    public GameObject superfoodPrefab;
    public GameObject poisonfoodPrefab;

    
    public FoodType foodType;
    //public SnakeController snakeController;
    public List<SnakeController> snakes = new List<SnakeController>();
    private int amount;

    public float minSpawnTimer = 3f;
    public float maxSpawnTimer = 5.5f;
    public float foodLifetime = 10f;

    private List<FoodEntry> spawnedFood = new List<FoodEntry>();
    private void Start()
    {
        StartCoroutine(SpawnFoodLoop());
    }

    public void Eat(SnakeController snake, FoodType type)
    {
        int amount = EatAmount(type);

        if(type == FoodType.Shield)
        {
            snake.ActivateShield();
            return;
        }

        if (amount >= 0)
        {    
                snake.Grow(amount);     
        }
        else  snake.Shrink(-amount);
    }

    public int EatAmount(FoodType type)
    {
        foodType = type;
        switch(type)
        {
            case FoodType.Regular:
                amount = 1;
                break;

            case FoodType.Super:
                amount = 2;
                break;

            case FoodType.Poison:
                amount = -1;
                break;

            case FoodType.Shield:  
                return 0;
                
        }
        return amount;
    }

    private IEnumerator SpawnFoodLoop()
    {
        while (true)
        {
            SpawnFood();

            float waitTime = Random.Range(minSpawnTimer, maxSpawnTimer);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void SpawnFood()
    {
        Bounds bounds = gridArea.bounds;

        float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));


        FoodType type;
        int roll = Random.Range(0, 100);

        if (roll < 50)
            type = FoodType.Regular;
        else if (roll < 65)
            type = FoodType.Super;
        else if (roll < 85)
            type = FoodType.Poison;
        else 
            type = FoodType.Shield;

        
        GameObject prefabToSpawn = null;
        switch (type)
        {
            case FoodType.Regular:
                prefabToSpawn = regularfoodPrefab;
                break;
            case FoodType.Super:
                prefabToSpawn = superfoodPrefab;
                break;
            case FoodType.Poison:
                prefabToSpawn = poisonfoodPrefab;
                break;

            case FoodType.Shield:
                prefabToSpawn = superfoodPrefab;
                break;
               
        }

        GameObject food = Instantiate(prefabToSpawn, new Vector3(x,y,0f),Quaternion.identity);
        spawnedFood.Add(new FoodEntry(food, type));

        StartCoroutine(DestroyFoodAfterLifetime(food));  
        
    }

    private IEnumerator DestroyFoodAfterLifetime(GameObject food)
    {
        yield return new WaitForSeconds(foodLifetime);

        RemoveFoodReference(food);
        Destroy(food);
    }

    public bool ContainsFood(GameObject food)
    {
        foreach (FoodEntry entry in spawnedFood)
        {
            if (entry.foodObj == food)

                return true;      
        }
        return false;
    }
    public FoodType GetFoodType(GameObject food)
    {
        foreach (FoodEntry entry in spawnedFood)
        {
            if (entry.foodObj == food)
            
                return entry.foodType;     
        }

        Debug.Log("Food type not found ");
        return FoodType.Regular;                                    //default
    }

    public void RemoveFoodReference(GameObject food)
    {
        spawnedFood.RemoveAll(entry => entry.foodObj == food);
    }
}
