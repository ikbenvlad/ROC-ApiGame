using UnityEngine;

public class test : MonoBehaviour
{    
    public Burger burger;

    [SerializeField] private float spawnPositionY = 0f;
    [SerializeField] private float spawnPositionZ = 0f;

    void Start()
    {
        burger = new Burger();

        // Check if there's a saved burger to load
        BurgerDataHolder burgerHolder = FindAnyObjectByType<BurgerDataHolder>();
        if (burgerHolder != null)
        {
            SaveBurger.BurgerData burgerData = burgerHolder.GetBurgerData();
            if (burgerData != null && burgerData.ingredients != null && burgerData.ingredients.Count > 0)
            {
                LoadBurgerFromData(burgerData);
                burgerHolder.ClearBurgerData();
                return;
            }
        }

        burger.LogIngredients();
    }

    private void LoadBurgerFromData(SaveBurger.BurgerData burgerData)
    {
        Debug.Log($"Loading burger: {burgerData.burgerName} by {burgerData.authorName}");
        
        foreach (var ingredientData in burgerData.ingredients)
        {
            // Get the ingredient prefab by name
            Ingredient ingredientPrefab = Resources.Load<Ingredient>($"Ingredients/{ingredientData.ingredientName}");
            
            if (ingredientPrefab != null)
            {
                Vector3 position = new Vector3(ingredientData.positionX, ingredientData.positionY, ingredientData.positionZ);
                Ingredient spawnedIngredient = Instantiate(ingredientPrefab, position, Quaternion.identity);
                spawnedIngredient.transform.parent = transform;
                burger.AddIngredient(spawnedIngredient);
                Debug.Log($"Loaded ingredient: {ingredientData.ingredientName}");
            }
            else
            {
                Debug.LogWarning($"Ingredient prefab not found: {ingredientData.ingredientName}");
            }
        }

        Debug.Log($"Burger loaded with {burger.burgerIngredients.Count} ingredients");
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
