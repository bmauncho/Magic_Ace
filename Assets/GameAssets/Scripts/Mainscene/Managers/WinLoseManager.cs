using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class winCardData
{
    public string name;
    public int row;
    public int col;
}
public enum WinType
{
    None,
    Normal,
    Wild,
    Both
}
public class WinLoseManager : MonoBehaviour
{
    [SerializeField] private WinType currentWinType;
    [Header("References")]
    MultiplierManager multiplierManager;
    APIManager apiManager;
    GridManager gridManager;
    public WinSequence winSequence_;
    public NormalSequence normalSequence_;
    public WildSequnce wildSequnce_;
    public DemoCheckWin demoCheckWin_;
    public NormalCheckWin normalCheckWin_;
    public LoseSequence loseSequence_;
    public EndWin endWin_;
    public RefillGrid refillGrid;
    public RotateGoldenCards rotateGoldenCards;
    public FlipCards flipCards;
    public JumpCards jumpCards;
    public GemCollector gemCollector;
    public WinAmount winAmount;
    public NormalGameWinUi normalGameWinUi;

    [Header("Win Data")]
    [SerializeField] private bool isWin = false;
    [SerializeField] private bool isWinSequenceRunning = false;

    [Header("Win Cards Data")]
    [SerializeField] private List<winCardData> WinningCards = new List<winCardData>();

