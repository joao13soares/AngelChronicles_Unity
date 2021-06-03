using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewSceneTrigger : MonoBehaviour
{
    // static void LoadNewS(int nextScene)
    // {
    //     SceneManager.LoadSceneAsync(nextScene);
    // }

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
