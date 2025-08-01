using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using LSL;

public class GrabAdvance : MonoBehaviour
{
    // Script that controls the grabbing functionality of the items. Functionalities include: 
    // outline when hovering over items, 
    // changing the materials of the item of interest per event, 
    // controlling the time per round and
    //sending the grab data to the main file / data recording file

    [Header("Linking main script")]
    public MainItem mainObjectFile;

    [Header("XRGrabInteractable")]
    public XRGrabInteractable grabInteractable;

    [Header("Object of interest (left / right tea package)")]
    public Renderer objectRenderer;
    public GameObject otherObject;
    public Renderer otherObjectRenderer;

    [Header("Ray interactor controllers")]
    public XRInteractorLineVisual rayInteractorL;
    public XRInteractorLineVisual rayInteractorR;

    [Header("Materials")]
    public Material blackMaterial;
    public Material greenMaterial;
    public Material targetMaterial;
    public Material[] diverseMaterial;

    public GameObject[] objectsTargetColliders;
    public GameObject[] objectsTargetCollidersOther;

    [Header("Timer per round")]
    float timer = 6f;
    public bool reportActiveItem = false;

    [Header("LSL")]
    private StreamOutlet outlet;
    private string[] sample = {""};

    void Start()
    {
        grabInteractable.onSelectEntered.AddListener(OnObjectGrabbed);

        mainObjectFile.grabbedInTime = false;
    }
    
    void OnDestroy()
    {
        grabInteractable.onSelectEntered.RemoveListener(OnObjectGrabbed);
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
        objectRenderer.material.color = Color.green;
    }

    // When the player points their controller to the item of interst, a light blue outline will appear
    void OnHoverEnter(XRBaseInteractor interactor)
    {
        if(!mainObjectFile.grabbedInTime)
        {
            float r = 0.8f;
            float g = 0.9f;
            float b = 1.0f;

            Color tempLightBlue = new Color(r, g, b);

            Material temp1 = objectRenderer.material;
            temp1.color = tempLightBlue;
            rayInteractorL.enabled = true;
            rayInteractorR.enabled = true;
        }
    }

    void OnHoverExit(XRBaseInteractor interactor)
    {
        if(!mainObjectFile.grabbedInTime)
        {
            Material temp1 = objectRenderer.material;
            temp1.color = Color.white;
            rayInteractorL.enabled = false;
            rayInteractorR.enabled = false;
        }
    }

    public void OnObjectGrabbed(XRBaseInteractor interactor)
    {
        mainObjectFile.reportActive = true;
        mainObjectFile.grabbedInTime = true;
        
        if(objectRenderer.name == "Item1")
        {
            mainObjectFile.grabItemLeft = true;
        }
        else
        {
            mainObjectFile.grabItemRight = true;
        }
        
        string itemType = (objectRenderer.name == "Item1") ? "Left" : "Right";
        string itemDiverseTarget = objectRenderer.material.ToString();
        string eventType = "Grabbing";
        itemDiverseTarget = itemDiverseTarget.Replace(" (Instance) (UnityEngine.Material)", "");

        // Record the grabbing event
        mainObjectFile.RecordEvent(eventType, itemType, itemDiverseTarget);

        objectRenderer.material.color = Color.green;
        reportActiveItem = true;
        grabInteractable.enabled = false;
        otherObject.SetActive(false);
        
        rayInteractorL.enabled = false;
        rayInteractorR.enabled = false; 

        //LSL
        var tempStage = mainObjectFile.stageNumber + 1;
        var itemOther = otherObjectRenderer.material.ToString();
        itemOther = itemOther.Replace(" (Instance) (UnityEngine.Material)", "");
        if(objectRenderer.name == "ItemLeft") {sample[0] = "Grab Item stage: " + tempStage + " / Left: " + itemDiverseTarget + " / Right: " +  itemOther + " / Chosen: left"; }
        else {sample[0] = "Grab Item stage: " + tempStage + " / Left: " + itemOther + " / Right: " +  itemDiverseTarget + " / Chosen: right";  }
        Debug.Log(sample[0]);
        mainObjectFile.PushSampleToOutlet(sample);
        //
    }

    public void Update()
    {
        if(reportActiveItem)
        {

            if(!mainObjectFile.updateItemGrab)
            {
                mainObjectFile.grabbedInTime = false;
                reportActiveItem = false;
                timer = 6f;
                grabInteractable.enabled = true;
                otherObject.SetActive(true);
                mainObjectFile.grabItemLeft = false;
                mainObjectFile.grabItemLeft = false;        
            }
            
        }
        
    }

