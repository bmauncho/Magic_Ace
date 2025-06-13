using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private int capacity = 5;
    [SerializeField] private int DeckSize; // Total number of cards in the deck
    [SerializeField] private Queue<GameObject> deckCards = new Queue<GameObject>();
    [SerializeField] private List<GameObject> TotalDeckCards = new List<GameObject>();

    void Update ()
    {
        if (!CommandCenter.Instance) return;
        // Remove extra cards
        if (deckCards.Count == capacity)
        {
            int count = transform.childCount;
            if (count > capacity)
            {
                foreach (Transform child in transform)
                {
                    if (!deckCards.Contains(child.gameObject))
                    {
                        // Return to pool
                        CommandCenter.Instance.poolManager_.ReturnToPool(PoolType.Cards , child.gameObject);
                    }
                }
            }

            return;
        }

        while (deckCards.Count > capacity)
        {
            var cardToRemove = deckCards.Dequeue();
            CommandCenter.Instance.poolManager_.ReturnToPool(PoolType.Cards,cardToRemove);
        }

        // Add new cards if under capacity
        while (deckCards.Count < capacity)
        {
            GameObject newCard = CommandCenter.Instance.poolManager_.GetFromPool(PoolType.Cards,transform.position , transform.rotation , transform);
            deckCards.Enqueue(newCard);
            TotalDeckCards.Add(newCard);
        }
        // Update DeckSize
        DeckSize = deckCards.Count;
    }

    public GameObject DrawCard ()
    {
        if (deckCards.Count > 0)
        {
            GameObject drawnCard = deckCards.Dequeue();
            if(TotalDeckCards.Contains(drawnCard))
            {
                TotalDeckCards.Remove(drawnCard);
            }
            return drawnCard;
        }

        return null;
    }
}
