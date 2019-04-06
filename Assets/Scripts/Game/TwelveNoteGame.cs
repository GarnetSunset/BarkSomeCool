using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Exchange;

public class TwelveNoteGame : MonoBehaviour
{
    public static List<NoteData> notes;
    public static List<Events> noteEvents;
    public static List<ObstacleData> noteObstilcales;
    public static float secPerBeat;
    public static float BeatPerSec;
    public static float dsptimesong;
    public static float songPosition;
    public static float songPosInBeats;
    public string file = "";
    public int noteIndex = 0;
    public int eventIndex = 0;
    public int obsticalIndex = 0;
    public bool paused = false;
    public static bool playtrack = false;
    float pauseSeconds = 0;

    // Use this for initialization
    void Start()
    {
        mainManager.saberBladeLeft.SetActive(true);
        mainManager.saberBladeRight.SetActive(true);

        currentPlaybackSource = FindCurrentPlayback();

        notes = new List<NoteData>();
        noteEvents = new List<Events>();
        noteObstilcales = new List<ObstacleData>();

        CreateNotes(selectedTwelveNoteChart, diffcultyLevel);

        mainManager.gameManager.noteTotal = (uint)notes.Count;

        setAllAudioTime(0);
        playAllAudio(0);
        playtrack = true;
        //Invoke("StartTrack", 3f);
    }

    void CreateNotes(Info infoForNotes, string file)
    {
        string path = Path.GetDirectoryName(infoForNotes.path);

        var info = File.ReadAllText(path + "/" + file);
        var te = JsonConvert.DeserializeObject<NoteInfo>(info);

        BeatPerSec = selectedTwelveNoteChart.beatsPerMinute / 60f;
        secPerBeat = 60f / selectedTwelveNoteChart.beatsPerMinute;

        dsptimesong = (float)AudioSettings.dspTime;

        foreach (NoteData note in te._notes)
        {
            notes.Add(note);
        }

        Comparison<NoteData> NoteComparison = (x, y) => x._time.CompareTo(y._time);
        notes.Sort(NoteComparison);
        
        foreach (Events _event in te._events)
        {
            noteEvents.Add(_event);
        }

        Comparison<Events> EventComparison = (x, y) => x.Time.CompareTo(y.Time);
        noteEvents.Sort(EventComparison);
        
        foreach (ObstacleData _obstacle in te._obstacles)
        {
            noteObstilcales.Add(_obstacle);
        }

        Comparison<ObstacleData> ObsticaleComparison = (x, y) => x._time.CompareTo(y._time);
        noteObstilcales.Sort(ObsticaleComparison);
    }

