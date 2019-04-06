using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Exchange;

public class GameModule : MonoBehaviour
{
    public AudioSource hit;
    public GameObject CameraEye;
    public GameObject CameraRig;
    public GameObject controllerLeft;
    public GameObject controllerRight;    
    public GameObject controllerMenu;
    public GameObject LeftStick;
    public GameObject Game;
    public GameObject grid;
    public GameObject wall;
    public GameObject SpawnPoint;
    public GameObject RightStick;
    public GameObject Lpos;
    public GameObject Rpos;
    public GameObject FailBar;
    public GameObject NotesParent;
    public TextMesh pointsObject;
    public TextMesh timeObject;
    public TextMesh streakObject;
    public TextMesh multiplyerObject;
    public TextMesh currentPlayingSongName;
    public TextMesh currentPlayingDiff;
    public Renderer[] RightLazers;
    public Renderer[] LeftLazers;
    public Renderer[] TopLeftRightLazers;
    public Renderer[] BackBottomLazers;
    public Material RedMaterial;
    public Material BlueMaterial;
    public Material LightOff;
    public uint streak = 0;
    public uint points = 0;
    public uint failPoints = 50;
    public uint FailTotal = 100;
    public uint multiplier = 1;
    public uint noteTotal = 0;
    public uint hitTotal = 0;
    public Light[] Lights;

    void Start()
    {
        if (mainManager.gameManager == null)
        {
            mainManager.gameManager = this;
        }
    }

    void Awake()
    {
        if (mainManager.gameManager == null)
        {
            mainManager.gameManager = this;
        }
    }

    public void UpdateScore(bool isMiss)
    {
        if (isMiss)
        {
            mainManager.gameManager.streak = 0;
            mainManager.gameManager.multiplier = 1;

            if (mainManager.gameManager.failPoints > 2)
            {
                mainManager.gameManager.failPoints = mainManager.gameManager.failPoints - 4;
                mainManager.gameManager.FailBar.transform.localScale = new Vector3((mainManager.gameManager.failPoints * 2) / 100f, 0.0013125f, 0.3125f);
            }
            else
            {
                //stopAllAudio();
                //resetAudio();
                //SceneManager.LoadScene("menu");
            }
        }
        else
        {
            mainManager.gameManager.streak += 1;
            mainManager.gameManager.hitTotal += 1;
            mainManager.gameManager.points += 50 * mainManager.gameManager.multiplier;

            if (mainManager.gameManager.failPoints < 100)
            {
                mainManager.gameManager.failPoints = mainManager.gameManager.failPoints + 2;
                mainManager.gameManager.FailBar.transform.localScale = new Vector3((mainManager.gameManager.failPoints * 2) / 100f, 0.0013125f, 0.3125f);
            }
        }

        mainManager.gameManager.pointsObject.text = mainManager.gameManager.points.ToString();
        mainManager.gameManager.streakObject.text = mainManager.gameManager.streak.ToString();
        mainManager.gameManager.multiplyerObject.text = "x " + mainManager.gameManager.multiplier.ToString();
    }

    public void MoveAndAttach(GameObject obj, GameObject objLoc, GameObject controller)
    {
        if (!obj.activeInHierarchy)
        {
            obj.SetActive(true);
        }

        obj.transform.position = objLoc.transform.position;
        obj.transform.rotation = objLoc.transform.rotation;

        var jointL = AddFixedJoint(controller);
        jointL.connectedBody = obj.GetComponent<Rigidbody>();
    }

    public void HideObjectInHand(GameObject obj)
    {
        if (!obj.activeInHierarchy)
            return;

        obj.SetActive(false);
    }
        
    void FixedUpdate()
    {
        deltaTime += (Time.smoothDeltaTime - deltaTime) * 0.1f;
    }
    // Update is called once per frame
    void Update()
    {

        if (streak >= 10 && streak <= 19)
            multiplier = 2;

        if (streak >= 20 && streak <= 29)
            multiplier = 4;

        if (streak >= 30 && streak <= 39)
            multiplier = 6;

        if (streak >= 40)
            multiplier = 8;

        if (streak <= 9)
            multiplier = 1;

    }
    float deltaTime = 0.0f;

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
