using UnityEngine;
public enum GameType
{
    Base,
    Free
}
public class CommandCenter : MonoBehaviour
{
    public static CommandCenter Instance { get; private set; }
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

    public void SetGameType ( GameType type )
    {
        gameType = type;
    }

    public GameType GetTheGameType ()
    {
        return gameType;
    }

    private void OnDestroy ()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
