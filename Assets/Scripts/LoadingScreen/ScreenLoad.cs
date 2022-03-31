using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenLoad : MonoBehaviour
{
    public Text loadingText;

    private static ScreenLoad instance;
    private static bool shouldPlayOpening = false;

    private Animator componentAnimator;
    private AsyncOperation loadingScene;

    private void Start()
    {
        instance = this;
        componentAnimator = GetComponent<Animator>();

        if (shouldPlayOpening)
            componentAnimator.SetTrigger("sceneOpening");
    }
    private void Update()
    {
        if(loadingScene != null)
            loadingText.text = "Loading..." + Mathf.RoundToInt(loadingScene.progress * 100) + "%";
    }

    public static void LoadScene(string sceneName)
    {
        instance.componentAnimator.SetTrigger("sceneClosing");

        instance.loadingScene = SceneManager.LoadSceneAsync(sceneName);
        instance.loadingScene.allowSceneActivation = false;
    }

    public void OnAnimationOver()
    {
        shouldPlayOpening = true;
        loadingScene.allowSceneActivation = true;
    }
}