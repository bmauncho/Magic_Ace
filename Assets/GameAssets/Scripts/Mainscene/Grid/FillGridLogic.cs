using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillGridLogic : MonoBehaviour
{

    public IEnumerator normalfillGrid ( List<CardPos> CardSlots,bool isFirstTime )
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
        CommandCenter.Instance.winLoseManager_.WinLoseSequence(()=>
        {
            Debug.Log("WinLoseSequence completed");
        });
    }

    public IEnumerator quickfillGrid ( List<CardPos> CardSlots )
    {
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
        CommandCenter.Instance.winLoseManager_.WinLoseSequence(() =>
        {
            Debug.Log("WinLoseSequence completed");
        });
    }

    public IEnumerator turbofillGrid ( List<CardPos> CardSlots)
    {
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
        CommandCenter.Instance.winLoseManager_.WinLoseSequence(() =>
        {
            Debug.Log("WinLoseSequence completed");
        });
    }

    // Separate coroutine to handle individual delays
    private IEnumerator DelayedMove ( Card card , Transform target , float delay , Slot _slot , bool isInitialization )
    {
        yield return new WaitForSeconds(delay);
        card.moveCard(target , _slot , isInitialization);
        yield return null;
    }
}
