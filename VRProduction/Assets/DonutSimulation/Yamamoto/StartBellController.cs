using UnityEngine;

public class StartBellController : MonoBehaviour
{
    public GameTimeManager timeManager;
    public AudioSource bellAudio;
    public Animator bellAnimator;

    private bool hasStarted = false;

    void Update()
    {
        if (!hasStarted && IsStartInput())
        {
            StartSimulation();
        }
    }

    void StartSimulation()
    {
        hasStarted = true;

        // 時間スタート
        timeManager.timeStart = true;

        // ベル音
        if (bellAudio != null)
            bellAudio.Play();

        // アニメーション
        if (bellAnimator != null)
            bellAnimator.SetTrigger("Ring");
    }
    bool IsStartInput()
    {
        return Input.GetKeyDown(KeyCode.P)
            || Input.GetKeyDown(KeyCode.JoystickButton1); // VRコントローラ
    }
}
