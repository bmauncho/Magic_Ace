using UnityEngine;

public class wildCardEffects : MonoBehaviour
{
    public void ActivateCardEffects ()
    {
        GameObject Effect = GetComponentInParent<Card>().wildEffects();
        Effect.SetActive(true);
        Effect.GetComponent<Animator>().Rebind();
    }

    public void DeactivateCardEffects ()
    {
        GameObject Effect = GetComponentInParent<Card>().wildEffects();
        Effect.SetActive(false);
    }
}