    [Header("Win Slots Data")]
    [SerializeField] private GameObject SlotsHolder;
    [SerializeField] private List<CardPos> AvailableWinCardSlots = new List<CardPos>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        apiManager = CommandCenter.Instance.apiManager_;
        gridManager = CommandCenter.Instance.gridManager_;
        multiplierManager = CommandCenter.Instance.multiplierManager_;
    }

    [ContextMenu("InitializeWinSlots")]
    private void InitializeWinSlots ()
    {
        List<GameObject> items = new List<GameObject>();
        foreach (Transform t in SlotsHolder.transform)
        {
            items.Add(t.gameObject);
        }

        AvailableWinCardSlots.Clear();
        for (int col = 0 ; col < 5 ; col++)
        {
            AvailableWinCardSlots.Add(new CardPos());
        }
        int index = 0;
        for (int col = 0 ; col < AvailableWinCardSlots.Count ; col++)
        {
            for (int row = 0 ; row < 4 ; row++)
            {
                AvailableWinCardSlots [col].CardPosInRow.Add(items [index]);
                index++;
            }
        }
    }

    public List<winCardData> GetWinningCards ()
    {
        return WinningCards;
    }

    public List<CardPos> GetWinCardSlots ()
    {
        return AvailableWinCardSlots;
    }

    public void ClearWinningCards ()
    {
        WinningCards.Clear(); 
    }

    public void WinLoseSequence ( Action OnComplete = null )
    {
        ClearWinningCards();
        if (IsPlayerWin())
        {
            Debug.Log("won!");
            //player has won
            StartCoroutine(winSequence(OnComplete));
        }
        else
        {
            Debug.Log("lost!");
            //player has lost
            StartCoroutine(loseSequence(OnComplete));
        }
    }

    public IEnumerator winSequence(Action OnComplete = null )
    {
        if (isWinSequenceRunning) yield break;
        isWinSequenceRunning = true;

        Debug.Log("Handle Win");
        yield return StartCoroutine(winSequence_.WinEffect(WinningCards, OnComplete));

        yield return null;
    }  
    public IEnumerator loseSequence(Action OnComplete = null )
    {
        Debug.Log("Handle No Win");
        winSequence_.DeactivateWinBg();
        CommandCenter.Instance.gridManager_.GetWinningBg_Wild().Deactivate();
        CommandCenter.Instance.gridManager_.moveCardsBacktoSlots();
        yield return StartCoroutine(loseSequence_.loseSequence(OnComplete));
        yield return null;
    }

    public bool IsPlayerWin ()
    {
        if (CommandCenter.Instance.gameMode == GameMode.Demo)
        {
            currentWinType = demoCheckWin_.CheckWin(WinningCards);
            if (currentWinType == WinType.None)
            {
                isWin = false;
            }
            else
            {
                isWin = true;
            }
        }
        else
        {
            if (CommandCenter.Instance.featureManager_.GetActiveFeature() == Features.None)
            {
               //normal win logic
               currentWinType = normalCheckWin_.CheckWin(WinningCards);
                if(currentWinType == WinType.None)
                {
                    isWin = false;
                }
                else
                {
                    isWin = true;
                }
            }
            else
            {
                currentWinType = demoCheckWin_.CheckWin(WinningCards);
                if (currentWinType == WinType.None)
                {
                    isWin = false;
                }
                else
                {
                    isWin = true;
                }
            }
        }
        return isWin;
    }

    public WinType GetWinType ()
    {
        return currentWinType;
    }
    public void ResetWinType ()
    {
        currentWinType = WinType.None;
    }

    public IEnumerator RecheckWin ()
    {
        Debug.Log("recheckWin");
        ClearWinningCards();
        if (IsPlayerWin())
        {
            bool isDone = false;

            StartCoroutine(winSequence(() =>
            {
                Debug.Log("Recheck win Done!");
                isDone = true;
            }));

            yield return new WaitWhile(() => !isDone);

        }
        else
        {
            StartCoroutine(loseSequence());
        }
    }

    public void RecheckForWildCards ()
    {
        int rowCount = 4;
        int colCount = 5;

        List<CardPos> Cards = new List<CardPos>(CommandCenter.Instance.gridManager_.GetGridCards());
        HashSet<string> addedKeys = new HashSet<string>();
        ClearWinningCards();

        HashSet<string> allTypes = new HashSet<string>();
        Dictionary<int , bool> wildCols = new Dictionary<int , bool>();


        // Pass 1 � gather all card types and track wild columns
        for (int col = 0 ; col < colCount ; col++)
        {
            for (int row = 0 ; row < rowCount ; row++)
            {
                Slot slot = Cards [col].CardPosInRow [row].GetComponent<Slot>();
                GameObject cardObj = slot.GetTheOwner();
                if (cardObj == null) continue;

                string type = cardObj.GetComponent<Card>().GetCardType().ToString();
                allTypes.Add(type);

                if (type == CardType.SCATTER.ToString())
                    wildCols [col] = true;
            }
        }

        // Wild-only win check
        if (wildCols.Count >= 1)
        {
            List<winCardData> tempWildCards = new List<winCardData>();

            foreach (var col in wildCols.Keys)
            {
                for (int row = 0 ; row < rowCount ; row++)
                {
                    var slot = Cards [col].CardPosInRow [row].GetComponent<Slot>();
                    var cardObj = slot.GetTheOwner();
                    if (cardObj == null) continue;

                    var card = cardObj.GetComponent<Card>();
                    if (card.GetCardType().ToString() == CardType.SCATTER.ToString())
                    {
                        string key = $"{col}-{row}";

                        if (!addedKeys.Contains(key))
                        {
                            tempWildCards.Add(new winCardData
                            {
                                name = CardType.SCATTER.ToString() ,
                                row = row ,
                                col = col
                            });
                            addedKeys.Add(key);
                        }
                    }
                }
            }

            if (tempWildCards.Count > 2)
            {
                WinningCards.AddRange(tempWildCards);
            }
        }
    }

    public void EndTheWinSequence ( 
        List<(GameObject card, List<(int col, int row)> Positions)> remainingGoldenCards = null ,
        List<GameObject> remainingBigJokerCards = null , 
        List<GameObject> remainingSuperJokerCards = null , 
        Action OnComplete = null,
        List<GameObject> wildCards =null,
        List<GameObject> remainingCards = null)
    {
        StartCoroutine(endWin(
            remainingGoldenCards , 
            remainingBigJokerCards ,
            remainingSuperJokerCards,
            OnComplete,
            wildCards,
            remainingCards));
    }

    private IEnumerator endWin ( 
        List<(GameObject card, List<(int col, int row)> Positions)> remainingGoldenCards = null ,
        List<GameObject> remainingBigJokerCards = null , 
        List<GameObject> remainingSuperJokerCards = null , 
        Action OnComplete = null,
        List<GameObject> wildCards = null,
        List<GameObject> remainingCards = null)
    {
        Debug.Log("End Win Sequence Started");
        yield return new WaitWhile(() => !winSequence_.IsWinSequenceDone());

        if (!CommandCenter.Instance.freeSpinManager_.IsFreeGameWin())
        {
            yield return StartCoroutine(refillGrid.RefillTheGrid());
            yield return new WaitWhile(() => !apiManager.refillApi.isRefillCardsFetched());
        }

        Debug.Log("End Win Sequence ...");
        yield return StartCoroutine(flipCards.flipBack(
            remainingGoldenCards , 
            remainingBigJokerCards,
            remainingSuperJokerCards));

        yield return new WaitWhile(() => gridManager.IsGridRefilling());
        yield return new WaitWhile(() => winSequence_.IsFlipping());
        //Debug.Log($"is grid reflling! : {gridManager.IsGridRefilling()}");
        WinSequence winSequence = GetComponent<WinSequence>();
        //Debug.Log($"remaining Big joker cards {remainingBigJokerCards.Count}");
        if (remainingBigJokerCards != null)
        {
            //Debug.Log("List is not null");
            if (remainingBigJokerCards.Count > 0)
            {
                yield return StartCoroutine(jumpCards.JumpBigJokerCards(remainingBigJokerCards));
            }
        }

        if (wildCards != null && remainingCards!= null)
        {
            if(wildCards.Count > 0 && remainingCards.Count >0)
            {
                yield return StartCoroutine(wildSequnce_.BothWildSequence(
                    wildCards,
                    remainingCards));
            }
        }


        if (remainingSuperJokerCards != null)
        {
            if (remainingSuperJokerCards.Count > 0)
            {

                Debug.Log("Super joker cards are present");
                if (multiplierManager.GetCurrentType() == MultiplierType.Free)
                {
                    Debug.Log("Super joker cards are present ... In free game");
                    if (multiplierManager.GetFreeSpinUpgradeCount() < multiplierManager.GetMaxFreeSpinUpgradeCount())
                    {
                        Debug.Log("Super joker cards are present ... In free game...sequence");
                        yield return StartCoroutine(gemCollector.collectFreeGameGems(remainingSuperJokerCards));
                    }
                }
                else
                {
                    Debug.Log("Super joker cards are present ... In normal Game");
                    yield return StartCoroutine(gemCollector.CollectGems(remainingSuperJokerCards));
                }
            }
        }


        //Debug.Log("Grid Refilled");
        isWin = false;
        OnComplete?.Invoke();

        isWinSequenceRunning = false;
        //increase refill counter
        if (!CommandCenter.Instance.freeSpinManager_.IsFreeGameWin() &&
            !CommandCenter.Instance.freeSpinManager_.IsFreeSpinRetrigger() &&
            CommandCenter.Instance.featureManager_.GetActiveFeature() != Features.None)
        {
            CommandCenter.Instance.featureManager_.IncreaseRefillCounter();
        }
        else
        {
            Debug.Log("Free Spin Win or freespin Retrigger, not increasing refill counter");
        }

        if (CommandCenter.Instance.freeSpinManager_.IsFreeGame())
        {
            CommandCenter.Instance.gridManager_.checkForWinings();
        }
        else
        {
            yield return StartCoroutine(RecheckWin());
        }
    }

    public bool IsWinSequencerunning ()
    {
        return isWinSequenceRunning;
    }
}
