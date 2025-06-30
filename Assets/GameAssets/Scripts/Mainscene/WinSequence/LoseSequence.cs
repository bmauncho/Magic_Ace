using System;
using System.Collections;
using UnityEngine;

public class LoseSequence : MonoBehaviour
{
    GridManager gridManager;
    WinSequence WinSequence;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridManager = CommandCenter.Instance.gridManager_;
    }

    public IEnumerator loseSequence ( Action OnComplete = null )
    {
        Debug.Log("Handle Lose");
        gridManager.SetIsRefilling(false);
        // Debug.Log("Show Win-1");
        if (CommandCenter.Instance.currencyManager_.GetWinAmount() > 0)
        {
            if (CommandCenter.Instance.comboManager_.GetComboCounter() >= 3)
            {
                //Debug.Log("Show Win-2");
                Debug.Log("Show win amount");
                Debug.Log("Wait for Win Amount animation to complete");
            }

        }

        OnComplete?.Invoke();
        CommandCenter.Instance.gridManager_.checkForWinings();
        yield return null;
    }
}
