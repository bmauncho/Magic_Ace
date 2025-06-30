using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class FetchGameRefillData : MonoBehaviour
{
    public List<GridInfo> InitializeCards ( string [] [] reels , SpecialSymbols specialSymbols )
    {
        List<GridInfo> gridInfos = new List<GridInfo>();

        List<(int reel, int row)> goldenCardspos = GetGoldenCards(specialSymbols.goldenCards);
        List<(string name, List<(int reel, int row)> positions)> jokerCardsPos = GetJokerCards(specialSymbols.jokerCards);
        List<(int reel, int row)> scatterCardspos = GetTargetCards(specialSymbols.targetSymbols);
        List<(int reel, int row)> retriggerCardspos = GetNewTargetCards(specialSymbols.newTargetSymbols);

        for (int i = 0 ; i < reels.Length ; i++)
        {
            GridInfo gridInfo = new GridInfo();
            gridInfos.Add(gridInfo);

            for (int j = 0 ; j < reels [i].Length ; j++)
            {
                CardDatas cardDatas = new CardDatas
                {
                    name = HasPrefixName(reels [i] [j]) ? card(reels [i] [j]).ToString() : card(reels [i] [j]).ToString() ,
                    isGolden = HasPrefixName(reels [i] [j]) ? true : false ,
                };

                bool isGolden = false;

                if (goldenCardspos != null)
                {
                    foreach (var pos in goldenCardspos)
                    {
                        if (pos.reel == i && pos.row == j)
                        {
                            cardDatas.isGolden = true;
                            break;
                        }
                    }
                }

                if (!isGolden && jokerCardsPos != null)
                {
                    foreach (var pos in jokerCardsPos)
                    {
                        if (pos.positions != null)
                        {
                            foreach (var position in pos.positions)
                            {
                                if (position.reel == i && position.row == j)
                                {
                                    cardDatas.name = pos.name;
                                    break;
                                }
                            }
                        }
                    }
                }

                gridInfo.List.Add(cardDatas);
            }
        }

        return gridInfos;
    }

    private List<(int col, int row)> GetGoldenCards ( TargetSymbols [] goldenCards )
    {
        if (goldenCards == null || goldenCards.Length == 0)
            return null;

        List<(int reel, int row)> temp = new List<(int reel, int row)>();
        for (int i = 0 ; i < goldenCards.Length ; i++)
        {
            if (goldenCards [i] != null)
            {
                temp.Add((goldenCards [i].reel, goldenCards [i].row));
            }
        }
        return temp;
    }


    private List<(string name, List<(int reel, int row)> positions)> GetJokerCards ( JokerData [] jokerCards )
    {
        if (jokerCards == null || jokerCards.Length == 0) return null;
        List<(string name, List<(int reel, int row)> positions)> temp = new List<(string name, List<(int reel, int row)> positions)>();

        for (int i = 0 ; i < jokerCards.Length ; i++)
        {
            if (jokerCards [i] != null)
            {
                List<(int reel, int row)> positions = new List<(int reel, int row)>();
                positions.Add((jokerCards [i].position.reel, jokerCards [i].position.row));
                temp.Add((jokerCards [i].mode, positions));
            }
        }
        return temp;
    }
    private List<(int col, int row)> GetTargetCards ( TargetSymbols [] TargetCards )
    {
        if (TargetCards == null || TargetCards.Length == 0) return null;
        List<(int reel, int row)> temp = new List<(int reel, int row)>();

        for (int i = 0 ; i < TargetCards.Length ; i++)
        {
            if (TargetCards [i] != null)
            {
                temp.Add((TargetCards [i].reel, TargetCards [i].row));
            }
        }
        return temp;
    }
    private List<(int col, int row)> GetNewTargetCards ( TargetSymbols [] newTargetCards )
    {
        if (newTargetCards == null || newTargetCards.Length == 0) return null;
        List<(int reel, int row)> temp = new List<(int reel, int row)>();

        for (int i = 0 ; i < newTargetCards.Length ; i++)
        {
            if (newTargetCards [i] != null)
            {
                temp.Add((newTargetCards [i].reel, newTargetCards [i].row));
            }
        }
        return temp;
    }
    [ContextMenu("Test")]
    public void Test ()
    {
        string name = "Ace";
        Debug.Log($"Has prefix : {HasPrefixName(name)}");
    }

    public bool HasPrefixName ( string name )
    {
        string namePrefix = "GOLDEN_";
        return name.StartsWith(namePrefix , System.StringComparison.OrdinalIgnoreCase);
    }

    public CardType card ( string name )
    {
        string namePrefix = "GOLDEN_";

        string newName = name.StartsWith(namePrefix , System.StringComparison.OrdinalIgnoreCase)
            ? name.Substring(namePrefix.Length)
            : name;

        newName = newName == "target" ? "scatter" : newName;

        return CommandCenter.Instance.ConvertToEnum(newName);
    }
}
