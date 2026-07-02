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
    private Burger burger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnSaveButtonClicked()
    {
        // Find the burger from the test script
        test testScript = FindObjectOfType<test>();
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

        // String that grabs the user's downloads folder
        string downloadsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
        downloadsPath = Path.Combine(downloadsPath, "Downloads");

        // Create the JSON string
        string json = JsonUtility.ToJson(burgerdata, true);

        // Create the filename with the burger name and author name
        string filename = $"{burgerdata.burgerName}_" + System.DateTime.Now.ToString("dd-MM-HH-mm") + ".json";
        string filepath = Path.Combine(downloadsPath, filename);

        // Save the file
        File.WriteAllText(filepath, json);

        Debug.Log($"Burger data saved to: {filepath}");
        Debug.Log($"Ingredients saved: {ingredientDataList.Count}");
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
