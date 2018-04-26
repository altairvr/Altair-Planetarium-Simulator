using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using RenderHeads.Media.AVProVideo;

public class Tutorial_ScrollView : MonoBehaviour
{
    // public string directoryPath = "D:/Unity/Video/FilmVr/";
    private string directoryPath = "/storage/emulated/0/FilmVr/";

    public GameObject Button_Template, message;
    private List<string> fileNamesList = new List<string>();
   
    public MediaPlayer myPlayer;
    public Touchpad touch;
    public SceneLoad loadScene;


    private void Awake()
    {
        // Создание папки для фильма, если её нет
        string folderPath = directoryPath + "/";
        DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
            Debug.Log("Create new directory for film");
        }
    }

    void Start()
    {
        string[] filePaths = Directory.GetFiles(directoryPath, "*.mp4");
        Debug.Log("Количество файлов формата .mp4");
        Debug.Log(filePaths.Length);

        if (filePaths.Length == 0) message.SetActive(true);
        else {
            foreach (string fileName in filePaths)
            {
                fileNamesList.Add(Path.GetFileName(fileName));
                Debug.Log(Path.GetFileName(fileName));
            }

            foreach (string str in fileNamesList)
            {
                GameObject filmButton = Instantiate(Button_Template) as GameObject;
                filmButton.SetActive(true);
                Tutorial_Button TB = filmButton.GetComponent<Tutorial_Button>();
                TB.SetName(str);
                filmButton.transform.SetParent(Button_Template.transform.parent);
            }
        } 
    }

   
    public void ButtonClicked(string str)
    {
        Debug.Log(str + " button clicked.");
        touch.ButtonClick(str);
    }
}