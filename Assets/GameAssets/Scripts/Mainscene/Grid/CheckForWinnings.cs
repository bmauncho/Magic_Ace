using System.Collections;
using UnityEngine;

public class CheckForWinnings : MonoBehaviour
{
    GridManager gridManager;
    private bool isCheckingForWins = false;

    public void checkForWinings ()
    {
        Debug.Log("CheckForWinings!");
        if (isCheckingForWins) return;
        StartCoroutine(checkforWins());
    }

    private IEnumerator checkforWins ()
    {
        isCheckingForWins = true;
        gridManager = CommandCenter.Instance.gridManager_;
        yield return new WaitWhile(() => CommandCenter.Instance.winLoseManager_.IsWinSequencerunning());

        CommandCenter.Instance.spinManager_.SetCanSpin(true);
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

        isCheckingForWins = false;
    }

    IEnumerator Freegame ()
    {
        gridManager.setisNormalWnSequenceDone(true);
        yield return null;
    }

    IEnumerator NormalGame ()
    {
        CommandCenter.Instance.spinManager_.enableButtons();
        CommandCenter.Instance.winLoseManager_.ClearWinningCards();
        CommandCenter.Instance.winLoseManager_.ResetWinType();
        gridManager.setisNormalWnSequenceDone(true);

        if (CommandCenter.Instance.autoSpinManager_.isAutoSpin())
        {
            CommandCenter.Instance.cardManager_.SetWildChance();
            CommandCenter.Instance.featureManager_.ResetFeatures();
            CommandCenter.Instance.featureManager_.GetFeatureBuyMenu().ResetOptions();

            if (CommandCenter.Instance.autoSpinManager_.Spins() > 0)
            {
                CommandCenter.Instance.mainMenuController_.Spin();
                CommandCenter.Instance.autoSpinManager_.ReduceSpins();
            }
            else if (CommandCenter.Instance.autoSpinManager_.Spins() <= 0)
            {
                CommandCenter.Instance.autoSpinManager_.DeactivateAutospin();
                CommandCenter.Instance.mainMenuController_.ToggleAutoSpinMenu();
            }
        }

        yield return null;
    }

}
