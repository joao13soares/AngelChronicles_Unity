using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class RestartGame : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
    }

    private void ReloadFirstScene()
    {
        SceneManager.LoadSceneAsync(0);
    }

    private void Update()
    {
        if (videoPlayer.isPlaying) return;
        
        ReloadFirstScene();
    }
}
