using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
[System.Serializable]
public class GameInfo
{
    public string client_Id;
    public string game_Id;
    public string player_Id;
    public string bet_id;
    public GameState gameState;

}

[System.Serializable]
public class GameState
{
    public BetDetails bet;
    public string gameMode;
    public FreeSpins freeSpins;
    public string [] [] reels;
    public JokerData [] jokerCards;
    public int boomingMultiplier;
    public float totalWin;
    public bool cascading;
    public int scatterCount;
    public LastWinDetails [] lastWinDetails;
    public SpecialSymbols specialSymbols;
    public int cascadeCount;
}


[System.Serializable]
public class BetDetails
{
    public float amount;
    public int multiplier;
    public bool extraBetEnabled;
}
[System.Serializable]
public class FreeSpins
{
    public int remaining;
    public int totalAwarded;
    public int scatterstriggered;
}
[System.Serializable]
public class JokerData
{
    public Payline position;
    public string mode;
    public int remainingRounds;
}

[System.Serializable]
public class SpecialSymbols
{
    public TargetSymbols [] goldenCards;
    public JokerData [] jokerCards;
    public TargetSymbols [] targetSymbols;
    public TargetSymbols [] newTargetSymbols;
}

[System.Serializable]
public class TargetSymbols
{
    public int reel;
    public int row;
}

[System.Serializable]
public class LastWinDetails
{
    public string [] symbols;
    public Payline [] payline;
    public float payout;
    public Payline [] goldenCards;
}

[System.Serializable]
public class Payline
{
    public int reel;
    public int row;
}

[System.Serializable]
public class CardDatas
{
    public string name = "";
    public bool isGolden;
    public string substitute = "";
}

[System.Serializable]
public class GameInfoResponse
{
    public string status;
    public string message;
    public GameState gameState;
    public winDetails [] winDetails;
    public float totalCost;
}

[System.Serializable]
public class winDetails
{
    public string [] symbols;
    public Payline [] payline;
    public float payout;
    public Payline [] goldenCards;
}

[System.Serializable]
public class GridInfo
{
    public List<CardDatas> List = new List<CardDatas>();
}

public class GameDataApi : MonoBehaviour
{
    CardManager cardManager;
    [Header("Api references")]
    public GameInfoResponse apiResponse;
    [Header("Api Values")]
    public string Player_Id;
    public string Game_Id;
    public string Client_id;
    public string bet_id;
    public float BetAmount;
    public bool IsDataFetched = false;
    //demo Mode
    [SerializeField] private List<GridInfo> gridInfos = new List<GridInfo>();

    //Production Mode

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        cardManager = CommandCenter.Instance.cardManager_;
        configureIds();
    }

    private void configureIds ()
    {
        Player_Id = CommandCenter.Instance.apiManager_.Player_Id;
        Game_Id = CommandCenter.Instance.apiManager_.Game_Id;
        Client_id = CommandCenter.Instance.apiManager_.Client_id;
        bet_id = CommandCenter.Instance.apiManager_.bet_id;
    }

    // Update is called once per frame
    void Update ()
    {
        if (CommandCenter.Instance)
        {
            BetAmount = float.Parse(CommandCenter.Instance.currencyManager_.GetTheBetAmount());

        }
    }
    
    public List<GridInfo> startCards = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.KING.ToString(), isGolden = true },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },

            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
               new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
               new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
               new CardDatas { name = CardType.KING.ToString(), isGolden = false },
               new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
            }
        }
    };


    [ContextMenu("Fetch Data")]
    public void FetchData ()
    {
        IsDataFetched = false;
        StartCoroutine(fetchDataCoroutine(CommandCenter.Instance.gameMode));
    }

    private IEnumerator fetchDataCoroutine ( GameMode mode )
    {
        Debug.Log("Game Data Api Called!");
        gridInfos.Clear();
        if (mode == GameMode.Demo)
        {
            if (CanShowFeature())
            {
                // Debug.Log("Features Show!");
                ShowFeatures();
            }
            else if (CanShowFeatureBuy())
            {
                // Debug.Log("Features buy show!");
                showFeatureBuy();
            }
            else
            {
                //Debug.Log("normal show!");
                normalGame();
            }
            IsDataFetched = true;
        }
        else
        {
           

        }
        yield return null;
    }

    

    public CardDatas GetCardInfo ( int col , int row )
    {
        CardDatas info = null;
        info = gridInfos [col].List [row];
        return info;
    }

    public List<GridInfo> GetGridCardInfo ()
    {
        return gridInfos;
    }

    public List<GridInfo> UpdateGridInfo ( List<GridInfo> newGridInfos )
    {
        Debug.Log("Updating Grid Infos");
        gridInfos = newGridInfos;
        return gridInfos;
    }

    public void ClearGridInfo ()
    {
        gridInfos.Clear();
    }

    public void UpdateGameDataApi ( CardDatas data , int row , int col )
    {
        gridInfos [col].List [row] = new CardDatas
        {
            name = data.name ,
            isGolden = data.isGolden ,
            substitute = data.substitute ,
        };
    }

    private void ShowFeatures ()
    {
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
       
    }

    private void showFeatureBuy ()
    {
        
    }

    private void normalGame ()
    {
        // Use the demo 
        for (int i = 0 ; i < 5 ; i++)
        {
            GridInfo gridInfo = new GridInfo();
            gridInfos.Add(gridInfo);
            for (int j = 0 ; j < 4 ; j++)
            {
                CardDatas cardDatas = new CardDatas();
                gridInfos [i].List.Add(cardDatas);
                // Get a random card from the card manager

                cardDatas = new CardDatas
                {
                    name = cardManager.GetRandomnCard(j).CardType.ToString() ,
                    isGolden = cardManager.GetRandomnCard(j).isGolden ,
                    substitute = cardDatas.isGolden ? cardManager.GetRandomnCard(j).CardType.ToString() : "" ,

                };
                // Assign the card data to the grid
                gridInfos [i].List [j] = cardDatas;
            }
        }
    }


   

    private void FeatureBuy_A ()
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


    private void FeatureBuy_B ()
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

    public bool CanShowFeature ()
    {
        return false;
    }

    public bool CanShowFeatureBuy ()
    {
        return false;
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

    public bool HasJokerCard ()
    {
        int superJokerCards = 0;

        List<CardPos> CardsInSlots = new List<CardPos>(CommandCenter.Instance.gridManager_.GetGridCards());

        if (CardsInSlots.Count == 0 || CardsInSlots == null)
        {
            return false;
        }

        for (int i = 0 ; i < CardsInSlots.Count ; i++)
        {
            for (int j = 0 ; j < CardsInSlots [i].CardPosInRow.Count ; j++)
            {
                Slot slot = CardsInSlots [i].CardPosInRow [j].GetComponent<Slot>();
                Card card = slot.GetTheOwner().GetComponent<Card>();
                if (card != null && card.GetCardType() == CardType.SUPER_JOKER)
                {
                    superJokerCards++;
                }
            }
        }
        if (superJokerCards > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameInfoResponse GameInfoResponse_ ()
    {
        return apiResponse;
    }
}
