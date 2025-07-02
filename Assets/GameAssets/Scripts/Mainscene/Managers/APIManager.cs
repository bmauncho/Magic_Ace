using System;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


[System.Serializable]
public class CardDetails
{
    public string name;
    public string substitute;
    public bool golden;
    public bool transformed;
}
public class APIManager : MonoBehaviour
{
    [Header("Api references")]
    public GameDataApi gameDataApi;
    public PlaceBet placeBet;
    public UpdateBet updateBet;
    public RefillApi refillApi;
    public FeatureBuyApi featureBuyApi;


    [Header("Api Values")]
    public float BetAmount;
    public string bet_id;
    public string Player_Id = "85";
    public string Game_Id = "8";
    public string Client_id = "12345";

    public TMP_Text[] TransactionText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        configureIds();
    }
    private void configureIds ()
    {
        Debug.Log("configure - " +GetType().Name);
        Player_Id = GameManager.Instance.GetPlayerId();
        Game_Id = GameManager.Instance.GetGameId();
        Client_id = GameManager.Instance.GetClientId();
        bet_id = GetBetId();
        Debug.Log("ApiManagerReady");
        updateTransactionText();
        Invoke(nameof(AddTransText) , .1f);
    }

    public void AddTransText ()
    {
        for (int i = 0 ; i < 2 ; i++)
        {
            GameManager.Instance.AddTransactionText(TransactionText [i]);
        }
    }
    public void PlaceBet ()
    {
        placeBet.Bet();
    }

    public void UpdateBet ()
    {
       updateBet.UpdateTheBet();
    }

    public string GetBetId ()
    {
        int id = Random.Range(1 , 1000000000);
        string betId = ConfigMan.Instance.GetBetId();
        return id.ToString();
    }

    public void setBetId ()
    {
        bet_id = GetBetId();
    }

    public void updateTransactionText ()
    {
        for(int i = 0 ; i < TransactionText.Length ; i++)
        {
            if(CommandCenter.Instance.gameMode == GameMode.Demo)
            {
                TransactionText [i].text = "<DEMO GAME>";
            }
            else
            {
                string transaction = "No.";
                string thetrans = transaction + " 748838" ; 
                TransactionText [i].text = thetrans + GetBetId();
            }
        }
    }
}
