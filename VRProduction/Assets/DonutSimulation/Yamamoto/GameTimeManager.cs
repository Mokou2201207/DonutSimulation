using UnityEngine;
using TMPro;

public class GameTimeManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI timeText;

    [Header("Time Settings")]
    public int hour = 6;   // ★6時スタート！
    public int minute = 0;
    public float timeSpeed = 60f;

    private float timer = 0f;

    [Header("Game Flow")]
    public bool timeStart = false;

    [Header("Lighting")]
    public Light mainLight;
    public float dayLightIntensity = 1f;
    public float nightLightIntensity = 0.2f;
    public float transitionSpeed = 1f;

    [Header("Sky System")]
    public Ultrabolt.SkyEngine.SkyCore skyCore;

    void Update()
    {
        UpdateUI();
        UpdateLighting();

        if (timeStart)
            UpdateTime();

        UpdateSky();
    }

    //==============================
    // 時間進行（6時スタート & 21時で停止）
    //==============================
    void UpdateTime()
    {
        if (hour >= 21)
        {
            timeStart = false;
            return;
        }

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
    }

    //==============================
    // UI
    //==============================
    void UpdateUI()
    {
        if (timeText != null)
            timeText.text = $"{hour:00}:{minute:00}";
    }

    //==============================
    // ライト
    //==============================
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
    // SkyEngine の時間補正
    //==============================
    void UpdateSky()
    {
        if (skyCore == null) return;

        // ★ 0:00 → 0
        // ★ 6:00 → 0（SkyCore内部の朝）
        // ★ 21:00 でストップ
        float totalMinutes = (hour * 60 + minute);

        // 1日の進行度（0〜1）
        float dayProgress = totalMinutes / (24f * 60f);

        // ★SkyCore の 0 = 6:00 → 補正
        dayProgress -= (6f / 24f);

        if (dayProgress < 0)
            dayProgress += 1f;

        // SkyCoreに反映
        skyCore.timeOfDay = dayProgress;
    }
}
