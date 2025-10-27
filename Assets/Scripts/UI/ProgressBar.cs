using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Gradient gradient;
    public float lerpRate = 10;
    public bool lerpValue = true;

    float currentValue, targetValue;

    void Start()
    {
        currentValue = 0;
    }

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
        SetValue(value);
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
    }

    void Update()
    {
        if (lerpValue)
        {
            if (currentValue != targetValue)
            {
                currentValue = Mathf.Lerp(currentValue, targetValue, lerpRate * Time.deltaTime);

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
