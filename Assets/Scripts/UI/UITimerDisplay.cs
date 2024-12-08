using TMPro;
using UnityEditor;
using UnityEngine;

public class UITimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textObj;

    public void UpdateTimerDisplay(float value)
    {
        textObj.text = $"{value:F2}";
    }
}