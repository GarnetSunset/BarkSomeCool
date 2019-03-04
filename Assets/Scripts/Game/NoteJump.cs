﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Exchange;

public class NoteJump : MonoBehaviour
{
    public int noteIndex;
    public NoteData note;
    public Vector3 startPos;
    public Vector3 secondStartPos;
    public bool isTop = false;
    public bool isConnected = false;
    public float lerpTime;
    public float currentLerpTime;
    public float secondLerpTime;
    public Color startColor;
    public Color endColor;
    public Hand currentColor;
    public _cutType _cutType;
    public Renderer ringRenderer;
    public Renderer lightingRenderer;
    GameObject noteGO;
    float perc = 0;
    float sexondPerc = 0;
    bool canUpdate = true;

    // Use this for initialization
    void Start()
    {
        lerpTime = ((float)note._time - TwelveNoteGame.songPosInBeats) * TwelveNoteGame.secPerBeat;
        _cutType = note._cutDirection;
        ringRenderer = GetComponent<Renderer>();
        startPos = transform.position;

        Transform[] ts = transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts)
        {
            if (t.gameObject.name == "Box")
            {
                noteGO = t.gameObject;
                noteGO.transform.rotation = Quaternion.Euler(0, 0, GetNoteRotation(note));
            }
        }
    }

    float GetNoteRotation(NoteData note)
    {
        float tmp = 0;
        switch (note._cutDirection)
        {
            case _cutType._bottomLeft:
                tmp = 225f;
                break;
            case _cutType._bottomRight:
                tmp = 135f;
                break;
            case _cutType._down:
                tmp = 0f;
                break;
            case _cutType._left:
                tmp = 270f;
                break;
            case _cutType._right:
                tmp = 90f;
                break;
            case _cutType._topLeft:
                tmp = 315f;
                break;
            case _cutType._topRight:
                tmp = 45f;
                break;
            case _cutType._up:
                tmp = 180f;
                break;
        }
        return tmp;
    }

    public void doHitCheck()
    {
        /*if (hitPad.hitTime <= ((float)note._time * jsonGame.secPerBeat) && hitPad.hitTime >= ((float)note._time * jsonGame.secPerBeat) - (0.2f * jsonGame.secPerBeat)) // Early hit
        {
            hitPad.jumpingNotes.Remove(this);

            if (hitPad.lastController.handColor != note._type)
            {
                Color newendColor = new Color(0.1320755f, 0.1320755f, 0.1320755f, 1f);
                mainManager.gameManager.UpdateScore(true);
            }
            else
            {
                mainManager.gameManager.UpdateScore(false);

                gameObject.transform.localScale = new Vector3(0.5f, 0.02f, 0.5f);
                gameObject.SetActive(false);

            }

            if (hitPad.hitTime <= ((float)note._time * jsonGame.secPerBeat) + (0.2f * jsonGame.secPerBeat) && hitPad.hitTime >= (float)note._time * jsonGame.secPerBeat) // late hit
            {
                hitPad.jumpingNotes.Remove(this);

                if (hitPad.lastController.handColor != note._type)
                {
                    Color newendColor = new Color(0.1320755f, 0.1320755f, 0.1320755f, 1f);
                    mainManager.gameManager.UpdateScore(true);
                }
                else
                {
                    mainManager.gameManager.UpdateScore(false);
                }

                gameObject.transform.localScale = new Vector3(0.5f, 0.02f, 0.5f);
                gameObject.SetActive(false);

            }*/
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        lerpTime = (4 * TwelveNoteGame.secPerBeat) / currentPlaybackSource.pitch;

        if (perc == 1)
        {
            if(canUpdate)
            {
                canUpdate = false;
                secondStartPos = transform.position;
            }

            secondLerpTime += Time.smoothDeltaTime;
            
            sexondPerc = secondLerpTime / lerpTime;
            DoEndLerps(sexondPerc);
            return;

        }
        
        currentLerpTime += Time.smoothDeltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        perc = currentLerpTime / lerpTime;
        DoLerps(perc);
    }

    private void DoLerps(float eperc)
    {
        transform.position = Vector3.Lerp(startPos, new Vector3(GameObject.Find(note._lineLayer.ToString() + note._lineIndex.ToString()).transform.position.x, GameObject.Find(note._lineLayer.ToString() + note._lineIndex.ToString()).transform.position.y, mainManager.gameManager.grid.transform.position.z), eperc);
        //transform.LookAt(mainManager.gameManager.wall.transform.position);
    }
    

    private void DoEndLerps(float sperc)
    {
        transform.position = Vector3.Lerp(secondStartPos, new Vector3(GameObject.Find(note._lineLayer.ToString() + note._lineIndex.ToString()).transform.position.x, GameObject.Find(note._lineLayer.ToString() + note._lineIndex.ToString()).transform.position.y, mainManager.gameManager.wall.transform.position.z), sperc);
        //transform.LookAt(mainManager.gameManager.wall.transform.position);
    }
}

