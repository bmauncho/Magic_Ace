using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class FreeSpinTotalWin : MonoBehaviour
{
    public GameObject Bg;
    public GameObject Stars;
    public GameObject Effect;
    public GameObject Ballons;
    public GameObject Content;
    public TMP_Text title;
    public TMP_Text TotalPayout;
    [SerializeField] private double totalAmount;
    [SerializeField] private float Duration = 2f;
    [SerializeField] private double currentWinAmount;
    [SerializeField] private bool isFreeSpinWinUIDone = true;
    private float elapsed = 0f;
    private bool isAnimating = false;
    private double startValue = 0;
    private double endValue = 0;
    public void SetTotalAmount ( double totalAmount_ )
    {
        totalAmount = totalAmount_;
    }

    public void AnimateWinAmount ()
    {
        elapsed = 0f;
        startValue = 0;
        //test value 
        totalAmount = 55.55;
        endValue = totalAmount;
        isAnimating = true;
        isFreeSpinWinUIDone = false;
    }

    private void Update ()
    {
        if (!isAnimating)
            return;

        elapsed += Time.deltaTime / 2;
        float t = Mathf.Clamp01(elapsed / Duration);
        double currentValue = Mathf.Lerp((float)startValue , (float)endValue , t);
        currentWinAmount = currentValue;

        TotalPayout.text = GetWinAmount(currentValue);
        if (t >= 1f)
        {
            isAnimating = false;
            TotalPayout.text = GetWinAmount(endValue);

        }
    }

    private string GetWinAmount ( double WinAmount )
    {
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

        return result;
    }

    public IEnumerator showTotalWin ()
    {
        Ballons.SetActive(true);
        yield return new WaitForSeconds(.25f);
        ShowBg();
        yield return new WaitForSeconds(.25f);
        Effect.SetActive(true);
        ShowContent();
        AnimateWinAmount();
        yield return new WaitForSeconds(.25f);
        Stars.SetActive(true);
        yield return StartCoroutine(DeactivateFreeSpinsTotalWins());
        yield return null;
    }

    public void ShowBg ()
    {
        Bg.SetActive(true);
        CanvasGroup canvasGroup = Bg.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        Tween myTween = canvasGroup.DOFade(1 , 0.5f);
    }

    public void ShowContent ()
    {
        Content.SetActive(true);
        CanvasGroup canvasGroup = Content.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        Tween myTween = canvasGroup.DOFade(1 , 0.5f);
    }

    public IEnumerator DeactivateFreeSpinsTotalWins ()
    {
        yield return new WaitWhile(() => isAnimating);
        isFreeSpinWinUIDone = true;
        yield return new WaitForSeconds(1f);
        Content.SetActive(false);
        yield return null;
    }
}
