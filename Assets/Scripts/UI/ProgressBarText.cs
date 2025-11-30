using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarText : MonoBehaviour
{
    public TMP_Text healthText;
    public ProgressBar bar;
    Slider slider;

    void Start()
    {
        slider = bar.slider;
    }

    void Update()
    {
        healthText.text = slider.value.ToString("F1") + " / " + slider.maxValue.ToString("F1");
    }
}
