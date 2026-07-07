using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class Burger
{
    public string burgerName;
    public string username;
    public List<Ingredient> burgerIngredients;

    public Burger()
    {
        burgerName = "Mik Burger";
        username = "Chom";
        burgerIngredients = new List<Ingredient>();
    }

    public void LogIngredients()
    {
        //Debug.Log("Burger: " + burgerName + " by " + username);
        foreach (Ingredient ingredient in burgerIngredients)
        {
            Debug.Log("Ingredient: " + ingredient.name);
        }
    }

    public void AddIngredient(Ingredient ingredient)
    {
        burgerIngredients.Add(ingredient);
        Debug.Log("Added ingredient: " + ingredient.name + "." + " Burger count is now " + burgerIngredients.Count);
        //Debug.Log(this.ToJson());
    }

    public void DebugBurger()
    {
        string info = JsonUtility.ToJson(this);
        Debug.Log(this);
        Debug.Log(info);
    }

    public string ToJson()
    {
        string json = "{";
        json += "\"burgerName\": \"" + burgerName + "\",";
        json += "\"username\": \"" + username + "\",";
        json += "\"burgerIngredients\": [";
        foreach (Ingredient ingredient in burgerIngredients)
        {
            json += ingredient.name + ",";
        }
        json = json.TrimEnd(',');
        json += "]}";
        return json;
    }
}