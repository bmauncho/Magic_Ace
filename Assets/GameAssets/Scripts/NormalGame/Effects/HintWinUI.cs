using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HintWinInfo
{
    public WinEffectType winEffectType;
    public Sprite hintBG;
    public Sprite hintFrame;
}

public class HintWinUI : MonoBehaviour
{
    public Image BG;
    public Image BGFrame;
    public WinEffectType winEffectType;
    public HintWinInfo [] hintWinInfos;
    [SerializeField] private GameObject winText;
    [SerializeField] private TMP_Text TotalWinText;
    [SerializeField] private TMP_Text winAmount;
    [SerializeField] private double currentWinAmount;
    [SerializeField] private double TotalWinAmount;
    [SerializeField] private float Duration = 2f;
    [SerializeField] private float elapsed = 0f;
    [SerializeField] private double startValue = 0;
    [SerializeField] private double endValue = 0;
    [SerializeField] private bool isAnimating = false;
    [SerializeField] private bool isSkipping = false;
    [SerializeField] private bool isHintWinUiDone =false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void AnimateWinAmount ()
    {
        elapsed = 0f;
        startValue = 0;
        endValue = TotalWinAmount;
        isAnimating = true;
        isHintWinUiDone = false;
        CommandCenter.Instance.soundManager_.PlayAmbientSound("Common_Winrank");
    }

    // Update is called once per frame
    private void Update ()
    {
        if (!isAnimating)
            return;
        if(isSkipping)
            return;

        if (Input.GetMouseButton(0))
        {
            if (!isSkipping)
            {
                isSkipping = true;
                isAnimating = false;
                SkipWinUi();
            }
        }

        elapsed += Time.deltaTime / 2;
        float t = Mathf.Clamp01(elapsed / Duration);
        double currentValue = Mathf.Lerp((float)startValue , (float)endValue , t);
        currentWinAmount = currentValue;
        winAmount.text = GetWinAmount(currentValue);


        if (t >= 1f)
        {
            isAnimating = false;
            winAmount.text = GetWinAmount(endValue);
            OnComplete();
        }
    }
    public void SetWinEffectType( WinEffectType type )
    {
        winEffectType = type;
    }

    public void OnComplete ()
    {
        Debug.Log("Oncomplete!");
        isHintWinUiDone = true;
        winText.transform.DOPunchScale(new Vector3(0.2f , 0.2f , 0.2f) , .5f,1 , 1)
            .OnComplete(() =>
            {
                ResetTheHintWinUI();
            });
    }

    [ContextMenu("Show Win Effect Type")]
    public void ShowWinEffectType ()
    {
        for (int i = 0 ; i < hintWinInfos.Length ; i++)
        {
            HintWinInfo currentHint = hintWinInfos [i];
            if (currentHint.winEffectType == winEffectType)
            {
                BG.sprite = currentHint.hintBG;
                BGFrame.sprite = currentHint.hintFrame;
                break;
            }
        }

        StartCoroutine(showAmount());

    }

    IEnumerator showAmount ()
    {
        winText.SetActive(true);
        CommandCenter.Instance.hintsManager_.ShowWinText();
        TotalWinAmount = CommandCenter.Instance.currencyManager_.GetWinAmount();
        //TotalWinText.text = GetTotalWin();
        AnimateWinAmount();
        yield return null;
    }

    public void ShowHintWinUIAmount ( double winAmount_ )
    {
        winText.SetActive(true);
        CommandCenter.Instance.hintsManager_.ShowWinText();
        winAmount.text = GetWinAmount(winAmount_);
        TotalWinText.GetComponent<LanguageTextTranslator>().UpdateImageText();
    }

    public void HideHintWinUIAmount ()
    {
        winText.SetActive(false);
        CommandCenter.Instance.hintsManager_.HideWinText();
    }

    public void UpdateTotalWinLanguage ()
    {

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
            else if (c == ',')
            {
                result += "<sprite name=JP_FNT_comma>";
            }
            else
            {
                result += $"<sprite name=JP_FNT_{c}>";
            }
        }

        return result;
    }

    private string GetTotalWin ()
    {
        string result = string.Empty;

        result = $"<sprite name=Base_Marquee_Total_en>" +
            $"<sprite name=Base_Marquee_Win_en>";

        return result;
    }

    public void SkipWinUi ()
    {
        if (!isAnimating) return;
        if (isSkipping) return;

        isSkipping = true;
        isAnimating = false;

        StopAllCoroutines(); // Stop any running coroutine (like winUiSequence)

        // Immediately show final win amount
        currentWinAmount = TotalWinAmount;
        winAmount.text = GetWinAmount(TotalWinAmount);
        OnComplete();
        CommandCenter.Instance.soundManager_.PlayAmbientSound("Base_BG");
        isSkipping = false;
    }

    public void ResetTheHintWinUI ()
    {
        isAnimating = false;
        isSkipping = false;
        isHintWinUiDone = false;
        currentWinAmount = 0;
        TotalWinAmount = 0;
        BG.sprite = hintWinInfos [0].hintBG;
        BGFrame.sprite = hintWinInfos [0].hintFrame;
        CommandCenter.Instance.hintsManager_.HideWinText(); 
        winText.SetActive(false);
    }
}
