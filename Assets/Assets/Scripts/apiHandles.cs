using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class apiHandler : MonoBehaviour
{
    private string home = "http://127.0.0.1/api.php";

    void Start()
    {
        Debug.Log("Start");
        StartCoroutine(ReqPing());

    }
    


    private IEnumerator ReqPing()
    {
        using (UnityWebRequest request = UnityWebRequest.Post(home, "{ \"request\":\"ping\"}", "application/json"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    private IEnumerator ReqAnswer()
    {
        using (UnityWebRequest request = UnityWebRequest.Post(home, "{ \"request\":\"bericht\"}", "application/json"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                answer response = JsonUtility.FromJson<answer>(request.downloadHandler.text);
                Debug.Log(response.payload);
            }
        }
    }

    public void SaveBurgerToDatabase(string burgerJson)
    {
        StartCoroutine(ReqSaveBurger(burgerJson));
    }

    private IEnumerator ReqSaveBurger(string burgerJson)
    {
        string requestData = "{ \"request\":\"saveBurger\", \"data\":" + burgerJson + "}";
        
        using (UnityWebRequest request = UnityWebRequest.Post(home, requestData, "application/json"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to save burger: " + request.error);
            }
            else
            {
                Debug.Log("Burger saved successfully: " + request.downloadHandler.text);
                answer response = JsonUtility.FromJson<answer>(request.downloadHandler.text);
                Debug.Log(response.message);
            }
        }
    }

}
