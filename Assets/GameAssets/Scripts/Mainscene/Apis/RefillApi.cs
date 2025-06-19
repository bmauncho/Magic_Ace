using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
[System.Serializable]
public class RefillCardsApi
{
    public GameState gameState;
    public string client_Id;
    public string game_Id;
    public string player_Id;
    public string bet_id;
}

[System.Serializable]
public class refillResponse
{
    public string status;
    public string message;
    public GameState gameState;
    public winDetails [] winDetails;
    public float totalCost;
}


public class RefillApi : MonoBehaviour
{
    APIManager apiManager;
    CardManager cardManager;
    GridManager gridManager;
    [SerializeField] private bool isRefillCardsfetched = false;
    [SerializeField] private RefillFeatureDemo refillFeatureDemo;
    [SerializeField] private refillResponse refillResponse_;
    [SerializeField] private List<GridInfo> RefillCardsData = new List<GridInfo>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        apiManager = CommandCenter.Instance.apiManager_;
        cardManager = CommandCenter.Instance.cardManager_;
        gridManager = CommandCenter.Instance.gridManager_;
    }
    [ContextMenu("FetchData")]
    public void FetchData ()
    {
        //Debug.Log("Fetch Refill data");
        isRefillCardsfetched = false; // Reset the flag before fetching new data
        StartCoroutine(fetchDataCoroutine(CommandCenter.Instance.gameMode)); // Pass 'true' for demo mode if needed
    }

    private IEnumerator fetchDataCoroutine ( GameMode mode )
    {
        //Debug.Log($"is Demo Mode: {isDemo}");
        //Debug.Log($"grid info count : {apiManager.gameDataApi.GetGridCardInfo().Count}");
        RefillCardsData.Clear();
        if (mode == GameMode.Demo)
        {

            if (CanShowFeature())
            {
                //Debug.Log("Show Features refill!");
                ShowReffillFeature();
            }
            else
            {
                //Debug.Log("Show Normal refill!");
                normalGame();
            }
            isRefillCardsfetched = true;
        }
        else
        {


        }

        updateGameDataApi();

        yield return null; // Placeholder for coroutine
    }

    public RefillCardsApi refillCardsApi ()
    {
        //Debug.Log("bet id : " + apiManager.bet_id);

        RefillCardsApi Data = new RefillCardsApi
        {
            gameState = apiManager.gameDataApi.apiResponse.gameState ,
            client_Id = apiManager.gameDataApi.Client_id ,
            player_Id = apiManager.gameDataApi.Player_Id ,
            game_Id = apiManager.gameDataApi.Game_Id ,
            bet_id = apiManager.bet_id ,
        };

        if (CommandCenter.Instance.GetTheGameType() == GameType.Base)
        {
            Data.gameState.gameMode = "base";
        }
        else
        {
            Data.gameState.gameMode = "freeSpins";
        }
        return Data;
    }
    public RefillCardsApi refillCardsApiCascade ()
    {
        RefillCardsApi Data = new RefillCardsApi
        {
            gameState = refillResponse_.gameState ,
            client_Id = apiManager.gameDataApi.Client_id ,
            player_Id = apiManager.gameDataApi.Player_Id ,
            game_Id = apiManager.gameDataApi.Game_Id ,
            bet_id = apiManager.bet_id ,
        };

        if (CommandCenter.Instance.GetTheGameType() == GameType.Base)
        {
            Data.gameState.gameMode = "base";
        }
        else
        {
            Data.gameState.gameMode = "freeSpins";
        }
        return Data;
    }
    private void updateGameDataApi ()
    {
        apiManager.gameDataApi.UpdateGridInfo(RefillCardsData);
    }

    public CardDatas GetCardInfo ( int col , int row )
    {
        CardDatas info = null;
        info = RefillCardsData [col].List [row];
        return info;
    }

    public bool isRefillCardsFetched ()
    {
        return isRefillCardsfetched;
    }

    public void normalGame ()
    {
        //card info list
        List<GridInfo> GameCardsData = new List<GridInfo>(apiManager.gameDataApi.GetGridCardInfo());

        //slots list
        List<CardPos> GameCardSlots = new List<CardPos>(gridManager.GetGridCards());

        for (int i = 0 ; i < 5 ; i++)
        {
            GridInfo info = new GridInfo();
            RefillCardsData.Add(info);
        }

        List<GridInfo> GameCardsApiData = new List<GridInfo>(apiManager.gameDataApi.GetGridCardInfo());

        for (int i = 0 ; i < 5 ; i++)
        {
            for (int j = 0 ; j < 4 ; j++)
            {
                CardDatas cardData = new CardDatas();
                RefillCardsData [i].List.Add(cardData);
                Slot slot = GameCardSlots [i].CardPosInRow [j].GetComponent<Slot>();
                if (slot.GetTheOwner() == null)
                {
                    // If the slot is empty, assign a random card
                    cardData = new CardDatas
                    {
                        name = cardManager.GetRandomnCard(j).CardType.ToString() ,
                        isGolden = cardManager.GetRandomnCard(j).isGolden ,
                        substitute = cardData.isGolden ? cardManager.GetRandomnCard(j).CardType.ToString() : "" ,

                    };
                    //Debug.Log("Card Data is null, assigning random card: " + cardData.name);
                }
                else
                {
                    cardData = new CardDatas
                    {
                        name = slot.GetTheOwner().GetComponent<Card>().GetCardType().ToString() ,
                        isGolden = slot.GetTheOwner().GetComponent<Card>().isGoldenCard() ,
                        substitute = GameCardsApiData [i].List [j].substitute ,

                    };
                }

                RefillCardsData [i].List [j] = new CardDatas
                {
                    name = cardData.name ,
                    isGolden = cardData.isGolden ,
                    substitute = cardData.substitute ,

                };
            }
        }
    }

    private void ShowReffillFeature ()
    {
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        switch (featureManager.GetActiveFeature())
        {
            case Features.None:
                break;
            case Features.Feature_A:
                refillFeatureDemo.FeatureA(RefillCardsData , isRefillCardsfetched);
                break;
            case Features.Feature_B:
                refillFeatureDemo.FeatureB(RefillCardsData , isRefillCardsfetched);
                break;
            case Features.Feature_C:
                refillFeatureDemo.FeatureC(RefillCardsData , isRefillCardsfetched);
                break;
        }

    }

    private bool CanShowFeature ()
    {
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        if (featureManager.GetActiveFeature() == Features.None)
        {
            return false;
        }

        return true;
    }

    public refillResponse RefillResponse ()
    {
        return refillResponse_;
    }

    public List<GridInfo> GetRefillData ()
    {
        return RefillCardsData;
    }
}
