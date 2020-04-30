using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // YOU USE THIS EVERYTIME YOU WANT TO LOAD A SCENE
public class MainMenu : MonoBehaviour
{
    // ----------- THIS CODE IS TO ROUTE PLAY BUTTON TO LOAD SCENE! ------------------------------------
    public void LoadGame()
    {
        SceneManager.LoadScene("SampleScene"); 
    }

    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("TitleMenu");
    }
}
