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
    public BaseGameCollector baseGameCollector;
    public FreeSpinsCollector freeSpinsCollector;
    public float totalWin;
    public bool cascading;
    public int targetCount;
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
    public int retriggers;
}
[System.Serializable]
public class JokerData
{
    public Payline position;
    public string mode;
    public int remainingRounds;
}

[System.Serializable]
public class  BaseGameCollector
{
    public int collectedCount;
    public bool isActive;
    public int remainingRounds;
}

[System.Serializable]
public class FreeSpinsCollector
{
    public int upgradeCount;
    public int maxUpgrades;
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
    FreeSpinManager freeSpinManager;
    CurrencyManager currencyManager;
    ExtraBetMenu extraBetMenu;
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
    [Header("References")]
    [SerializeField] private FeatureBuyDemo featureBuyDemo;
    [SerializeField] private FetchGameData fetchGameData;
    //Production Mode

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        cardManager = CommandCenter.Instance.cardManager_;
        freeSpinManager = CommandCenter.Instance.freeSpinManager_;
        currencyManager = CommandCenter.Instance.currencyManager_;
        extraBetMenu = CommandCenter.Instance.currencyManager_.GetExtraBetInfo();
        configureIds();
    }

    private void configureIds ()
    {
        Debug.Log("Configure - " + GetType().Name);
        Player_Id = GameManager.Instance.GetPlayerId();
        Game_Id = GameManager.Instance.GetGameId();
        Client_id = GameManager.Instance.GetClientId();
       
    }

    // Update is called once per frame
    void Update ()
    {
        if (CommandCenter.Instance)
        {
            BetAmount = float.Parse(CommandCenter.Instance.currencyManager_.GetTheBetAmount());
            bet_id = CommandCenter.Instance.apiManager_.bet_id;
        }
    }
    #region[startCards]
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
    #endregion
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
                Debug.Log("Features Show!");
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
            if (CanShowFeature())
            {
                ShowFeatures();
            }
            else if (CanShowFeatureBuy())
            {
                CommandCenter.Instance.apiManager_.featureBuyApi.FetchData();
            }
            else
            {
                Debug.Log("normal show!");
                //set up the game info
                GameType currenttype = CommandCenter.Instance.GetTheGameType();
                int remaining = 0;
                string BetId = CommandCenter.Instance.apiManager_.bet_id;
                GameInfo Data = null;
                string jsonData = string.Empty;
                string ApiUrl = string.Empty;
                //converting floats
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new FloatTrimConverter());
                settings.Formatting = Formatting.Indented;

                //Request Data

                Data = NormalGameInfo(
                    Client_id ,
                    Game_Id ,
                    Player_Id ,
                    BetId ,
                    currencyManager ,
                    extraBetMenu ,
                    currenttype ,
                    remaining ,
                    freeSpinManager
                );

                jsonData = JsonConvert.SerializeObject(Data , settings);
                ApiUrl = "https://b.api.ibibe.africa" + "/spin/magicaceoriginal";
                bool isDone = false;
                yield return StartCoroutine(FetchGridData(
                    ApiUrl ,
                    jsonData 
                ));
                IsDataFetched = true;
                Debug.Log("Done");
            }
        }
        yield return null;
    }


    public IEnumerator FetchGridData (
        string ApiUrl ,
        string jsonData)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(ApiUrl , "POST"))
        {
            byte [] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type" , "application/json");
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
                PromptManager.Instance.ShowErrorPrompt(
                      webRequest.responseCode.ToString() ,
                      webRequest.result.ToString() + " : " + webRequest.error.ToString());
            }
            else
            {
                //Debug
                string output = webRequest.downloadHandler.text;
                var parsedJson = JsonConvert.DeserializeObject(output);
                string formattedOutput = JsonConvert.SerializeObject(parsedJson , Formatting.Indented);
                Debug.Log("Spin Response: " + formattedOutput);
                //set up 
                apiResponse = JsonConvert.DeserializeObject<GameInfoResponse>(output);
                gridInfos.Clear();
                string [] [] reels = apiResponse.gameState.reels;
                SpecialSymbols specialSymbols = apiResponse.gameState.specialSymbols;
                //if (specialSymbols != null)
                //{
                //    Debug.Log("goldenSymbols : " + ( specialSymbols.goldenCards != null ? specialSymbols.goldenCards.Length : 0 ));
                //    Debug.Log("jokersymbols : " + ( specialSymbols.jokerCards != null ? specialSymbols.jokerCards.Length : 0 ));
                //    Debug.Log("scatterCards : " + ( specialSymbols.targetSymbols != null ? specialSymbols.targetSymbols.Length : 0 ));
                //    Debug.Log("newScatterCards : " + ( specialSymbols.newTargetSymbols != null ? specialSymbols.newTargetSymbols.Length : 0 ));
                //}
                gridInfos = fetchGameData.InitializeCards(reels , specialSymbols);
                Debug.Log("IsDone");
            }
        }
    }


    public GameInfo NormalGameInfo (
        string client_Id,
        string game_Id,
        string client_id ,
        string bet_Id,
        CurrencyManager currencyMan,
        ExtraBetMenu extraBetMenu,
        GameType currenttype,
        int remaining,
        FreeSpinManager freeSpinManager  )
    {
        GameInfo gameInfo = new GameInfo
        {
            client_Id = client_Id ,
            game_Id = game_Id ,
            player_Id = Player_Id ,
            bet_id = bet_Id ,
            gameState = new GameState
            {
                bet = new BetDetails
                {
                    amount = BetAmount ,
                    multiplier = int.Parse(currencyMan.GetBetMultiplier()) ,
                    extraBetEnabled = extraBetMenu.HasExtraBetEffect() ,
                } ,
                gameMode = currenttype == GameType.Base ? "base" : "freeSpins" ,
                freeSpins = new FreeSpins
                {
                    remaining = currenttype == GameType.Base ? 0 : remaining ,
                    totalAwarded = currenttype == GameType.Base ? 0 : freeSpinManager.GetSactterCards() ,
                    retriggers = currenttype == GameType.Base ? 0 : freeSpinManager.GetSactterCards() ,
                } ,
                reels = null ,
                jokerCards = null,
                boomingMultiplier = 1 ,
                baseGameCollector = new BaseGameCollector
                {
                    collectedCount = 0 ,
                    isActive = false ,
                    remainingRounds = 0
                } ,
                freeSpinsCollector = new FreeSpinsCollector
                {
                    upgradeCount = 0 ,
                    maxUpgrades = 0
                } ,
                totalWin = 0.0f ,
                cascading = false ,
                targetCount = 0 ,
                lastWinDetails = null ,
                specialSymbols = new SpecialSymbols
                {
                    goldenCards = null ,
                    jokerCards = null ,
                    targetSymbols = null ,
                    newTargetSymbols = null
                } ,
                cascadeCount = 0
            }
        };

        return gameInfo;
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
        FeaturesDemo featuresDemo = GetComponentInChildren<FeaturesDemo>();
        switch (featureManager.GetActiveFeature())
        {
            case Features.None:
                break;
            case Features.Feature_A:
                featuresDemo.featureA(gridInfos);
                break;
            case Features.Feature_B:
                featuresDemo.featureB(gridInfos);
                break;
            case Features.Feature_C:
                featuresDemo.featureC(gridInfos);
                break;
        }

    }

    private void showFeatureBuy ()
    {
        FeatureBuyMenu featureBuyMenu = CommandCenter.Instance.featureManager_.GetFeatureBuyMenu();
        switch (featureBuyMenu.GetFeatureBuyOption())
        {
            case FeaturBuyOption.None:
                break;
            case FeaturBuyOption.FeatureBuy:
                featureBuyDemo.FeatureBuy(gridInfos);
                break;
        }
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
    public bool CanShowFeature ()
    {
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        if (featureManager.GetActiveFeature() == Features.None)
        {
            return false;
        }

        return true;
    }

    public bool CanShowFeatureBuy ()
    {
        FeatureBuyMenu featureBuyMenu = CommandCenter.Instance.featureManager_.GetFeatureBuyMenu();
        if (featureBuyMenu.GetFeatureBuyOption() == FeaturBuyOption.None)
        {
            return false;
        }
        return true;
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

    public void UpdateGameDataApiList ( CardDatas data )
    {
        GridManager gridManager = CommandCenter.Instance.gridManager_;
        WinLoseManager winLoseManager = CommandCenter.Instance.winLoseManager_;
        List<CardPos> CardSlots = new List<CardPos>(gridManager.GetGridCards());
        List<CardPos> winCardSlots = new List<CardPos>(winLoseManager.GetWinCardSlots());
        for (int i = 0 ; i < CardSlots.Count ; i++)
        {
            for (int j = 0 ; j < CardSlots [i].CardPosInRow.Count ; j++)
            {
                WinSlot winSlot = winCardSlots [i].CardPosInRow [j].GetComponent<WinSlot>();

                if (winSlot != null && winSlot.GetTheOwner() != null)
                {

                    CardType type = winSlot.GetTheOwner().GetComponent<Card>().GetCardType();

                    if (gridInfos [i].List [j].name != type.ToString())
                    {
                        gridInfos [i].List [j] = new CardDatas
                        {
                            name = data.name ,
                            isGolden = data.isGolden ,
                            substitute = data.substitute ,
                        };
                    }
                }
            }
        }
    }
}
