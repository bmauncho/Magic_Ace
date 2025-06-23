using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
public enum WinEffectType
{
    None ,
    Big_Win,
    Mega_Win,
    Ultra_Win,
}

[System.Serializable]
public class EffectType
{
    public WinEffectType type;
    public Sprite background;
    public Sprite BgEffect;
    public EffecttypeLanguage [] effectTypeLanguages;
    public double WinEffectChangePoint;
}

[System.Serializable]
public class EffecttypeLanguage
{
    public TheLanguage language;
    public TMP_SpriteAsset spriteAsset;
    public string winName_item1;
    public string winName_item2;
}
public class NormalGameWinUi : MonoBehaviour
{
    public TheLanguage currentLanguage;
    [SerializeField] private WinEffectType currentType;
    [SerializeField] private GameObject FeatureButton;
    [SerializeField] private GameObject WinUi;
    [SerializeField] private GameObject Confetti;
    [SerializeField] private GameObject Lights;
    [SerializeField] private Image BackGround;
    [SerializeField] private Image Backgroundeffect;
    [SerializeField] private TMP_Text winName;
    [SerializeField] private TMP_Text winAmount;
    [SerializeField] private double currentWinAmount;
    [SerializeField] private double TotalWinAmount;
    [SerializeField] private float Duration = 2f;
    [SerializeField] private bool isWinUIDone = true;
    [SerializeField]private bool isSkipping = false;

    [SerializeField] private List<EffectType> effectTypes = new List<EffectType>();


    private float elapsed = 0f;
    private bool isAnimating = false;
    private double startValue = 0;
    private double endValue = 0;

    private void OnEnable ()
    {
        if (LanguageMan.instance)
        {
            currentLanguage = LanguageMan.instance.ActiveLanguage;
            changeLanguage();
        }
    }
    public void changeLanguage ()
    {
        if (LanguageMan.instance && currentLanguage != LanguageMan.instance.ActiveLanguage)
        {
            currentLanguage = LanguageMan.instance.ActiveLanguage;
            for(int i = 0 ; i < effectTypes.Count ; i++)
            {
                EffectType effectType = effectTypes [i];
                if(effectType.type == currentType)
                {

                    TMP_SpriteAsset spriteAsset = effecttypeLanguageInfo(effectType).spriteAsset;
                    string winName_item1 = effecttypeLanguageInfo(effectType).winName_item1;
                    string winName_item2 = effecttypeLanguageInfo(effectType).winName_item2;
                    ChangeWinUI(i , spriteAsset , winName_item1 , winName_item2);
                }
            }
           
        }
    }
    public void AnimateWinAmount ()
    {
        elapsed = 0f;
        startValue = 0;
        endValue = TotalWinAmount;
        isAnimating = true;
        CommandCenter.Instance.soundManager_.PlayAmbientSound("Common_Winrank");
        Duration = GetWinDurationLog();
    }

    private void Update ()
    {
        changeLanguage();
        if (!isAnimating)
            return;

        elapsed += Time.deltaTime / 2;
        float t = Mathf.Clamp01(elapsed / Duration);
        double currentValue = Mathf.Lerp((float)startValue , (float)endValue , t);
        currentWinAmount = currentValue;
        winAmount.text = GetWinAmount(currentValue);

        CheckWinEffects();

        if (t >= 1f)
        {
            isAnimating = false;
            winAmount.text = GetWinAmount(endValue);
        }
    }

    private float GetWinDurationLog ()
    {
        double minWin = 1;     // Can't take log of 0
        double maxWin = TotalWinAmount;

        double clampedWin = Math.Max(minWin , Math.Min(TotalWinAmount , maxWin));
        double normalizedLog = Math.Log10(clampedWin / minWin) / Math.Log10(maxWin / minWin);

        return Mathf.Lerp(2f , 10f , (float)normalizedLog);
    }
    private int lastTriggeredEffectIndex = -1;
    private void CheckWinEffects ()
    {
        for (int i = effectTypes.Count - 1 ; i >= 0 ; i--) // Check from highest to lowest
        {
            if (currentWinAmount >= effectTypes [i].WinEffectChangePoint)
            {
                if (lastTriggeredEffectIndex == i) return; // Already triggered, skip

                lastTriggeredEffectIndex = i;

                if (effectTypes [i].type == WinEffectType.Big_Win) return;

                var effectType = effectTypes [i];
                if (effectType.type == WinEffectType.Ultra_Win)
                {
                    ShowLights();
                }

                SetEffectWinType(effectType.type);
                TMP_SpriteAsset spriteAsset = effecttypeLanguageInfo(effectType).spriteAsset;
                string winName_item1 = effecttypeLanguageInfo(effectType).winName_item1;
                string winName_item2 = effecttypeLanguageInfo(effectType).winName_item2;
                Sequence seq = DOTween.Sequence();
                seq.Append(WinUi.transform.DOScale(0 , .15f))
                   .AppendCallback(() => ChangeWinUI(i , spriteAsset , winName_item1 , winName_item2))
                   .Append(WinUi.transform.DOScale(1 , .15f))
                   .Append(WinUi.transform.DOPunchScale(new Vector3(0.2f , 0.2f , 0.2f) , .15f , 0 , 1))
                   .Play();

                break;
            }
        }
    }

