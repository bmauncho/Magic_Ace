using UnityEngine;

public class GameModeManager : MonoBehaviour
{

    [SerializeField] private bool isDemoMode;
    [SerializeField] private bool isNormalMode;

    public void SetGameMode ( bool isDemo )
    {
        isDemoMode = isDemo;
        isNormalMode = !isDemo;
    }

    public bool IsDemoMode ()
    {
        return isDemoMode;
    }
    public bool IsNormalMode ()
    {
        return isNormalMode;
    }
}
