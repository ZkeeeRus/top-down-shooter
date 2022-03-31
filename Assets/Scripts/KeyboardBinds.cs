using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyboardBinds : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetAxis("Exit") > 0)
            ScreenLoad.LoadScene("StartMenu");

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();

            ScreenLoad.LoadScene("main");
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            ScreenLoad.LoadScene("main");
        }
    }
}