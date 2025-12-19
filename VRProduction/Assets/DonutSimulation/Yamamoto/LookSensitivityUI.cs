using UnityEngine;
using UnityEngine.UI;

public class LookSensitivityUI : MonoBehaviour
{
    public Slider horizontalSlider;
    public Slider verticalSlider;
    public Slider overallSlider;
    public Move move;

    void Start()
    {
        float h = PlayerPrefs.GetFloat("LookSensitivity_H", 120f);
        float v = PlayerPrefs.GetFloat("LookSensitivity_V", 60f);
        float mul = PlayerPrefs.GetFloat("LookSensitivity_Mul", 1f);

        horizontalSlider.value = h;
        verticalSlider.value = v;
        overallSlider.value = mul;

        move.SetHorizontalSensitivity(h);
        move.SetVerticalSensitivity(v);
        move.SetLookSensitivityMul(mul);

        horizontalSlider.onValueChanged.AddListener(move.SetHorizontalSensitivity);
        verticalSlider.onValueChanged.AddListener(move.SetVerticalSensitivity);
        overallSlider.onValueChanged.AddListener(move.SetLookSensitivityMul);
    }
}
