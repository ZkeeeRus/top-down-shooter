using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyboardBinds : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetAxis("Exit") > 0)
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();

            SceneManager.LoadSceneAsync("main");
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadSceneAsync("main");
        }
    }
}