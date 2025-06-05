using TMPro;
using UnityEngine;

public class Combo : MonoBehaviour
{
    public TMP_Text comboText;
    public Animator BgAnim;
    public Animator NumberAnim;
    public Animator NumberEffectAnim;
    public Animator ComboEffectAnim;
    public Animator BgEffectAnim;
    public int TheCombo;
    public bool init = false;
    public int comboCounter = 0;
    public RectTransform numbertrans;
}
