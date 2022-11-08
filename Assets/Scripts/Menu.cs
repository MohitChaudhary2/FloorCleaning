using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject LoadingScene;
    public void StartGame()
    {
        StartCoroutine(loadScene());
    }
    IEnumerator loadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        while (!operation.isDone)
        {
            LoadingScene.SetActive(true);
            yield return null;
        }
    }
}
