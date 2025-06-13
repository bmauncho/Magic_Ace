using TMPro;
using UnityEngine;

public class FreeSpinUI : MonoBehaviour
{
    public int TotalSpins;
    public TMP_Text freeSpinCountText;
    
    public void SetTotalSpins(int spins )
    {
        TotalSpins = spins;
    }

    public void UpdateFreeSpinCount ( int count )
    {
        int currentCount = TotalSpins - count;
        if (freeSpinCountText != null)
        {
            string freeSpinText = "<sprite name=Free_MSG_en>";
            string freeSpin = "Free_MSG_";
            string freSpin_FrontSlash = "<sprite name=Free_MSG_/>";
            string result = string.Empty;
            string TotalSpinsText = string.Empty;
            foreach(char c in currentCount.ToString())
            {
                result += $"<sprite name=Free_MSG_{c}>";
            }
            foreach (char c in TotalSpins.ToString())
            {
                TotalSpinsText += $"<sprite name=Free_MSG_{c}>";
            }
         

            freeSpinCountText.text = freeSpinText + result + freSpin_FrontSlash + TotalSpinsText;
        }
        else
        {
            Debug.LogWarning("FreeSpinCountText is not assigned in the inspector.");
        }
    }
}
