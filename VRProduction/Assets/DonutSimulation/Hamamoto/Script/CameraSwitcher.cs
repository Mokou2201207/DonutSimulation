using UnityEngine;
using UnityEngine.XR;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject pcCamera;   // PC用カメラ
    public GameObject xrOrigin;   // VR用カメラの入ったXR Origin

    void Start()
    {
        if (XRSettings.isDeviceActive)
        {
            // VRモード
            pcCamera.SetActive(false);
            xrOrigin.SetActive(true);
        }
        else
        {
            // PCモード
            pcCamera.SetActive(true);
            xrOrigin.SetActive(false);
        }
    }
}
