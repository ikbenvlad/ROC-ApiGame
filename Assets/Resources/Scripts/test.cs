using UnityEngine;

public class test : MonoBehaviour
{    
    public Burger burger;

    [SerializeField] private float spawnPositionY = 0f;
    [SerializeField] private float spawnPositionZ = 0f;

    void Start()
    {
        burger = new Burger();

        burger.LogIngredients();
    }

    public void burgerBuilder(Ingredient ingredientToAdd) 
    {
        ingredientToAdd = Instantiate(ingredientToAdd, new Vector3(0, spawnPositionY, spawnPositionZ), Quaternion.identity) as Ingredient;
        ingredientToAdd.transform.parent = transform;
        burger.AddIngredient(ingredientToAdd);
        spawnPositionY += 0.8f;
        spawnPositionZ += -0.5f;
    }

    public void undoLast()
    {
        Debug.Log(burger.burgerIngredients.Count);
        if (burger.burgerIngredients.Count > 0)
        {
            Ingredient lastIngredient = burger.burgerIngredients[burger.burgerIngredients.Count - 1];
            burger.burgerIngredients.RemoveAt(burger.burgerIngredients.Count - 1);
            Destroy(lastIngredient.gameObject);

            spawnPositionY -= 0.8f;
            spawnPositionZ -= -0.5f;

            Debug.Log("Ingredient removed. Burger now has " + burger.burgerIngredients.Count + " ingredients.");
        }
        else
        {
            Debug.Log("No ingredients to remove!");
        }
    }
}
