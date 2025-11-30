using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    public TMP_Text text;

    public void SetValue(float damage, Color color)
    {
        text.text = damage.ToString("F1");
        text.color = color;
    }

    public void DisableSelf()
    {
        DamageIndicatorManager.Instance.ReturnIndicator(gameObject);
    }

    void OnDisable()
    {
        GetComponent<Animator>().speed = 1;
        transform.localScale = Vector3.one;
    }
}
