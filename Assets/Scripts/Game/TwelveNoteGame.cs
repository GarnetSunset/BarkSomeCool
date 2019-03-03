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
    public static float secPerBeat;
    public static float BeatPerSec;
    public static float dsptimesong;
    public static float songPosition;
    public static float songPosInBeats;
    public string file = "";
    public int noteIndex = 0;
    public int starIndex = 0;
    public bool paused = false;
    public static bool playtrack = false;
    float pauseSeconds = 0;
    public bool isStar = false;

    // Use this for initialization
    void Start()
    {
        mainManager.saberBladeLeft.SetActive(true);
        mainManager.saberBladeRight.SetActive(true);

        currentPlaybackSource = FindCurrentPlayback();

        notes = new List<NoteData>();

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

        Comparison<NoteData> comparison = (x, y) => x._time.CompareTo(y._time);
        notes.Sort(comparison);
        /*
        foreach (Events _event in te._events)
        {
            SpawnEvent(transform, _event);
        }

        EffectsTrack.transform.position = EffectsTrackPos.transform.position;
        EffectsTrack.transform.rotation = EffectsTrackPos.transform.rotation;

        foreach (ObstacleData _obstacle in te._obstacles)
        {
            SpawnObstacle(transform, _obstacle);
        }*/
    }

    void StartTrack()
    {
        playtrack = true;
    }

    private void CreateNoteForGame(NoteData note, int index)
    {
        if (note._type == Hand.blue)
        {
            Spawner.SpawnPurpleNote(note, index, isStar);
            return;
        }

        if (note._type == Hand.red)
        {
            Spawner.SpawnRedNote(note, index, isStar);
            return;
        }

        if (note._type == Hand.Bomb)
        {
            //Spawner.SpawnWhiteNote(note, index, isStar);
            return;
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
        mainManager.saberBladeLeft.SetActive(true);
        mainManager.saberBladeRight.SetActive(true);

        
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
