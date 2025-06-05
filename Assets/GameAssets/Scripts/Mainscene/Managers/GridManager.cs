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
        Invoke(nameof(GridPopulator) , 1f);
        Debug.Log("Initialization");
        string [] sounds = new string [] { "Base_Firstenter_01" , "Base_Firstenter_02" , "Base_Firstenter_03" , "Base_Firstenter_04" , "Base_Firstenter_05" };

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
                        Destroy(card);
                    });
                }

            }
        }
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
                Debug.LogError("Not enough available slots to place all the cards.");
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
        Deck [] Decks = CommandCenter.Instance.deckManager_.GetDecks();
        int rowCount = 4;
        int colCount = 5;
        int col = 0;
        int row = 0;
        int activeAnimations = 0;
        float delayIncrement = 0.1f;

        // Debug.Log("normal spin");
        
        List<int> wildColumns = new List<int>();
        List<(string cardName, List<(int col, int row)>)> wildCards = new List<(string cardName, List<(int col, int row)>)>();
        Card cardComponent = null;
        List<(Card wildcard, List<(int col, int row)>)> wildWinCards = new List<(Card wildcard, List<(int col, int row)>)>();
        for (col = 0 ; col < colCount ; col++)
        {
            bool preTriggerWild = false;
            var currDeck = Decks [col];
            int currentAnims = 0;
            bool ScatterFound = false;
            for (row = rowCount - 1 ; row >= 0 ; row--)
            {
                Slot slot = CardSlots [col].CardPosInRow [row].transform.GetComponent<Slot>();
                Transform target = CardSlots [col].CardPosInRow [row].transform;

                // Check for SUPER_JOKER in slot

                Card existingCard = slot.GetComponentInChildren<Card>();

                GameObject newCard = currDeck.DrawCard();
                newCard.transform.SetParent(target);
                cardComponent = newCard.GetComponent<Card>();

                if (isFirstTime)
                {
                    CommandCenter.Instance.cardManager_.setUpfirstTimeCards(cardComponent , col , row);
                }
                else
                {
                    CommandCenter.Instance.cardManager_.setUpCard(cardComponent , row , col);

                    if (cardComponent.GetCardType() == CardType.SCATTER)
                    {
                        ScatterFound = true;
                        wildColumns.Add(col);
                        wildCards.Add((cardComponent.GetCardType().ToString(), new List<(int col, int row)> { (col, row) }));
                    }

                    if (wildCards.Count >= 2)
                    {
                        preTriggerWild = true;
                    }

                    if (preTriggerWild && cardComponent.GetCardType() == CardType.SCATTER)
                    {
                        //wildEffect.ToggleEffect(col);
                        List<(int col, int row)> cardPos = new List<(int col, int row)>();
                        cardPos.Add((col, row));
                        wildWinCards.Add((cardComponent, cardPos));
                    }
                }

                slot.AddOwner(newCard); // Ensure owner is set properly

                activeAnimations++;
                currentAnims++;
                cardComponent.OnComplete += () => activeAnimations--;
                cardComponent.OnComplete += () => currentAnims--;

                float delay = preTriggerWild ? row * delayIncrement : 0f;
                yield return StartCoroutine(DelayedMove(cardComponent , target , delay , slot , false));
            }

            ScatterFound = false;
        }


        while (activeAnimations > 0)
            yield return null;

        Debug.Log("all animations Done!");

        if (isFirstTime)
            isFirstTime = false;
        CommandCenter.Instance.spinManager_.DeactivateFillSpin();
        yield return new WaitForSeconds(.25f);
        //return cards to slots
        yield return new WaitForSeconds(.25f);
        // Check for win
        yield return null;
    }

    // Separate coroutine to handle individual delays
    private IEnumerator DelayedMove ( Card card , Transform target , float delay , Slot _slot , bool isInitialization )
    {
        yield return new WaitForSeconds(delay);
        card.moveCard(target , _slot , isInitialization);
        yield return null;
    }

    public IEnumerator QuickFill ()
    {
        Debug.Log("quick spin");
        Deck [] Decks = CommandCenter.Instance.deckManager_.GetDecks();
        int colCount = 5;
        int rowCount = 4;
        int activeAnimations = 0;
        for (int col = 0 ; col < colCount ; col++)
        {
            var currDeck = Decks [col];
            for (int row = rowCount - 1 ; row >= 0 ; row--)
            {
                Slot slot = CardSlots [col].CardPosInRow [row].transform.GetComponent<Slot>();
                Transform target = CardSlots [col].CardPosInRow [row].transform;
                GameObject newCard = currDeck.DrawCard();
                newCard.transform.SetParent(target);

                Card cardComponent = newCard.GetComponent<Card>();
                CommandCenter.Instance.cardManager_.setUpCard(cardComponent , row , col);
                activeAnimations++;
                cardComponent.OnComplete += () => activeAnimations--;

                cardComponent.moveCard(target , slot);
            }
        }

        // Wait until all animations are completed
        while (activeAnimations > 0)
            yield return null;
        if (isFirstTime)
        {
            isFirstTime = false;
        }
        Debug.Log("all animations Done!");
        CommandCenter.Instance.spinManager_.DeactivateFillSpin();
        yield return new WaitForSeconds(.5f);
    }

    public IEnumerator TurboFill ()
    {
        Debug.Log("Turbo spin");
        Deck [] Decks = CommandCenter.Instance.deckManager_.GetDecks();
        int rowCount = 4;
        int colCount = 5;
        int activeAnimations = 0;
        for (int col = 0 ; col < colCount ; col++)
        {
            var currDeck = Decks [col];
            for (int row = rowCount - 1 ; row >= 0 ; row--)
            {
                Slot slot = CardSlots [col].CardPosInRow [row].transform.GetComponent<Slot>();
                Transform target = CardSlots [col].CardPosInRow [row].transform;

                GameObject newCard = currDeck.DrawCard();
                newCard.transform.SetParent(target);

                Card cardComponent = newCard.GetComponent<Card>();
                CommandCenter.Instance.cardManager_.setUpCard(cardComponent , row , col);
                // check if is a wining card 
                activeAnimations++;
                cardComponent.OnComplete += () => activeAnimations--;

                cardComponent.moveCard(target , slot);
            }
        }

        // Wait until all animations are completed
        while (activeAnimations > 0)
            yield return null;
        if (isFirstTime)
        {
            isFirstTime = false;
        }
        Debug.Log("all animations Done!");
        CommandCenter.Instance.spinManager_.DeactivateFillSpin();
        yield return new WaitForSeconds(.5f);
    }

    #endregion


    public void checkForWinings ()
    {

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
}
