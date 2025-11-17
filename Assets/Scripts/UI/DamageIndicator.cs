using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    public TMP_Text text;

    public void SetValue(float damage)
    {
        text.text = damage.ToString("F0");
    }

    public void DisableSelf()
    {
        DamageIndicatorManager.Instance.ReturnIndicator(gameObject);
    }
}
