using UnityEngine;
using UnityEditor;
using static Exchange;

public class Spawner
{
    static NoteData lastNote;
    static GameObject lastGO;
    public static bool isAny = false;

    public static void SpawnBlueNote(NoteData note, int noteIndex)
    {
        if (note._cutDirection == _cutType._any)
        {
            isAny = true;
        }

        GameObject newNote = ObjectPooler._current.GetPooledBlueNote(isAny);

        Vector3 position = new Vector3(GameObject.Find(note._lineLayer.ToString() + note._lineIndex.ToString()).transform.position.x, GameObject.Find(note._lineLayer.ToString() + note._lineIndex.ToString()).transform.position.y, mainManager.gameManager.SpawnPoint.transform.position.z);

        NoteJump tempCube = newNote.AddComponent<NoteJump>();
        tempCube.noteIndex = noteIndex;
        tempCube.note = note;
        tempCube.currentColor = note._type;

        newNote.transform.position = position;

        newNote.SetActive(true);

        isAny = false;
    }

    public static void SpawnRedNote(NoteData note, int noteIndex)
    {
        if (note._cutDirection == _cutType._any)
        {
            isAny = true;
        }

        GameObject newNote = ObjectPooler._current.GetPooledRedNote(isAny);

        Vector3 position = new Vector3(GameObject.Find(note._lineLayer.ToString() + note._lineIndex.ToString()).transform.position.x, GameObject.Find(note._lineLayer.ToString() + note._lineIndex.ToString()).transform.position.y, mainManager.gameManager.SpawnPoint.transform.position.z);
        
        NoteJump tempCube = newNote.AddComponent<NoteJump>();
        tempCube.noteIndex = noteIndex;
        tempCube.note = note;
        tempCube.currentColor = note._type;
        
        newNote.transform.position = position;

        newNote.SetActive(true);

        isAny = false;
    }

    public static void SpawnBombNote(NoteData note, int noteIndex)
    {

        GameObject newNote = ObjectPooler._current.GetPooledBombNote();

        Vector3 position = new Vector3(GameObject.Find(note._lineLayer.ToString() + note._lineIndex.ToString()).transform.position.x, GameObject.Find(note._lineLayer.ToString() + note._lineIndex.ToString()).transform.position.y, mainManager.gameManager.SpawnPoint.transform.position.z);

        NoteJump tempCube = newNote.AddComponent<NoteJump>();
        tempCube.noteIndex = noteIndex;
        tempCube.note = note;
        tempCube.currentColor = note._type;

        newNote.transform.position = position;

        newNote.SetActive(true);
    }
}