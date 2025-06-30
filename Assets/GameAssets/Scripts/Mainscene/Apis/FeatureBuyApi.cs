using Newtonsoft.Json;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
[System.Serializable]
public class FeatureBuyRequest
{
    public string client_Id;
    public string game_Id;
    public string player_Id;
    public string bet_Id;
    public GameState gameState;
}

[System.Serializable]
public class FeatureBuyResponse
{
    public string status;
    public string message;
    public GameState gameState;
    public winDetails [] winDetails;
    public float totalCost;
}
public class FeatureBuyApi : MonoBehaviour
{
    APIManager apiManager;
    FeatureBuyMenu featureBuyMenu;
    FreeSpinManager freeSpinManager;
    CurrencyManager currencyMan;
    [SerializeField] private bool isFeatureBuyDataFetched = false;
    [SerializeField] private SetUpCards SetUpCards;
    public FeatureBuyResponse featureBuyResponse;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        apiManager = CommandCenter.Instance.apiManager_;
        featureBuyMenu = CommandCenter.Instance.featureManager_.GetFeatureBuyMenu();
    }

    [ContextMenu("Fetch featureBuy Data")]
    public void FetchData ()
    {
        isFeatureBuyDataFetched = false;
        Debug.Log("Fetch Refill data");
        StartCoroutine(fetchDataCoroutine());
    }

    private IEnumerator fetchDataCoroutine ()
    {
        GameType currenttype = CommandCenter.Instance.GetTheGameType();
        FreeSpinManager freeSpinManager = CommandCenter.Instance.freeSpinManager_;
        CurrencyManager currencyMan = CommandCenter.Instance.currencyManager_;
        ExtraBetMenu extraBetMenu = CommandCenter.Instance.currencyManager_.GetExtraBetInfo();
        FeatureBuyRequest Data = new FeatureBuyRequest
        {
            client_Id = apiManager.gameDataApi.Client_id ,
            game_Id = apiManager.gameDataApi.Game_Id ,
            player_Id = apiManager.gameDataApi.Player_Id ,
            bet_Id = apiManager.bet_id ,
            gameState = new GameState
            {
                bet = new BetDetails
                {
                    amount = apiManager.gameDataApi.BetAmount ,
                    multiplier = int.Parse(currencyMan.GetBetMultiplier()) ,
                } ,
            } ,

        };
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new FloatTrimConverter());
        settings.Formatting = Formatting.Indented;
        string jsonData = JsonConvert.SerializeObject(Data , settings);
        Debug.Log("feature  buy payload : " + jsonData);
        yield return StartCoroutine(GetData(jsonData));
        yield return null;
    }

    public IEnumerator GetData ( string jsonData )
    {
        string ApiUrl = "https://b.api.ibibe.africa"+ "/featureBuy/magicaceoriginal";
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
                isFeatureBuyDataFetched = true;
            }
            else
            {
                //Debug
                string output = webRequest.downloadHandler.text;
                var parsedJson = JsonConvert.DeserializeObject(output);
                string formattedOutput = JsonConvert.SerializeObject(parsedJson , Formatting.Indented);
                Debug.Log("Update Feature Response: " + formattedOutput);
                //set up 
                featureBuyResponse = JsonConvert.DeserializeObject<FeatureBuyResponse>(output);
                apiManager.gameDataApi.ClearGridInfo();
                string [] [] reels = featureBuyResponse.gameState.reels;
                Debug.Log(reels.Length);
                SpecialSymbols specialSymbols = featureBuyResponse.gameState.specialSymbols;
                if (specialSymbols != null)
                {
                    Debug.Log("goldenSymbols : " + ( specialSymbols.goldenCards != null ? specialSymbols.goldenCards.Length : 0 ));
                    Debug.Log("jokersymbols : " + ( specialSymbols.jokerCards != null ? specialSymbols.jokerCards.Length : 0 ));
                    Debug.Log("scatterCards : " + ( specialSymbols.targetSymbols != null ? specialSymbols.targetSymbols.Length : 0 ));
                    Debug.Log("newScatterCards : " + ( specialSymbols.newTargetSymbols != null ? specialSymbols.newTargetSymbols.Length : 0 ));
                }
                apiManager.gameDataApi.UpdateGridInfo(SetUpCards.InitializeCards(reels , specialSymbols));
                isFeatureBuyDataFetched = true;
            }
        }
    }


    public bool IsFeatureBuyDataFetched ()
    {
        return isFeatureBuyDataFetched;
    }

}
