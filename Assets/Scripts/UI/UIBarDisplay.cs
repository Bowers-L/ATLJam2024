using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIBarDisplay : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI textObj;

    private int _maxValue;

    public void OnValueChange(int value)
    {
        slider.value = value == 0 ? 0 : (float)value / _maxValue;
        if (textObj != null)
        {
            textObj.text = $"{value}";
        }
    }

    public void OnMaxValueChange(int maxValue)
    {
        _maxValue = maxValue;
    }
}