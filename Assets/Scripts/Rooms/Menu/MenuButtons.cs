using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private Text recordText;
    private void Start()
    {
        recordText.text = "Level Record: " + PlayerPrefs.GetInt("LevelRecord", 0);
    }
    public void PlayClick()
    {
        PlayerPrefs.DeleteKey("Level");
        PlayerPrefs.DeleteKey("AddDamage");
        PlayerPrefs.DeleteKey("PlayerHealth");
        GameAssets.isPlayerDeath = false;

        ScreenLoad.LoadScene("main");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}