using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using LSL;

public class StartMainGame : MonoBehaviour
{
    // Start of the main events
    [Header("XR Grab Interactable")]
    public XRGrabInteractable grabInteractable;

    [Header("Start button")]
    public GameObject selfObject;

    [Header("Item left and item right")]
    public GameObject item1;
    public MainItem mainObjectFile;
    public GrabAdvance item5;
    public Renderer item6;
    public Renderer item7;
    public GameObject textItem;

    public GameObject item6_l;
    public GameObject item7_r;

    [Header("Tutorial items")]
    public GameObject tutorialItems;
    public Collider[] colliderRacksTutorialItems;

    [Header("First activation and audio being played")]
    public bool firstActivated = false;
    private bool firstActivatedTemp = false;
    public AudioSource audio1;

    [Header("Ray interactors")]
    public XRInteractorLineVisual rayInteractorL;
    public XRInteractorLineVisual rayInteractorR;

    [Header("LSL")]
    public StreamOutlet outlet;
    private string[] sample = {""};
    
    public int tempIndex0;

    private int numPressed = 0;
    // Start is called before the first frame update
    public void Start()
    {
        grabInteractable.onSelectEntered.AddListener(OnObjectGrabbed);

        if(mainObjectFile.stageNumber % 10 == 0 && mainObjectFile.stageNumber != 0)
        {
            rayInteractorL.enabled = true;
            rayInteractorR.enabled = true;
            XRGrabInteractable interactorL = item6_l.GetComponent<XRGrabInteractable>();
            interactorL.enabled = true;
            XRGrabInteractable interactorR = item7_r.GetComponent<XRGrabInteractable>();
            interactorR.enabled = true;
        }

        if(firstActivated && mainObjectFile.stageNumber != 100)
        {
            selfObject.SetActive(true);
            item1.SetActive(false);
            audio1.Play(); 
        }
    }

    public void OnObjectGrabbed(XRBaseInteractor interactor)
    {
        numPressed++;
        if(numPressed < 3) { 
                //LSL
                sample[0] = "First Start press: " + numPressed;
                Debug.Log(sample[0]);
                mainObjectFile.PushSampleToOutlet(sample);
                //
        }
        if(numPressed == 3) {firstActivatedTemp = true;}
        if(numPressed >= 3 && firstActivatedTemp == true)
        {
            if(!firstActivated)
            {
                textItem.SetActive(true);
                firstActivated = true;
                item1.SetActive(true);
                
                selfObject.SetActive(false);
                mainObjectFile.reportActive = true;

                tempIndex0 = mainObjectFile.distributionProducts[0];
                item5.UpdateMaterialFirstTime(tempIndex0);
                item6.material.color = Color.white;
                item7.material.color = Color.white;

                item6.enabled = true;
                item7.enabled = true;
                tutorialItems.SetActive(false);
                for(int i = 0; i < 3; i++)
                {
                    colliderRacksTutorialItems[i].enabled = false;;
                }

                //LSL
                sample[0] = "First Start";
                Debug.Log(sample[0]);
                mainObjectFile.PushSampleToOutlet(sample);
                //
            }
            else
            {
                item1.SetActive(true);
                selfObject.SetActive(false);
                mainObjectFile.reportActive = true;
                rayInteractorL.enabled = true;
                rayInteractorR.enabled = true;
                        
                if(mainObjectFile.stageNumber % 10 == 0 && mainObjectFile.stageNumber != 0 && mainObjectFile.stageTriggered)
                {
                        string itemType = "None";
                        string itemDiverseTarget = "None";
                        string eventType = "Begin";

                        mainObjectFile.RecordEvent(eventType, itemType, itemDiverseTarget);
                        mainObjectFile.stageTriggered = false;
                        item6.material.color = Color.white;
                        item7.material.color = Color.white;
                }

                //LSL
                sample[0] = "Start break: " + mainObjectFile.stageNumber;
                Debug.Log(sample[0]);
                mainObjectFile.PushSampleToOutlet(sample);
                //
            }
        numPressed = 3;
        }
    }

    void OnEnable()
    {
        grabInteractable.onHoverEntered.AddListener(OnHoverEnter);
        grabInteractable.onHoverExited.AddListener(OnHoverExit);
    }

    void OnDisable()
    {
        grabInteractable.onHoverEntered.RemoveListener(OnHoverEnter);
        grabInteractable.onHoverExited.RemoveListener(OnHoverExit);
    }

    void OnHoverEnter(XRBaseInteractor interactor)
    {
        if(!mainObjectFile.grabbedInTime)
        {
            rayInteractorL.enabled = true;
            rayInteractorR.enabled = true;
        }
    }

    void OnHoverExit(XRBaseInteractor interactor)
    {
        if(!mainObjectFile.grabbedInTime)
        {
            rayInteractorL.enabled = false;
            rayInteractorR.enabled = false;
        }
    }

    public void Quit()
    {
        selfObject.SetActive(false);
    }
}
