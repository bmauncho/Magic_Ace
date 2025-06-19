using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum FeaturBuyOption
{
    None,
    FeatureBuy,
}

public class FeatureBuyMenu : MonoBehaviour
{
    [SerializeField] private GameObject Menu;
    [SerializeField] private RectTransform FeatureMenu;
    [SerializeField] private bool IsFeatureBuyActive;
    [SerializeField] private float featureBet;
    [SerializeField] private TMP_Text featureBet_;
    [SerializeField] private TMP_Text BuyAmount;
    private float [] betAmounts = { 2f , 1f , 0.6f , 0.4f , 0.2f };
    private int betIndex = 3;
    private List<(float featurebet, float buyAmount)> featureAmounts = new List<(float featurebet, float buyAmount)>
    {
        (0.2f, 10),
        (0.4f, 20f),
        (0.6f, 30f ),
        (1f, 50f ),
        (2f,100f ),
    };

    [SerializeField] private Button IncreaseBet;
    [SerializeField] private Button DecreaseBet;
    [SerializeField] private Animator effectAnim;
    [SerializeField] private float Timer = 0f;
    [SerializeField] private float selectedBuyAmount;
    [SerializeField] private FeaturBuyOption option;
    [SerializeField] private Button FeatureBuyButton;
    //[SerializeField] private SelectedFeature selectedFeature = null;

    private void OnEnable ()
    {
        featureBet = betAmounts [betIndex];
        featureBet_.text = featureBet.ToString();
        UpdateFeatureAmounts();
        UpdateBetButtons();
        UpdateFeatureBuyBtn();
    }

    private void Update ()
    {
        if (IsFeatureBuyActive) { return; }

        Timer += Time.deltaTime;

        if (Timer >= 5f)
        {
            EnableEffectAnim();
            Timer = 0f;
        }

    }

    public void EnableEffectAnim ()
    {
        effectAnim.gameObject.SetActive(true);
        effectAnim.Rebind();
        effectAnim.Play("FB_ShinyEffect");
    }
    private void UpdateFeatureAmounts ()
    {
        for (int i = 0 ; i < featureAmounts.Count ; i++)
        {
            if (featureAmounts [i].Item1 == featureBet)
            {
                BuyAmount.text = featureAmounts [i].Item2.ToString();
            }
        }
    }

    private void showFeatureBuy ()
    {
        IsFeatureBuyActive = true;
        Menu.SetActive(true);
        FeatureMenu.gameObject.SetActive(true);
        FeatureMenu.DOAnchorPos(Vector2.zero , .25f);
    }

    public void hideFeatureBuy ()
    {
        Vector2 target = new Vector2(0 , -700f);
        FeatureMenu.DOAnchorPos(target , .25f).OnComplete(() =>
        {
            IsFeatureBuyActive = false;
            FeatureMenu.gameObject.SetActive(false);
            Menu.SetActive(false);
        });
    }

    public void ToggleFeatureBuy ()
    {
        if (IsFeatureBuyActive)
        {
            hideFeatureBuy();
        }
        else
        {
            showFeatureBuy();
        }
    }

    public void IncreaseBetAmount ()
    {
        if (betIndex > 0)
        {
            betIndex--;
            featureBet = betAmounts [betIndex];
            featureBet_.text = featureBet.ToString();

            UpdateFeatureAmounts();
            UpdateBetButtons();
            UpdateFeatureBuyBtn();
        }
    }

    public void DecreaseBetAmount ()
    {
        if (betIndex < betAmounts.Length - 1)
        {
            betIndex++;
            featureBet = betAmounts [betIndex];

            featureBet_.text = featureBet.ToString();

            UpdateFeatureAmounts();
            UpdateBetButtons();
            UpdateFeatureBuyBtn();
        }
    }

    private void UpdateBetButtons ()
    {
        IncreaseBet.interactable = betIndex > 0;
        DecreaseBet.interactable = betIndex < betAmounts.Length - 1;
    }

    public void UpdateFeatureBuyBtn ()
    {
        float amountToBuy = GetBuyAmount();
        TMP_Text buttonText = FeatureBuyButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = amountToBuy.ToString();
    }


    public void ActivateFeatureBuy ()
    {
        hideFeatureBuy();
        float amounttoDecrease = GetBuyAmount();
        double myDouble = (double)amounttoDecrease;
        CommandCenter.Instance.currencyManager_.DecreaseCashAmount(myDouble);
        Invoke(nameof(spin) , .5f);
    }

    private void spin ()
    {
        Debug.Log("Spin");
    }

    public void SetFeatureBuy ()
    {
        option = FeaturBuyOption.FeatureBuy;
    }

    public void ResetOptions ()
    {
        option = FeaturBuyOption.None;
    }

    public FeaturBuyOption GetFeatureBuyOption ()
    {
        return option;
    }

    public float GetBuyAmount ()
    {
        for(int i = 0 ; i < featureAmounts.Count ; i++)
        {
            if (featureAmounts [i].Item1 == featureBet)
            {
                selectedBuyAmount = featureAmounts [i].Item2;
                break;
            }
        }

        return selectedBuyAmount;
    }
}
