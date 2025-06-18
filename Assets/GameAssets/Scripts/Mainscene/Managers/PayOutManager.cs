using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class payOut
{
    public CardType cardtype;
    public List<int> payout;
}
public class PayOutManager : MonoBehaviour
{
    [SerializeField] private PayOut payOutWinEffect;
    [SerializeField] private payOut [] payOuts;

    public double GetTotalPayOut ( List<winCardData> WinningCards )
    {
        HashSet<CardType> cardTypeSet = new HashSet<CardType>();

        foreach (var winCard in WinningCards)
        {
            if (Enum.TryParse(typeof(CardType) , winCard.name , true , out var cardTypeEnum))
            {
                cardTypeSet.Add((CardType)cardTypeEnum);
            }
            else
            {
                Debug.LogError("Invalid card type: " + winCard.name);
            }
        }

        double payout = 0f;
        int multiplier = CommandCenter.Instance.multiplierManager_.GetMultiplier();
        double totalBet = double.Parse(CommandCenter.Instance.currencyManager_.GetTheBetAmount());
        foreach (CardType cardType in cardTypeSet)
        {
            int totalWinningWays = GetWiningWays(cardType , WinningCards);
            int totalWinningReels = GetWinningReels(cardType , WinningCards);
            float pay = (float)GetPayOut(cardType , totalWinningReels);

            payout += pay * totalBet * totalWinningWays;
        }

        return payout * multiplier;
    }


    public int GetPayOut ( CardType cardType , int winingReelsCount )
    {
        foreach (payOut p in payOuts)
        {
            if (p.cardtype == cardType && winingReelsCount > 2 && winingReelsCount < 6)
            {
                return p.payout [5 - winingReelsCount]; // Reverse index
            }
        }
        return 0;
    }


    public int GetWiningWays ( CardType cardType , List<winCardData> WinningCards )
    {
        // Step 1: Gather all unique card types from the winning cards
        HashSet<CardType> uniquecardType = new HashSet<CardType>();
        List<(CardType cardtype, List<int> colwithwinCard)> values = new List<(CardType cardtype, List<int> colwithwinCard)>();

        for (int i = 0 ; i < WinningCards.Count ; i++)
        {
            string cardName = WinningCards [i].name;
            if (Enum.TryParse(typeof(CardType) , cardName , true , out var cardTypeEnum))
            {
                uniquecardType.Add((CardType)cardTypeEnum);
            }
        }

        // Step 2: Collect the column indices for each card type
        List<CardType> cardTypeList = uniquecardType.ToList();
        for (int i = 0 ; i < cardTypeList.Count ; i++)
        {
            values.Add((cardTypeList [i], new List<int>()));
            for (int j = 0 ; j < WinningCards.Count ; j++)
            {
                string cardName = WinningCards [j].name;
                if (Enum.TryParse(typeof(CardType) , cardName , true , out var cardTypeEnum))
                {
                    if (cardTypeList [i] == (CardType)cardTypeEnum)
                    {
                        values [i].colwithwinCard.Add(WinningCards [j].col);
                    }
                }
            }
        }

        // Step 3: Find the matching cardType data
        var match = values.FirstOrDefault(v => v.cardtype == cardType);
        if (match.colwithwinCard == null || match.colwithwinCard.Count == 0)
            return 0;

        // Step 4: Group by column and count how many times the card appears in each column
        var columnGroups = match.colwithwinCard
            .GroupBy(col => col)
            .OrderBy(g => g.Key)
            .ToList();

        // Step 5: Multiply the counts together to get the number of winning ways
        int winWays = 1;
        foreach (var group in columnGroups)
        {
            winWays *= group.Count();
        }

        return winWays;
    }



    public int GetWinningReels ( CardType cardType , List<winCardData> WinningCards )
    {
        HashSet<int> uniqueColumns = new HashSet<int>();

        foreach (var card in WinningCards)
        {
            if (Enum.TryParse(typeof(CardType) , card.name , true , out var parsedType))
            {
                if ((CardType)parsedType == cardType)
                {
                    uniqueColumns.Add(card.col);
                }
            }
            else
            {
                Debug.LogError("Invalid card type: " + card.name);
            }
        }

        return uniqueColumns.Count;
    }

    public PayOut GetPayOut ()
    {
        return payOutWinEffect;
    }


    #region[Test]
    [ContextMenu("Test")]
    private void Test ()
    {
        List<winCardData> winningCards = new List<winCardData>
        {
            new winCardData { name = "QUEEN" , row = 0 , col = 0 },
            new winCardData { name = "QUEEN" , row = 1 , col = 1 },
            new winCardData { name = "QUEEN" , row = 2 , col = 2 },
            new winCardData { name = "QUEEN" , row = 3 , col = 3 },
            new winCardData { name = "QUEEN" , row = 3 , col = 4 },
            new winCardData { name = "QUEEN" , row = 2 , col = 4 }
        };
        Debug.Log("Total PayOut: " + GetTotalPayOut(winningCards));
        Debug.Log("PayOut: " + GetPayOut(CardType.QUEEN , GetWinningReels(CardType.QUEEN , winningCards)));
        Debug.Log("winning ways " + GetWiningWays(CardType.QUEEN , winningCards));
        Debug.Log("winning reels" + GetWinningReels(CardType.QUEEN , winningCards));
    }
    #endregion[Test]
}