    public EffecttypeLanguage effecttypeLanguageInfo (EffectType effectType)
    {
        for (int i = 0 ; i < effectType.effectTypeLanguages.Length ; i++)
        {
            if (effectType.effectTypeLanguages [i].language == currentLanguage)
            {
                return effectType.effectTypeLanguages [i];
            }
        }
        return effectTypes [0].effectTypeLanguages [0];
    }

    private void SetEffectWinType ( WinEffectType type )
    {
        currentType = type;
    }

    private void showConfetti ()
    {
        Confetti.SetActive(true);
        Confetti.GetComponentInChildren<Animator>().Rebind();
    }

    public void ShowLights ()
    {
        Lights.SetActive(true);
    }

    private void hideLights ()
    {
        Lights.SetActive(false);
    }

    public void hideConfetti ()
    {
        Debug.Log("Hide confetti");
        Confetti.SetActive(false);
    }
    private string GetWinAmount (double WinAmount)
    {
        string result = string.Empty;

        foreach (char c in WinAmount.ToString("N2"))
        {
            if (c == '.')
            {
                result += "<sprite name=JP_FNT_period>";
            }
            else if(c == ',')
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

    public void ShowWinUi ()
    {
        FeatureButton.SetActive (false);
        WinUi.SetActive(true);
    }

    public void HideWinUi ()
    {
        FeatureButton.SetActive (true);
        WinUi.SetActive(false);
        Lights.SetActive (false);   
        hideLights ();
        this.gameObject.SetActive (false);
    }
    [ContextMenu("AnimateWinAmount")]
    public void NormalWinUiSequence ()
    {
        this.gameObject.SetActive(true);
        isWinUIDone = false;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1 , .5f).OnComplete(() =>
        {
            StartCoroutine(winUiSequence());
        });
        
    }

    private IEnumerator winUiSequence ()
    {
        ShowWinUi();
        TotalWinAmount = CommandCenter.Instance.currencyManager_.GetWinAmount();
        //TotalWinAmount = 100;
        AnimateWinAmount();
        yield return new WaitUntil(() => !isAnimating || isSkipping);
        if (isSkipping) yield break;
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(true);
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(0, .5f).OnComplete(() =>
        {
            SetEffectWinType(WinEffectType.Big_Win);
            resetUI();
            HideWinUi();
            isWinUIDone = true;
            CommandCenter.Instance.soundManager_.PlayAmbientSound("Base_BG");
        });
        yield return null;
    }

    private void ChangeWinUI (int index,TMP_SpriteAsset spriteAsset,string winName_item1,string winName_item2)
    {
        BackGround.sprite = effectTypes [index].background;
        Backgroundeffect.sprite = effectTypes [index].BgEffect;
        winName.spriteAsset = spriteAsset;
        if(string.IsNullOrEmpty(winName_item1) && !string.IsNullOrEmpty(winName_item2))
        {
            winName.text = $"<sprite name={winName_item2}>";
        }
        else if (!string.IsNullOrEmpty(winName_item1) && string.IsNullOrEmpty(winName_item2))
        {
            winName.text = $"<sprite name={winName_item1}>";
        }
        else if (!string.IsNullOrEmpty(winName_item1) && !string.IsNullOrEmpty(winName_item2))
        {
            winName.text = $"<sprite name={winName_item1}><sprite name={winName_item2}>";
        }
    }

    public bool GetIsAnimating ()
    {
        return isAnimating;
    }

    public bool IsWinUiDone ()
    {
        return isWinUIDone;
    }

    public void resetUI ()
    {
        BackGround.sprite = effectTypes [0].background;
        Backgroundeffect.sprite = effectTypes [0].BgEffect;
        winName.spriteAsset = effectTypes [0].effectTypeLanguages [0].spriteAsset;
        string winName_item1 = effectTypes [0].effectTypeLanguages [0].winName_item1;
        string winName_item2 = effectTypes [0].effectTypeLanguages [0].winName_item2;
        if (string.IsNullOrEmpty(winName_item1) && !string.IsNullOrEmpty(winName_item2))
        {
            winName.text = $"<sprite name={effectTypes [0].effectTypeLanguages [0].winName_item2}>";
        }
        else if (!string.IsNullOrEmpty(winName_item1) && string.IsNullOrEmpty(winName_item2))
        {
            winName.text = $"<sprite name={effectTypes [0].effectTypeLanguages [0].winName_item1}>";
        }
        else if (!string.IsNullOrEmpty(winName_item1) && !string.IsNullOrEmpty(winName_item2))
        {
            winName.text = $"<sprite name={effectTypes [0].effectTypeLanguages [0].winName_item1}><sprite name={effectTypes [0].effectTypeLanguages [0].winName_item2}>";
        }
        
    }

    public void SkipWinUi ()
    {
        if (!isAnimating) return;
        if(isSkipping) return;

        isSkipping = true;
        isAnimating = false;

        StopAllCoroutines(); // Stop any running coroutine (like winUiSequence)

        // Immediately show final win amount
        currentWinAmount = TotalWinAmount;
        winAmount.text = GetWinAmount(TotalWinAmount);

        // Apply final win effect
        CheckWinEffects();

        // Fade out UI and clean up
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0 , 0.3f).OnComplete(() =>
        {
            SetEffectWinType(WinEffectType.Big_Win);
            resetUI();
            HideWinUi();
            isWinUIDone = true;
            CommandCenter.Instance.soundManager_.PlayAmbientSound("Base_BG");
            isSkipping = false;
        });
    }

    public bool CanSkip ()
    {
        return isAnimating && !isSkipping;
    }

}
