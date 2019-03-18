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
    NoteJump hitNote;
    Vector3 hitSpeed;
    VelocityEstimator velocityEstimator;
    // Use this for initialization
    void Start () {
        controllerInHand = Controller.GetComponent<SteamVR_TrackedObject>();
        velocityEstimator = ControllerTip.GetComponent<VelocityEstimator>();
        velocityEstimator.BeginEstimatingVelocity();
        if (mainManager._currentControllers == null)
        {
            mainManager._currentControllers = new List<controller>();
        }

        if (mainManager._currentControllers.Count >= 2)
            return;

        mainManager._currentControllers.Add(this);        
    }
    
    private void Awake()
    {
        velocityEstimator = ControllerTip.GetComponent<VelocityEstimator>();
        velocityEstimator.BeginEstimatingVelocity();
    }

    // Update is called once per frame
    void Update () {

        if (device == null)
        {
            if ((int)controllerInHand.index != -1)
            {
                device = SteamVR_Controller.Input((int)controllerInHand.index);
            }
        }        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Box")
        {
            velocityEstimator.BeginEstimatingVelocity();            
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
                    
                    if (hitNote.note._type == handColor)
                    {
                        hitSpeed = velocityEstimator.speed;

                        if (correctHit(hitNote.note._cutDirection))
                        {
                            mainManager.gameManager.UpdateScore(false);
                        }
                        else
                        {
                            //Play a wrong hit sound here
                            mainManager.gameManager.UpdateScore(true);
                        }
                    }
                    else
                    {
                        //Play a wrong hit sound here
                        mainManager.gameManager.UpdateScore(true);
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
                return (hitSpeed.y > 0f) && (hitSpeed.x < 0f);
            case _cutType._bottomRight:                
                return (hitSpeed.y > 0f) && (hitSpeed.x > 0f);
            case _cutType._down:
                return (hitSpeed.y < 0f);
            case _cutType._left:
                return (hitSpeed.x < 0f);
            case _cutType._right:
                return (hitSpeed.x > 0f);
            case _cutType._topLeft:
                return (hitSpeed.y < 0f) && (hitSpeed.x < 0f);
            case _cutType._topRight:
                return (hitSpeed.y < 0f) && (hitSpeed.x > 0f);
            case _cutType._up:
                return (hitSpeed.y > 0f);
            default:
                return false;
        }
    }
}
