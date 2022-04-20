using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyboardBinds : MonoBehaviour
{
    PlayerHandler player;
    private void Start()
    {
        player = FindObjectOfType<PlayerHandler>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ScreenLoad.LoadScene("StartMenu");

        if (Input.GetKeyDown(KeyCode.H))
            player.healthSystem.Heal();
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    PlayerPrefs.DeleteAll();

        //    ScreenLoad.LoadScene("main");
        //}
        //else if (Input.GetKeyDown(KeyCode.O))
        //{
        //    ScreenLoad.LoadScene("main");
        //}
    }
}