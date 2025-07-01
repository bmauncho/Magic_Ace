using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAmount : MonoBehaviour
{
    APIManager apiManager;
    private void Start ()
    {
        apiManager = CommandCenter.Instance.apiManager_;
    }
    public IEnumerator winAmountEffect ( List<winCardData> WinningCards )
    {
        PayOutManager payOutManager = CommandCenter.Instance.payOutManager_;
        CurrencyManager currencyManager = CommandCenter.Instance.currencyManager_;
        yield return new WaitForSeconds(.25f);
        //play win amount effect
        // Debug.Log("Play Win Amount Effect");
        bool isDemo = CommandCenter.Instance.gameMode == GameMode.Demo;
        bool canShowFeature = CommandCenter.Instance.apiManager_.gameDataApi.CanShowFeature();
        if (isDemo)
        {
            double totalPayOut = payOutManager.GetTotalPayOut(WinningCards);
            // Debug.Log("Total PayOut: " + totalPayOut);
            CommandCenter.Instance.currencyManager_.UpdateWinAmount(totalPayOut);
            double hintTotalWinAmount = currencyManager.GetWinAmount();
            payOutManager.GetPayOut().ShowWin(totalPayOut);
            CommandCenter.Instance.hintsManager_.hintWinUI.ShowHintWinUIAmount(hintTotalWinAmount);
        }
        else
        {
            if (canShowFeature)
            {
                double totalPayOut = payOutManager.GetTotalPayOut(WinningCards);
                //Debug.Log("Total PayOut: " + totalPayOut);
                CommandCenter.Instance.currencyManager_.UpdateWinAmount(totalPayOut);
                double hintTotalWinAmount = currencyManager.GetWinAmount();
                payOutManager.GetPayOut().ShowWin(totalPayOut);
                CommandCenter.Instance.hintsManager_.hintWinUI.ShowHintWinUIAmount(hintTotalWinAmount);
            }
            else
            {
                double totalPayOut = 0;
                bool isRefillingDone = CommandCenter.Instance.gridManager_.IsRefillSequnceDone();
                if (!isRefillingDone && apiManager.refillApi.RefillResponse().gameState.cascading)
                {
                    totalPayOut = apiManager.refillApi.RefillResponse().gameState.totalWin;
                }
                else
                {
                    totalPayOut = apiManager.gameDataApi.apiResponse.gameState.totalWin;
                }
                //Debug.Log("Total PayOut: " + totalPayOut);
                CommandCenter.Instance.currencyManager_.UpdateWinAmount(totalPayOut);
                double hintTotalWinAmount = currencyManager.GetWinAmount();
                payOutManager.GetPayOut().ShowWin(totalPayOut);
                CommandCenter.Instance.hintsManager_.hintWinUI.ShowHintWinUIAmount(hintTotalWinAmount);
            }

        }
        yield return null;
    }
}
