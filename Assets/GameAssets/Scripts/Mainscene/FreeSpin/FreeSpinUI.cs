using TMPro;
using UnityEngine;

public class FreeSpinUI : MonoBehaviour
{
    public int TotalSpins;
    public TMP_Text freeSpinCountText;

    private void OnEnable ()
    {
        if (LanguageMan.instance)
        {
            LanguageMan.instance.onLanguageRefresh.AddListener(resetUI);
            resetUI();
        }
    }

    void resetUI ()
    {
        string freeSpinText = string.Empty;
        switch (LanguageMan.instance)
        {
            case { ActiveLanguage: TheLanguage.English }:
                freeSpinText = "<sprite name=Free_MSG_en>";
                break;
            case { ActiveLanguage: TheLanguage.Chinese }:
                freeSpinText = "<sprite name=Free_MSG_cn>";
                break;
            case { ActiveLanguage: TheLanguage.Portoguese }:
                freeSpinText = "<sprite name=Free_MSG_pt>";
                break;
            case { ActiveLanguage: TheLanguage.Thai }:
                freeSpinText = "<sprite name=Free_MSG_th>";
                break;
            default:
                Debug.LogWarning("Unsupported language. Defaulting to English.");
                freeSpinText = "<sprite name=Free_MSG_en>";
                break;
        }
        string freSpin_FrontSlash = "<sprite name=Free_MSG_/>";
        string result = string.Empty;
        string TotalSpinsText = string.Empty;

        foreach (char c in TotalSpins.ToString())
        {
            TotalSpinsText += $"<sprite name=Free_MSG_{c}>";
        }

        string count = $"<sprite name=Free_MSG_{0}>";

        freeSpinCountText.text = freeSpinText + count + freSpin_FrontSlash + TotalSpinsText;
    }

    public void SetTotalSpins(int spins )
    {
        TotalSpins = spins;
    }

    public void UpdateFreeSpinCount ( int count )
    {
        int currentCount = TotalSpins - count;
        if (freeSpinCountText != null)
        {
            string freeSpinText = string.Empty;
            switch (LanguageMan.instance)
            {
                case { ActiveLanguage: TheLanguage.English }:
                    freeSpinText = "<sprite name=Free_MSG_en>";
                    break;
                case { ActiveLanguage: TheLanguage.Chinese }:
                    freeSpinText = "<sprite name=Free_MSG_cn>";
                    break;
                case { ActiveLanguage: TheLanguage.Portoguese }:
                    freeSpinText = "<sprite name=Free_MSG_pt>";
                    break;
                case { ActiveLanguage: TheLanguage.Thai }:
                    freeSpinText = "<sprite name=Free_MSG_th>";
                    break;
                default:
                    Debug.LogWarning("Unsupported language. Defaulting to English.");
                    freeSpinText = "<sprite name=Free_MSG_en>";
                    break;
            }
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

    public void ResetFreeSpinCount ()
    {
        TotalSpins = 10;
        int currentCount = 0;
        if (freeSpinCountText != null)
        {
            string freeSpinText = "<sprite name=Free_MSG_en>";
            string freeSpin = "Free_MSG_";
            string freSpin_FrontSlash = "<sprite name=Free_MSG_/>";
            string result = string.Empty;
            string TotalSpinsText = string.Empty;
            foreach (char c in currentCount.ToString())
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
