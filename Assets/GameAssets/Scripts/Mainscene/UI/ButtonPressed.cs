using DG.Tweening;
using UnityEngine;

public class ButtonPressed : MonoBehaviour
{
    public GameObject Button;
    
    public void pressed ()
    {
        if (Button == null) { return; }

        Button.transform.DOPunchScale(new Vector3(0.1f , 0.1f , 0.1f) , .25f,0 , 0);
    }
}
