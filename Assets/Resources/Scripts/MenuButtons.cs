using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void CreateBurger()
    {
        SceneManager.LoadScene("Create");
    }

    public void ViewBurgers()
    {
        Application.OpenURL("http://localhost/burgers.php");
    }

}