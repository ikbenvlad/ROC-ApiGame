using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections.Generic;

public class SaveBurger : MonoBehaviour
{
    public TMP_InputField burgerNameField;
    public TMP_InputField authorNameField;
    public Button saveButton;
    public apiHandler apiHandler;
    private Burger burger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If not assigned in Inspector, try to find it
        if (apiHandler == null)
        {
            apiHandler = FindAnyObjectByType<apiHandler>();
        }
    }

    public void OnSaveButtonClicked()
    {
        // Find the burger from the test script
        test testScript = FindAnyObjectByType<test>();
        if (testScript != null)
        {
            burger = testScript.burger;
        }
        else
        {
            Debug.LogError("test script not found in scene!");
        }

        if (saveButton != null)
        {
            saveButton.onClick.AddListener(OnSaveButtonClicked);
        }

        if (burger == null)
        {
            Debug.LogError("Burger reference is null. Cannot save.");
            return;
        }

        if (string.IsNullOrEmpty(burgerNameField.text) || string.IsNullOrEmpty(authorNameField.text))
        {
            Debug.LogWarning("Burger name or author name is empty. Please fill in both fields.");
            return;
        }

        SaveBurgerData();
    }

    private void SaveBurgerData()
    {
        if (burger.burgerIngredients.Count == 0)
        {
            Debug.LogWarning("No ingredients to save!");
            return;
        }

        List<IngredientData> ingredientDataList = new List<IngredientData>();
        
        for (int i = 0; i < burger.burgerIngredients.Count; i++)
        {
            Ingredient ingredient = burger.burgerIngredients[i];
            ingredientDataList.Add(new IngredientData
            {
                order = i,
                ingredientName = ingredient.ingredientName,
                positionX = ingredient.transform.position.x,
                positionY = ingredient.transform.position.y,
                positionZ = ingredient.transform.position.z
            });
        }

        BurgerData burgerdata = new BurgerData
        {
            authorName = authorNameField.text,
            burgerName = burgerNameField.text,
            ingredients = ingredientDataList
        };

        // Create the JSON string
        string json = JsonUtility.ToJson(burgerdata, true);

        // Send to database
        if (apiHandler != null)
        {
            apiHandler.SaveBurgerToDatabase(json);
        }
        else
        {
            Debug.LogError("apiHandler not found in scene!");
        }

        // Optionally: also save locally to Downloads
        SaveLocalCopy(burgerdata, json);
    }

    private void SaveLocalCopy(BurgerData burgerdata, string json)
    {
        string downloadsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
        downloadsPath = Path.Combine(downloadsPath, "Downloads");

        string filename = $"{burgerdata.burgerName}_" + System.DateTime.Now.ToString("dd-MM-HH-mm") + ".json";
        string filepath = Path.Combine(downloadsPath, filename);

        File.WriteAllText(filepath, json);

        Debug.Log($"Burger data saved locally to: {filepath}");
        Debug.Log($"Ingredients saved: {burgerdata.ingredients.Count}");
    }

    [System.Serializable]
    public class IngredientData
    {
        public int order;
        public string ingredientName;
        public float positionX;
        public float positionY;
        public float positionZ;
    }

    [System.Serializable]
    public class BurgerData
    {
        public string authorName;
        public string burgerName;
        public List<IngredientData> ingredients;
    }

}