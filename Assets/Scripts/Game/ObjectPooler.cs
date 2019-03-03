using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Exchange;

public class ObjectPooler : MonoBehaviour {
    public static ObjectPooler _current;

    public int BlueNotePoolAmount = 30;
    public int RedNotePoolAmount = 30;
    public int RedAnyNotePoolAmount = 30;
    public int BlueAnyNotePoolAmount = 30;
    public int bombPoolAmount = 30;

    public List<GameObject> PooledBlueNotes;
    public List<GameObject> PooledRedNotes;
    public List<GameObject> PooledAnyRedNotes;
    public List<GameObject> PooledAnyBlueNotes;
    public List<GameObject> Pooledbombs;

    public GameObject redPrefab;
    public GameObject bluePrefab;
    public GameObject blueAnyPrefab;
    public GameObject redAnyPrefab;
    public GameObject bombPrefab;
    private void Awake()
    {
        _current = this;
    }
    // Use this for initialization
    void Start () {

        SetPooledNotes();
    }

    public void SetPooledNotes()
    {
        PooledBlueNotes = new List<GameObject>();

        for (int i = 0; i < BlueNotePoolAmount; i++)
        {
            GameObject newNote = Instantiate(bluePrefab, mainManager.gameManager.Game.transform);
            //newNote.transform.localScale = new Vector3(0.5f, 0.02f, 0.5f);
            newNote.name = bluePrefab.name;
            newNote.SetActive(false);
            PooledBlueNotes.Add(newNote);
        }

        PooledRedNotes = new List<GameObject>();

        for (int i = 0; i < RedNotePoolAmount; i++)
        {
            GameObject newNote = Instantiate(redPrefab, mainManager.gameManager.Game.transform);
            //newNote.transform.localScale = new Vector3(0.5f, 0.02f, 0.5f);
            newNote.name = redPrefab.name;
            newNote.SetActive(false);
            PooledRedNotes.Add(newNote);
        }        
    }

    public GameObject GetPooledPurpleNote(bool isStar = false)
    {
        for (int i = 0; i < PooledBlueNotes.Count; i++)
        {
            if (!PooledBlueNotes[i].activeInHierarchy)
            {
                Component[] objs = PooledBlueNotes[i].GetComponents(typeof(Component)); //Remove Scripts
                foreach (Component comp in objs)
                {
                    if (comp.GetType() == typeof(NoteJump))
                    {
                        NoteJump test = comp as NoteJump;
                        Destroy(test);
                    }
                }

                Transform[] ts = PooledBlueNotes[i].transform.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in ts)
                {
                    if (t.gameObject.name == "Box")
                    {
                        t.gameObject.transform.localRotation = Quaternion.identity;
                    }
                }
                PooledBlueNotes[i].transform.localRotation = Quaternion.identity;
                return PooledBlueNotes[i];
            }
        }

        GameObject newPurple = Instantiate(bluePrefab);
        newPurple.SetActive(false);
        PooledBlueNotes.Add(newPurple);
        return newPurple;
    }
    
    public GameObject GetPooledRedNote(bool isStar = false)
    {
        for (int i = 0; i < PooledRedNotes.Count; i++)
        {
            if(!PooledRedNotes[i].activeInHierarchy)
            {
                Component[] objs = PooledRedNotes[i].GetComponents(typeof(Component));
                foreach (Component comp in objs)
                {
                    if (comp.GetType() == typeof(NoteJump))
                    {
                        NoteJump test = comp as NoteJump;
                        Destroy(test);
                    }
                }

                Transform[] ts = PooledRedNotes[i].transform.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in ts)
                {
                    if (t.gameObject.name == "Box")
                    {
                        t.gameObject.transform.localRotation = Quaternion.identity;
                    }
                }
                PooledRedNotes[i].transform.localRotation = Quaternion.identity;

                return PooledRedNotes[i];
            }
        }

        GameObject newRed = Instantiate(redPrefab);
        newRed.SetActive(false);
        PooledRedNotes.Add(newRed);
        return newRed;
    }    
        
    // Update is called once per frame
    void Update () {
		
	}
}
