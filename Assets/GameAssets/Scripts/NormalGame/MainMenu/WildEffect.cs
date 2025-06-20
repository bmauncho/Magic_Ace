using UnityEngine;

public class WildEffect : MonoBehaviour
{
    [SerializeField] private GameObject [] effects;
    public void ToggleEffect(int col )
    {
        if(col < 0 || col >= effects.Length)
        {
            //Debug.LogError("Invalid effect index");
            return;
        }
        for (int i = 0 ; i < effects.Length ; i++)
        {
            effects [i].SetActive(i==col);
        }
    }

    public void DisAbleEffect ()
    {
        for (int i = 0 ; i < effects.Length ; i++)
        {
            effects [i].SetActive(false);
        }
    }
}
