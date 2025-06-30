using Newtonsoft.Json;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;
[System.Serializable]
public class BetRequest
{
    public string player_id;
    public string amount;
    public string bet_id;
    public string game_id;
    public string client_id;
}

[System.Serializable]
public class BetResponse
{
    public float status_code = 0;
    public string message = "";
    public int bet_id;
    public float rich_card_balance = 0;
    public int game_id;
    public float new_wallet_balance;
    public ExternalResponse externalResponse_;
}

[System.Serializable]
public class ExternalResponse
{
    public string client_id;
    public float old_rtp;
    public string player_id;
    public string game_id;
    public float new_rtp;

}
public class PlaceBet : MonoBehaviour
{
    [Header("Api Reference")]
    private const string ApiUrl = "https://admin-api.ibibe.africa/api/v1/bet/place_bet";
    public BetResponse betResponse;

    [Header("Api Values")]
    [Header("Api Values")]
    public string Player_Id = "85";
    public string Game_Id = "8";
    public string Client_id = "12345";
    public string bet_id;
    public float BetAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        configureIds();
    }
    private void configureIds ()
    {
        Debug.Log("configure - " + GetType().Name);
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
    [ContextMenu("Place Bet")]
    public void Bet ()
    {
        bet_id = CommandCenter.Instance.apiManager_.bet_id;
        BetRequest betRequest = new BetRequest
        {
            player_id = Player_Id ,
            amount = BetAmount.ToString() ,
            bet_id = bet_id ,
            game_id = Game_Id ,
            client_id = Client_id
        };
        string jsonData = JsonUtility.ToJson(betRequest , true);
        Debug.Log("place bet payload " + jsonData);
        StartCoroutine(placeBet(jsonData));
        GameManager.Instance.ShowTransaction(bet_id);
    }

    private IEnumerator placeBet ( string jsonData )
    {
        string ApiUrl = ConfigMan.Instance.Base_url + "/api/v1/bet/place_bet";
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
            }
            else
            {
                string responseText = webRequest.downloadHandler.text;
                betResponse = JsonUtility.FromJson<BetResponse>(responseText);
                string formattedOutput = JsonConvert.SerializeObject(webRequest.downloadHandler.text , Formatting.Indented);
                Debug.Log($"Bet placed successfully:{formattedOutput}");
                double CashAmount = betResponse.new_wallet_balance;
                CommandCenter.Instance.currencyManager_.UpdateCashAmount(CashAmount);
            }
        }
    }
}
