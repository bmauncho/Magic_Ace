using TMPro;
using UnityEngine;

public class Bet : MonoBehaviour
{
    [SerializeField] private string BetAmount;
    [SerializeField] private string DefaultBetAmount;
    [SerializeField] private TMP_Text TheBetAmount;

    private void OnEnable ()
    {
        float parsedBetAmount = float.Parse(DefaultBetAmount);
        float amount = 0;
        if (CommandCenter.Instance.currencyManager_.GetExtraBetInfo().HasExtraBetEffect())
        {
            amount = parsedBetAmount + ( parsedBetAmount * 0.5f );
            BetAmount = amount.ToString();
        }
        else
        {
            BetAmount = DefaultBetAmount;
        }
        TheBetAmount.text = BetAmount;
    }

    public string GetBet ()
    {
        return BetAmount;
    }

    public void Selectbet ()
    {
        GetComponentInParent<BetOptions>().GetSelectedBet(this);
    }
}
