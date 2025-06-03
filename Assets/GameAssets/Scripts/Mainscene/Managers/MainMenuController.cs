using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject FeatureMenu;

    [Header("References")]
    public ExtraBetMenu extraBetMenu;
    public Settings settings;
    public GameRules gameRules;
    //public BetMenu betMenu;
    public FeatureBuyMenu featureBuyMenu;
    //public AutoSpinMenu autoSpinMenu;

    public void showFeatureMenu ()
    {
        FeatureMenu.SetActive(true);
    }

    public void hideFeatureMenu ()
    {
        FeatureMenu.SetActive(false);
    }

    public void ToggleExtraBetInfo ()
    {
        extraBetMenu.ToggleEx_Info();
    }

    public void ToggleExtraBetMenu ()
    {
        extraBetMenu.ToggleExtraBetMenu();
    }

    public void ToggleExtraBet ()
    {
        extraBetMenu.ToggleExtraBet();
    }

    public void ToggleSettings ()
    {
        settings.ToggleSettings();
    }

    public void ToggleGameRules ()
    {
        gameRules.ToggleGameRules();
    }

    public void ToggleBetMenu ()
    {
        //betMenu.ToggleBetMenu();
    }

    public void ToggleFeatureBuyMenu ()
    {
        featureBuyMenu.ToggleFeatureBuy();
    }

    public void ToggleAutoSpinMenu ()
    {
        //autoSpinMenu.ToogleAutoSpin();
    }

    public void Spin ()
    {
        //CommandCenter.Instance.spinManager_.Spin();
        //CommandCenter.Instance.soundManager_.PlaySound("Base_Spin");
    }
}
