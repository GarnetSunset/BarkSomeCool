using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Exchange;

public class difficultyButton : MonoBehaviour {
    

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(GoToTarget);
    }

    void GoToTarget()
    {
        setSelectedDiff(gameObject);


        foreach (DifficultyLevels info in selectedTwelveNoteChart.difficultyLevels)
        {
            if (info.difficulty == gameObject.name)
            {
                diffcultyLevel = info.jsonPath;
                mainManager.start.GetComponent<Button>().interactable = true;
            }
        }
    }
}
