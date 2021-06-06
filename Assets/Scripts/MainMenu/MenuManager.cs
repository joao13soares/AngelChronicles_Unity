using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MenuManager : MonoBehaviour
{
    [SerializeField]private VideoPlayer videoPlayer;

  

    
    //Main Menu Animations
    [SerializeField] private VideoClip mainMenuIntroAnimation;
    [SerializeField] private VideoClip normalMainMenuAnimation;
    [SerializeField] private VideoClip leaveControlsMenu;

    //Main Menu Buttons
    [SerializeField] private GameObject[] mainMenuButtons;

    
    //Controls Menu
    [SerializeField] private VideoClip ToControlsMenuAnim;
    [SerializeField] private GameObject controlsMenuBackButton;
    

    
    
    
    private delegate void nextFuntionToExecute();
    private nextFuntionToExecute functionQueue;


    private void Awake()
    {

        videoPlayer.prepareCompleted += source => videoPlayer.Play();

        DeactivateMainMenuButtons();
        videoPlayer.clip = mainMenuIntroAnimation;

        videoPlayer.Prepare();    
        functionQueue = ActivateMainMenuButtons;

    }


    void ActivateMainMenuButtons()
    {
        foreach(GameObject but in mainMenuButtons) but.SetActive(true);
        
        functionQueue -= ActivateMainMenuButtons;

    }

    void DeactivateMainMenuButtons()
    {
        foreach(GameObject but in mainMenuButtons) but.SetActive(false);

    }
    
        
        
        
    
    public void GoToControls()
    {
        DeactivateMainMenuButtons();
        
        videoPlayer.clip = ToControlsMenuAnim;

        videoPlayer.Play();


        functionQueue = ActivateControlsButtons;
    }

    void ActivateControlsButtons()
    {
        controlsMenuBackButton.SetActive(true);
        functionQueue -= ActivateControlsButtons;

    } 

    public void LeaveControlsMenu()
    {
        controlsMenuBackButton.SetActive(false);
        videoPlayer.clip = leaveControlsMenu;

        videoPlayer.Play();


        functionQueue = BackToMainMenu;


    }

    void BackToMainMenu()
    {
        videoPlayer.clip = mainMenuIntroAnimation;
        videoPlayer.Play();



        functionQueue -= BackToMainMenu;
        functionQueue = ActivateMainMenuButtons;
    }
    
    

    public void QuitGame() => Application.Quit();

    public void Play() => SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    
    // Update is called once per frame
    void Update()
    {
        
        Debug.Log(videoPlayer.isPlaying);
        
        if (videoPlayer.isPlaying || functionQueue == null) return;
        
        functionQueue.Invoke();
    }
}
