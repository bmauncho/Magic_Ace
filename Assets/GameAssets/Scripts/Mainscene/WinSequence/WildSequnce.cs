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
        winSequence = GetComponentInParent<WinSequence>();
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
        //Debug.Log("Play Wild Card Effect");
        int activeCoroutines = wildCards.Count;
        if (wildCards.Count >= 3)
        {
            for (int i = 0 ; i < wildCards.Count ; i++)
            {
                wildCards [i].GetComponent<Card>().OnWildAnimComplete += () => activeCoroutines--;
            }
        }


        yield return new WaitUntil(() => activeCoroutines <= 0);
        // Debug.Log("Show Free Spin Intro");
        //show free spin intro
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
}
