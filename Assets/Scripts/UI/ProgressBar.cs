using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Gradient gradient;
    public float lerpRate = 10;
    public bool lerpValue = true;
    
    [Header("Optional - Player health / xp")]
    public TMP_Text fillText;

    float currentValue, targetValue;

    void Start()
    {
        currentValue = 0;
    }

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
    }

    public void SetValue(float value)
    {
        targetValue = value;

        if (!lerpValue)
        {
            SetBar(value);
        }
    }

    void SetBar(float value)
    {
        slider.value = value;
        fill.color = gradient.Evaluate(value / slider.maxValue);

        if (fillText != null)
            fillText.text = slider.value.ToString("F1") + " / " + slider.maxValue.ToString("F1");
    }

    void Update()
    {
        if (lerpValue)
        {
            if (currentValue != targetValue)
            {
                currentValue = Mathf.Lerp(currentValue, targetValue, lerpRate * Time.unscaledDeltaTime);

                SetBar(currentValue);

                if (Mathf.Abs(currentValue - targetValue) < 0.1)
                {
                    currentValue = targetValue;
                    SetBar(targetValue);
                }
            }
        }
    }
}
