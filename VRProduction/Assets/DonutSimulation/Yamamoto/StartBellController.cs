using UnityEngine;

public class StartBellController : MonoBehaviour
{
    public GameTimeManager timeManager;
    public AudioSource bellAudio;
    public Animator bellAnimator;

    private bool hasStarted = false;

    void Update()
    {
        // まだ時間が始まっていない時だけ
        if (!hasStarted && Input.GetKeyDown(KeyCode.P))
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
}
