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
        List<GameObject> remainingBigJokerCards = null ,
        List<GameObject> remainingSuperJokerCards = null ,
        Action OnComplete = null )
    {
        Debug.Log("Wild Sequence Started!");
        //play wild card effect
        //show FreeSpin intro
        //Activate freeSpin

        int activeCoroutines = wildCards.Count;
        if (wildCards.Count >= 3)
        {
            for (int i = 0 ; i < wildCards.Count ; i++)
            {
                wildCards [i].GetComponent<Card>().showWildAnim();
                wildCards [i].GetComponent<Card>().OnWildAnimComplete += () => activeCoroutines--;
            }
        }

        CommandCenter.Instance.soundManager_.PlayAmbientSound("Bonus_Alarm");
        yield return new WaitUntil(() => activeCoroutines <= 0);

        //show free spin intro
        FreeSpinManager freeSpinManager = CommandCenter.Instance.freeSpinManager_;
        if(CommandCenter.Instance.gameType != GameType.Free)
        {
            CommandCenter.Instance.cardManager_.SetWildChance(0.005f);
            bool isFreeSpinIntroComplete = false;
            freeSpinManager.OnFreeSpinIntroComplete += () => isFreeSpinIntroComplete = true;
            freeSpinManager.ShowFreeSpinIntro();
            CommandCenter.Instance.soundManager_.PlayAmbientSound("Free_TS");
            yield return new WaitUntil(() => isFreeSpinIntroComplete);
            yield return new WaitForSeconds(3f); // Wait for intro animation to finish
            CommandCenter.Instance.soundManager_.PlayAmbientSound("Free_BG");
            CommandCenter.Instance.freeSpinManager_.SetFreeSpins(10);
            CommandCenter.Instance.SetGameType(GameType.Free);
        }
        else
        {
            if(!CommandCenter.Instance.freeSpinManager_.IsFreeSpinRetrigger())
            {
                //show Retrigger
                bool isDone = false;
                bool isFreeSpinRetriggerComplete = false;
                freeSpinManager.OnFreeSpinRetriggerComplete += () => isFreeSpinRetriggerComplete = true;
                freeSpinManager.ShowFreeSpinRetrigger();
                yield return new WaitUntil(() => isFreeSpinRetriggerComplete);
                CommandCenter.Instance.freeSpinManager_.SetFreeSpins(5);
                CommandCenter.Instance.freeSpinManager_.UpdateFreeSpins();
                Debug.Log("Should hide retrigger UI here");
            }
        }
        //activate free spin
        StartCoroutine(WildCompletionSequence(
            occuppiedSlots , 
            winningCards , 
            remainingCards , 
            remainingGoldenCards , 
            remainingBigJokerCards,
            remainingSuperJokerCards));

        yield return null;
    }

    private IEnumerator WildCompletionSequence ( 
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

        //return the "wild cards" to the normal slots from win slots
        yield return StartCoroutine(winSequence.MoveCardsToNormalSlots(remainingCards));
        winSequence.clearWinSlots();
        yield return new WaitForSeconds(.25f);

        Debug.Log("WinSequence Done!");
        winSequence.SetIsWinSequenceDone(true);

        winLoseManager.EndTheWinSequence(
            remainingGoldenCards , 
            remainingBigJokerCards ,
            remainingSuperJokerCards ,
            OnComplete);
        yield return null;
    }

    public IEnumerator NormalWildCompletionSequence ( List<winCardData> winningCards )
    {
        CommandCenter.Instance.effectsManager_.ShowWinEffect(winningCards);
        yield return new WaitForSeconds(.25f);
        winSequence.DeactivateWinBg();

        yield return StartCoroutine(winLoseManager.refillGrid.RefillTheGrid());

        yield return new WaitWhile(() => CommandCenter.Instance.gridManager_.IsGridRefilling());

        yield return null;
    }

    public IEnumerator BothWildSequence ( 
        List<GameObject> wildCards,
        List<GameObject> remainingCards )
    {
        winSequence.ActivateWinBg();
        Debug.Log("Wild Sequence Started!");
        //play wild card effect
        //show FreeSpin intro
        //Activate freeSpin

        int activeCoroutines = wildCards.Count;
        if (wildCards.Count >= 3)
        {
            for (int i = 0 ; i < wildCards.Count ; i++)
            {
                wildCards [i].GetComponent<Card>().showWildAnim();
                wildCards [i].GetComponent<Card>().OnWildAnimComplete += () => activeCoroutines--;
            }
        }

        CommandCenter.Instance.soundManager_.PlayAmbientSound("Bonus_Alarm");
        yield return new WaitUntil(() => activeCoroutines <= 0);

        FreeSpinManager freeSpinManager = CommandCenter.Instance.freeSpinManager_;
        if (CommandCenter.Instance.gameType != GameType.Free)
        {
            CommandCenter.Instance.cardManager_.SetWildChance(0.005f);
            bool isFreeSpinIntroComplete = false;
            freeSpinManager.ShowFreeSpinIntro();

            freeSpinManager.OnFreeSpinIntroComplete += () => isFreeSpinIntroComplete = true;
            CommandCenter.Instance.soundManager_.PlayAmbientSound("Free_TS");
            yield return new WaitUntil(() => isFreeSpinIntroComplete);
            yield return new WaitForSeconds(3f); // Wait for intro animation to finish
            CommandCenter.Instance.soundManager_.PlayAmbientSound("Free_BG");
            CommandCenter.Instance.freeSpinManager_.SetFreeSpins(10);
            CommandCenter.Instance.SetGameType(GameType.Free);
        }
        else
        {
            if (!CommandCenter.Instance.freeSpinManager_.IsFreeSpinRetrigger())
            {
                //show Retrigger
                bool isDone = false;
                bool isFreeSpinRetriggerComplete = false;
                freeSpinManager.OnFreeSpinRetriggerComplete += () => isFreeSpinRetriggerComplete = true;
                freeSpinManager.ShowFreeSpinRetrigger();
                yield return new WaitUntil(() => isFreeSpinRetriggerComplete);
                CommandCenter.Instance.freeSpinManager_.SetFreeSpins(5);
                CommandCenter.Instance.freeSpinManager_.UpdateFreeSpins();
                Debug.Log("Should hide retrigger UI here");
            }
        }

        winLoseManager.ClearWinningCards();
        yield return new WaitForSeconds(.25f);
        winSequence.DeactivateWinBg();
        //return the "wild cards" to the normal slots from win slots
        yield return StartCoroutine(winSequence.MoveCardsToNormalSlots(remainingCards));
        winSequence.clearWinSlots();
        yield return new WaitForSeconds(.25f);

        Debug.Log("WinSequence Done!");
        winSequence.SetIsWinSequenceDone(true);

        yield return null;
    }
}
