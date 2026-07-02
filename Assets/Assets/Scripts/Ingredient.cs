using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public string ingredientName;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ingredientName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {

    }
}