using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpinManager : MonoBehaviour
{
    [Header("SpinMode")]
    [SerializeField] private bool isNormalSpin = true;
    [SerializeField] private bool isQuickSpin = false;
    [SerializeField] private bool isTurboSpin = false;
    [SerializeField] private Image spinModBtn;
    [SerializeField] private Sprite normalMode;
    [SerializeField] private Sprite quickMode;
    [SerializeField] private Sprite turboMode;

    [Header("Spin Ui")]
    [SerializeField] private Transform spinners;
    [SerializeField] private float degreesPerSecond;
    private float currentSpeed = 0f; // Tracks the current speed
    private float smoothTime = 0.5f; // Adjust this to control how smooth the transition is
    [SerializeField] private bool IsFillingGrid = false;

    [Header("Spin")]
    [SerializeField] private bool CanSpin = false;
    [SerializeField] private GameObject [] Buttons;

    [Header("Spin Animation")]
    public SpinSpeedUI SpinSpeedEffect;
    public SpinAnims SpinAnims;

    [ContextMenu("Set Spin Mode")]
    public void SetSpinMode ()
    {
        if (isNormalSpin)
        {
            SetMode(false , true , false , quickMode);
            //show Ui for QuickMode
            string modeName = "QUICK SPIN ENABLED";
            ShowSpinSpeedUI(quickMode,modeName);
        }
        else if (isQuickSpin)
        {
            SetMode(false , false , true , turboMode);
            //show Ui for TurboMode
            string modeName = "TURBO SPIN ENABLED";
            ShowSpinSpeedUI(turboMode,modeName);
        }
        else if (isTurboSpin)
        {
            SetMode(true , false , false , normalMode);
            //show Ui for NormalMode
            string modeName = "TURBO SPIN DISABLED";
            ShowSpinSpeedUI(normalMode, modeName);
        }
    }

    private void SetMode ( bool normal , bool quick , bool turbo , Sprite modeSprite )
    {
        isNormalSpin = normal;
        isQuickSpin = quick;
        isTurboSpin = turbo;
        spinModBtn.sprite = modeSprite;
    }

    public void ShowSpinSpeedUI ( Sprite modeIcon , string modeName = null )
    {
        CancelInvoke(nameof(HideSpinSpeedUI)); // Cancel any previously scheduled hide
        SpinSpeedEffect.gameObject.SetActive(true);
        SpinSpeedEffect.SpinSpeedEffect.Rebind();
        SpinSpeedEffect.Icon.sprite = modeIcon;
        SpinSpeedEffect.modeName.text = modeName;
        Invoke(nameof(HideSpinSpeedUI) , 2f); // Schedule the new hide
    }

    public void HideSpinSpeedUI ()
    {
        SpinSpeedEffect.gameObject.SetActive(false);
    }


    public bool NormalSpin ()
    {
        return isNormalSpin;
    }

    public bool QuickSpin ()
    {
        return isQuickSpin;
    }

    public bool TurboSpin ()
    {
        return isTurboSpin;
    }



    private void Update ()
    {
        if (CommandCenter.Instance)
        {
            bool isFirstTime = CommandCenter.Instance.gridManager_.IsFirstTime();
            float targetSpeed = IsFillingGrid && !isFirstTime ? degreesPerSecond * 12 : degreesPerSecond;

            // Smoothly interpolate between current speed and target speed
            currentSpeed = Mathf.Lerp(currentSpeed , targetSpeed , Time.deltaTime / smoothTime);

            // Apply rotation
            spinners.transform.Rotate(new Vector3(0 , 0 , currentSpeed) * Time.deltaTime);
        }
    }




    public void ActivateFillSpin ()
    {
        IsFillingGrid = true;
    }

    public void DeactivateFillSpin ()
    {
        IsFillingGrid = false;
    }
    public bool canSpin()
    {
        return CanSpin;
    }   
    public void SetCanSpin ( bool canSpin )
    {
        CanSpin = canSpin;
    }
    [ContextMenu("Spin")]
    public void Spin ()
    {
        Debug.Log("spin");
        if (!CanSpin) { return; }
        Debug.Log("Can spin");
        if (CommandCenter.Instance.gridManager_.IsRefreshingGrid()) { return; }
        Debug.Log("1");
        if (CommandCenter.Instance.gridManager_.IsGridRefilling()) { return; }
        Debug.Log("2");
        if (CommandCenter.Instance.gridManager_.IsCascading()) { return; }
        Debug.Log("3");
        if (!CommandCenter.Instance.gridManager_.IsNormalWinSequenceDone()) { return; }
        Debug.Log("4");
        if (CanSpin)
        {
            ActivateFillSpin();
            CanSpin = false;
            CommandCenter.Instance.apiManager_.gameDataApi.FetchData();
            // Debug.Log($"Spin is Demo{CommandCenter.Instance.gameModeManager.IsDemoMode()}");
            disableButtons();
            StartCoroutine(WaitForDataFetched());
            SpinAnims.PlaySpinAnim();
            SpinAnims.StopIdleSpinAnim();
            CommandCenter.Instance.soundManager_.PlaySound("Base_Spin");
        }
    }
    IEnumerator WaitForDataFetched ()
    {
        if (CommandCenter.Instance.gameMode == GameMode.Demo)
        {
            // Data has been fetched — continue execution here
            CommandCenter.Instance.gridManager_.RefreshGrid();
            CommandCenter.Instance.currencyManager_.DecreaseCash();
        }
        else
        {

           
        }

        if (!CommandCenter.Instance.freeSpinManager_.IsFreeGame())
        {
            CommandCenter.Instance.currencyManager_.ResetWinAmount();
        }

        yield return null;
    }
    public void enableButtons ()
    {
        Debug.Log("enable Buttons!");
        for (int i = 0 ; i < Buttons.Length ; i++)
        {
            Button btn = Buttons [i].GetComponentInChildren<Button>();
            if (!btn.interactable)
            {
                btn.interactable = true;
            }
        }
    }

    public void disableButtons ()
    {

        for (int i = 0 ; i < Buttons.Length ; i++)
        {
            Button btn = Buttons [i].GetComponentInChildren<Button>();
            if (btn.interactable)
            {
                btn.interactable = false;
            }
        }

    }
}
