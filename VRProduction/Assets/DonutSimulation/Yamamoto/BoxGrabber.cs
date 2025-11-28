using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BoxGrabber : MonoBehaviour
{
    public InputActionProperty gripAction;
    private BoxInventory currentBox;
    private XRGrabInteractable interactable;

    private void Awake()
    {
        interactable=GetComponent<XRGrabInteractable>();
    }

   

    void OnTriggerStay(Collider other)
    {
        // ” ‚Ì’†‚É‚¢‚é
        var box = other.GetComponent<BoxInventory>();
        if (box != null)
        {
            currentBox = box;

            float gripValue = gripAction.action.ReadValue<float>();

            // ‚Â‚©‚Þ“®ì‚ð‚µ‚½‚çŽæ‚èo‚·
            if (gripValue > 0.7f && box.HasStock())
            {
                box.TakeItem();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BoxInventory>() != null)
        {
            currentBox = null;
        }
    }
}
