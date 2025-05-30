using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject FeatureMenu;

    [Header("References")]
    //public Ex_Menu ex_Menu;
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
        //ex_Menu.ToggleEx_Info();
    }

    public void ToggleExtraBetMenu ()
    {
       // ex_Menu.ToggleExtraBetMenu();
    }

    public void ToggleExtraBet ()
    {
        //ex_Menu.ToggleExtraBet();
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
