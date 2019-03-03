using mid2chart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Exchange
{
    public static int _Version = 1;
    public static MainManager mainManager;
    public static GameObject SelectedTitle;
    public static GameObject SelectedDiff;
    public static GameObject leftTrail;
    public static GameObject RightTrail;
    public static Song selectedMid;
    public static string diffcultyLevel = "";
    public static float currentOffset = 0;
    public static bool chartsLoaded = false;
    public static bool hasBeenWarned = false;
    public static bool hasLoadedOnce = false;
    public static bool isValid = false;
    public static List<GoodFile> SongList;
    public static GoodFile currentFile;
    public static AudioSource currentPlaybackSource;
    public static Settings _currentSettings;
    public static List<CurrentScores> _currentScores;
    public static Info selectedTwelveNoteChart;

    public class Settings
    {
        public string UniqueID = "";
        public string RegistrationID = "";
        public string UserName = "";
    }

    public class CurrentScores
    {
        public string SongName;
        public uint Score;
        public string UserName;
    }

    public static void unSetSelected()
    {
        var regSprite = Resources.Load<Sprite>("Graphics/Trans");

        Transform[] ts = SelectedTitle.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts)
        {
            Transform[] ts1 = t.transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform t1 in ts1)
            {
                if (t1.gameObject.name == "selected")
                {
                    Image realmSelect = t1.gameObject.GetComponent<Image>();
                    realmSelect.sprite = regSprite;
                    SelectedTitle = null;
                }
            }
        }
    }

    ///<summary>
    ///spawnTime - (BPS / 3.14f * 2)
    ///</summary>
    public static float getSpeedOffset(float spawnTime)
    {
        return spawnTime - (TwelveNoteGame.secPerBeat / 3f * 2);
    }

    public static IEnumerator GetImage(GameObject coverImg, string location)
    {
        Image tmp = coverImg.GetComponent<Image>();
        WWW www = new WWW(location);
        yield return www;
        tmp.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }

    public static void unSetSelectedDiif()
    {
        var regSprite = Resources.Load<Sprite>("Graphics/Trans");

        Transform[] ts = SelectedDiff.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts)
        {
            Transform[] ts1 = t.transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform t1 in ts1)
            {
                if (t1.gameObject.name == "selected")
                {
                    Image realmSelect = t1.gameObject.GetComponent<Image>();
                    realmSelect.sprite = regSprite;
                    SelectedDiff = null;
                }
            }
        }
    }

    public static void stopAllAudio()
    {

        Transform[] ts = mainManager.AudioObjects.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts)
        {
            Component[] objs = t.gameObject.GetComponents(typeof(Component));
            foreach (Component comp in objs)
            {
                if (comp.GetType() == typeof(AudioSource))
                {
                    AudioSource temp = comp as AudioSource;
                    temp.Stop();
                }
            }
        }
    }

    public static void resetAudio()
    {
        Transform[] ts = mainManager.AudioObjects.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts)
        {
            Component[] objs = t.gameObject.GetComponents(typeof(Component));
            foreach (Component comp in objs)
            {
                if (comp.GetType() == typeof(AudioSource))
                {
                    mainManager.DestroyObj(t.gameObject);
                }
            }
        }
    }

    public static void setAllAudioTime(float time)
    {
        Transform[] ts = mainManager.AudioObjects.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts)
        {
            Component[] objs = t.gameObject.GetComponents(typeof(Component));
            foreach (Component comp in objs)
            {
                if (comp.GetType() == typeof(AudioSource))
                {
                    AudioSource temp = comp as AudioSource;
                    temp.time = time;
                }
            }
        }
    }

    public static void playAllAudio(float delay)
    {
        Transform[] ts = mainManager.AudioObjects.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts)
        {
            Component[] objs = t.gameObject.GetComponents(typeof(Component));
            foreach (Component comp in objs)
            {
                if (comp.GetType() == typeof(AudioSource))
                {
                    AudioSource temp = comp as AudioSource;
                    temp.PlayScheduled(AudioSettings.dspTime + delay);
                }
            }
        }
    }

    public static AudioSource FindCurrentPlayback()
    {
        Transform[] ts = mainManager.AudioObjects.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts)
        {
            Component[] objs = t.gameObject.GetComponents(typeof(Component));
            foreach (Component comp in objs)
            {
                if (comp.GetType() == typeof(AudioSource))
                {
                    AudioSource temp = comp as AudioSource;
                    if (temp.clip != null)
                        return temp;
                }
            }
        }

        return null;
    }

    public static void setSelected(GameObject target)
    {
        if (SelectedTitle != null)
            unSetSelected();

        SelectedTitle = target;

        var tarSprite = Resources.Load<Sprite>("Graphics/CharacterObjectTargeted");

        Transform[] ts = SelectedTitle.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts)
        {
            if (t.gameObject.name == "selected")
            {
                Image realmSelect = t.gameObject.GetComponent<Image>();
                realmSelect.sprite = tarSprite;
            }
        }
    }

    public static void setSelectedDiff(GameObject target)
    {
        if (SelectedDiff != null)
            unSetSelectedDiif();

        SelectedDiff = target;

        var tarSprite = Resources.Load<Sprite>("Graphics/CharacterObjectTargeted");

        Transform[] ts = SelectedDiff.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts)
        {
            if (t.gameObject.name == "selected")
            {
                Image realmSelect = t.gameObject.GetComponent<Image>();
                realmSelect.sprite = tarSprite;
            }
        }
    }
    
    public static FixedJoint AddFixedJoint(GameObject obj)
    {
        FixedJoint fx = obj.AddComponent<FixedJoint>();
        fx.breakForce = Mathf.Infinity;
        fx.breakTorque = Mathf.Infinity;
        return fx;
    }
}