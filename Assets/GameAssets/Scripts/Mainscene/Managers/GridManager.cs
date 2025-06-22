using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class CardPos
{
    public List<GameObject> CardPosInRow = new List<GameObject>();
}
public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private FillGridLogic fillGridLogic;
    [SerializeField] private RefillGridLogic refillGridLogic;
    [SerializeField] private GameObject initCard;
    [SerializeField] private GameObject SlotsHolder;
    [SerializeField] private bool isRefreshing = false;
    [SerializeField] private bool isFirstTime = false;
    [SerializeField] private bool isRefilling = false;
    [SerializeField] private bool isNormalWnSequenceDone = false;
    [SerializeField] private bool isRefillingSequenceDone = true;
    [SerializeField] private bool isSequenceComplete = false;
    [SerializeField] private bool isCascading = false;
    [SerializeField] private bool isCheckingForWins = false;
    [SerializeField] private int totalObjectsToPlace = 20;
    private float elapsedTime;
    private Vector3 startPosition;
    [SerializeField] private List<CardPos> CardSlots = new List<CardPos>();
    [SerializeField] private Sprite [] initCards;
    [SerializeField] private winningBg_Wild winningBg_Wild;
    [Header("Effects")]
    [SerializeField] private StartEffect startEffect;
    [SerializeField] private WildEffect wildEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        startPosition = transform.localPosition;
        InitializeSlots();
        InitializeGrid();
        CommandCenter.Instance.spinManager_.disableButtons();
        isNormalWnSequenceDone = true;
    }

    void InitializeSlots ()
    {
        List<GameObject> items = new List<GameObject>();
        foreach (Transform t in SlotsHolder.transform)
        {
            items.Add(t.gameObject);
        }

        CardSlots.Clear();
        for (int col = 0 ; col < 5 ; col++)
        {
            CardSlots.Add(new CardPos());
        }

        int index = 0;
        for (int col = 0 ; col < CardSlots.Count ; col++)
        {
            for (int row = 0 ; row < 4 ; row++)
            {
                CardSlots [col].CardPosInRow.Add(items [index]);
                index++;
            }
        }
    }

    void InitializeGrid ()
    {
        //Debug.Log("InitializeGrid");
        StartCoroutine(Initialization());
    }

    private IEnumerator Initialization ()
    {
        yield return new WaitForSeconds(.5f);
        Deck [] Decks = CommandCenter.Instance.deckManager_.GetDecks();
        int colCount = 5;
        int rowCount = 4;
        float delayIncrement = 0.1f; // Reduce delay for faster animation
        CommandCenter.Instance.spinManager_.disableButtons();
        Invoke(nameof(GridPopulator) , 1f);
        ShowStartEffects();
        Invoke(nameof(HideStartEffects) , 2f);
        Debug.Log("Initialization");
        string [] sounds = new string [] { "Base_Firstenter_01" , "Base_Firstenter_02" , "Base_Firstenter_03" , "Base_Firstenter_04" , "Base_Firstenter_05" };
        int animsToPlay = 20;
        for (int col = 0 ; col < colCount ; col++)
        {
            var currDeck = Decks [col];
            for (int row = 0 ; row < rowCount ; row++)
            {
                Slot slot = CardSlots [col].CardPosInRow [row].transform.GetComponent<Slot>();
                Transform target = CardSlots [col].CardPosInRow [row].transform;

                yield return new WaitForSeconds(delayIncrement);
                // Spawn card 
                GameObject card = Instantiate(initCard , target);
                card.transform.localPosition = Vector3.zero;
                // Fade the 2D sprite & assignCard

                Image cardImage = card.GetComponentInChildren<Image>();
                int index = row % initCards.Length;
                cardImage.sprite = initCards [index];

                if (cardImage != null)
                {
                    cardImage.DOFade(0 , 1).OnComplete(() =>
                    {
                        animsToPlay--;
                        Destroy(card);
                    });
                }

            }
            CommandCenter.Instance.soundManager_.PlaySound(sounds [col]);
        }

        yield return new WaitUntil(() => animsToPlay <= 0);
        Debug.Log("All init animations done!");
        CommandCenter.Instance.spinManager_.SetCanSpin(true);
        CommandCenter.Instance.mainMenuController_.ToggleExtraBetInfo();
        CommandCenter.Instance.mainMenuController_.ToggleExtraBetMenu();
        CommandCenter.Instance.spinManager_.enableButtons();
    }

    public void GridPopulator ()
    {
        StartCoroutine(PopulateGrid());
    }

    private void ShowStartEffects ()
    {
        startEffect.showEffect();
    }

    public void HideStartEffects ()
    {
        startEffect.HideEffect();
    }


    // Update is called once per frame
    void Update ()
    {
        if (isRefreshing)
        {
            float duration = SetDuration(); // or set directly
            Vector3 target = new Vector3(0 , -450 , 0);
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            SlotsHolder.transform.localPosition = Vector3.Lerp(startPosition , target , t);

            if (elapsedTime >= duration)
            {
                StartCoroutine(HandlePostRefresh());
                isRefreshing = false;
                elapsedTime = 0;
            }
        }

    }

    private IEnumerator HandlePostRefresh ()
    {
        // Ensure it lands exactly at target
        SlotsHolder.transform.localPosition = new Vector3(0 , -450 , 0);

        ReturnCardsToPool();

        yield return null; // Wait a single frame to avoid snap

        SlotsHolder.transform.localPosition = startPosition;

        yield return StartCoroutine(PopulateGrid());
    }


    float SetDuration ()
    {
        if (CommandCenter.Instance)
        {
            if (CommandCenter.Instance.spinManager_.NormalSpin())
            {
                return 0.25f;
            }
            else if (CommandCenter.Instance.spinManager_.QuickSpin())
            {
                return 0.2f;
            }
            else if (CommandCenter.Instance.spinManager_.TurboSpin())
            {
                return 0.1f;
            }
        }
        return 0;
    }

    [ContextMenu("Refresh Grid")]
    public void RefreshGrid ()
    {
        isSequenceComplete = false;
        isNormalWnSequenceDone = false;
        isRefreshing = true;
        elapsedTime = 0;
    }

    
   
    void ReturnCardsToPool ()
    {
        // First pass: Return cards via owner reference
        foreach (CardPos pos in CardSlots)
        {
            foreach (var obj in pos.CardPosInRow)
            {
                var slot = obj.GetComponent<Slot>();
                if (slot != null)
                {
                    var card = slot.GetTheOwner();
                    if (card != null)
                    {
                        CommandCenter.Instance.poolManager_.ReturnToPool(PoolType.Cards , card);
                        slot.RemoveOwner();
                    }
                }
            }
        }
    }


    public bool IsGridSpaceAvailable ()
    {
        List<Transform> emptySlot = new List<Transform>();
        foreach (CardPos pos in CardSlots)
        {
            foreach (var slot in pos.CardPosInRow)
            {
                if (!slot.GetComponent<Slot>().IsOwnerAvailable())
                {
                    emptySlot.Add(slot.transform);
                }
            }
        }

        if (emptySlot.Count < totalObjectsToPlace)
        {
            if (emptySlot.Count > 0)
            {
                Debug.LogWarning($"some slots are empty");
                return true;
            }
            else
            {
                Debug.LogWarning("Not enough available slots to place all the cards.");
                return false;

            }
        }

        return true;
    }

    [ContextMenu("Populate Grid")]
    public IEnumerator PopulateGrid ()
    {
        //Debug.Log("Populate Grid");
        if (IsGridSpaceAvailable())
        {
            if (CommandCenter.Instance.spinManager_.NormalSpin())
            {
                //normal spin
                yield return StartCoroutine(NormalFill());
            }
            else if (CommandCenter.Instance.spinManager_.QuickSpin())
            {
                // quickSpin
                yield return StartCoroutine(QuickFill());

            }
            else if (CommandCenter.Instance.spinManager_.TurboSpin())
            {
                // TurboSpin
                yield return StartCoroutine(TurboFill());
            }
        }
    }

    #region
    private IEnumerator NormalFill ()
    {
        yield return StartCoroutine(fillGridLogic.normalfillGrid(CardSlots , isFirstTime,wildEffect,winningBg_Wild));

        if(CommandCenter.Instance.featureManager_.GetActiveFeature() != Features.None)
        {
            CommandCenter.Instance.featureManager_.IncreaseSpinCounter();
        }


        if (isFirstTime)
            isFirstTime = false;
        CommandCenter.Instance.spinManager_.DeactivateFillSpin();
        if(CommandCenter.Instance.freeSpinManager_.IsFreeGame())
        {
            CommandCenter.Instance.freeSpinManager_.UpdateFreeSpins();
        }
        else
        {
            if(CommandCenter.Instance.autoSpinManager_.isAutoSpin()
                && CommandCenter.Instance.autoSpinManager_.Spins()>0)
            {
                CommandCenter.Instance.autoSpinManager_.ReduceSpins();
            }
        }

        yield return null;
    }

  

    public IEnumerator QuickFill ()
    {
        Debug.Log("quick spin");
        yield return StartCoroutine(fillGridLogic.quickfillGrid(CardSlots));
        if (isFirstTime)
        {
            isFirstTime = false;
        }
        Debug.Log("all animations Done!");
        if (CommandCenter.Instance.featureManager_.GetActiveFeature() != Features.None)
        {
            CommandCenter.Instance.featureManager_.IncreaseSpinCounter();
        }
        CommandCenter.Instance.spinManager_.DeactivateFillSpin();
        if (CommandCenter.Instance.freeSpinManager_.IsFreeGame())
        {
            CommandCenter.Instance.freeSpinManager_.UpdateFreeSpins();
        }
        else
        {
            if (CommandCenter.Instance.autoSpinManager_.isAutoSpin()
                && CommandCenter.Instance.autoSpinManager_.Spins() > 0)
            {
                CommandCenter.Instance.autoSpinManager_.ReduceSpins();
            }
        }
    }

    public IEnumerator TurboFill ()
    {
        Debug.Log("Turbo spin");
        yield return StartCoroutine(fillGridLogic.turbofillGrid(CardSlots));
        if (isFirstTime)
        {
            isFirstTime = false;
        }
        Debug.Log("all animations Done!");
        if (CommandCenter.Instance.featureManager_.GetActiveFeature() != Features.None)
        {
            CommandCenter.Instance.featureManager_.IncreaseSpinCounter();
        }
        CommandCenter.Instance.spinManager_.DeactivateFillSpin();
        if (CommandCenter.Instance.freeSpinManager_.IsFreeGame())
        {
            CommandCenter.Instance.freeSpinManager_.UpdateFreeSpins();
        }
        else
        {
            if (CommandCenter.Instance.autoSpinManager_.isAutoSpin()
                && CommandCenter.Instance.autoSpinManager_.Spins() > 0)
            {
                CommandCenter.Instance.autoSpinManager_.ReduceSpins();
            }
        }
    }

    #endregion
    public void RefillGrid ()
    {
        //Debug.Log("Refill Grid");
        if (IsGridSpaceAvailable())
        {
            isRefilling = true;
            if (CommandCenter.Instance.spinManager_.NormalSpin())
            {
                //normal spin
                StartCoroutine(NormalReFill());
            }
            else if (CommandCenter.Instance.spinManager_.QuickSpin())
            {
                // quickSpin
                StartCoroutine(QuickReFill());

            }
            else if (CommandCenter.Instance.spinManager_.TurboSpin())
            {
                // TurboSpin
                StartCoroutine(TurboReFill());
            }
        }
        else
        {
            SetIsRefilling(false);
        }
    }
    private IEnumerator NormalReFill ()
    {
        isCascading = true;
        isRefillingSequenceDone = false;
        yield return StartCoroutine(refillGridLogic.normalRefillGrid(CardSlots));
        SetIsRefilling(false);
    }

    public IEnumerator QuickReFill ()
    {
        isCascading = true;
        isRefillingSequenceDone = false;
        //Debug.Log("quick spin");
        yield return StartCoroutine(refillGridLogic.quickRefillGrid(CardSlots));

        SetIsRefilling(false);
    }

    public IEnumerator TurboReFill ()
    {
        isCascading = true;
        isRefillingSequenceDone = false;
        yield return StartCoroutine(refillGridLogic.turboRefillGrid(CardSlots));
        SetIsRefilling(false);
    }

    public void checkForWinings ()
    {
        GetComponentInChildren<CheckForWinnings>().checkForWinings();
    }

    public CheckForWinnings GetCheckForWinnings ()
    {
        return GetComponentInChildren<CheckForWinnings>();
    }

    public void SetIsRefilling ( bool isRefilling_ )
    {
        isRefilling = isRefilling_;
    }

    public void setisNormalWnSequenceDone ( bool isActive )
    {
        isNormalWnSequenceDone = isActive;
    }

    public void setIsCascading ( bool cascade )
    {
        isCascading = cascade;
    }
    public bool IsGridRefilling ()
    {
        return isRefilling;
    }

    public bool IsFirstTime ()
    {
        return isFirstTime;
    }

    public bool IsRefreshingGrid ()
    {
        return isRefreshing;
    }

    public List<CardPos> GetGridCards ()
    {
        return CardSlots;
    }

    public Slot GetSlot ( int row , int col )
    {
        if (row < 0 || row >= CardSlots.Count || col < 0 || col >= CardSlots [row].CardPosInRow.Count)
        {
            Debug.LogError("Invalid row or column index");
            return null;
        }
        return CardSlots [col].CardPosInRow [row].GetComponent<Slot>();
    }

    public winningBg_Wild GetWinningBg_Wild ()
    {
        return winningBg_Wild;
    }

    public bool IsRefillSequnceDone ()
    {
        return isRefillingSequenceDone;
    }
    public void SetIsRefillSequenceDone ( bool isDone )
    {
        isRefillingSequenceDone = isDone;
    }

    public bool IsCascading ()
    {
        return isCascading;
    }

    public bool IsNormalWinSequenceDone ()
    {
        return isNormalWnSequenceDone;
    }

    public void moveCardsBacktoSlots ()
    {
        //Debug.Log("Return to normal slots ! " + "Stack trace:\n" + Environment.StackTrace);
        List<CardPos> winCardSlots = new List<CardPos>(CommandCenter.Instance.winLoseManager_.GetWinCardSlots());

        for (int col = 0 ; col < CardSlots.Count ; col++)
        {
            for (int row = 0 ; row < CardSlots [col].CardPosInRow.Count ; row++)
            {
                WinSlot winSlot = winCardSlots [col].CardPosInRow [row].GetComponent<WinSlot>();
                Slot slot = CardSlots [col].CardPosInRow [row].GetComponent<Slot>();
                if (winSlot.GetTheOwner())
                {
                    GameObject card = winSlot.GetTheOwner();
                    if (card != null)
                    {
                        if (card == slot.GetTheOwner())
                        {
                            card.transform.SetParent(slot.transform);
                            card.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            CommandCenter.Instance.poolManager_.ReturnToPool(PoolType.Cards,card);
                            winSlot.RemoveOwner();
                        }
                    }
                }
            }
        }
    }

}
