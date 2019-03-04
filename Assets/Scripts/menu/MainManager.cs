
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Exchange;

public class MainManager : MonoBehaviour {

    string dir;
    public GameObject start;
    public GameModule gameManager;
    public GameObject controllerModelLeft;
    public GameObject controllerModelRight;
    public GameObject saberBladeLeft;
    public GameObject saberBladeRight;
    public GameObject songList;
    public GameObject menus;    
    public GameObject AudioObjects;
    public GameObject RightMenu;
    public GameObject SongObject;
    public Text LoadedText;
    public List<controller> _currentControllers;
    uint listIndex = 0;
    float deltaTime = 0.0f;
    List<BadFile> badFiles;
    void Start () {

        _currentControllers = new List<controller>();
             
        if (hasLoadedOnce)
            return;

        mainManager = this;

        dir = Application.dataPath + "/Songs/";
                        
        if (!File.Exists(Application.dataPath + "/Scanned.JSON"))
        {
            Instance().Enqueue(CreateScannedFile());
        }
        else
        {
            Instance().Enqueue(LoadSongs());
        }

        controllerModelLeft.SetActive(false);
        controllerModelRight.SetActive(false);

        mainManager.gameManager.MoveAndAttach(mainManager.gameManager.LeftStick, mainManager.gameManager.Lpos, mainManager.gameManager.controllerLeft);
        mainManager.gameManager.MoveAndAttach(mainManager.gameManager.RightStick, mainManager.gameManager.Rpos, mainManager.gameManager.controllerRight);
    }

    public void Vibrate(GameObject controller, SteamVR_Controller.Device device)
    {
        Instance().Enqueue(LongVibration(0.2f, 3999, controller, device));
    }

