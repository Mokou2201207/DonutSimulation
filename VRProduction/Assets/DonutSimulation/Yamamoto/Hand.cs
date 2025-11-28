using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public enum HandDirection
{
    Left,
    Right,
}

public class Hand : MonoBehaviour
{
    public HandDirection direction;
    public NearFarInteractor NfInteractor;
}
