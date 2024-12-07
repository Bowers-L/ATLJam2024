using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] private bool setSelectedOnOpen = true;
    [SerializeField] private GameObject firstSelected;
    protected bool isOffScreen = false;
    protected CanvasGroup canvas;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    public void Close(System.Action callback = null)
    {
        isOffScreen = true;
        Tweener fadeOut = canvas.DOFade(0.0f, 0.75f);
        fadeOut.OnComplete(() =>
        {
            callback?.Invoke();
            gameObject.SetActive(false);
        });
    }

    public void Open(System.Action callback = null)
    {
        Debug.Log("OPENING MODE SCREEN");
        gameObject.SetActive(true);
        if (canvas == null)
        {
            canvas = GetComponent<CanvasGroup>();
        }

        //gameObject.GetComponent<Button>().Select();
        canvas.alpha = 0;
        Tweener fadeIn = canvas.DOFade(1.0f, 0.75f);
        fadeIn.OnComplete(() =>
        {
            isOffScreen = false;
            if (setSelectedOnOpen)
            {
                EventSystem.current.SetSelectedGameObject(firstSelected);
            }
            callback?.Invoke();
        });
    }
}
