using TMPro;
using UnityEngine;

public class Spins : MonoBehaviour
{

    [SerializeField] private int spins;
    [SerializeField] private TMP_Text SpinAmount;

    private void OnEnable ()
    {
        SpinAmount.text = spins.ToString();
    }

    public void Selected ()
    {
        GetComponentInParent<AutoSpinMenu>().GetSelectedSpins(this , spins);
    }
}