    public void UpdateMaterial()
    {
        int materialIndex = mainObjectFile.stageNumber;
        var temp = mainObjectFile.distributionProducts[materialIndex];

        int randomIndex = 0;
        Material tempDiverseMaterial;
        //float randomNumber = Random.Range(0, 10);
        if(temp == 0)
            {
                int randomNumber = Random.Range(0, 10);

                randomIndex = UnityEngine.Random.Range(0, diverseMaterial.Length);
                tempDiverseMaterial = diverseMaterial[randomIndex];

                if(randomNumber <= 5)
                {
                    objectRenderer.material = targetMaterial;
                    for(int i = 0; i < 3; i++)
                    {
                        objectsTargetColliders[i].SetActive(true);
                        objectsTargetCollidersOther[i].SetActive(false);
                    }
                    

                    otherObjectRenderer.material = tempDiverseMaterial;
                }
                else
                {
                    objectRenderer.material = tempDiverseMaterial;

                    otherObjectRenderer.material = targetMaterial;
                    for(int i = 0; i < 3; i++)
                    {
                        objectsTargetColliders[i].SetActive(false);
                        objectsTargetCollidersOther[i].SetActive(true);
                    }
                }
                
            }
        else
            {
                var tempList = Enumerable.Range(0,diverseMaterial.Length).ToList();
                
                int randomIndexNum1 = UnityEngine.Random.Range(0, tempList.Count);
                
                int itemToRemoveIndex = tempList[randomIndexNum1];
                tempList.RemoveAt(randomIndexNum1);

                int randomIndexNum2 = UnityEngine.Random.Range(0, tempList.Count);
                
                while (randomIndexNum2 == randomIndexNum1)
                {
                    randomIndexNum2 = UnityEngine.Random.Range(0, tempList.Count);
                }
                
                Material tempDiverseMaterialNum1 = diverseMaterial[randomIndexNum1];
                Material tempDiverseMaterialNum2 = diverseMaterial[randomIndexNum2];

                objectRenderer.material = tempDiverseMaterialNum1;
                otherObjectRenderer.material = tempDiverseMaterialNum2;

                for(int i = 0; i < 3; i++)
                    {
                        objectsTargetColliders[i].SetActive(false);
                        objectsTargetCollidersOther[i].SetActive(false);
                    }
            }

        string itemType = "None";
        string itemDiverseTarget = "None";
        string eventType = "Begin";

        mainObjectFile.RecordEvent(eventType, itemType, itemDiverseTarget);  
    }
    
    public void UpdateMaterialFirstTime(int value)
    {
        int randomIndex1 = 0;
        Material tempDiverseMaterial1;
        if(value == 1)
        {
                var tempList = Enumerable.Range(0,diverseMaterial.Length).ToList();
                
                int randomIndexNum1 = UnityEngine.Random.Range(0, tempList.Count);
                
                int itemToRemoveIndex = tempList[randomIndexNum1];
                tempList.RemoveAt(randomIndexNum1);

                int randomIndexNum2 = UnityEngine.Random.Range(0, tempList.Count);
                
                while (randomIndexNum2 == randomIndexNum1)
                {
                    randomIndexNum2 = UnityEngine.Random.Range(0, tempList.Count);
                }
                
                Material tempDiverseMaterialNum1 = diverseMaterial[randomIndexNum1];
                Material tempDiverseMaterialNum2 = diverseMaterial[randomIndexNum2];

                objectRenderer.material = tempDiverseMaterialNum1;
                otherObjectRenderer.material = tempDiverseMaterialNum2;

                for(int i = 0; i < 3; i++)
                    {
                        objectsTargetColliders[i].SetActive(false);
                        objectsTargetCollidersOther[i].SetActive(false);
                    }
        }
        if(value == 0)
        {
            int randomNumber = Random.Range(0, 10);

            randomIndex1 = UnityEngine.Random.Range(0, diverseMaterial.Length);
            tempDiverseMaterial1 = diverseMaterial[randomIndex1];

                if(randomNumber <= 5)
                {
                    objectRenderer.material = targetMaterial;
                    for(int i = 0; i < 3; i++)
                    {
                        objectsTargetColliders[i].SetActive(true);
                        objectsTargetCollidersOther[i].SetActive(false);
                    }
                    
                    otherObjectRenderer.material = tempDiverseMaterial1;
                }
                else
                {
                    objectRenderer.material = tempDiverseMaterial1;
                    otherObjectRenderer.material = targetMaterial;
                    for(int i = 0; i < 3; i++)
                    {
                        objectsTargetColliders[i].SetActive(false);
                        objectsTargetCollidersOther[i].SetActive(true);
                    }
                }
        }    
    }
}
