using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetOptions : MonoBehaviour
{
    [SerializeField] private Bet Selectedbet;
    [SerializeField] private string BetAmount;
    [SerializeField] private TMP_Text currentBetAmount;
    [SerializeField] private Bet [] bets;
    private void OnEnable ()
    {
        selectCurrentBet();
    }
    public void GetSelectedBet ( Bet theBet )
    {
        Selectedbet = theBet;
        BetAmount = theBet.GetBet();
        currentBetAmount.text = BetAmount;
        CommandCenter.Instance.currencyManager_.UpdateBetAmount(BetAmount);
        Invoke(nameof(DisableBetMenu) , 0.15f);
    }

    private void selectCurrentBet ()
    {
        foreach (Bet Bet in bets)
        {
            if (Bet.GetBet() == CommandCenter.Instance.currencyManager_.GetBetAmount())
            {
                Bet.GetComponent<Button>().Select();
            }
        }
    }

    public void DisableBetMenu ()
    {
        CommandCenter.Instance.mainMenuController_.ToggleBetMenu();
    }
}
