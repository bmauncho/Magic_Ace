using UnityEngine;

public class GameModeManager : MonoBehaviour
{

    [SerializeField] private bool isDemoMode;
    [SerializeField] private bool isNormalMode;

    private void Start ()
    {
        //Debug.Log($"isDemo {GameManager.Instance.IsDemo()}");
        //SetGameMode(GameManager.Instance.IsDemo());
    }
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
