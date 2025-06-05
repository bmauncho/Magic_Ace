using System.Collections;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] private Combo combo;
    private void SetCombo ()
    {
        string result = "";
        foreach (char c in combo.TheCombo.ToString("N0"))
        {
            if (char.IsDigit(c))
                result += $"<sprite name=Combo_{c}>";
            else if (c == ',')
                result += ","; // Optional: handle commas or skip
        }

        combo.comboText.text = result;
    }

    [ContextMenu("Show Combo")]
    public void ShowCombo ()
    {
        StartCoroutine(ComboSequnce());
    }

    private void IncreaseCombo ()
    {
        combo.TheCombo++;
        combo.comboCounter++;
        if (combo.TheCombo >= 10)
        {
            combo.numbertrans.sizeDelta = new Vector2(40 , 40);
            combo.TheCombo = 10;
        }
    }

    private IEnumerator ComboSequnce ()
    {
        IncreaseCombo();
        if (!combo.init)
        {
            combo.init = true;
            yield return StartCoroutine(comboInitAnim());
        }
        else
        {
            ComboAnim();
        }
    }

    private IEnumerator comboInitAnim ()
    {
        showBgAnim();
        yield return new WaitForSeconds(.25f / 2);
        showComboEffectAnim();
        yield return new WaitForSeconds(0.5f);
        showNumberAnim();
        showNumberEffectAnim();
        yield return new WaitForSeconds(0.5f / 2);
        showBgEffectAnim();
        yield return new WaitForSeconds(1f);
        HideBgEffect();
        yield return new WaitForSeconds(1f);
        HideNumberEffect();
        yield return null;
    }

    private void ComboAnim ()
    {
        showNumberAnim();
    }

    private void showBgAnim ()
    {
        combo.BgAnim.gameObject.SetActive(true);
        combo.BgAnim.Rebind();
        combo.BgAnim.Play("ComboBGAnim");
    }

    private void showNumberAnim ()
    {
        SetCombo();
        combo.NumberAnim.gameObject.SetActive(true);
        combo.NumberAnim.Rebind();
        combo.NumberAnim.Play("ComboNumberAnim");
    }

    private void showNumberEffectAnim ()
    {
        combo.NumberEffectAnim.gameObject.SetActive(true);
        combo.NumberEffectAnim.Rebind();
        combo.NumberEffectAnim.Play("NumberEffectAnim");
    }

    private void showComboEffectAnim ()
    {
        combo.ComboEffectAnim.gameObject.SetActive(true);
        combo.ComboEffectAnim.Rebind();
        combo.ComboEffectAnim.Play("ComboTextAnim");
    }

    private void showBgEffectAnim ()
    {
        combo.BgEffectAnim.gameObject.SetActive(true);
        combo.BgEffectAnim.Rebind();
        combo.BgEffectAnim.Play("ComboLightEffect");
    }

    private void HideBg ()
    {
        combo.BgAnim.gameObject.SetActive(false);
    }

    private void HideNumber ()
    {
        combo.NumberAnim.gameObject.SetActive(false);
    }

    private void HideNumberEffect ()
    {
        combo.NumberEffectAnim.gameObject.SetActive(false);
    }

    private void HideComboEffect ()
    {
        combo.ComboEffectAnim.gameObject.SetActive(false);
    }

    private void HideBgEffect ()
    {
        combo.BgEffectAnim.gameObject.SetActive(false);
    }
    [ContextMenu("Hide Combo Test")]
    public void HideCombo ()
    {
        HideBg();
        HideNumber();
        HideNumberEffect();
        HideComboEffect();
        HideBgEffect();
        ResetCombo();
    }

    private void ResetCombo ()
    {
        combo.TheCombo = 0;
        combo.comboCounter = 0;
        SetCombo();
        combo.init = false;
        combo.numbertrans.sizeDelta = new Vector2(33 , 30);
    }

    public int GetComboCounter ()
    {
        return combo.comboCounter;
    }

    public int GetCombo ()
    {
        return combo.TheCombo;
    }
}
