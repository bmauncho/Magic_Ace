using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeSpinManager : MonoBehaviour
{
    [SerializeField] private int FreeSpins;
    [SerializeField] private int scatterCards;
    [SerializeField] private int retriggeScatterCards;
    [SerializeField] private bool isFreeGameWin;
    [SerializeField] private bool isFreeGame;
    [SerializeField] private bool isFreeSpinRetrigger;
    [SerializeField] private bool isFreeSpinGameStarted;
    [Header("Free spin references")]
    public FreeSpinIntro freeSpinIntro;
    public FreeSpinRetrigger freeSpinRetrigger;
    public FreeSpinTotalWin freeSpinTotalWin;

    public Action OnFreeSpinIntroComplete;
    public Action OnFreeSpinRetriggerComplete;

    [ContextMenu("Show FreeSpinIntro")]
    public void ShowFreeSpinIntro ()
    {
        isFreeGameWin = true;
        isFreeGame = true;

        StartCoroutine(FreeSpinIntro());
    }

    private IEnumerator FreeSpinIntro ()
    {

        CanvasGroup canvasGroup = freeSpinIntro.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        Tween myTween = canvasGroup.DOFade(1 , 0.5f);
        yield return new WaitForSeconds(0.25f);
        //
        OnFreeSpinIntroComplete?.Invoke();
    }

    [ContextMenu("Show FreeSpinRetrigger")]
    public void ShowFreeSpeinRetrigger ()
    {
        isFreeSpinRetrigger = true;

        StartCoroutine(FreeSpinRetrigger());
    }

    private IEnumerator FreeSpinRetrigger ()
    {
        CanvasGroup canvasGroup = freeSpinRetrigger.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        Tween myTween = canvasGroup.DOFade(1 , 0.5f);
        yield return new WaitForSeconds(0.25f);
        //
        OnFreeSpinIntroComplete?.Invoke();
    }

    [ContextMenu("Show FreeSpin Totalwin")]
    public void ShowFreeSpinTotalWin ()
    {
        StartCoroutine(FreeSpinTotalWin());
    }

    private IEnumerator FreeSpinTotalWin ()
    {
        CanvasGroup canvasGroup = freeSpinRetrigger.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        Tween myTween = canvasGroup.DOFade(1 , 0.5f);
        yield return new WaitForSeconds(0.25f);
        //
        OnFreeSpinIntroComplete?.Invoke();
    }

    public bool IsFreeSpinGameStarted ()
    {
        return isFreeSpinGameStarted;
    }

    public bool IsFreeSpinRetrigger ()
    {
        return isFreeSpinRetrigger;
    }

    public bool IsFreeGameWin ()
    {
        return isFreeGameWin;
    }

}
