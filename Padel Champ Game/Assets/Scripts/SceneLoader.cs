using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; 

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneAsync(int sceneIndex) 
    { 
        StartCoroutine(LoadSceneCoroutine(sceneIndex));
    }

    private IEnumerator LoadSceneCoroutine(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex); while (!asyncOperation.isDone)
        { 
           yield return null; 
        }
    }
}
