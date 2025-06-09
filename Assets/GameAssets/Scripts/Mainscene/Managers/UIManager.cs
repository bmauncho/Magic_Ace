using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Game references")]
    [SerializeField] private GameObject BackGround;
    [SerializeField] private GameObject ExtraBetMenu;
    [SerializeField] private GameObject currencies;
    [SerializeField] private GameObject featureBtn;
    [SerializeField] private GameObject featureBuyBtn;
    [SerializeField] private GameObject Buttons;
    [SerializeField] private GameObject SettingsBtn;
    [SerializeField] private GameObject AutoSpinMenuBtn;
    [SerializeField] private GameObject FreeSpinUi;
    [SerializeField] private GameObject FreeSpin_baseBoardBg;


    [Header("Ui references")]
    [SerializeField] private Image TopBanner;
    [SerializeField] private Sprite NormalGameBackGround;
    [SerializeField] private Sprite NormalGameTopBanner;
    [Space(10)]
    [SerializeField] private Sprite FreeGameBackGround;
    [SerializeField] private Sprite FreeGameTopBanner;
    [Header("Top Banner")]

    public TopBanner topBanner;
    [ContextMenu("Show Normal Game")]
    public void ShowNormalGameUI ()
    {
        ExtraBetMenu.SetActive(true);
        ExtraBetMenu.SetActive(true);
        currencies.SetActive(true);
        featureBtn.SetActive(true);
        featureBuyBtn.SetActive(true);
        Buttons.SetActive(true);
        SettingsBtn.SetActive(true);
        AutoSpinMenuBtn.SetActive(true);
        FreeSpinUi.SetActive(false);
        FreeSpin_baseBoardBg.SetActive(false);
        SetNormalGameBG();
        SetNormalGameTopBanner();
    }

    [ContextMenu("Show Free Game")]
    public void ShowFreeGameUI ()
    {
        ExtraBetMenu.SetActive(false);
        ExtraBetMenu.SetActive(false);
        currencies.SetActive(false);
        featureBtn.SetActive(false);
        featureBuyBtn.SetActive(false);
        Buttons.SetActive(false);
        SettingsBtn.SetActive(false);
        AutoSpinMenuBtn.SetActive(false);
        FreeSpinUi.SetActive(true);
        FreeSpin_baseBoardBg.SetActive(true);
        SetFreeGameBG();
        SetFreeGameTopBanner();
    }

    private void SetNormalGameBG ()
    {
        BackGround.GetComponent<Image>().sprite = NormalGameBackGround;
    }

    private void SetFreeGameBG ()
    {
        BackGround.GetComponent<Image>().sprite = FreeGameBackGround;
    }

    private void SetNormalGameTopBanner ()
    {
        TopBanner.GetComponentInChildren<Image>().sprite = NormalGameTopBanner;
    }

    private void SetFreeGameTopBanner ()
    {
        TopBanner.sprite = FreeGameTopBanner;
    }
}
