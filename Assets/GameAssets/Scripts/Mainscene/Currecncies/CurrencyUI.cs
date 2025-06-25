using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    public TMP_Text cashAmount;
    public TMP_Text betAmount;
    public TMP_Text winAmount;

    // Update is called once per frame
    void Update()
    {
        if(CommandCenter.Instance == null)
            return;

        var spinManager = CommandCenter.Instance.spinManager_;
        if(spinManager == null) return;
        bool canSpin = spinManager.canSpin();
        setColor(canSpin);
    }

    private void setColor ( bool canSpin )
    {
        string code = "#FFFFFF"; // Default color
        if (canSpin)
        {
            code = "#74D4D4";
            
        }
        else
        {
            code = "#FFFFFF";// Disabled color
        }
        if (ColorUtility.TryParseHtmlString(code , out Color color))
        {
            cashAmount.color = color;
            betAmount.color = color;
            winAmount.color = color;
        }
        else
        {
            Debug.LogWarning($"Invalid color code: {code}");
        }
    }

}
