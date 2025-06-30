using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSequence : MonoBehaviour
{
    WinLoseManager winLoseManager;
    WinSequence winSequence;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        winLoseManager = GetComponentInParent<WinLoseManager>();
        winSequence = winLoseManager.winSequence_;
    }

    public IEnumerator NormalCompletionSequence ( 
        List<GameObject> occuppiedSlots , 
        List<winCardData> winningCards , 
        List<GameObject> remainingCards , 
        List<(GameObject card, List<(int col, int row)> Positions)> remainingGoldenCards = null , 
        List<GameObject> remainingBigJokerCards = null ,
        List<GameObject> remainingSuperJokerCards = null ,
        Action OnComplete = null )
    {
        CommandCenter.Instance.effectsManager_.ShowWinEffect(winningCards);
        winLoseManager.ClearWinningCards();
        yield return new WaitForSeconds(.25f);
        winSequence.DeactivateWinBg();

        //return the "wild cards" to the normal slots from win slots

        if (remainingBigJokerCards != null && remainingBigJokerCards.Count > 0)
        {
            for (int i = 0 ; i < remainingBigJokerCards.Count ; i++)
            {
                remainingCards.Add(remainingBigJokerCards [i]);
            }
        }
        Debug.Log($"remaining superJoker {remainingSuperJokerCards.Count}");
        if (remainingSuperJokerCards != null && remainingSuperJokerCards.Count > 0)
        {
            for (int i = 0 ; i < remainingSuperJokerCards.Count ; i++)
            {
                remainingCards.Add(remainingSuperJokerCards [i]);
            }
        }

        yield return StartCoroutine(winSequence.MoveCardsToNormalSlots(remainingCards));
        winSequence.clearWinSlots();
        yield return new WaitForSeconds(.25f);
        //Debug.Log("WinSequence Done!");
        winSequence.SetIsWinSequenceDone(true);
        winLoseManager.EndTheWinSequence(
            remainingGoldenCards , 
            remainingBigJokerCards ,
            remainingSuperJokerCards,
            OnComplete);
        yield return null;
    }
}
