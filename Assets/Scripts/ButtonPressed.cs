using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonPressed : MonoBehaviour
{
    // When the player presses the start or next button, a click sound is played
    [Header("Input")]
    public XRGrabInteractable grabInteractable;
    public AudioSource buttonPressedSound;
    public AudioClip clipSound;

    void Start()
    {
        buttonPressedSound.clip = clipSound;
        grabInteractable.onSelectEntered.AddListener(OnObjectGrabbed);
    }

    public void OnObjectGrabbed(XRBaseInteractor interactor)
    {
        if (buttonPressedSound != null)
        {
            buttonPressedSound.Play();
        }
    }

    void OnDestroy()
    {
        grabInteractable.onSelectEntered.RemoveListener(OnObjectGrabbed);
    }
}
