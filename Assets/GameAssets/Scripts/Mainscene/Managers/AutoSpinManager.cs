using TMPro;
using UnityEngine;

public class AutoSpinManager : MonoBehaviour
{
    [SerializeField] private int spins;
    [SerializeField] private bool IsAutoSpin;
    [SerializeField] private GameObject AutoSpinUI;
    [SerializeField] private TMP_Text currentSpins;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {

    }

    public int Spins ()
    {
        return spins;
    }
    public void GetSpins ( int spins_ )
    {
        spins = spins_;
    }

    public void ReduceSpins ()
    {
        if (spins > 0)
        {
            spins--;
        }
        else
        {
            spins = 0;
        }
        updateSpins();
    }

    void updateSpins ()
    {
        string result = string.Empty;
        foreach (char c in spins.ToString())
        {
            result += $"<sprite name=Base_AutoSpin_FNT_{c}>";
        }
        currentSpins.text = result;
    }

    public bool isAutoSpin ()
    {
        return IsAutoSpin;
    }

    public void ActivateAutospin ()
    {
        IsAutoSpin = true;
        toggleButtons();
    }

    public void DeactivateAutospin ()
    {
        IsAutoSpin = false;
        toggleButtons();
    }

    public void toggleButtons ()
    {
        if (IsAutoSpin)
        {
            //disable buttons
            CommandCenter.Instance.spinManager_.disableButtons();
            AutoSpinUI.SetActive(true);
            updateSpins();
        }
        else
        {
            //enable buttons
            CommandCenter.Instance.spinManager_.enableButtons();
            AutoSpinUI.SetActive(false);
        }
    }


    public void SetAutoSpin ( bool isActive )
    {
        IsAutoSpin = isActive;
    }

}
