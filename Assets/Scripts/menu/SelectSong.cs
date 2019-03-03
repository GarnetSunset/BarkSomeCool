using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using mid2chart;
using System.IO;
using static Exchange;

public class SelectSong : MonoBehaviour
{
    public Canvas TargetCanvas;
    public int arrayID;
    public string noteArrayCount;
    float startTime = 15f;
    public static bool audioUpdated = false;
    public static bool checkScores = true;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(GoToTarget);
    }

    public IEnumerator setAudio()
    {
        string path = Path.GetDirectoryName(SongList[arrayID]._chartLocation);
        string[] mp3Files = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
        string[] oggFiles = Directory.GetFiles(path, "*.ogg", SearchOption.AllDirectories);

        if (mp3Files.Length > 0)
        {
            foreach (string loc in mp3Files)
            {
                bool isLast = false;
                
                if (loc == mp3Files[mp3Files.Length - 1])
                    isLast = true;

                GameObject audio = new GameObject();                
                audio.transform.SetParent(mainManager.AudioObjects.transform);
                AudioSource newSource = audio.AddComponent<AudioSource>();
                newSource.playOnAwake = false;
                StartCoroutine(mainManager.RecieveAudio(loc, newSource, isLast));
                yield return null;
            }
        }

        if (oggFiles.Length > 0)
        {
            foreach (string loc in oggFiles)
            {
                bool isLast = false;

                if(loc == oggFiles[oggFiles.Length - 1])
                    isLast = true;

                GameObject audio = new GameObject();
                audio.transform.SetParent(mainManager.AudioObjects.transform);
                AudioSource newSource = audio.AddComponent<AudioSource>();
                newSource.playOnAwake = false;
                StartCoroutine(mainManager.RecieveAudio(loc, newSource, isLast));
                yield return null;
            }
        }
        yield return null;
        stopAllAudio();
        yield return null;
        setSelected(gameObject);

        if (SongList[arrayID].initialData.PreviewStartTime / 1000 > 15)
            startTime = (float)SongList[arrayID].initialData.PreviewStartTime / 1000;

    }

    void GoToTarget()
    {
        selectedTwelveNoteChart = null;

        if (mainManager.start.GetComponent<Button>().interactable)
            mainManager.start.GetComponent<Button>().interactable = false;
        
        resetAudio();

        selectedTwelveNoteChart = mainManager.LoadJSONData(SongList[arrayID]._chartLocation);
        selectedTwelveNoteChart.path = SongList[arrayID]._chartLocation;


        /*PacketWriter RequestScores = new PacketWriter(Opcodes.REQUEST_SCORES);
        RequestScores.WriteString(_currentSettings.UniqueID);
        RequestScores.WriteString(SongList[arrayID].initialData.SongName);
        RequestScores.WriteString(SongList[arrayID].initialData.Artist);
        mSocket.SendPacket(RequestScores);*/

        StartCoroutine(setAudio());

        setSelected(gameObject);

        if (SongList[arrayID].initialData.PreviewStartTime / 1000 > 15)
            startTime = (float)SongList[arrayID].initialData.PreviewStartTime / 1000;

        currentOffset = (float)SongList[arrayID].initialData.offset / 1000;

        currentFile = SongList[arrayID];
    }
        
    void Update()
    {
        if (audioUpdated)
        {
            audioUpdated = false;
            setAllAudioTime(startTime);
            playAllAudio(0);
            difficultyContent.updateDiff = true;
        }

        /*if (checkScores)
        {
            if(_currentScores != null)
            {
                checkScores = false;

                RespawnPlayerScore();

                for (int i = 0; i < _currentScores.Count; i++)
                {
                    if(_currentScores[i].UserName == _currentSettings.UserName)
                    {
                        SpawnPlayerScore(_currentScores[i], (uint)(i + 1));
                    }

                    ScoreBlock newBlock = Instantiate(mainManager.ScoreObject.gameObject).GetComponent<ScoreBlock>();
                    newBlock.transform.SetParent(mainManager.ScoreObject.transform.parent);
                    newBlock.transform.localPosition = mainManager.ScoreObject.gameObject.transform.localPosition;
                    newBlock.transform.localScale = mainManager.ScoreObject.gameObject.transform.localScale;
                    newBlock.transform.localRotation = mainManager.ScoreObject.gameObject.transform.localRotation;
                    newBlock.user.text = _currentScores[i].UserName;
                    newBlock.score.text = _currentScores[i].Score.ToString();
                    newBlock.position.text = (i + 1).ToString();
                    newBlock.gameObject.SetActive(true);
                }

                mainManager.RightMenu.SetActive(true);
            }
            else
            {
                if(mainManager.RightMenu.activeInHierarchy)
                {
                    mainManager.RightMenu.SetActive(false);
                }
            }
        }
    }

    void RespawnPlayerScore()
    {
        mainManager.PlayerScore.user.text = "Complete this song to earn a score.";
        mainManager.PlayerScore.score.text = "";
        mainManager.PlayerScore.position.text = "";
    }

    void SpawnPlayerScore(CurrentScores currentScores, uint place)
    {
        mainManager.PlayerScore.user.text = currentScores.UserName;
        mainManager.PlayerScore.score.text = currentScores.Score.ToString();
        mainManager.PlayerScore.position.text = place.ToString();*/
    }
    //StartCoroutine(action);
}
