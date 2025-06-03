using DG.Tweening;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ExtraBetMenu : MonoBehaviour
{
    [SerializeField] private GameObject ExtraBet_Info;
    [SerializeField] private RectTransform ExtraBetMenuUI;
    [SerializeField] private Image ExtraBetToggle;

    [Header("Booleans")]
    [SerializeField] private bool isInfoActive = false;
    [SerializeField] private bool isExtraBetMenuShown = false;
    [SerializeField] private bool isExtraBet = false;

    [Header("Sprites")]
    [SerializeField] private Sprite ExtraBetOn;
    [SerializeField] private Sprite ExtraBetOff;
    [SerializeField] private Sprite BetAmountBgMain;
    [SerializeField] private Sprite BetAmountBgEx;

    [Header("Images")]
    [SerializeField] private Image BetAmountBg_MainScene;
    [SerializeField] private Image BetAmountBg_BetMenu;

    [SerializeField] private float Timer;
    private Vector2 ExtraBetMenuStartPos;

    private void Start ()
    {
        ExtraBetMenuStartPos = ExtraBetMenuUI.anchoredPosition;
    }

    private void Update ()
    {
        Timer += Time.deltaTime;
        if (Timer >= 3f)
        {
            if (ExtraBet_Info.activeInHierarchy && isInfoActive)
            {
                isInfoActive = false;
                HideEx_Info();
                HideExtraBetBtns();
                Timer = 0;
            }
            else if (isExtraBetMenuShown)
            {
                isExtraBetMenuShown = false;
                HideExtraBetBtns();
                Timer = 0;
            }

        }
    }

    public void ToggleEx_Info ()
    {
        Timer = 0f; // Reset timer when toggling
        if (isInfoActive)
        {
            HideEx_Info();
        }
        else
        {
            ShowEx_Info();
        }
    }

    private void ShowEx_Info ()
    {
        isInfoActive = true;
        Timer = 0f; // Reset timer when showing info
        ExtraBet_Info.SetActive(true);

        CanvasGroup canvasGroup = ExtraBet_Info.GetComponent<CanvasGroup>() ?? ExtraBet_Info.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1 , 0.5f);
    }

    private void HideEx_Info ()
    {
        isInfoActive = false;
        CanvasGroup canvasGroup = ExtraBet_Info.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.DOFade(0 , 0.5f).OnComplete(() => ExtraBet_Info.SetActive(false));
        }
        else
        {
            ExtraBet_Info.SetActive(false);
        }
    }

    public void ToggleExtraBetMenu ()
    {
        Timer = 0;
        if (isExtraBetMenuShown)
        {
            HideExtraBetBtns();
        }
        else
        {
            ShowExtraBetBtns();
        }
    }

    private void ShowExtraBetBtns ()
    {
        isExtraBetMenuShown = true;
        Vector2 targetPos = new Vector2(0 , ExtraBetMenuUI.anchoredPosition.y);
        ExtraBetMenuUI.DOAnchorPos(targetPos , 0.25f);
    }

    private void HideExtraBetBtns ()
    {
        isExtraBetMenuShown = false;
        ExtraBetMenuUI.DOAnchorPos(ExtraBetMenuStartPos , 0.25f);
    }

    public void ToggleExtraBet ()
    {
        if (isExtraBet)
        {
            hideExtraBet();
        }
        else
        {
            showExtraBet();
        }
    }

    void showExtraBet ()
    {
        isExtraBet = true;
        ExtraBetToggle.sprite = ExtraBetOn;
        BetAmountBg_BetMenu.sprite = BetAmountBgEx;
        BetAmountBg_MainScene.sprite = BetAmountBgEx;
    }
    void hideExtraBet ()
    {
        isExtraBet = false;
        ExtraBetToggle.sprite = ExtraBetOff;
        BetAmountBg_BetMenu.sprite = BetAmountBgMain;
        BetAmountBg_MainScene.sprite = BetAmountBgMain;
    }

    public bool HasExtraBetEffect ()
    {
        return isExtraBet;
    }
}
