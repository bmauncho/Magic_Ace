using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private int capacity = 5;
    [SerializeField] private Queue<GameObject> deckCards = new Queue<GameObject>();

    void Update ()
    {
        if (!CommandCenter.Instance) return;

        // Remove extra cards
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
        }
    }

    public GameObject DrawCard ()
    {
        if (deckCards.Count > 0)
        {
            return deckCards.Dequeue();
        }

        return null;
    }
}
