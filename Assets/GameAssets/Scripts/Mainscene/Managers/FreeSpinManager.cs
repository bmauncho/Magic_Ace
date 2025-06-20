using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeSpinManager : MonoBehaviour
{
    [Header("Free Spin Variables")]
    [SerializeField] private int freeSpinCount;
    [SerializeField] private int FreeSpins;
    [SerializeField] private int scatterCards;
    [SerializeField] private int retriggerScatterCards;
    [SerializeField] private bool isFreeGameWin;
    [SerializeField] private bool isFreeGame;
    [SerializeField] private bool isFreeSpinRetrigger;
    [SerializeField] private bool isFreeSpinGameStarted;
    [SerializeField] private bool isFreeSpinDone;
    [Header("Free spin references")]
    public FreeSpinIntro freeSpinIntro;
    public FreeSpinRetrigger freeSpinRetrigger;
    public FreeSpinTotalWin freeSpinTotalWin;
    public FreeSpinUI freeSpinUI;

    public Action OnFreeSpinIntroComplete;
    public Action OnFreeSpinRetriggerComplete;
    public Action OnFreeSpinComplete;

    [ContextMenu("Show FreeSpinIntro")]
    public void ShowFreeSpinIntro ()
    {
        isFreeGameWin = true;
        isFreeGame = true;
        CommandCenter.Instance.multiplierManager_.SetMultiplierType(MultiplierType.Free);
        StartCoroutine(FreeSpinIntro());
    }

    private IEnumerator FreeSpinIntro ()
    {
        yield return StartCoroutine(freeSpinIntro.ShowFreeSpinIntro());
        OnFreeSpinIntroComplete?.Invoke();
    }

    [ContextMenu("Show FreeSpinRetrigger")]
    public void ShowFreeSpinRetrigger ()
    {
        Debug.Log("retrigger free spin called!");
        isFreeSpinRetrigger = true;

        StartCoroutine(FreeSpinRetrigger());
    }

    private IEnumerator FreeSpinRetrigger ()
    {
        freeSpinRetrigger.animator_.Rebind();
        freeSpinRetrigger.gameObject.SetActive(true);
        CanvasGroup canvasGroup = freeSpinRetrigger.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        Tween myTween = canvasGroup.DOFade(1 , 0.5f);
        yield return StartCoroutine(freeSpinRetrigger.retriggerFreeSpin());
        Debug.Log("Free trigger complete!");
        OnFreeSpinRetriggerComplete?.Invoke();
    }

    public void DeactivateFreeSpin (Action OnComplete)
    {
        StartCoroutine(freeSpinRetrigger.Deactivate(OnComplete));
    }

    [ContextMenu("Show FreeSpin Totalwin")]
    public void ShowFreeSpinTotalWin ()
    {
        StartCoroutine(FreeSpinTotalWin());
    }

    private IEnumerator FreeSpinTotalWin ()
    {
        freeSpinTotalWin.gameObject.SetActive(true);
        yield return StartCoroutine (freeSpinTotalWin.showTotalWin());
        freeSpinTotalWin.gameObject.SetActive(false);
        OnFreeSpinComplete?.Invoke();
    }

    public void ShowStartBtn ()
    {
        freeSpinIntro.StartBtn.SetActive(true);
    }

    public void HideStartBtn ()
    {
        freeSpinIntro.StartBtn.SetActive(false);
    }

    public void StartFreeGame ()
    {
        freeSpinIntro.DeactivateFreeIntro();
        CommandCenter.Instance.uiManager_.ShowFreeGameUI();
        CommandCenter.Instance.spinManager_.SetCanSpin(true);
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart ()
    {
        yield return new WaitForSeconds(1f);
        CommandCenter.Instance.currencyManager_.ResetWinAmount();
        CommandCenter.Instance.spinManager_.Spin();
    }

    public void SetFreeSpins (int value)
    {
        freeSpinCount += value;
        FreeSpins += value;
        freeSpinUI.SetTotalSpins(freeSpinCount);
    }

    public void UpdateFreeSpins ()
    {
        if (FreeSpins > 0)
        {
            FreeSpins--;
        }
        freeSpinUI.UpdateFreeSpinCount(FreeSpins);
    }

    public bool IsFreeGame ()
    {
        return isFreeGame;
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
   
    public bool IsFreeSpinDone ()
    {
        if (FreeSpins <= 0)
        {
            isFreeSpinDone = true;
        }
        else
        {
            isFreeSpinDone = false;
        }
        return isFreeSpinDone;
    }
   
    public void SetIsFreeGameStarted ( bool value )
    {
        isFreeSpinGameStarted = value;
    }

    public void SetIsFreeSpinRetrigger ( bool value )
    {
        isFreeSpinRetrigger = value;
    }

    public void SetIsFreeGameWin ( bool value )
    {
        isFreeGameWin = value;
    }

    public void SetIsFreeSpinDone ( bool value )
    {
        isFreeSpinDone = value;
    }
    public void SetIsFreeGame ( bool value )
    {
        isFreeGame = value;
    }
}
