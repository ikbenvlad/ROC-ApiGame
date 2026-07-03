using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class LoadBurger : MonoBehaviour
{
    public test burgerBuilder;
    public Dictionary<string, Ingredient> ingredientPrefabs;

    public void LoadBurgerFromFile(string filepath = "")
    {
        // If no filepath provided, show available burgers
        if (string.IsNullOrEmpty(filepath))
        {
            ShowBurgerSelectionUI();
            return;
        }

        // Load the selected burger
        PerformLoadBurger(filepath);
    }

    private void ShowBurgerSelectionUI()
    {
        string downloadsFolder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "Downloads");
        
        if (!Directory.Exists(downloadsFolder))
        {
            Debug.LogError("Downloads folder not found.");
            return;
        }

        string[] jsonFiles = Directory.GetFiles(downloadsFolder, "*.json");

        if (jsonFiles.Length == 0)
        {
            Debug.LogWarning("No burger files found in Downloads folder.");
            return;
        }

        // Sort by most recent first
        jsonFiles = jsonFiles.OrderByDescending(f => File.GetLastWriteTime(f)).ToArray();

        // Display file names and let player choose
        Debug.Log("Available burger files:");
        for (int i = 0; i < jsonFiles.Length; i++)
        {
            Debug.Log($"{i}: {Path.GetFileNameWithoutExtension(jsonFiles[i])}");
        }

        // For now, load the first one, but you should add UI buttons for selection
        // TODO: Create UI buttons to let player click and select
        Debug.Log($"Loading: {Path.GetFileName(jsonFiles[0])}");
        PerformLoadBurger(jsonFiles[0]);
    }

    private void PerformLoadBurger(string filepath)
    {
        if (!File.Exists(filepath))
        {
            Debug.LogError($"File not found: {filepath}");
            return;
        }

        string json = File.ReadAllText(filepath);

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError($"Burger file is empty: {filepath}");
            return;
        }

        SaveBurger.BurgerData burgerData = JsonUtility.FromJson<SaveBurger.BurgerData>(json);

        if (burgerData == null || burgerData.ingredients == null)
        {
            Debug.LogError("Failed to parse burger data.");
            return;
        }

        // Ensure BurgerDataHolder exists
        if (BurgerDataHolder.instance == null)
        {
            GameObject holderObject = new GameObject("BurgerDataHolder");
            holderObject.AddComponent<BurgerDataHolder>();
        }

        // Store the burger data to load after scene transition
        BurgerDataHolder.instance.SetBurgerData(burgerData);

        // Load the Create scene
        SceneManager.LoadScene("Create");
    }
}

public class BurgerDataHolder : MonoBehaviour
{
    public static BurgerDataHolder instance;
    private SaveBurger.BurgerData pendingBurgerData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetBurgerData(SaveBurger.BurgerData burgerData)
    {
        pendingBurgerData = burgerData;
    }

    public SaveBurger.BurgerData GetBurgerData()
    {
        return pendingBurgerData;
    }

    public void ClearBurgerData()
    {
        pendingBurgerData = null;
    }
}
