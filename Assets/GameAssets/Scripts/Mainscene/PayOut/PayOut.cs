using DG.Tweening;
using TMPro;
using UnityEngine;
public class PayOut : MonoBehaviour
{
    [SerializeField] private TMP_Text winText;
    public void ShowWin ( double winAmount )
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.DOFade(1 , 0.5f).OnComplete(() =>
        {
            canvasGroup.alpha = 1;
            winText.text = winAmount.ToString("N2");
            UpdateTotalWin(winAmount);
            winText.gameObject.SetActive(true);
            Invoke(nameof(HideWin) , 2f);
        });
    }
    public void UpdateTotalWin ( double WinAmount )
    {
        string result = string.Empty;

        foreach (char c in WinAmount.ToString("N2"))
        {
            if (c == '.')
            {
                result += "<sprite name=JP_FNT_period>";
            }
            else if (c == ',')
            {
                result += "<sprite name=JP_FNT_comma>";
            }
            else
            {
                result += $"<sprite name=JP_FNT_{c}>";
            }
        }

        winText.text = result;
    }
    [ContextMenu("SetUp")]
    public void Update_TotalWin ()
    {
        double WinAmount = 12.40;
        string result = string.Empty;

        foreach (char c in WinAmount.ToString("N2"))
        {
            if (c == '.')
            {
                result += "<sprite name=JP_FNT_period>";
            }
            else
            {
                result += $"<sprite name=JP_FNT_{c}>";
            }
        }

        winText.text = result;
    }



    public void HideWin ()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0 , 0.5f).OnComplete(() =>
        {
            canvasGroup.alpha = 0;
            winText.gameObject.SetActive(false);
        });
    }
}
