using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Exchange;

public class ObjectPooler : MonoBehaviour {
    public static ObjectPooler _current;    
    public int PoolAmount = 30;

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

        for (int i = 0; i < PoolAmount; i++)
        {
            GameObject newNote = Instantiate(bluePrefab, mainManager.gameManager.NotesParent.transform);
            //newNote.transform.localScale = new Vector3(0.5f, 0.02f, 0.5f);
            newNote.name = bluePrefab.name;
            newNote.SetActive(false);
            PooledBlueNotes.Add(newNote);
        }


        PooledRedNotes = new List<GameObject>();

        for (int i = 0; i < PoolAmount; i++)
        {
            GameObject newNote = Instantiate(redPrefab, mainManager.gameManager.NotesParent.transform);
            //newNote.transform.localScale = new Vector3(0.5f, 0.02f, 0.5f);
            newNote.name = redPrefab.name;
            newNote.SetActive(false);
            PooledRedNotes.Add(newNote);
        }

        PooledAnyRedNotes = new List<GameObject>();

        for (int i = 0; i < PoolAmount; i++)
        {
            GameObject newNote = Instantiate(redAnyPrefab, mainManager.gameManager.NotesParent.transform);
            //newNote.transform.localScale = new Vector3(0.5f, 0.02f, 0.5f);
            newNote.name = redAnyPrefab.name;
            newNote.SetActive(false);
            PooledAnyRedNotes.Add(newNote);
        }

        PooledAnyBlueNotes = new List<GameObject>();

        for (int i = 0; i < PoolAmount; i++)
        {
            GameObject newNote = Instantiate(blueAnyPrefab, mainManager.gameManager.NotesParent.transform);
            //newNote.transform.localScale = new Vector3(0.5f, 0.02f, 0.5f);
            newNote.name = blueAnyPrefab.name;
            newNote.SetActive(false);
            PooledAnyBlueNotes.Add(newNote);
        }

        Pooledbombs = new List<GameObject>();

        for (int i = 0; i < PoolAmount; i++)
        {
            GameObject newNote = Instantiate(bombPrefab, mainManager.gameManager.NotesParent.transform);
            //newNote.transform.localScale = new Vector3(0.5f, 0.02f, 0.5f);
            newNote.name = bombPrefab.name;
            newNote.SetActive(false);
            Pooledbombs.Add(newNote);
        }
    }

    public GameObject GetPooledBombNote()
    {
        for (int i = 0; i < Pooledbombs.Count; i++)
        {
            if (!Pooledbombs[i].activeInHierarchy)
            {
                Component[] objs = Pooledbombs[i].GetComponents(typeof(Component)); //Remove Scripts
                foreach (Component comp in objs)
                {
                    if (comp.GetType() == typeof(NoteJump))
                    {
                        NoteJump test = comp as NoteJump;
                        Destroy(test);
                    }
                }

                Transform[] ts = Pooledbombs[i].transform.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in ts)
                {
                    if (t.gameObject.name == "Box")
                    {
                        t.gameObject.transform.localRotation = Quaternion.identity;
                    }
                }
                Pooledbombs[i].transform.localRotation = Quaternion.identity;
                return Pooledbombs[i];
            }
        }

        GameObject newPurple = Instantiate(bombPrefab, mainManager.gameManager.NotesParent.transform);
        newPurple.SetActive(false);
        Pooledbombs.Add(newPurple);
        return newPurple;
    }

    public GameObject GetPooledBlueNote(bool isAny = false)
    {
        if (!isAny)
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

            GameObject newPurple = Instantiate(bluePrefab, mainManager.gameManager.NotesParent.transform);
            newPurple.SetActive(false);
            PooledBlueNotes.Add(newPurple);
            return newPurple;
        }
        else
        {
            for (int i = 0; i < PooledAnyBlueNotes.Count; i++)
            {
                if (!PooledAnyBlueNotes[i].activeInHierarchy)
                {
                    Component[] objs = PooledAnyBlueNotes[i].GetComponents(typeof(Component)); //Remove Scripts
                    foreach (Component comp in objs)
                    {
                        if (comp.GetType() == typeof(NoteJump))
                        {
                            NoteJump test = comp as NoteJump;
                            Destroy(test);
                        }
                    }

                    Transform[] ts = PooledAnyBlueNotes[i].transform.GetComponentsInChildren<Transform>(true);
                    foreach (Transform t in ts)
                    {
                        if (t.gameObject.name == "Box")
                        {
                            t.gameObject.transform.localRotation = Quaternion.identity;
                        }
                    }
                    PooledAnyBlueNotes[i].transform.localRotation = Quaternion.identity;
                    return PooledAnyBlueNotes[i];
                }
            }

            GameObject newPurple = Instantiate(blueAnyPrefab, mainManager.gameManager.NotesParent.transform);
            newPurple.SetActive(false);
            PooledAnyBlueNotes.Add(newPurple);
            return newPurple;
        }
    }
    
    public GameObject GetPooledRedNote(bool isAny = false)
    {
        if (!isAny)
        {
            for (int i = 0; i < PooledRedNotes.Count; i++)
            {
                if (!PooledRedNotes[i].activeInHierarchy)
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

            GameObject newRed = Instantiate(redPrefab, mainManager.gameManager.NotesParent.transform);
            newRed.SetActive(false);
            PooledRedNotes.Add(newRed);
            return newRed;
        }
        else
        {
            for (int i = 0; i < PooledAnyRedNotes.Count; i++)
            {
                if (!PooledAnyRedNotes[i].activeInHierarchy)
                {
                    Component[] objs = PooledAnyRedNotes[i].GetComponents(typeof(Component));
                    foreach (Component comp in objs)
                    {
                        if (comp.GetType() == typeof(NoteJump))
                        {
                            NoteJump test = comp as NoteJump;
                            Destroy(test);
                        }
                    }

                    Transform[] ts = PooledAnyRedNotes[i].transform.GetComponentsInChildren<Transform>(true);
                    foreach (Transform t in ts)
                    {
                        if (t.gameObject.name == "Box")
                        {
                            t.gameObject.transform.localRotation = Quaternion.identity;
                        }
                    }
                    PooledAnyRedNotes[i].transform.localRotation = Quaternion.identity;

                    return PooledAnyRedNotes[i];
                }
            }

            GameObject newRed = Instantiate(redAnyPrefab, mainManager.gameManager.NotesParent.transform);
            newRed.SetActive(false);
            PooledAnyRedNotes.Add(newRed);
            return newRed;
        }
    }    
        
    // Update is called once per frame
    void Update () {
		
	}
}