    private void CreateNoteForGame(NoteData note, int index)
    {
        if (note._type == Hand.blue)
        {
            Spawner.SpawnBlueNote(note, index);
            return;
        }

        if (note._type == Hand.red)
        {
            Spawner.SpawnRedNote(note, index);
            return;
        }

        if (note._type == Hand.Bomb)
        {
            Spawner.SpawnBombNote(note, index);
            return;
        }
    }
    private void ObstacleForGame(ObstacleData note, int index)
    {
        
    }
    private void EventNoteForGame(Events EventNote, int index)
    {        
        switch (EventNote.Type)
        {
            case BeatSaberEventType.TopLeftRightLazers:
                switch((BeatSaberEventColorType)EventNote.Value)
                {
                    case BeatSaberEventColorType.LightsOff: 
                        foreach (Renderer light in mainManager.gameManager.TopLeftRightLazers)
                        {
                            light.material = mainManager.gameManager.LightOff;
                        }
                        break;
                    case BeatSaberEventColorType.Blue:
                    case BeatSaberEventColorType.BlueUnk:
                        foreach (Renderer light in mainManager.gameManager.TopLeftRightLazers)
                        {
                            light.material = mainManager.gameManager.BlueMaterial;
                        }
                        break;
                    case BeatSaberEventColorType.Bluefade:
                        foreach (Renderer light in mainManager.gameManager.TopLeftRightLazers)
                        {
                            light.material = mainManager.gameManager.BlueMaterial;
                            StartCoroutine(FadeLight(light, mainManager.gameManager.LightOff));
                        }
                        break;
                    case BeatSaberEventColorType.Red:
                    case BeatSaberEventColorType.RedUnk:
                        foreach (Renderer light in mainManager.gameManager.TopLeftRightLazers)
                        {
                            light.material = mainManager.gameManager.RedMaterial;
                        }
                        break;
                    case BeatSaberEventColorType.RedFade:
                        foreach (Renderer light in mainManager.gameManager.TopLeftRightLazers)
                        {
                            light.material = mainManager.gameManager.RedMaterial;
                            StartCoroutine(FadeLight(light, mainManager.gameManager.LightOff));
                        }
                        break;                    
                }
                break;
            case BeatSaberEventType.RightLazer:
                switch ((BeatSaberEventColorType)EventNote.Value)
                {
                    case BeatSaberEventColorType.LightsOff:
                        foreach (Renderer light in mainManager.gameManager.RightLazers)
                        {
                            light.material = mainManager.gameManager.LightOff;
                        }
                        break;
                    case BeatSaberEventColorType.Blue:
                    case BeatSaberEventColorType.BlueUnk:
                        foreach (Renderer light in mainManager.gameManager.RightLazers)
                        {
                            light.material = mainManager.gameManager.BlueMaterial;
                        }
                        break;
                    case BeatSaberEventColorType.Bluefade:
                        foreach (Renderer light in mainManager.gameManager.RightLazers)
                        {
                            light.material = mainManager.gameManager.BlueMaterial;
                            StartCoroutine(FadeLight(light, mainManager.gameManager.LightOff));
                        }
                        break;
                    case BeatSaberEventColorType.Red:
                    case BeatSaberEventColorType.RedUnk:
                        foreach (Renderer light in mainManager.gameManager.RightLazers)
                        {
                            light.material = mainManager.gameManager.RedMaterial;
                        }
                        break;
                    case BeatSaberEventColorType.RedFade:
                        foreach (Renderer light in mainManager.gameManager.RightLazers)
                        {
                            light.material = mainManager.gameManager.RedMaterial;
                            StartCoroutine(FadeLight(light, mainManager.gameManager.LightOff));
                        }
                        break;
                }
                break;
            case BeatSaberEventType.LeftLazer:
                switch ((BeatSaberEventColorType)EventNote.Value)
                {
                    case BeatSaberEventColorType.LightsOff:
                        foreach (Renderer light in mainManager.gameManager.LeftLazers)
                        {
                            light.material = mainManager.gameManager.LightOff;
                        }
                        break;
                    case BeatSaberEventColorType.Blue:
                    case BeatSaberEventColorType.BlueUnk:
                        foreach (Renderer light in mainManager.gameManager.LeftLazers)
                        {
                            light.material = mainManager.gameManager.BlueMaterial;
                        }
                        break;
                    case BeatSaberEventColorType.Bluefade:
                        foreach (Renderer light in mainManager.gameManager.LeftLazers)
                        {
                            light.material = mainManager.gameManager.BlueMaterial;
                            StartCoroutine(FadeLight(light, mainManager.gameManager.LightOff));
                        }
                        break;
                    case BeatSaberEventColorType.Red:
                    case BeatSaberEventColorType.RedUnk:
                        foreach (Renderer light in mainManager.gameManager.LeftLazers)
                        {
                            light.material = mainManager.gameManager.RedMaterial;
                        }
                        break;
                    case BeatSaberEventColorType.RedFade:
                        foreach (Renderer light in mainManager.gameManager.LeftLazers)
                        {
                            light.material = mainManager.gameManager.RedMaterial;
                            StartCoroutine(FadeLight(light, mainManager.gameManager.LightOff));
                        }
                        break;
                }
                break;
            case BeatSaberEventType.BackTopLazer:
                switch ((BeatSaberEventColorType)EventNote.Value)
                {
                    case BeatSaberEventColorType.LightsOff:
                        foreach (Renderer light in mainManager.gameManager.BackBottomLazers)
                        {
                            light.material = mainManager.gameManager.LightOff;
                        }
                        break;
                    case BeatSaberEventColorType.Blue:
                    case BeatSaberEventColorType.BlueUnk:
                        foreach (Renderer light in mainManager.gameManager.BackBottomLazers)
                        {
                            light.material = mainManager.gameManager.BlueMaterial;
                        }
                        break;
                    case BeatSaberEventColorType.Bluefade:
                        foreach (Renderer light in mainManager.gameManager.BackBottomLazers)
                        {
                            light.material = mainManager.gameManager.BlueMaterial;
                            StartCoroutine(FadeLight(light, mainManager.gameManager.LightOff));
                        }
                        break;
                    case BeatSaberEventColorType.Red:
                    case BeatSaberEventColorType.RedUnk:
                        foreach (Renderer light in mainManager.gameManager.BackBottomLazers)
                        {
                            light.material = mainManager.gameManager.RedMaterial;
                        }
                        break;
                    case BeatSaberEventColorType.RedFade:
                        foreach (Renderer light in mainManager.gameManager.BackBottomLazers)
                        {
                            light.material = mainManager.gameManager.RedMaterial;
                            StartCoroutine(FadeLight(light, mainManager.gameManager.LightOff));
                        }
                        break;
                }
                break;
            default:
                break;
        }
    }

