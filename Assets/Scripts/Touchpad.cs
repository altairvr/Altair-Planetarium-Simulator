using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Touchpad : MonoBehaviour {

    Status currentStatus;

   //private string myPath = "D:/Unity/Video/FilmVr/";  
    private string myPath = "/storage/emulated/0/FilmVr/";
    
    public MediaPlayer myPlayer;

    private string Name;
    public Text curTimeText ,durTimeText;
    public GameObject scroll, list, player, pause, play;
    public Slider sliderFilm, sliderControl;
    float val;
    TimeSpan curTime, durTime;

    public SceneLoad loadScene;
    public GameObject content, scenes, videoTester, aim;
    public OculusFadeCameraEffect fadeEffect;
    public Tutorial_ScrollView tutorial;

    int counterTime = 0;
    bool statusCoroutine = false;

    void Start ()
    {
        currentStatus = Status.wait;
        OVRTouchpad.Create();
        fadeEffect.Startup();
        loadScene.Startup();
        loadScene.LoadEnvironment("altair_ms_cutted_final");

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => { CheckOldStatus();
            videoTester.transform.rotation= loadScene.head.transform.rotation;
            videoTester.transform.position = loadScene.environment.guiPivot.position;
        };
    }

    private void OnEnable()
    {
        OVRTouchpad.TouchHandler += HandleTouchHandler;
    }

    private void OnDisable()
    {
        OVRTouchpad.TouchHandler -= HandleTouchHandler;
    }

    private void HandleTouchHandler(object sender, EventArgs e)
    {
        var touchArgs = (OVRTouchpad.TouchArgs)e;

        if (touchArgs.TouchType == OVRTouchpad.TouchEvent.Up)
        {
            if (scroll.GetComponent<Scrollbar>().value <= 1f)
                scroll.GetComponent<Scrollbar>().value += 0.2f;
        }
        if (touchArgs.TouchType == OVRTouchpad.TouchEvent.Down)
        {
            if (scroll.GetComponent<Scrollbar>().value >= 0f)
                scroll.GetComponent<Scrollbar>().value -= 0.2f;

        }
        if (touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap)
        {
            if (currentStatus != Status.wait)
            {

                if (!statusCoroutine)
                {
                    player.SetActive(true);
                    scenes.SetActive(true);
                    aim.SetActive(true);
                    StartCoroutine("UIPlayer");
                    statusCoroutine = true;
                }
            }
        }

    }


    
    private IEnumerator UIPlayer()
    {
        counterTime = 0;
        while (true)
        {
            counterTime++;

            if (counterTime == 5)
            { 
                player.SetActive(false);
                scenes.SetActive(false);
                aim.SetActive(false);
                StopCoroutine("UIPlayer");
                statusCoroutine = false;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }


    public void PauseButton()
    {
        myPlayer.Pause();

        StopCoroutine("UIPlayer");
        statusCoroutine = false;

        play.SetActive(true);
        pause.SetActive(false);
        currentStatus = Status.pausedFilm;
    }

    public void PlayButton()
    {
        myPlayer.Play();

        StopCoroutine("UIPlayer");
        statusCoroutine = false;

        play.SetActive(false);
        pause.SetActive(true);
        loadScene.environment.InstantToPlayTransistion();
        currentStatus = Status.playedFilm;
    }

    public void StopButton()
    {
        myPlayer.Stop();

        StopCoroutine("UIPlayer");
        statusCoroutine = false;

        myPlayer.Rewind(true);
        play.SetActive(true);
        pause.SetActive(false);
        sliderControl.GetComponent<Slider>().value = 0;
        currentStatus = Status.stoppedFilm;
    }

    public void ListButton()
    {
        currentStatus = Status.wait;
        myPlayer.Pause();
        StopCoroutine("Go");
        StopCoroutine("UIPlayer");
        statusCoroutine = false;

        player.SetActive(false);
        scenes.SetActive(false);
        aim.SetActive(true);

        loadScene.environment.PlayToWaitingTransition(() => {
              list.SetActive(true);
            scenes.SetActive(true);
        });
    }

    public void AltairButton()
    {
        loadScene.isLoading = false;
        loadScene.LoadEnvironment("altair_ms_cutted_final");
    }
    public void LakeButton()
    {
        loadScene.isLoading = false;
        loadScene.LoadEnvironment("StarLake");
    }
    public void VoidButton()
    {
        loadScene.isLoading = false;
        loadScene.LoadEnvironment("void");
    }

    public void OnSlideProgress()
    {
        StopCoroutine("UIPlayer");
        statusCoroutine = false;
        val = sliderControl.GetComponent<Slider>().value;
        myPlayer.Control.Seek(val);
    }

    public void ButtonClick(string str)
    {
        Debug.Log(str + " button clicked.");
        loadScene.environment.WaitingToPlayTransition(() => {
            currentStatus = Status.loadingFilm;
            myPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, myPath + str, true); });
        player.SetActive(true);
        play.SetActive(false);
        pause.SetActive(true);
        StartCoroutine("Go");
        list.SetActive(false);
        currentStatus = Status.playedFilm;
        if (!statusCoroutine)
        {
            StartCoroutine("UIPlayer");
            statusCoroutine = true;
        }
    }

    IEnumerator Go() {
        while (true) {

            if (myPlayer.Control.IsFinished()|| myPlayer.Control.IsPaused())
            {
                play.SetActive(true);
                pause.SetActive(false);
            }
                       
            durTime = TimeSpan.FromSeconds(myPlayer.Info.GetDurationMs()/1000);
            curTime = TimeSpan.FromSeconds(myPlayer.Control.GetCurrentTimeMs()/1000);

            if (durTime.Hours > 0)
            {
                durTimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", durTime.Hours, durTime.Minutes, durTime.Seconds);
                curTimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", curTime.Hours, curTime.Minutes, curTime.Seconds);
            }
            else
            {
                durTimeText.text = string.Format("{0:D2}:{1:D2}", durTime.Minutes, durTime.Seconds);
                curTimeText.text = string.Format("{0:D2}:{1:D2}", curTime.Minutes, curTime.Seconds);
            }


            sliderFilm.GetComponent<Slider>().minValue = 0;
            sliderFilm.GetComponent<Slider>().maxValue = myPlayer.Info.GetDurationMs();
            sliderControl.GetComponent<Slider>().minValue = 0;
            sliderControl.GetComponent<Slider>().maxValue = myPlayer.Info.GetDurationMs();
            sliderFilm.GetComponent<Slider>().value =  myPlayer.Control.GetCurrentTimeMs();
            yield return new WaitForSeconds(0.1f);
        } 
    }

    void CheckOldStatus()
    {
        if (currentStatus == Status.playedFilm || currentStatus == Status.loadingFilm || currentStatus == Status.pausedFilm)
           loadScene.environment.InstantToPlayTransistion();
    }

}
