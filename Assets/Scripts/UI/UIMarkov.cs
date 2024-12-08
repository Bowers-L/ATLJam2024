using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG;
using DG.Tweening;

public class UIMarkov : MonoBehaviour
{
    [SerializeField] private Image[] stateIcons;
    [SerializeField] private LineRenderer[] stateTransitions;   //0 - 1 to 2, 1 - 0 to 2, 2 - 0 to 1
    [SerializeField] private float bigScale = 2f;
    [SerializeField] private float smallScale = 0.5f;
    [SerializeField] private float maxLineWidth = 0.2f;
    [SerializeField] private float minLineWidth = 0.05f;
    [SerializeField] private float animLength = 0.5f;
   

    public void OnStateChanged(MarkovState state)
    {
        for (int i = 0; i < stateIcons.Length; i++)
        {
            if (i == (int) state)
            {
                stateIcons[i].transform.DOScale(Vector3.one * bigScale, animLength);
                stateTransitions[i].enabled = false;
            } else
            {
                stateIcons[i].transform.DOScale(Vector3.one * smallScale, animLength);
                stateTransitions[i].enabled = true;
                stateTransitions[i].startWidth = maxLineWidth;
                stateTransitions[i].endWidth = minLineWidth;
            }
        }
    }
}
