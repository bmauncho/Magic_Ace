using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum cannonType
{
    cannon1,
    cannon2,
    cannon3,
    cannon4,
}

[System.Serializable]
public class CannonData
{
    public cannonType type;
    public Multipliers currentMultiplier;
    public GameObject cannon;
    public TMP_Text cannonMultiplier;
    public Image cannonImage;
    public Image cannonImageBg;
    public Animator Anim;
}
public class TopBanner : MonoBehaviour
{
    [Header("Cannon Data")]
    public Image BackGround;
    public CannonData [] cannons;
    int activeIndex = -1;
    public void UpdateCanon(cannonType cannonType,Multipliers multipliers,
        Multipliers activemultiplier,Sprite thecanon,Sprite thecanonBg, TMP_SpriteAsset spriteAsset,bool isReset = false)
    {
        StartCoroutine(updateUi(cannonType , multipliers , activemultiplier , thecanon,thecanonBg , spriteAsset,isReset));
    }

    private IEnumerator updateUi ( cannonType cannonType , Multipliers multipliers ,
        Multipliers activeMultiplier , Sprite cannonSprite , Sprite cannonBgSprite , TMP_SpriteAsset spriteAsset ,bool isReset = false )
    {
        // First, find the active cannon index for reference
        for (int i = 0 ; i < cannons.Length ; i++)
        {
            if (cannons [i].type == cannonType && cannons [i].currentMultiplier == activeMultiplier)
            {
                activeIndex = i;
                break;
            }
        }
        //Debug.Log("Active index : " + activeIndex);
        List<int> previousIndices = new List<int>();

        for (int i = 0 ; i < activeIndex && i < cannons.Length ; i++)
        {
            previousIndices.Add(i);
        }
        //Debug.Log("previous cannons count: " + previousIndices.Count);


        for (int i = 0 ; i < cannons.Length ; i++)
        {
            if (cannons [i].type != cannonType)
                continue;

            var cannon = cannons [i];
            cannon.currentMultiplier = multipliers;
            cannon.cannonMultiplier.spriteAsset = spriteAsset;

            bool isActive = ( activeMultiplier == multipliers );
            cannon.cannonMultiplier.text = SetText(multipliers.ToString() , isActive);

            if (isActive && !isReset)
            {
                if (previousIndices == null || previousIndices.Count == 0)
                {
                    Debug.LogWarning("prev cannon is null");
                }
                else
                {
                    Debug.Log("previous cannons count: " + previousIndices.Count);
                    // Play the previous cannon's animation if it exists and is not the same as the current cannon
                    foreach (int index in previousIndices)
                    {
                        var prevCannon = cannons [index];
                        if (prevCannon == cannon) continue;

                        string prevAnimName = $"Canon_{index + 1}_inactive";
                        //Debug.Log($"Playing animation: {prevAnimName} for INACTIVE cannon type: {cannonType}");
                        StartCoroutine(PlayAnim(prevCannon.Anim , prevAnimName));
                    }
                }
                yield return new WaitForSeconds(0.1f); // Wait a bit before playing the active animation
                string animName = $"Canon_{i + 1}_Anim";
                //Debug.Log($"Playing animation: {animName} for ACTIVE cannon type: {cannonType}");
                StartCoroutine(PlayAnim(cannon.Anim , animName));
            }
 
            if (cannon.cannonImage != null)
                cannon.cannonImage.sprite = cannonSprite;

            if (cannon.cannonImageBg != null)
                cannon.cannonImageBg.sprite = cannonBgSprite;
        }
        yield return null;
    }
    private IEnumerator PlayAnim ( Animator anim , string whichAnim )
    {
        anim.Rebind();
        anim.Play(whichAnim);

        // Wait until the animator updates (next frame)
        yield return null;

        // Wait for the animation to finish
        while (true)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName(whichAnim) && stateInfo.normalizedTime >= 1f)
            {
                Debug.Log("Animation is complete!");
                break;
            }

            yield return null; // Wait for the next frame
        }
    }

    private string SetText ( string text , bool isActive )
    {
        string result = string.Empty;
        if (isActive)
        {
            foreach (char c in text)
            {
                result += $"<sprite name=Base_Muiltiple_FNT_{c}>";
            }
        }
        else
        {
            foreach (char c in text)
            {
                result += $"<sprite name=Base_Muiltiple_D_FNT_{c}>";
            }
        }

        return result;
    }
}
