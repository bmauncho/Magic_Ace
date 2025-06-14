using System.Collections;
using UnityEngine;

public class CheckForWinnings : MonoBehaviour
{
    GridManager gridManager;
    [SerializeField] private bool isCheckingForWins = false;
    [SerializeField] private bool freeSpinRetriggerComplete = false;
    [SerializeField] private bool isFreeSpinComplete = false;

    private void OnFreeSpinsComnplete ()
    {
        isFreeSpinComplete = true;
    }
    private void OnRetriggerComplete ()
    {
        freeSpinRetriggerComplete = true;
    }

    private void OnFreeSpinsReset ()
    {
        freeSpinRetriggerComplete = false;
        isFreeSpinComplete = false;
    }

    private void OnRetriggerReset ()
    {
        freeSpinRetriggerComplete = false;
    }

    public void checkForWinings ()
    {
        Debug.Log("CheckForWinings!");
        Debug.Log("Is Check For Wins : " + isCheckingForWins);
        if (isCheckingForWins) return;
        StartCoroutine(checkforWins());
    }

    private IEnumerator checkforWins ()
    {
        isCheckingForWins = true;
        gridManager = CommandCenter.Instance.gridManager_;
        Debug.Log("is win sequence running : " + CommandCenter.Instance.winLoseManager_.IsWinSequencerunning());
        yield return new WaitWhile(() => CommandCenter.Instance.winLoseManager_.IsWinSequencerunning());

        gridManager.SetIsRefillSequenceDone(true);
        gridManager.setIsCascading(false);

        if (CommandCenter.Instance.gameType == GameType.Free )
        {
            Debug.Log("free GameCheck!");
            yield return StartCoroutine(Freegame());
        }
        else
        {
            Debug.Log("Normal GameCheck!");
            yield return StartCoroutine(NormalGame());
        }
    }