    IEnumerator LongVibration(float length, float strength, GameObject controller, SteamVR_Controller.Device device)
    {
        strength = Mathf.Clamp01(strength);
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - startTime <= length)
        {
            int valveStrength = Mathf.RoundToInt(Mathf.Lerp(0, 3999, strength));

            device.TriggerHapticPulse((ushort)valveStrength);

            yield return null;
        }        
    }

    public IEnumerator LoadSongs()
    {
        string[] JSONFiles = Directory.GetFiles(dir, "info.JSON", SearchOption.AllDirectories);
        ScannedFiles _preLoaded = LoadsongData(Application.dataPath + "/Scanned.JSON");
        SongList = _preLoaded.GoodSongs;

        if (JSONFiles.Length != _preLoaded.GoodSongs.Count + _preLoaded.BadSongs.Count)
        {
            Instance().Enqueue(CreateScannedFile());
            yield break;
        }

        yield return null;

        foreach (GoodFile indi in _preLoaded.GoodSongs)
        {
            GameObject Song = Instantiate(SongObject);
            Song.transform.SetParent(songList.transform);
            Song.transform.localScale = new Vector3(1, 1, 1);
            Song.transform.localPosition = new Vector3(Song.transform.localPosition.x, Song.transform.localPosition.y, 0);
            Song.name = indi.ID.ToString();
            Song.GetComponent<SelectSong>().arrayID = (int)indi.ID;

            Transform[] ts = Song.transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in ts)
            {
                if (t.gameObject.name == "Artist")
                {
                    t.gameObject.GetComponent<Text>().text = indi.initialData.Artist;
                }
                if (t.gameObject.name == "SongName")
                {
                    t.gameObject.GetComponent<Text>().text = indi.initialData.SongName;
                }
                if (t.gameObject.name == "noteCount")
                {
                    t.gameObject.GetComponent<Text>().text = "5";
                }
                if (t.gameObject.name == "Cover")
                {
                    string path = Path.GetDirectoryName(indi._chartLocation);
                    string[] pngFiles = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);

                    if (pngFiles.Length == 0)
                        continue;

                    StartCoroutine(GetImage(t.gameObject, pngFiles[0]));
                }

                yield return null;

            }

            yield return null;

        }
        
        LoadedText.text = JSONFiles.Length.ToString() + " total songs. " + _preLoaded.GoodSongs.Count.ToString() + " JSONs read successfully. Song folder contains " + _preLoaded.BadSongs.Count.ToString() + " unplayable song(s).";

        chartsLoaded = true;
        hasLoadedOnce = true;
        yield return null;
    }

    ScannedFiles LoadsongData(string loc)
    {
        var info = File.ReadAllText(loc);
        return JsonConvert.DeserializeObject<ScannedFiles>(info);
    }
    
    public IEnumerator CreateScannedFile()
    {        
        ScannedFiles _data = new ScannedFiles()
        {
            GoodSongs = MakeScannedData(),
            BadSongs = badFiles
        };

        var jsonSerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        string json = JsonConvert.SerializeObject(_data, Formatting.Indented, jsonSerializerSettings);

        if (!File.Exists(@Application.dataPath + "/Scanned.JSON"))
        {
            using (StreamWriter w = File.AppendText(@Application.dataPath + "/Scanned.JSON"))
            {
                w.WriteLine(json);
            }
        }
        else
        {
            File.Delete(@Application.dataPath + "/Scanned.JSON");

            using (StreamWriter w = File.AppendText(@Application.dataPath + "/Scanned.JSON"))
            {
                w.WriteLine(json);
            }
        }

        Instance().Enqueue(LoadSongs());

        yield return null;
    }

    public List<GoodFile> MakeScannedData()
    {
        badFiles = new List<BadFile>();
        List<GoodFile> _data = new List<GoodFile>();
        listIndex = 0;
        uint badSongIndex = 0;
        
       
        string[] JSONFiles = Directory.GetFiles(dir, "info.JSON", SearchOption.AllDirectories);

        foreach (string s in JSONFiles)
        {

            try
            {
                Info songTwelveNoteInfo = new Info();
                string newDir = @"" + s;
                songTwelveNoteInfo = LoadJSONData(newDir);

                _data.Add(new GoodFile()
                {
                    ID = listIndex,
                    initialData = loadINIData(songTwelveNoteInfo),
                    _chartLocation = s,
                });

                listIndex++;
            }
            catch (Exception e)
            {
                badSongIndex++;

                badFiles.Add(new BadFile()
                {
                    ID = badSongIndex,
                    _chartLocation = s,
                });
            }
        }

        return _data;
    }

    public Info LoadJSONData(string loc)
    {
        var info = File.ReadAllText(loc);
        return JsonConvert.DeserializeObject<Info>(info);
    }

    public FileData loadINIData(Info songTwelveNoteInfo)
    {
        FileData temp = new FileData();
        
        temp.Artist = songTwelveNoteInfo.authorName;
        temp.SongName = songTwelveNoteInfo.songName;
        temp.Charter = songTwelveNoteInfo.songSubName;
        temp.PreviewStartTime = (long)songTwelveNoteInfo.previewStartTime;
        temp.offset = songTwelveNoteInfo.difficultyLevels[0].offset;
        temp.Album = "Album Unknown";

        return temp;
    }

    

    // Update is called once per frame
    void Update () {
        deltaTime += (Time.smoothDeltaTime - deltaTime) * 0.1f;
        
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }

    public IEnumerator RecieveAudio(string path, AudioSource loc, bool isLast = false)
    {
        string tempPath = Path.GetDirectoryName(path);
        string extension = Path.GetExtension(path);
        string fileName = path.Substring(tempPath.Length + 1, path.Length - (tempPath.Length + 1) - extension.Length);

        var newFileLoc = "file:///" + path;//Uri.EscapeUriString(path);
        
        switch (path.Substring(path.Length - 1, 1))
        {
            case "3":
                if (!File.Exists(tempPath + "\\" + fileName + ".wav"))
                {

                    string OutputAudioFilePath = @tempPath + "\\" + fileName + ".wav";
                    using (var reader = new Mp3FileReader(path))
                    {
                        WaveFileWriter.CreateWaveFile(OutputAudioFilePath, reader);
                    }
                }
                WWW wwwforWav = new WWW("file:///" + tempPath + "\\" + fileName + ".wav");
                wwwforWav.threadPriority = ThreadPriority.High;
                Application.backgroundLoadingPriority = ThreadPriority.High;
                yield return null;
                while (!wwwforWav.isDone)
                { yield return null; }
                yield return null;
                loc.gameObject.name = path;
                loc.clip = wwwforWav.GetAudioClip();
                yield return null;
                break;
            case "g":
            case "G":
                WWW www = new WWW(newFileLoc);
                www.threadPriority = ThreadPriority.High;
                Application.backgroundLoadingPriority = ThreadPriority.High;
                yield return null;
                while (!www.isDone)
                { yield return null; }
                yield return null;
                loc.gameObject.name = path;
                loc.clip = www.GetAudioClip();
                yield return null;
                break;
        }

        if (isLast)
            SelectSong.audioUpdated = true;

        yield return null;
    }

    public void DestroyObj(GameObject go)
    {
        Instance().Enqueue(DestroyObject(go));
    }

    public IEnumerator DestroyObject(GameObject go)
    {
        Destroy(go);

        yield return null;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }

    public void Enqueue(IEnumerator action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(() => {
                StartCoroutine(action);
            });
        }
    }
    public void Enqueue(Action action)
    {
        Enqueue(ActionWrapper(action));
    }
    IEnumerator ActionWrapper(Action a)
    {
        a();
        yield return null;
    }
    private static MainManager _instance;
    public static bool Exists()
    {
        return _instance != null;
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject.transform.parent);
        }
    }
    public static MainManager Instance()
    {
        if (!Exists())
        {
            throw new Exception("UnityMainThreadDispatcher could not find the UnityMainThreadDispatcher object.");
        }
        return _instance;
    }
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();
}
