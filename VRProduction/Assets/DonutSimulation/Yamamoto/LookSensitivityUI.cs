using UnityEngine;
using UnityEngine.UI;

public class LookSensitivityUI : MonoBehaviour
{
    public Slider horizontalSlider;
    public Slider verticalSlider;
    public Slider overallSlider;

    public Text horizontalText;
    public Text verticalText;
    public Text overallText;

    public Move move;

    void Start()
    {
        float h = PlayerPrefs.GetFloat("LookSensitivity_H", 120f);
        float v = PlayerPrefs.GetFloat("LookSensitivity_V", 60f);
        float mul = PlayerPrefs.GetFloat("LookSensitivity_Mul", 1f);

        horizontalSlider.value = h;
        verticalSlider.value = v;
        overallSlider.value = mul;

        ApplyHorizontal(h);
        ApplyVertical(v);
        ApplyOverall(mul);

        horizontalSlider.onValueChanged.AddListener(ApplyHorizontal);
        verticalSlider.onValueChanged.AddListener(ApplyVertical);
        overallSlider.onValueChanged.AddListener(ApplyOverall);
    }

    // ===== 横感度 =====
    void ApplyHorizontal(float value)
    {
        move.SetHorizontalSensitivity(value);
        horizontalText.text = $"横視点感度 : {value:F0}";
    }

    // ===== 縦感度 =====
    void ApplyVertical(float value)
    {
        move.SetVerticalSensitivity(value);
        verticalText.text = $"縦視点感度 : {value:F0}";
    }

    // ===== 全体倍率 =====
    void ApplyOverall(float value)
    {
        move.SetLookSensitivityMul(value);
        overallText.text = $"感度倍率 : {value:F1}";
    }
}