    IEnumerator Freegame ()
    {
        gridManager.setisNormalWnSequenceDone(true);

        if (CommandCenter.Instance.freeSpinManager_.IsFreeGameWin())
        {
            Debug.Log("Free game win");
            CommandCenter.Instance.spinManager_.SetCanSpin(true);
            CommandCenter.Instance.freeSpinManager_.ShowStartBtn();
            CommandCenter.Instance.featureManager_.GetFeatureBuyMenu().ResetOptions();
            CommandCenter.Instance.freeSpinManager_.SetIsFreeGameWin(false);
            CommandCenter.Instance.comboManager_.HideCombo();
            isCheckingForWins = false;
            yield break;
        }
        else
        {
            if (CommandCenter.Instance.freeSpinManager_.IsFreeSpinRetrigger())
            {
                //show retrigger UI
                CommandCenter.Instance.freeSpinManager_.ShowFreeSpinRetrigger();
                CommandCenter.Instance.freeSpinManager_.OnFreeSpinRetriggerComplete += OnRetriggerComplete;
                yield return new WaitWhile(() => !freeSpinRetriggerComplete);
                CommandCenter.Instance.freeSpinManager_.OnFreeSpinRetriggerComplete -= OnRetriggerReset;
                CommandCenter.Instance.freeSpinManager_.SetIsFreeSpinRetrigger(false);
                CommandCenter.Instance.spinManager_.SetCanSpin(true);
                CommandCenter.Instance.comboManager_.HideCombo();
                isCheckingForWins = false;
                yield break;
            }
            else
            {
                WinLoseManager winLoseManager = CommandCenter.Instance.winLoseManager_;
                winLoseManager.ClearWinningCards();
                bool isWin = winLoseManager.IsPlayerWin();
                if (isWin)
                {
                    //Debug.Log("Recheck win - free Game");
                    isCheckingForWins = false;
                    yield return StartCoroutine(winLoseManager.RecheckWin());
                }
                else
                {
                    //Debug.Log("Recheck Lose!-free Game");
                    if (CommandCenter.Instance.freeSpinManager_.IsFreeSpinDone())
                    {
                        //show free spin end UI
                        // if win show win ui 
                        // if lose go directly to normal game

                        CommandCenter.Instance.freeSpinManager_.ShowFreeSpinTotalWin();
                        CommandCenter.Instance.freeSpinManager_.OnFreeSpinComplete += OnFreeSpinsComnplete;
                        yield return new WaitWhile(() => !isFreeSpinComplete);
                        CommandCenter.Instance.freeSpinManager_.OnFreeSpinComplete -= OnFreeSpinsReset;
                        Debug.Log("Free Spin Complete!");
                        CommandCenter.Instance.uiManager_.ShowNormalGameUI();
                        CommandCenter.Instance.comboManager_.HideCombo();
                        isCheckingForWins = false;
                        CommandCenter.Instance.SetGameType(GameType.Base);
                        CommandCenter.Instance.multiplierManager_.SetMultiplierType(MultiplierType.Normal);
                        CommandCenter.Instance.freeSpinManager_.SetIsFreeSpinDone(true);
                        CommandCenter.Instance.freeSpinManager_.SetIsFreeGame(false);
                        CommandCenter.Instance.spinManager_.SetCanSpin(true);
                        CommandCenter.Instance.freeSpinManager_.freeSpinUI.ResetFreeSpinCount();
                        CommandCenter.Instance.spinManager_.enableButtons();
                        CommandCenter.Instance.multiplierManager_.ResetMultiplier();
                        if (CommandCenter.Instance.autoSpinManager_.isAutoSpin())
                        {
                            CommandCenter.Instance.winLoseManager_.ClearWinningCards();
                            CommandCenter.Instance.winLoseManager_.ResetWinType();
                            yield return new WaitForSeconds(0.5f);
                            CommandCenter.Instance.mainMenuController_.Spin();
                        }

                    }
                    else
                    {
                        //move  to next free spin
                        CommandCenter.Instance.spinManager_.SetCanSpin(true);
                        CommandCenter.Instance.spinManager_.enableButtons();
                        CommandCenter.Instance.winLoseManager_.ClearWinningCards();
                        CommandCenter.Instance.winLoseManager_.ResetWinType();
                        isCheckingForWins = false;
                        CommandCenter.Instance.comboManager_.HideCombo();
                        CommandCenter.Instance.multiplierManager_.ResetMultiplier();
                        yield return new WaitForSeconds(0.5f);
                        CommandCenter.Instance.spinManager_.Spin();
                        yield break;
                    }

                }
            }

        }
        yield return null;
    }

    IEnumerator NormalGame ()
    {
        CommandCenter.Instance.spinManager_.enableButtons();
        CommandCenter.Instance.winLoseManager_.ClearWinningCards();
        CommandCenter.Instance.winLoseManager_.ResetWinType();
        CommandCenter.Instance.comboManager_.HideCombo();
        CommandCenter.Instance.multiplierManager_.ResetMultiplier();
        gridManager.setisNormalWnSequenceDone(true);

        if (CommandCenter.Instance.autoSpinManager_.isAutoSpin())
        {
           yield return StartCoroutine(Autospin());
        }
        else
        {
            CommandCenter.Instance.spinManager_.SetCanSpin(true);
        }
        isCheckingForWins = false;
        yield return null;
    }

    IEnumerator Autospin ()
    {
        CommandCenter.Instance.cardManager_.SetWildChance();
        CommandCenter.Instance.featureManager_.ResetFeatures();
        CommandCenter.Instance.featureManager_.GetFeatureBuyMenu().ResetOptions();
        CommandCenter.Instance.spinManager_.SetCanSpin(true);

        if (CommandCenter.Instance.autoSpinManager_.Spins() > 0)
        {
            CommandCenter.Instance.mainMenuController_.Spin();
        }
        else if (CommandCenter.Instance.autoSpinManager_.Spins() <= 0)
        {
            CommandCenter.Instance.autoSpinManager_.DeactivateAutospin();
            yield return new WaitForSeconds(0.5f);
            CommandCenter.Instance.mainMenuController_.ToggleAutoSpinMenu();
        }
        yield return null;
    }

}
