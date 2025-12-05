using UnityEngine;
using TMPro;

public class GameTimeManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI timeText;

    [Header("Time Settings")]
    public int hour = 0;
    public int minute = 0;
    public float timeSpeed = 60f; // 1秒 = 1ゲーム内分

    private float timer = 0f;

    [Header("Shop Settings")]
    public int openTime = 9;
    public int closeTime = 22;

    [Header("Lighting")]
    public Light mainLight;
    public float dayLightIntensity = 1f;
    public float nightLightIntensity = 0.2f;
    public float transitionSpeed = 1f;

    [Header("Sky System")]
    public Ultrabolt.SkyEngine.SkyCore skyCore;
    void Update()
    {
        UpdateTime();
        UpdateUI();
        UpdateLighting();
        UpdateSky(); // ★時間と空を連動
    }

    void UpdateTime()
    {
        timer += Time.deltaTime * timeSpeed;

        if (timer >= 60f)
        {
            minute++;
            timer = 0f;
        }

        if (minute >= 60)
        {
            minute = 0;
            hour++;
        }

        if (hour >= 24)
            hour = 0;
    }

    void UpdateUI()
    {
        if (timeText != null)
            timeText.text = $"{hour:00}:{minute:00}";
    }

    public bool IsOpen()
    {
        return hour >= openTime && hour < closeTime;
    }

    void UpdateLighting()
    {
        if (mainLight == null) return;

        float target = (hour >= 22 || hour < 6) ? nightLightIntensity : dayLightIntensity;

        mainLight.intensity = Mathf.Lerp(
            mainLight.intensity,
            target,
            Time.deltaTime * transitionSpeed
        );
    }

    //==============================
    // ★ SkyEngine の太陽・月角度更新
    //==============================

    void UpdateSky()
    {
        if (skyCore == null) return;

        float totalMinutes = hour * 60 + minute;

        // 1日の進行度（0〜1）
        float dayProgress = totalMinutes / (24f * 60f);

        // SkyCore は 6時スタートなので、逆補正する
        dayProgress -= (6f / 24f);

        // マイナスになった時の補正
        if (dayProgress < 0)
            dayProgress += 1f;

        // SkyCore に渡す
        skyCore.timeOfDay = dayProgress;
    }

}
