using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using EzySlice;
using static Exchange;

public class controller : MonoBehaviour {

    public SteamVR_Controller.Device device;
    public SteamVR_TrackedObject controllerInHand;
    public GameObject ControllerTip;
    public GameObject Controller;
    public GameObject cutPlane;
    public Hand handColor;
    public Material crossMat;
    Vector3 oldPos;
    Vector3 newPos;
    NoteJump hitNote;
    // Use this for initialization
    void Start () {
        oldPos = ControllerTip.transform.position;
        controllerInHand = Controller.GetComponent<SteamVR_TrackedObject>();
        //GetComponent<VelocityEstimator>().BeginEstimatingVelocity();

        if (mainManager._currentControllers == null)
        {
            mainManager._currentControllers = new List<controller>();
        }

        if (mainManager._currentControllers.Count >= 2)
            return;

        mainManager._currentControllers.Add(this);        
    }

    public bool goingUp = false;
    public bool goingDown = false;
    public bool goingLeft = false;
    public bool goingRight = false;
    public bool goingDownLeft = false;
    public bool goingDownRight = false;
    public bool goingUpLeft = false;
    public bool goingUpRight = false;
    private void Awake()
    {
        //GetComponent<VelocityEstimator>().BeginEstimatingVelocity();
    }
    void FixedUpdate()
    {
        newPos = ControllerTip.transform.position;

        if (device == null)
        {
            if ((int)controllerInHand.index != -1)
            {
                device = SteamVR_Controller.Input((int)controllerInHand.index);
            }
        }

        if (oldPos.y > newPos.y)
        {
            goingDown = true;
            goingUp = false;

            if(goingRight)
            {
                goingDownLeft = false;
                goingDownRight = true;
            }
            else
            {
                goingDownRight = false;
                goingDownLeft = true;
            }
        }
        else
        {
            goingDown = false;
            goingUp = true;

            if (goingRight)
            {
                goingUpLeft = false;
                goingUpRight = true;
            }
            else
            {
                goingUpRight = false;
                goingUpLeft = true;
            }
        }

        if (oldPos.x < newPos.x)
        {
            goingLeft = false;
            goingRight = true;
        }
        else
        {
            goingRight = false;
            goingLeft = true;
        }

        oldPos = newPos;
    }

    // Update is called once per frame
    void Update () {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Box")
        {
            hitNote = null;

            mainManager.Vibrate(Controller, device);

            _Sliced hull = other.gameObject.Slice(cutPlane.transform.position, cutPlane.transform.up, crossMat);

            if (hull != null)
            {
                hull.CreateLowerHull(other.gameObject, crossMat);
                hull.CreateUpperHull(other.gameObject, crossMat);
            }

            Component[] objs = other.gameObject.transform.parent.GetComponents(typeof(Component)); //Remove Scripts
            foreach (Component comp in objs)
            {
                if (comp.GetType() == typeof(NoteJump))
                {
                    hitNote = comp as NoteJump;

                    if(hitNote.note._type != handColor)
                    {
                        mainManager.gameManager.UpdateScore(true); //Wrong Color

                        //Play a wrong hit sound here
                        return;
                    }

                    if(correctHit(hitNote.note._cutDirection))
                    {
                        mainManager.gameManager.UpdateScore(false);
                    }

                    hitNote.gameObject.SetActive(false);
                }
            }
        }

    }

    public bool correctHit(_cutType type)
    {
        switch(type)
        {
            case _cutType._any:
                return true;
            case _cutType._bottomLeft:
                return goingUpRight;
            case _cutType._bottomRight:
                return goingUpLeft;
            case _cutType._down:
                return goingDown;
            case _cutType._left:
                return goingLeft;
            case _cutType._right:
                return goingRight;
            case _cutType._topLeft:
                return goingDownRight;
            case _cutType._topRight:
                return goingDownLeft;
            case _cutType._up:
                return goingUp;
            default:
                return false;
        }
    }
}
