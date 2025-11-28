using UnityEngine;
using TMPro;

public class GameTimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    public int hour = 0;       // 0時スタート
    public int minute = 0;     // 0分スタート
    public float timeSpeed = 60f; // 1秒で1分進む

    public int openTime = 9;
    public int closeTime = 22;

    private float timer = 0f;

    public Light mainLight;
    public float dayLightIntensity = 1f;
    public float nightLightIntensity = 0.2f;
    public float transitionSpeed = 1f;

    void Update()
    {
        // 時間進行
        timer += Time.deltaTime * timeSpeed;
        if (timer >= 60f)
        {
            minute++;
            timer = 0;
        }

        if (minute >= 60)
        {
            minute = 0;
            hour++;
        }

        if (hour >= 24)
        {
            hour = 0;
        }

        // UI更新
        timeText.text = $"{hour:00}:{minute:00}";

        // 昼夜ライト更新
        UpdateLighting();
    }

    public bool IsOpen()
    {
        return hour >= openTime && hour < closeTime;
    }

    void UpdateLighting()
    {
        float targetIntensity;

        if (hour >= 22 || hour < 6)
        {
            targetIntensity = nightLightIntensity;
        }
        else
        {
            targetIntensity = dayLightIntensity;
        }

        mainLight.intensity = Mathf.Lerp(mainLight.intensity, targetIntensity, Time.deltaTime * transitionSpeed);
    }
}
