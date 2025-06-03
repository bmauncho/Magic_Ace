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
    [SerializeField] private Animator IdleAnim;
    [SerializeField] private Animator SpinAnim;
    [SerializeField] private bool canPlayIdle;
    private float Timer = 0f;

    public SpinSpeedUI SpinSpeedEffect;
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
            bool isFirstTime = true;
            float targetSpeed = IsFillingGrid && !isFirstTime ? degreesPerSecond * 12 : degreesPerSecond;

            // Smoothly interpolate between current speed and target speed
            currentSpeed = Mathf.Lerp(currentSpeed , targetSpeed , Time.deltaTime / smoothTime);

            // Apply rotation
            spinners.transform.Rotate(new Vector3(0 , 0 , currentSpeed) * Time.deltaTime);

            if (canPlayIdle)
            {
                Timer += Time.deltaTime;
                if (Timer >= 5f)
                {
                    if (IdleAnim.gameObject.activeInHierarchy)
                    {
                        PlayIdleAnim();
                    }
                }
            }
        }
    }




    public void ActivateFillSpin ()
    {
        IsFillingGrid = true;
    }

    public void DeactivateFillSpin ()
    {
        IsFillingGrid = false;
        canPlayIdle = true;
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
        if (CanSpin)
        {
            ActivateFillSpin();
            CanSpin = false;
            canPlayIdle = false;
            disableButtons();
            StartCoroutine(WaitForDataFetched());
        }
    }
    IEnumerator WaitForDataFetched ()
    {
        yield return null;
    }
    public void enableButtons ()
    {
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

    private void PlayIdleAnim ()
    {
        Timer = 0f;
    }
}
