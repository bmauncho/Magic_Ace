using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private Deck [] Decks;

    public Deck [] GetDecks ()
    {
        return Decks;
    }
}
