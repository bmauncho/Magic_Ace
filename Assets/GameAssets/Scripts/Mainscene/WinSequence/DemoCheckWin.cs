using System.Collections.Generic;
using UnityEngine;

public class DemoCheckWin : MonoBehaviour
{
    public WinType CheckWin ( List<winCardData> WinningCards )
    {
        int colCount = 5;
        int rowCount = 4;

        List<CardPos> Cards = new List<CardPos>(CommandCenter.Instance.gridManager_.GetGridCards());
        GetComponentInParent<WinLoseManager>().ClearWinningCards();
        HashSet<string> allTypes = GetAllCardTypes(Cards , rowCount , colCount);
        Dictionary<int , bool> wildCols = GetWildColumns(Cards , rowCount , colCount);

        HashSet<string> addedKeys = new HashSet<string>();
        bool hasNormalWin = false;
        bool hasWildWin = false;

        foreach (string type in allTypes)
        {
            if (type == CardType.SCATTER.ToString()) continue;

            List<winCardData> currentWinningCards = new List<winCardData>();
            HashSet<string> tempKeys = new HashSet<string>();
            int consecutiveCols = 0;

            for (int col = 0 ; col < colCount ; col++)
            {
                bool foundMatch = false;

                for (int row = 0 ; row < rowCount ; row++)
                {
                    var slot = Cards [col].CardPosInRow [row].GetComponent<Slot>();
                    var cardObj = slot.GetTheOwner();
                    if (cardObj == null) continue;

                    string cardType = cardObj.GetComponent<Card>().GetCardType().ToString();
                    if (cardType == type || IsJoker(cardType))
                    {
                        string key = $"{col}-{row}";
                        if (!tempKeys.Contains(key))
                        {
                            currentWinningCards.Add(new winCardData
                            {
                                name = IsJoker(cardType) ? type : cardType ,
                                row = row ,
                                col = col
                            });
                            tempKeys.Add(key);
                        }
                        foundMatch = true;
                    }
                }

                if (foundMatch)
                    consecutiveCols++;
                else
                    break;
            }

            if (consecutiveCols >= 3)
            {
                hasNormalWin = true;
                foreach (var winCard in currentWinningCards)
                {
                    string key = $"{winCard.row}-{winCard.col}";
                    if (!addedKeys.Contains(key))
                    {
                        WinningCards.Add(winCard);
                        addedKeys.Add(key);
                    }
                }
            }
        }

        // Wild-only win (3+ wilds in different columns)
        List<winCardData> wildOnlyWinningCards = new List<winCardData>();
        foreach (var col in wildCols.Keys)
        {
            for (int row = 0 ; row < rowCount ; row++)
            {
                var slot = Cards [col].CardPosInRow [row].GetComponent<Slot>();
                var cardObj = slot.GetTheOwner();
                if (cardObj == null) continue;

                string cardType = cardObj.GetComponent<Card>().GetCardType().ToString();
                if(cardType != CardType.SCATTER.ToString()) continue;
                // Only add if it's a scatter and not already added
                if (cardType == CardType.SCATTER.ToString())
                {
                    string key = $"{col}-{row}";
                    if (!addedKeys.Contains(key))
                    {
                        wildOnlyWinningCards.Add(new winCardData
                        {
                            name = cardType.ToString() ,
                            row = row ,
                            col = col
                        });
                        addedKeys.Add(key);
                    }
                }
            }
        }

        if (wildOnlyWinningCards.Count > 0 && wildOnlyWinningCards.Count < 3)
        {
            WinningCards.AddRange(wildOnlyWinningCards);
        }
        else if (wildOnlyWinningCards.Count >= 3)
        {
            WinningCards.AddRange(wildOnlyWinningCards);
            hasWildWin = true;
        }

        if (hasNormalWin && hasWildWin) return WinType.Both;
        if (hasNormalWin) return WinType.Normal;
        if (hasWildWin) return WinType.Wild;
        return WinType.None;
    }
    private bool IsJoker ( string type )
    {
        return type == CardType.SUPER_JOKER.ToString() ||
               type == CardType.BIG_JOKER.ToString() ||
               type == CardType.SMALL_JOKER.ToString();
    }

    private HashSet<string> GetAllCardTypes ( List<CardPos> Cards , int rowCount , int colCount )
    {
        HashSet<string> allTypes = new HashSet<string>();
        for (int col = 0 ; col < colCount ; col++)
        {
            for (int row = 0 ; row < rowCount ; row++)
            {
                var cardObj = Cards [col].CardPosInRow [row].GetComponent<Slot>().GetTheOwner();
                if (cardObj == null) continue;

                string type = cardObj.GetComponent<Card>().GetCardType().ToString();
                allTypes.Add(type);
            }
        }
        return allTypes;
    }

    private Dictionary<int , bool> GetWildColumns ( List<CardPos> Cards , int rowCount , int colCount )
    {
        Dictionary<int , bool> wildCols = new Dictionary<int , bool>();
        for (int col = 0 ; col < colCount ; col++)
        {
            for (int row = 0 ; row < rowCount ; row++)
            {
                var cardObj = Cards [col].CardPosInRow [row].GetComponent<Slot>().GetTheOwner();
                if (cardObj == null) continue;

                string type = cardObj.GetComponent<Card>().GetCardType().ToString();
                if (type == CardType.SCATTER.ToString())
                {
                    wildCols [col] = true;
                    break;
                }
            }
        }
        return wildCols;
    }
}
