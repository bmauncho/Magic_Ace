using System.Collections.Generic;
using UnityEngine;

public class Feature_A_Sequence : MonoBehaviour
{
    public List<GridInfo> Spin_1 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        }
    };
}
