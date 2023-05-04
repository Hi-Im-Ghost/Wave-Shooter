using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public static bool shouldLoad = false;

    //metoda do uruchomienia gry
    public void StartGame()
    {
        Debug.Log("Click");
        SceneManager.LoadScene(1);
    }
    //metoda do wczytania gry
    public void LoadGame()
    {
        Debug.Log("Click1");
        shouldLoad = true;
        SceneManager.LoadScene(1);
    }
    //metoda do wylaczenia gry
    public void Exit()
    {
        Debug.Log("Click2");
        Application.Quit();
    }


}
