using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Exchange;

public class startButton : MonoBehaviour {

    // Use this for initialization
    void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(startTrack);
    }

    void startTrack()
    {
        stopAllAudio();
        mainManager.menus.SetActive(false);
        mainManager.gameManager.gameObject.SetActive(true);
        GameObject newGame = Instantiate(Resources.Load("Prefabs/grid") as GameObject, mainManager.gameManager.Game.transform);
        newGame.transform.localPosition = new Vector3(0.30f, 1.272f, 0.69f);
        mainManager.gameManager.grid = newGame;
    } 

    // Update is called once per frame
    void Update () {
		
	}
}
