using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class RestartGame : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    
    void Awake()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
    }

    private void ReloadFirstScene() => SceneManager.LoadSceneAsync(0);
        
    

    private void Update()
    {
        // Waits for credits video to end
        if (videoPlayer.isPlaying) return;
        
        ReloadFirstScene();
    }
}
