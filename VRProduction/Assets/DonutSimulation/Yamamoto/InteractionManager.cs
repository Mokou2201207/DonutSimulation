using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class InteractionManager : MonoBehaviour
{
    private static InteractionManager instance;
    public XRInteractionManager manager;

    private void Awake()
    {
        instance= this;
    }

    public static void StartGrab(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        if (instance == null) return;
        instance.manager.SelectEnter(interactor, interactable);
    }
}
