using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{

    [SerializeField] private double CashAmount;
    [SerializeField] private string BetAmount;
    [SerializeField] private string BetMultiplier_;
    [SerializeField] private double winAmount;
    [SerializeField] private TMP_Text [] BetAmount_;
    [SerializeField] private TMP_Text [] cash;
    [SerializeField] private TMP_Text [] WinAmount;
    [SerializeField] private TMP_Text [] BetMultiplier;
    private string [] betAmounts = { "2" , "1" , "0.6" , "0.4" , "0.2" };
    private string [] betAmountsExtraBet = { "3" , "1.5" , "0.9" , "0.6" , "0.3" };
    private string [] betMultiplier = { "10" , "5" , "3" , "2" , "1" };
    [SerializeField] private int betIndex = 3; // Default to 0.4f (4th value in the array)
    [SerializeField] private Button IncreaseBet;
    [SerializeField] private Button DecreaseBet;
    [SerializeField] private ExtraBetMenu ExtraBetMenu_;
    public string TheBetAmount;
    public BetLimitsInfo BetLimitsInfo;
    public CurrencyUI CurrencyUI;
    private Dictionary<string , string> baseToExtra = new Dictionary<string , string>
    {
        { "2", "3" },
        { "1", "1.5" },
        { "0.6", "0.9" },
        { "0.4", "0.6" },
        { "0.2", "0.3" }
    };

    private Dictionary<string , string> extraToBase = new Dictionary<string , string>
    {
        { "3", "2" },
        { "1.5", "1" },
        { "0.9", "0.6" },
        { "0.6", "0.4" },
        { "0.3", "0.2" }
    };
    void Start ()
    {
        SetUpCurrencyMan();
    }

    public void SetUpCurrencyMan ()
    {
        Debug.Log("configure - " + GetType().Name);
        if (CommandCenter.Instance.gameModeManager_.IsDemoMode())
        {
            Debug.Log("Demo");
            CashAmount = 2000;
        }
        else
        {
            Debug.Log("Live");
            CashAmount = double.Parse(GameManager.Instance.GetCashAmount());
        }

        UpdateCashUI();
        BetAmount = betAmounts [betIndex];
        updateBetAmount();
        updateBetMultiplier();
    }

    private void updateBetAmount ()
    {
        for (int i = 0 ; i < BetAmount_.Length ; i++)
        {
            BetAmount_ [i].text = BetAmount.ToString();
        }
    }

    public double GetCashAmount ()
    {
        return CashAmount;
    }

    public string GetBetAmount ()
    {
        if (string.IsNullOrEmpty(BetAmount))
        {
            BetAmount = betAmounts [betIndex]; ;
        }
        float parsedBetAmount = float.Parse(BetAmount);
        if (CommandCenter.Instance.gameModeManager_.IsDemoMode())
        {
            if (ExtraBetMenu_.HasExtraBetEffect())
            {
                Debug.Log("Extra Bet Enabled");
                if (baseToExtra.TryGetValue(BetAmount , out string newBet))
                {
                    Debug.Log($"Bet Amount: {BetAmount} : Extra Bet Amount: {newBet}");
                    TheBetAmount = newBet;
                    //set bet index

                    betIndex = Array.IndexOf(betAmountsExtraBet , TheBetAmount);
                }
            }
            else
            {
                Debug.Log("Extra Bet Disabled");
                if (extraToBase.TryGetValue(BetAmount , out string newBet))
                {
                    Debug.Log($"Bet Amount: {BetAmount} : Base Bet Amount: {newBet}");
                    TheBetAmount = newBet;

                    betIndex = Array.IndexOf(betAmounts , TheBetAmount);
                }
            }

            BetAmount = TheBetAmount;
           
        }
        else
        {
            if (string.IsNullOrEmpty(TheBetAmount))
            {
                BetAmount = betAmounts [betIndex]; ;
            }
            else
            {
                //GameInfoResponse response = CommandCenter.Instance.apiManager_.gameDataApi.apiResponse;
                bool isExtraBet = false/*response.gameState.bet.extraBetEnabled*/;

                if (ExtraBetMenu_.HasExtraBetEffect())
                {
                    Debug.Log("Extra Bet Enabled");
                    if (baseToExtra.TryGetValue(BetAmount , out string newBet))
                    {
                        Debug.Log($"Bet Amount: {BetAmount} : Extra Bet Amount: {newBet}");
                        TheBetAmount = newBet;
                        betIndex = Array.IndexOf(betAmountsExtraBet , TheBetAmount);
                    }
                }
                else
                {
                    Debug.Log("Extra Bet Disabled");
                    if (extraToBase.TryGetValue(BetAmount , out string newBet))
                    {
                        Debug.Log($"Bet Amount: {BetAmount} : Base Bet Amount: {newBet}");
                        TheBetAmount = newBet;
                        betIndex = Array.IndexOf(betAmounts , TheBetAmount);
                    }
                }

                BetAmount = TheBetAmount;
            }
        }

        for (int i = 0 ; i < BetAmount_.Length ; i++)
        {
            BetAmount_ [i].text = BetAmount.ToString();
        }

        return BetAmount;
    }


    public string GetTheBetAmount ()
    {
        if (string.IsNullOrEmpty(TheBetAmount))
        {
            TheBetAmount = betAmounts [betIndex];
        }

        return TheBetAmount;
    }

    [ContextMenu("updateBetMultiplier")]
    public void updateBetMultiplier ()
    {
        //Debug.Log("Updating bet multipliers using index: " + betIndex);
        for (int j = 0 ; j < BetMultiplier.Length ; j++)
        {
            BetMultiplier [j].text = betMultiplier [betIndex];

            //Debug.Log($"Set BetMultiplier[{j}] to {betMultiplier [betIndex]}");
        }

        BetMultiplier_ = betMultiplier [betIndex];
    }



    public void UpdateCashUI ()
    {
        for (int i = 0 ; i < cash.Length ; i++)
        {
            cash [i].text = CashAmount.ToString("N2");
        }
    }

    public void UpdateCashAmount ( double amount )
    {
        CashAmount = amount;
        UpdateCashUI();
    }

    public void IncreaseBetAmount ()
    {
        if (betIndex > 0)
        {
            betIndex--;
            TheBetAmount = betAmounts [betIndex];
            BetAmount = betAmounts [betIndex];
            for (int i = 0 ; i < BetAmount_.Length ; i++)
            {
                BetAmount_ [i].text = BetAmount.ToString();
                BetAmount_ [i].transform.DOPunchScale(new Vector3(0.25f , 0.25f , 0.25f) , 0.2f , 10 , 1);
            }

            UpdateBetButtons();
        }
    }

    public void DecreaseBetAmount ()
    {
        if (betIndex < betAmounts.Length - 1)
        {
            betIndex++;
            TheBetAmount = betAmounts [betIndex];
            BetAmount = betAmounts [betIndex];
            for (int i = 0 ; i < BetAmount_.Length ; i++)
            {
                BetAmount_ [i].text = BetAmount.ToString();
                BetAmount_ [i].transform.DOPunchScale(new Vector3(0.25f , 0.25f , 0.25f) , 0.2f , 10 , 1);
            }

            UpdateBetButtons();
        }
    }

    public void IncreaseCash ( double amount )
    {
        if(CommandCenter.Instance.gameMode != GameMode.Demo)
        {
            return;
        }
        double prevCashAmount = CashAmount;
        double currentCashAmount = 0;
        CashAmount += amount;
        currentCashAmount = CashAmount;
        Debug.Log($"Prev : {prevCashAmount} current : {CashAmount}");
        Debug.Log($"currnt - prev = {currentCashAmount - prevCashAmount}");
        UpdateCashUI();
    }

    public void DecreaseCash ()
    {
        CashAmount -= double.Parse(BetAmount);
        if (CashAmount <= 0)
        {
            CashAmount = 0;
        }

        UpdateCashUI();
    }

    public void DecreaseCashAmount ( double amount )
    {
        CashAmount -= amount;
        if (CashAmount <= 0)
        {
            CashAmount = 0;
        }
    }

    public void UpdateBetAmount ( string betAmount_ )
    {
        BetAmount = betAmount_;
        Debug.Log("Bet Amount: " + BetAmount);
        TheBetAmount = BetAmount;
        for (int i = 0 ; i < BetAmount_.Length ; i++)
        {
            BetAmount_ [i].text = BetAmount;
        }
        updateBetMultiplier();
    }

    private void UpdateBetButtons ()
    {
        IncreaseBet.interactable = betIndex > 0;
        DecreaseBet.interactable = betIndex < betAmounts.Length - 1;
        ShowBetLimits();
    }

    public void ShowBetLimits ()
    {
        if (betIndex >= betAmounts.Length - 1)
        {
            //show maximum bet limit
            minimumBetLimit();
           
        }
        else if(betIndex <= 0)
        {
            //show minimum bet limit
            maximumBetLimit();
        }
    }
    Coroutine minimumBet;
    Coroutine maximumBet;
    public void minimumBetLimit ()
    {
        string text = $"{LanguageMan.instance.RequestForText("L_13")}";
        if(maximumBet != null)
        {
            StopCoroutine(maximumBet);
        }
        CancelInvoke(nameof(DeactivateBetLimits));
        BetLimitsInfo.gameObject.SetActive(true);
        if (BetLimitsInfo != null)
        {
            BetLimitsInfo.SetText(text);
            minimumBet = StartCoroutine(BetLimitsInfo.bounce());
            Invoke(nameof(DeactivateBetLimits) , 2f);
        }
        else
        {
            Debug.LogWarning("BetLimitsInfo is not assigned in the CurrencyManager.");
        }
    }

    void DeactivateBetLimits ()
    {
        BetLimitsInfo.gameObject.SetActive(false);
    }

    public void maximumBetLimit ()
    {
        if (minimumBet != null)
        {
            StopCoroutine(minimumBet);
        }
        CancelInvoke(nameof(DeactivateBetLimits));
        BetLimitsInfo.gameObject.SetActive(true);
        string text = $"{LanguageMan.instance.RequestForText("L_14")}";
        if (BetLimitsInfo != null)
        {
            BetLimitsInfo.SetText(text);
            maximumBet = StartCoroutine(BetLimitsInfo.bounce());
            Invoke(nameof(DeactivateBetLimits) , 2f);
        }
        else
        {
            Debug.LogWarning("BetLimitsInfo is not assigned in the CurrencyManager.");
        }
    }

    public void UpdateWinAmount ( double winAmount_ )
    {
        winAmount += winAmount_;
        for (int i = 0 ; i < WinAmount.Length ; i++)
        {
            WinAmount [i].text = winAmount.ToString("N2");
        }
    }

    public void ResetWinAmount ()
    {
        winAmount = 0;
        for (int i = 0 ; i < WinAmount.Length ; i++)
        {
            WinAmount [i].text = winAmount.ToString("N2");
        }
    }

    public double GetWinAmount ()
    {
        return winAmount;
    }

    public ExtraBetMenu GetExtraBetInfo ()
    {
        return ExtraBetMenu_;
    }

    public string GetBetMultiplier ()
    {
        if (string.IsNullOrEmpty(BetMultiplier_))
        {
            BetMultiplier_ = betMultiplier [betIndex];
        }
        return BetMultiplier_;
    }

    public void SetCashAmount ( double amount )
    {
        CashAmount = amount;
    }
}
