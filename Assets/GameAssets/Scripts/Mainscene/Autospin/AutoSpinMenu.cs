using DG.Tweening;
using UnityEngine;

public class AutoSpinMenu : MonoBehaviour
{
    [SerializeField] private GameObject Menu;
    [SerializeField] private RectTransform autoSpinMenu;
    [SerializeField] private bool IsAutoSpinMenuActive = false;
    [SerializeField] private Spins selectedSpins;
    [SerializeField] private int spins;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {

    }

    private void showAutospinMenu ()
    {
        IsAutoSpinMenuActive = true;
        Menu.SetActive(true);
        autoSpinMenu.gameObject.SetActive(true);
        autoSpinMenu.DOAnchorPos(Vector2.zero , 0.25f);
    }
    private void hideAutospinMenu ()
    {
        Vector2 target = new Vector2(0 , -300f);
        autoSpinMenu.DOAnchorPos(target , 0.25f).OnComplete(() =>
        {
            autoSpinMenu.gameObject.SetActive(false);
            Menu.SetActive(false);
            IsAutoSpinMenuActive = false;
        });
    }

    public void ToogleAutoSpin ()
    {
        if (IsAutoSpinMenuActive)
        {
            hideAutospinMenu();
        }
        else
        {
            showAutospinMenu();
        }
    }

    public void GetSelectedSpins ( Spins selected , int spins_ )
    {
        selectedSpins = selected;
        spins = spins_;
        CommandCenter.Instance.autoSpinManager_.GetSpins(spins);
        CommandCenter.Instance.autoSpinManager_.ActivateAutospin();
        //start spin
        Debug.Log("Start spin");
        //Disable Autospin menu
        ToogleAutoSpin();

        Invoke(nameof(StartSpin) , .5f);
    }

    void StartSpin ()
    {
        CommandCenter.Instance.mainMenuController_.Spin();
    }
}