    IEnumerator FadeLight(Renderer light, Material mat)
    {
        Material oldMat = light.material;
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            light.material.Lerp(oldMat, mat, i);
            yield return null;
        }
    }

    public void endSong()
    {
        /*PacketWriter SubmitScores = new PacketWriter(Opcodes.SCORE_SUBMIT);
        SubmitScores.WriteString(Exchange._currentSettings.UserName);
        SubmitScores.WriteUInt32(Exchange.gameManager.points);
        SubmitScores.WriteString(Exchange.currentGoodData.initialData.SongName);
        SubmitScores.WriteString(Exchange.currentGoodData.initialData.Artist);
        Exchange.mSocket.SendPacket(SubmitScores);*/

        Transform[] ts = mainManager.gameManager.NotesParent.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts)
        {
            if (t.gameObject.name == "Notes")
            {
                continue;
            }

            t.gameObject.SetActive(false);
        }
        mainManager.saberBladeLeft.SetActive(false);
        mainManager.saberBladeRight.SetActive(false);       
        mainManager.menus.SetActive(true);
        mainManager.gameManager.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dsptimesong);
        songPosInBeats = songPosition / secPerBeat;

        if (playtrack)
        {
            if (playtrack && currentPlaybackSource.isPlaying)
            {
                float offsetnow = selectedTwelveNoteChart.difficultyLevels[0].offset / 1000;
                float timer = offsetnow + songPosition;
                if (noteIndex < notes.Count && (notes[noteIndex]._time * secPerBeat) < timer + (4 * secPerBeat) && currentPlaybackSource.isPlaying)
                {
                    CreateNoteForGame(notes[noteIndex], noteIndex);
                    noteIndex++;
                }

                if (eventIndex < noteEvents.Count && (noteEvents[eventIndex].Time * secPerBeat) < timer && currentPlaybackSource.isPlaying)
                {
                    EventNoteForGame(noteEvents[eventIndex], eventIndex);
                    eventIndex++;
                }

                if (obsticalIndex < noteObstilcales.Count && (noteObstilcales[obsticalIndex]._time * secPerBeat) < timer + (4 * secPerBeat) && currentPlaybackSource.isPlaying)
                {
                    ObstacleForGame(noteObstilcales[obsticalIndex], obsticalIndex);
                    obsticalIndex++;
                }
            }

            /*if (mainManager.gameManager.controllerLeft.GetComponent<SteamVR_TrackedController>().menuPressed)
            {
                playtrack = false;
                pauseSeconds = currentPlaybackSource.time;
                stopAllAudio();
                mainManager.gameManager.controllerMenu.SetActive(true);
                Invoke("Paused", 1f);
                return;
            }*/


            if (noteIndex == notes.Count && !currentPlaybackSource.isPlaying)
            {
                endSong();
            }
        }

        /*if (paused)
        {
            if (mainManager.gameManager.controllerLeft.GetComponent<SteamVR_TrackedController>().menuPressed)
            {
                paused = false;
                Invoke("unPaused", 0.5f);

            }
        }*/
    }

}
