using UnityEngine;
using UnityEditor;
using static Exchange;

public class Spawner
{
    static NoteData lastNote;
    static GameObject lastGO;
    public static void SpawnPurpleNote(NoteData note, int noteIndex, bool isStarPower = false)
    {        
        GameObject newNote = ObjectPooler._current.GetPooledPurpleNote(isStarPower);

        Vector3 position = new Vector3(GameObject.Find(note._lineLayer.ToString() + note._lineIndex.ToString()).transform.position.x, GameObject.Find(note._lineLayer.ToString() + note._lineIndex.ToString()).transform.position.y, mainManager.gameManager.SpawnPoint.transform.position.z);

        NoteJump tempCube = newNote.AddComponent<NoteJump>();
        tempCube.noteIndex = noteIndex;
        tempCube.note = note;
        tempCube.currentColor = note._type;

        newNote.transform.position = position;

        newNote.SetActive(true);
    }

    public static void SpawnRedNote(NoteData note, int noteIndex, bool isStarPower = false)
    {
        GameObject newNote = ObjectPooler._current.GetPooledRedNote(isStarPower);

        Vector3 position = new Vector3(GameObject.Find(note._lineLayer.ToString() + note._lineIndex.ToString()).transform.position.x, GameObject.Find(note._lineLayer.ToString() + note._lineIndex.ToString()).transform.position.y, mainManager.gameManager.SpawnPoint.transform.position.z);
        
        NoteJump tempCube = newNote.AddComponent<NoteJump>();
        tempCube.noteIndex = noteIndex;
        tempCube.note = note;
        tempCube.currentColor = note._type;
        
        newNote.transform.position = position;

        newNote.SetActive(true);
    }    
}