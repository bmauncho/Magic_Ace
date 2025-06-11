using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildSequnce : MonoBehaviour
{
    WinLoseManager winLoseManager;
    WinSequence winSequence;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        winLoseManager = GetComponentInParent<WinLoseManager>();
        winSequence = winLoseManager.winSequence_;
    }

    public IEnumerator WildCardsSequnce ( 
        List<GameObject> wildCards , 
        List<GameObject> occuppiedSlots ,
        List<winCardData> winningCards , 
        List<GameObject> remainingCards , 
        List<(GameObject card, List<(int col, int row)> Positions)> remainingGoldenCards = null ,
        List<GameObject> remainingBigJokerCards = null , Action OnComplete = null )
    {
        //play wild card effect
        //show FreeSpin intro
        //Activate freeSpin

        //int activeCoroutines = wildCards.Count;
        //if (wildCards.Count >= 3)
        //{
        //    for (int i = 0 ; i < wildCards.Count ; i++)
        //    {
        //        wildCards [i].GetComponent<Card>().OnWildAnimComplete += () => activeCoroutines--;
        //    }
        //}


        //yield return new WaitUntil(() => activeCoroutines <= 0);

        //show free spin intro
        FreeSpinManager freeSpinManager = CommandCenter.Instance.freeSpinManager_;
        if(CommandCenter.Instance.gameType != GameType.Free)
        {
            CommandCenter.Instance.cardManager_.SetWildChance(0.005f);
            bool isFreeSpinIntroComplete = false;
            freeSpinManager.ShowFreeSpinIntro();
            freeSpinManager.OnFreeSpinIntroComplete += () => isFreeSpinIntroComplete = true;
            yield return new WaitUntil(() => isFreeSpinIntroComplete);
        }
        else
        {
            //show Retrigger
            bool isFreeSpinRetriggerComplete = false;
            freeSpinManager.ShowFreeSpinRetrigger();
            freeSpinManager.OnFreeSpinRetriggerComplete += () => isFreeSpinRetriggerComplete = true;
            yield return new WaitUntil(() => isFreeSpinRetriggerComplete);
        }
        //activate free spin
        StartCoroutine(WildCompletionSequence(occuppiedSlots , winningCards , remainingCards , remainingGoldenCards , remainingBigJokerCards));
        yield return null;
    }

    private IEnumerator WildCompletionSequence ( 
        List<GameObject> occuppiedSlots , List<winCardData> winningCards ,
        List<GameObject> remainingCards ,
        List<(GameObject card, List<(int col, int row)> Positions)> remainingGoldenCards = null ,
        List<GameObject> remainingBigJokerCards = null , Action OnComplete = null )
    {
        winLoseManager.ClearWinningCards();
        yield return new WaitForSeconds(.25f);
        winSequence.DeactivateWinBg();

        if (remainingBigJokerCards != null && remainingBigJokerCards.Count > 0)
        {
            for (int i = 0 ; i < remainingBigJokerCards.Count ; i++)
            {
                remainingCards.Add(remainingBigJokerCards [i]);
            }
        }

        //return the "wild cards" to the normal slots from win slots
        yield return StartCoroutine(winSequence.MoveCardsToNormalSlots(remainingCards));
        winSequence.clearWinSlots();
        yield return new WaitForSeconds(.25f);

        Debug.Log("WinSequence Done!");
        winSequence.SetIsWinSequence(true);

        winLoseManager.EndTheWinSequence(remainingGoldenCards , remainingBigJokerCards , OnComplete);
        yield return null;
    }

    public IEnumerator NormalWildCompletionSequence ( List<winCardData> winningCards )
    {
        yield return new WaitForSeconds(.25f);
        winSequence.DeactivateWinBg();

        yield return StartCoroutine(winLoseManager.refillGrid.RefillTheGrid());

        yield return new WaitWhile(() => CommandCenter.Instance.gridManager_.IsGridRefilling());

        yield return null;
    }
}
