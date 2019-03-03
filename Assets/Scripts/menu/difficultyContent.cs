using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Exchange;

public class difficultyContent : MonoBehaviour {
    public static bool updateDiff = false;
    public GameObject DifficultyObject;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (updateDiff)
        {
            updateDiff = false;

            Transform[] ts1 = transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in ts1)
            {
                if (t.gameObject.name == "difficultyContent")
                    continue;

                Destroy(t.gameObject);
            }


            foreach (DifficultyLevels info in selectedTwelveNoteChart.difficultyLevels)
            {
                GameObject Song = Instantiate(DifficultyObject);
                Song.transform.parent = gameObject.transform;
                Song.transform.localScale = new Vector3(1, 1, 1);
                Song.transform.localPosition = new Vector3(Song.transform.localPosition.x, Song.transform.localPosition.y, 0);
                Song.name = info.difficulty;
                Transform[] ts = Song.transform.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in ts)
                {
                    if (t.gameObject.name == "difficulty")
                    {
                        t.gameObject.GetComponent<Text>().text = info.difficulty;
                    }
                }
            }
        }
    }

    private string RetrieveDifficulty(string difficulty)
    {
        string result;
        switch (difficulty)
        {
            case "EasySingle":
                result = "Easy";
                break;
            case "MediumSingle":
                result = "Medium";
                break;
            case "HardSingle":
                result = "Hard";
                break;
            case "ExpertSingle":
                result = "Expert";
                break;
            case "EasyDrums":
                result = "Easy Drums";
                break;
            case "MediumDrums":
                result = "Medium Drums";
                break;
            case "HardDrums":
                result = "Hard Drums";
                break;
            case "ExpertDrums":
                result = "Expert Drums";
                break;
            default:
                result = difficulty;
                break;
        }

        return result;
    }    
}
