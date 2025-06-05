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

    public IEnumerator NormalWildCompletionSequence ( List<winCardData> winningCards )
    {
        yield return new WaitForSeconds(.25f);
        winSequence.DeactivateWinBg();

        yield return StartCoroutine(winLoseManager.refillGrid.RefillTheGrid());

        yield return new WaitWhile(() => CommandCenter.Instance.gridManager_.IsGridRefilling());

        yield return null;
    }
}
