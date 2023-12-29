using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public const string START_MENU_NAME = "Start Menu";
    public const string CREDITS_SCREEN_NAME = "Credits Screen";
    public const string CONTROLS_SCREEN_NAME = "Controls Screen";

    public void LoadNextScene()
    {
        AddCollected();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadNextScene(float time)
    {
        StartCoroutine(LoadNextSceneTimed(time));
    }

    private IEnumerator LoadNextSceneTimed(float time)
    {
        yield return new WaitForSeconds(time);

        AddCollected();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ReloadScene(float time)
    {
        StartCoroutine(ReloadSceneTimed(time));
    }


    private IEnumerator ReloadSceneTimed(float time)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex - 1);
    }

    //  try making this a generic type
    public void LoadTargetScene(int targetSceneIndex)
    {
        SceneManager.LoadScene(targetSceneIndex);
    }


    public void LoadStartMenu()
    {
        SceneManager.LoadScene(START_MENU_NAME);
    }

    public void LoadCreditsScreen()
    {
        SceneManager.LoadScene(CREDITS_SCREEN_NAME);
    }

    public void LoadControlsScreen()
    {
        SceneManager.LoadScene(CONTROLS_SCREEN_NAME);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    private void AddCollected()
    {
        if(!FindObjectOfType<CoinUI>()) { return; }
        GameComponents.Instance.gameData.totalCoins += FindObjectOfType<CoinUI>().currentCoins;
    }
}

