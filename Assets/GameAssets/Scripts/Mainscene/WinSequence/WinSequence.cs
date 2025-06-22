using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSequence : MonoBehaviour
{
    WinLoseManager winLoseManager;
    APIManager apiManager;
    GridManager gridManager;
    PoolManager poolManager;
    [SerializeField] private WildSequnce wildSequence;
    [SerializeField] private GameObject WinBg;
    [SerializeField] private bool isWinSequenceDone = false;
    [SerializeField] private bool isFlipping = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        apiManager = CommandCenter.Instance.apiManager_;
        gridManager = CommandCenter.Instance.gridManager_;
        poolManager = CommandCenter.Instance.poolManager_;
        winLoseManager = CommandCenter.Instance.winLoseManager_;
    }

    public void ActivateWinBg ()
    {
        WinBg.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void DeactivateWinBg ()
    {
        WinBg.GetComponent<CanvasGroup>().alpha = 0;
    }

    public IEnumerator WinEffect ( List<winCardData> WinningCards , Action OnComplete = null )
    {
        isWinSequenceDone = false;
        List<(GameObject slot, List<(int col, int row)> Positions)> OccuppiedSlots = new List<(GameObject, List<(int col, int row)>)>();
        ActivateWinBg();
        CommandCenter.Instance.gridManager_.GetWinningBg_Wild().Deactivate();

        yield return StartCoroutine(MoveCardsToWinSlots(WinningCards , OccuppiedSlots));

        CommandCenter.Instance.comboManager_.ShowCombo();
        //show multiplier
        CommandCenter.Instance.multiplierManager_.ShowMultiplier();
        yield return new WaitForSeconds(.25f);
        //play win effect
        yield return StartCoroutine(PlayWinEffect(WinningCards , OccuppiedSlots , OnComplete));

        yield return null;
    }

    private IEnumerator MoveCardsToWinSlots ( List<winCardData> WinningCards , List<(GameObject slot, List<(int col, int row)> Positions)> occupiedSlots )
    {
        if (WinningCards.Count == 0)
        {
            yield break;
        }
        List<CardPos> CardSlots = new List<CardPos>(gridManager.GetGridCards());
        List<CardPos> AvailableWinCardSlots = new List<CardPos>(winLoseManager.GetWinCardSlots());
        int hasOwner = 0;
        //check if they are empty
        for (int col = 0 ; col < AvailableWinCardSlots.Count ; col++)
        {
            for (int row = 0 ; row < AvailableWinCardSlots [col].CardPosInRow.Count ; row++)
            {
                GameObject owner = AvailableWinCardSlots [col].CardPosInRow [row].GetComponent<WinSlot>().GetTheOwner();
                if (owner != null)
                {
                    Card cardComponent = owner.GetComponent<Card>();
                    if (cardComponent.GetCardType() == CardType.SCATTER)
                    {
                        // Define a HashSet to store unique row and column pairs
                        HashSet<(int, int)> addedRowsCols = new HashSet<(int, int)>();
                        foreach (var card in WinningCards)
                        {
                            int cardrow = card.row;
                            int cardcol = card.col;

                            addedRowsCols.Add((cardrow, cardcol));
                        }

                        // Create the winCardData
                        winCardData winCardData = new winCardData
                        {
                            name = cardComponent.GetCardType().ToString() ,
                            row = row ,
                            col = col ,
                        };

                        // Check if the row and col combination already exists in the HashSet
                        if (!addedRowsCols.Contains((row, col)))
                        {
                            // If not already added, add to the HashSet and the WinningCards list
                            addedRowsCols.Add((row, col));
                            WinningCards.Add(winCardData);
                        }
                    }
                    else
                    {
                        hasOwner++;
                    }
                }
            }
        }
        if (hasOwner > 0)
        {
            Debug.LogWarning("Win Slots are not empty!");
            yield break;
        }
        //move  wining cards fron card slots to win slots

        for (int i = 0 ; i < WinningCards.Count ; i++)
        {
            for (int col = 0 ; col < CardSlots.Count ; col++)
            {
                for (int row = 0 ; row < CardSlots [col].CardPosInRow.Count ; row++)
                {
                    if (WinningCards [i].row == row && WinningCards [i].col == col)
                    {
                        Slot slot = CardSlots [col].CardPosInRow [row].GetComponent<Slot>();
                        WinSlot winSlot = AvailableWinCardSlots [col].CardPosInRow [row].GetComponent<WinSlot>();
                        GameObject card = slot.GetTheOwner();
                        if (card != null)
                        {
                            winSlot.AddOwner(card);
                            card.transform.SetParent(winSlot.transform);
                            List<(int col, int row)> cardPos = new List<(int col, int row)>();
                            cardPos.Add((col, row));
                            occupiedSlots.Add((winSlot.gameObject, cardPos));
                        }
                    }
                }
            }
        }

        //Debug.Log("Cards Moved to Win Slots!");
        yield return null;
    }

    private IEnumerator PlayWinEffect ( List<winCardData> WinningCards , 
        List<(GameObject slot, List<(int col, int row)> Positions)> occuppiedSlots , Action OnComplete = null )
    {
        Debug.Log("Play win effect!");
        yield return new WaitForSeconds(.25f);
        Sequence sequence = DOTween.Sequence(); // main sequence
        //Debug.Log("occupied slots: " + occuppiedSlots.Count);   
        for (int i = 0 ; i < occuppiedSlots.Count ; i++)
        {
            GameObject card = occuppiedSlots [i].slot.GetComponent<WinSlot>().GetTheOwner();
            int col = occuppiedSlots [i].Positions [0].col;
            int row = occuppiedSlots [i].Positions [0].row;
            card.transform.localScale = Vector3.one;

            if (card != null)
            {
                Card cardComponent = card.GetComponent<Card>();

                if (cardComponent.GetCardType() != CardType.SCATTER)
                {
                    cardComponent.ShowCardWinGlow();
                    Sequence cardSeq = DOTween.Sequence();
                    cardSeq.AppendCallback(() => card.transform.localScale = Vector3.one)
                           .Append(card.transform.DOPunchScale(new Vector3(.2f , .2f , .2f) , .5f , 10 , 1f))
                           .AppendCallback(() =>
                           {
                               cardComponent.HideCardWinGlow();
                               card.transform.localScale = Vector3.one;
                           });

                    sequence.Join(cardSeq);
                }
            }
        }


        yield return sequence.WaitForCompletion();

        if (winLoseManager.GetWinType() == WinType.Normal)
        {
            // show normal win effect
            yield return StartCoroutine(winLoseManager.winAmount.winAmountEffect(WinningCards));
        }
        else if (winLoseManager.GetWinType() == WinType.Wild)
        {
            //play wild effect

        }
        else if (winLoseManager.GetWinType() == WinType.Both)
        {
            //show win effect for both wild and normal
            yield return StartCoroutine(winLoseManager.winAmount.winAmountEffect(WinningCards));
        }

        yield return sequence.WaitForCompletion();
        //Debug.Log("hide cards");
        yield return StartCoroutine(hideCardsSequence(occuppiedSlots , WinningCards , OnComplete));
    }

    private IEnumerator hideCardsSequence ( List<(GameObject slot, List<(int col, int row)> Positions)> occuppiedSlots , 
        List<winCardData> winningCards , Action OnComplete = null )
    {
        Debug.Log("Hide Cards !");
        yield return new WaitForSeconds(.25f);
        List<GameObject> remainingCards = new List<GameObject>();
        List<GameObject> remainingWildCards = new List<GameObject>();
        List<(GameObject card, List<(int col, int row)> Positions)> remainingGoldenCards = new List<(GameObject card, List<(int col, int row)> Positions)>();
        List<GameObject> remainingBigJokerCards = new List<GameObject>();
        List<GameObject> remainingSuperJokerCards = new List<GameObject>();
        List<GameObject> cardsToHide = new List<GameObject>();
        List<GameObject> newOccupiedSlots = new List<GameObject>();
        for (int i = 0 ; i < occuppiedSlots.Count ; i++)
        {
            GameObject card = occuppiedSlots [i].slot.GetComponent<WinSlot>().GetTheOwner();
            newOccupiedSlots.Add(occuppiedSlots [i].slot);
            if (card != null)
            {
                Card cardComponent = card.GetComponent<Card>();
                // If the card is SCATTER, we don't return it to the pool YET
                if (cardComponent.isWildCard())
                {
                    //wild card sequence
                    remainingCards.Add(card);
                    remainingWildCards.Add(card);
                }
                else if (cardComponent.isGoldenCard())
                {
                    //gold card sequence
                    remainingCards.Add(card);
                    remainingGoldenCards.Add((card, occuppiedSlots [i].Positions));
                }
                else
                {
                    //normal sequence
                    cardsToHide.Add(card);
                }
            }
        }
        //Debug.Log("WinType: " + winLoseManager.GetWinType());
        if (winLoseManager.GetWinType() == WinType.Normal)
        {
            yield return StartCoroutine(
                HideCards(
                    cardsToHide , 
                    remainingGoldenCards , 
                    remainingBigJokerCards,
                    remainingSuperJokerCards));

            NormalSequence normalSequence = winLoseManager.normalSequence_;
            yield return StartCoroutine(
                normalSequence.NormalCompletionSequence(
                    newOccupiedSlots , 
                    winningCards , 
                    remainingCards , 
                    remainingGoldenCards , 
                    remainingBigJokerCards ,
                    remainingSuperJokerCards ,
                    OnComplete));
        }
        else if (winLoseManager.GetWinType() == WinType.Wild)
        {
            //play wildeffect
            yield return StartCoroutine(
                winLoseManager.wildSequnce_.WildCardsSequnce(
                    remainingWildCards , 
                    newOccupiedSlots , 
                    winningCards , 
                    remainingCards , 
                    remainingGoldenCards , 
                    remainingBigJokerCards ,
                    remainingSuperJokerCards ,
                    OnComplete));
        }
        else if (winLoseManager.GetWinType() == WinType.Both)
        {
            yield return StartCoroutine(HideCards(
                cardsToHide , 
                remainingGoldenCards , 
                remainingBigJokerCards,
                remainingSuperJokerCards));

            //refill the hidden Cards
            yield return StartCoroutine(winLoseManager.wildSequnce_.NormalWildCompletionSequence(winningCards));

            if (remainingBigJokerCards != null && remainingBigJokerCards.Count > 0 )
            {
                for (int i = 0 ; i < remainingBigJokerCards.Count ; i++)
                {
                    remainingCards.Add(remainingBigJokerCards [i]);
                }
            }

            if(remainingSuperJokerCards != null && remainingSuperJokerCards.Count > 0)
            {
                for (int i = 0 ; i < remainingSuperJokerCards.Count ; i++)
                {
                    remainingCards.Add(remainingSuperJokerCards [i]);
                }
            }

            yield return StartCoroutine(MoveCardsToNormalSlots(remainingCards));
            clearWinSlots();

            //check for extra wild Cards
            winLoseManager.RecheckForWildCards();
            List<winCardData> newwinningCards = new List<winCardData>(winLoseManager.GetWinningCards());
            List<GameObject> newRemainingWildCards = new List<GameObject>();
            occuppiedSlots.Clear();
            yield return StartCoroutine(MoveCardsToWinSlots(newwinningCards , occuppiedSlots));
            Debug.Log(occuppiedSlots.Count);
            for (int i = 0 ; i < occuppiedSlots.Count ; i++)
            {
                GameObject card = occuppiedSlots[i].slot.GetComponent<WinSlot>().GetTheOwner();
                if (card != null)
                {
                    Card cardComponent = card.GetComponent<Card>();
                    if (cardComponent.isWildCard())
                    {
                        newRemainingWildCards.Add(card);
                    }
                }
            }
            Debug.Log(newRemainingWildCards.Count);
            //playwild effect
            winLoseManager.EndTheWinSequence(
                    remainingGoldenCards ,
                    remainingBigJokerCards ,
                    remainingSuperJokerCards ,
                    OnComplete,
                    newRemainingWildCards ,
                    remainingCards);

            //yield return StartCoroutine(winLoseManager.wildSequnce_.WildCardsSequnce(
            //    newRemainingWildCards , 
            //    newOccupiedSlots , 
            //    winningCards , 
            //    remainingCards ,
            //    remainingGoldenCards , 
            //    remainingBigJokerCards , 
            //    remainingSuperJokerCards ,
            //    OnComplete));
        }
    }

    public IEnumerator MoveCardsToNormalSlots ( List<GameObject> remainingCards )
    {
        //return wildcards to wining cards fron win slots to card slots
        if (remainingCards.Count == 0)
        {
            yield break;
        }
        List<CardPos> CardSlots = new List<CardPos>(gridManager.GetGridCards());
        List<CardPos> winCardSlots = new List<CardPos>(winLoseManager.GetWinCardSlots());

        for (int i = 0 ; i < remainingCards.Count ; i++)
        {
            for (int row = 0 ; row < CardSlots.Count ; row++)
            {
                for (int col = 0 ; col < CardSlots [row].CardPosInRow.Count ; col++)
                {
                    WinSlot winSlot = winCardSlots [row].CardPosInRow [col].GetComponent<WinSlot>();
                    if (winSlot.GetTheOwner())
                    {
                        Slot slot = CardSlots [row].CardPosInRow [col].GetComponent<Slot>();
                        GameObject card = winSlot.GetTheOwner();
                        card.transform.SetParent(slot.transform);
                        card.transform.localPosition = Vector3.zero;
                    }
                }
            }
        }
        // Debug.Log("Cards Moved to Normal Slots!");
        yield return null;
    }

    private IEnumerator HideCards ( 
        List<GameObject> cardsToHide , 
        List<(GameObject card, List<(int col, int row)> Positions)> remainingGoldenCards , 
        List<GameObject> remainingBigJokerCards, 
        List<GameObject> remainingSuperJokerCards )
    {
        //hide cards from the grid
        //return the cards to the pool
        //remove the owner from the slot
        //remove the owner from the win slot
        if (cardsToHide.Count == 0)
        {
            yield break;
        }
        List<CardPos> winCardSlots = new List<CardPos>(winLoseManager.GetWinCardSlots());
        List<CardPos> CardSlots = new List<CardPos>(gridManager.GetGridCards());
        List<(int row, int col)> Keys = new List<(int row, int col)>();
        for (int i = 0 ; i < cardsToHide.Count ; i++)
        {
            for (int col = 0 ; col < CardSlots.Count ; col++)
            {
                for (int row = 0 ; row < CardSlots [col].CardPosInRow.Count ; row++)
                {
                    WinSlot winSlot = winCardSlots [col].CardPosInRow [row].GetComponent<WinSlot>();
                    var slot = CardSlots [col].CardPosInRow [row].GetComponent<Slot>();
                    if (slot.GetTheOwner() == cardsToHide [i])
                    {
                        var cardComponent = cardsToHide [i].GetComponent<Card>();

                        poolManager.ReturnToPool(PoolType.Cards,cardsToHide [i]);
                        slot.RemoveOwner();
                        winSlot.RemoveOwner();
                    }
                }
            }
        }

        // Debug.Log("Cards Hidden!");

        if (remainingGoldenCards.Count > 0)
        {
            yield return StartCoroutine(winLoseManager.rotateGoldenCards.rotateGoldenCards(remainingGoldenCards));
        }
        yield return null;
    }

    public void clearWinSlots ()
    {
        //clear remaining cards in winslots
        List<CardPos> winCardSlots = new List<CardPos>(winLoseManager.GetWinCardSlots());
        for (int col = 0 ; col < winCardSlots.Count ; col++)
        {
            for (int row = 0 ; row < winCardSlots [col].CardPosInRow.Count ; row++)
            {
                WinSlot winSlot = winCardSlots [col].CardPosInRow [row].GetComponent<WinSlot>();
                if (winSlot.IsOwnerAvailable())
                {
                    winSlot.RemoveOwner();
                }
            }
        }
    }

    public void SetIsWinSequence ( bool isActive )
    {
        isWinSequenceDone = isActive;
    }

    public void SetIsFlipping ( bool isActive )
    {
        isFlipping = isActive;
    }

    public bool IsWinSequenceDone ()
    {
        return isWinSequenceDone;
    }
    public bool IsFlipping ()
    {
        return isFlipping;
    }
}
