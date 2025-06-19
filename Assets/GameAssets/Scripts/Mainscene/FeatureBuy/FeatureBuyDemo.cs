using System.Collections.Generic;
using UnityEngine;

public class FeatureBuyDemo : MonoBehaviour
{
    GameDataApi gameDataApi;
    CardManager cardManager;

    private void Start ()
    {
        gameDataApi = GetComponentInParent<GameDataApi>();
        cardManager = CommandCenter.Instance.cardManager_;
    }
    public void FeatureBuy ( List<GridInfo> gridInfos )
    {
        List<(int row, int col)> gridPositions = GenerateRandomGridPositions(3);
        HashSet<(int, int)> wildPositions = new HashSet<(int, int)>(gridPositions);

        for (int i = 0 ; i < 5 ; i++)
        {
            GridInfo gridInfo = new GridInfo();
            gridInfos.Add(gridInfo);

            for (int j = 0 ; j < 4 ; j++)
            {
                CardDatas cardDatas;

                if (wildPositions.Contains((i, j)))
                {
                    cardDatas = new CardDatas
                    {
                        name = CardType.SCATTER.ToString() ,
                        isGolden = false ,
                        substitute = null ,
                    };
                }
                else
                {
                    var randomCard = cardManager.GetRandomnCard(j);
                    cardDatas = new CardDatas
                    {
                        name = randomCard.CardType.ToString() ,
                        isGolden = randomCard.isGolden ,
                        substitute = randomCard.isGolden ? randomCard.CardType.ToString() : "" ,
                    };
                }

                gridInfo.List.Add(cardDatas);
            }
        }
    }
    private List<(int col, int row)> GenerateRandomGridPositions ( int count )
    {
        List<(int col, int row)> GridPositions = new List<(int row, int col)>();
        for (int i = 0 ; i < count ; i++)
        {
            int x = UnityEngine.Random.Range(0 , 5); // 0 to 3 inclusive
            int y = UnityEngine.Random.Range(0 , 4); // 0 to 4 inclusive
            GridPositions.Add((x, y));
        }
        return GridPositions;
    }
}
