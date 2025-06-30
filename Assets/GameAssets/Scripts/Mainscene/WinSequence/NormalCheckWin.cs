using System.Collections.Generic;
using UnityEngine;

public class NormalCheckWin : MonoBehaviour
{
    GameDataApi gameDataApi;
    RefillApi refillApi;

    private void Start ()
    {
        gameDataApi = CommandCenter.Instance.apiManager_.gameDataApi;
        refillApi = CommandCenter.Instance.apiManager_.refillApi;
    }
    public WinType CheckWin ( List<winCardData> WinningCards )
    {
        Debug.Log("checking win...");
        if (WinningCards == null)
            return WinType.None;

        winDetails [] winDetails = null;
        bool isRefillingDone = CommandCenter.Instance.gridManager_.IsRefillSequnceDone();
        FeatureBuyMenu featureBuyMenu = CommandCenter.Instance.featureManager_.GetFeatureBuyMenu();
        if (isRefillingDone)
        {
            if (featureBuyMenu.GetFeatureBuyOption() == FeaturBuyOption.None)
            {
                //Debug.Log("checking win... normal");
                winDetails = gameDataApi.apiResponse.winDetails;
                if (winDetails == null)
                {
                    return GetSpinWinType(WinningCards);
                }

            }
            else
            {
                //Debug.Log($"checking win... {featureBuyMenu.GetFeatureBuyOption()}");
                return GetFeatureWinType(WinningCards);
            }

        }
        else
        {
            if (CommandCenter.Instance.gridManager_.IsCascading())
            {
                Debug.Log("checking win... cascading");
                winDetails = refillApi.RefillResponse().winDetails;
                //Debug.Log(winDetails.Length);
                if (winDetails == null)
                {
                    return GetRefillWinType(WinningCards);
                }
            }
            else
            {
                Debug.Log("checking win... normal");
                winDetails = gameDataApi.apiResponse.winDetails;
                if (winDetails == null)
                {
                    return GetSpinWinType(WinningCards);
                }

            }

        }

        if (winDetails == null)
            return WinType.None;
        for (int i = 0 ; i < winDetails.Length ; i++)
        {
            for (int j = 0 ; j < winDetails [i].symbols.Length ; j++)
            {
                string symbol = winDetails [i].symbols [j];
                WinningCards.Add(new winCardData
                {
                    name = symbol.ToUpper() ,
                    row = winDetails [i].payline [j].row ,
                    col = winDetails [i].payline [j].reel ,
                });
            }
        }
        int normalCount = 0;
        int scatterCount = 0;
        for (int i = 0 ; i < WinningCards.Count ; i++)
        {
            string scatterCard = CardType.SCATTER.ToString();
            if (WinningCards [i].name != scatterCard)
            {
                normalCount++;
                continue;
            }
                

            if (WinningCards [i].name == scatterCard)
            {
                scatterCount++;
            }
          
        }

        //if only normalCards
        //if only ScatterCards
        //if both
        //Debug.Log($"normal Cards {normalCount}");
        //Debug.Log($"Scatter Cards {scatterCount}");

        if (scatterCount > 0 && normalCount > 0)
        {
            return WinType.Both;
        }
        else if (scatterCount > 0 && normalCount <= 0)
        {
            return WinType.Wild;
        }
        else if (scatterCount <= 0 && normalCount > 0)
        {
            return WinType.Normal;
        }
        else
        {
            return WinType.None;
        }
    }

    public WinType GetFeatureWinType ( List<winCardData> WinningCards )
    {

        SpecialSymbols specialSymbols = CommandCenter.Instance.apiManager_.featureBuyApi.featureBuyResponse.gameState.specialSymbols;

        if (specialSymbols != null)
        {
            Debug.Log("special symbols is not null");
            if (specialSymbols.targetSymbols != null)
            {
                Debug.Log($"targetSymbols : {specialSymbols.targetSymbols.Length}");
                for (int i = 0 ; i < specialSymbols.targetSymbols.Length ; i++)
                {
                    WinningCards.Add(new winCardData
                    {
                        name = CardType.SCATTER.ToString() ,
                        row = specialSymbols.targetSymbols [i].row ,
                        col = specialSymbols.targetSymbols [i].reel ,
                    });
                }
                Debug.Log("winningCards " + WinningCards.Count);
            }
        }
        return WinningCards.Count >= 3 ? WinType.Wild : WinType.None;
    }

    public WinType GetRefillWinType ( List<winCardData> WinningCards )
    {
        SpecialSymbols specialSymbols = CommandCenter.Instance.apiManager_.refillApi.RefillResponse().gameState.specialSymbols;

        if (specialSymbols != null)
        {
            Debug.Log("special symbols is not null");
            if (specialSymbols.targetSymbols != null)
            {
                Debug.Log($"targetSymbols : {specialSymbols.targetSymbols.Length}");
                for (int i = 0 ; i < specialSymbols.targetSymbols.Length ; i++)
                {
                    WinningCards.Add(new winCardData
                    {
                        name = CardType.SCATTER.ToString() ,
                        row = specialSymbols.targetSymbols [i].row ,
                        col = specialSymbols.targetSymbols [i].reel ,
                    });
                }
                Debug.Log("winningCards " + WinningCards.Count);
            }
        }
        return WinningCards.Count >= 3 ? WinType.Wild : WinType.None;
    }

    public WinType GetSpinWinType ( List<winCardData> WinningCards )
    {
        SpecialSymbols specialSymbols = CommandCenter.Instance.apiManager_.gameDataApi.apiResponse.gameState.specialSymbols;

        if (specialSymbols != null)
        {
            Debug.Log("special symbols is not null");
            if (specialSymbols.targetSymbols != null)
            {
                Debug.Log($"targetSymbols : {specialSymbols.targetSymbols.Length}");
                for (int i = 0 ; i < specialSymbols.targetSymbols.Length ; i++)
                {
                    WinningCards.Add(new winCardData
                    {
                        name = CardType.SCATTER.ToString() ,
                        row = specialSymbols.targetSymbols [i].row ,
                        col = specialSymbols.targetSymbols [i].reel ,
                    });
                }
                Debug.Log("winningCards " + WinningCards.Count);
            }
        }
        return WinningCards.Count >= 3 ? WinType.Wild : WinType.None;
    }
}
