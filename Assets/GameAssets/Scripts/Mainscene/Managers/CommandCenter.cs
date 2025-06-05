using System;
using UnityEngine;
public enum GameMode
{
    Demo,
    Live,
}
public enum GameType
{
    Base,
    Free
}
public class CommandCenter : MonoBehaviour
{
    public static CommandCenter Instance { get; private set; }
    public GameMode gameMode; // Live or Demo
    public GameType gameType;
    public PoolManager poolManager_;
    public DeckManager deckManager_;
    public MainMenuController mainMenuController_;
    public SpinManager spinManager_;
    public CurrencyManager currencyManager_;
    public SettingsManager settingsManager_;
    public GameModeManager gameModeManager_;
    public AutoSpinManager autoSpinManager_;
    public MultiplierManager multiplierManager_;
    public HintsManager hintsManager_;
    public CardManager cardManager_;
    public ComboManager comboManager_;
    public APIManager apiManager_;
    public GridManager gridManager_;
    public FeatureManager featureManager_;
    public WinLoseManager winLoseManager_;
    private void Awake ()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("This is extra!");
            //Destroy(gameObject); // Ensures only one instance exists
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetGameMode ( GameMode mode )
    {
        gameMode = mode;
    }

    public GameMode GetTheGameMode ()
    {
        return gameMode;
    }

    public void SetGameType ( GameType type )
    {
        gameType = type;
    }

    public GameType GetTheGameType ()
    {
        return gameType;
    }

    public CardType ConvertToEnum ( string value )
    {
        if (Enum.TryParse<CardType>(value , ignoreCase: true , out var result))
        {
            return result;
        }
        else
        {
            Debug.LogError($"Invalid enum value: {value}");
            // Optionally return a default value or throw
            return default;
        }
    }

    private void OnDestroy ()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
