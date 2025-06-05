using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillGridLogic : MonoBehaviour
{
    public IEnumerator normalRefillGrid ( List<CardPos> CardSlots)
    {
        Deck [] Decks = CommandCenter.Instance.deckManager_.GetDecks();
        int rowCount = 4;
        int colCount = 5;
        int activeAnimations = 0;
        float delayIncrement = 0.1f; // Reduce delay for faster animation
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        // Debug.Log("normal refill");

        for (int col = 0 ; col < colCount ; col++)
        {
            var currDeck = Decks [col];
            for (int row = rowCount - 1 ; row >= 0 ; row--)
            {
                GameObject newCard = currDeck.DrawCard();
                Slot slot = CardSlots [col].CardPosInRow [row].transform.GetComponent<Slot>();
                if (slot.GetTheOwner() != null)
                {
                    Card existingCard = slot.GetTheOwner().GetComponent<Card>();

                    if (existingCard != null)
                    {
                        continue;
                    }
                }

                Transform target = CardSlots [col].CardPosInRow [row].transform;
                newCard.transform.SetParent(target);
                Card cardComponent = newCard.GetComponent<Card>();
                CommandCenter.Instance.cardManager_.setUpRefillCard(cardComponent , row , col);
                //get Wining Cards
                activeAnimations++;
                cardComponent.OnComplete += () => activeAnimations--;


                float delay = ( row * delayIncrement ) / 8;
                yield return StartCoroutine(DelayedMove(cardComponent , target , delay , slot , false));
            }
        }

        while (activeAnimations > 0)
            yield return null;
    }

    public IEnumerator quickRefillGrid ( List<CardPos> CardSlots )
    {
        Deck [] Decks = CommandCenter.Instance.deckManager_.GetDecks();
        int rowCount = 4;
        int colCount = 5;
        int activeAnimations = 0;
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;

        for (int col = 0 ; col < colCount ; col++)
        {
            var currDeck = Decks [col];
            for (int row = rowCount - 1 ; row >= 0 ; row--)
            {
                GameObject newCard = currDeck.DrawCard();
                Slot slot = CardSlots [col].CardPosInRow [row].transform.GetComponent<Slot>();

                if (slot.GetTheOwner() != null)
                {
                    Card existingCard = slot.GetTheOwner().GetComponent<Card>();
                    if (existingCard != null)
                    {
                        continue;
                    }
                }
                Transform target = CardSlots [col].CardPosInRow [row].transform;
                newCard.transform.SetParent(target);

                Card cardComponent = newCard.GetComponent<Card>();
                CommandCenter.Instance.cardManager_.setUpRefillCard(cardComponent , row , col);
                //get Wining Cards
                activeAnimations++;
                cardComponent.OnComplete += () => activeAnimations--;

                cardComponent.moveCard(target , slot);
            }
        }

        // Wait until all animations are completed
        while (activeAnimations > 0)
            yield return null;
    }

    public IEnumerator turboRefillGrid ( List<CardPos> CardSlots )
    {
        Deck [] Decks = CommandCenter.Instance.deckManager_.GetDecks();
        int rowCount = 4;
        int colCount = 5;
        int activeAnimations = 0;
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;

        for (int col = 0 ; col < colCount ; col++)
        {
            var currDeck = Decks [col];
            for (int row = rowCount - 1 ; row >= 0 ; row--)
            {
                GameObject newCard = currDeck.DrawCard();
                Slot slot = CardSlots [col].CardPosInRow [row].transform.GetComponent<Slot>();
                if (slot.GetTheOwner() != null)
                {
                    Card existingCard = slot.GetTheOwner().GetComponent<Card>();

                    if (existingCard != null)
                    {
                        continue;
                    }
                }
                Transform target = CardSlots [col].CardPosInRow [row].transform;
                newCard.transform.SetParent(target);

                Card cardComponent = newCard.GetComponent<Card>();

                CommandCenter.Instance.cardManager_.setUpRefillCard(cardComponent , row , col);
                //get Wining Cards
                activeAnimations++;
                cardComponent.OnComplete += () => activeAnimations--;

                cardComponent.moveCard(target , slot);
            }
        }

        // Wait until all animations are completed
        while (activeAnimations > 0)
            yield return null;
    }

    // Separate coroutine to handle individual delays
    private IEnumerator DelayedMove ( Card card , Transform target , float delay , Slot _slot , bool isInitialization )
    {
        yield return new WaitForSeconds(delay);
        card.moveCard(target , _slot , isInitialization);
        yield return null;
    }
}
