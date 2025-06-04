using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public enum Multipliers
{
    x1 = 1,
    x2 = 2,
    x3 = 3,
    x4 = 4,
    x5 = 5,
    x6 = 6,
    x7 = 7,
    x8 = 8,
    x9 = 9,
    x10 = 10,
    x11 = 11,
    x12 = 12,
    x13 = 13,
    x14 = 14,
    x15 = 15,
    x16 = 16,
    x17 = 17,
    x18 = 18,
    x19 = 19,
    x20 = 20
}

public enum MultiplierType
{
    Normal,
    collector,
    Free,
}

[System.Serializable]
public class MultiplierCanonInfo
{
    public MultiplierType multiplierType;
    [Tooltip("used for canon 1 & 4")]
    public Sprite canon_1_active;
    public Sprite canon_1_inactive;
    [Tooltip("used for canon 2 & 3")]
    public Sprite canon_2_active;
    public Sprite canon_2_inactive;
}

public class MultiplierManager : MonoBehaviour
{
    [SerializeField] private Multipliers activeMultiplier = Multipliers.x1;
    [SerializeField] private MultiplierType currentType = MultiplierType.Normal;
    [SerializeField] private TopBanner topBanner_;
    [SerializeField] private TMP_SpriteAsset spriteAsset_active;
    [SerializeField] private TMP_SpriteAsset spriteAsset_inactive;
    [SerializeField] private List<Multipliers> orderOfMultipliers = new List<Multipliers>();
    [SerializeField] private MultiplierCanonInfo[] multiplierCanonInfos;
    private static readonly Dictionary<MultiplierType , List<Multipliers>> multiplierProgressions =
        new Dictionary<MultiplierType , List<Multipliers>>()
        {
            { MultiplierType.Normal,   new List<Multipliers> { Multipliers.x1, Multipliers.x2, Multipliers.x3, Multipliers.x5 } },
            { MultiplierType.Free, new List<Multipliers> { Multipliers.x2, Multipliers.x4, Multipliers.x6, Multipliers.x10 } }
        };
    public Multipliers GetActiveMultiplier () => activeMultiplier;
    public MultiplierType GetCurrentType () => currentType;
    private int cannonIndex = 0;
    [SerializeField] private bool hasRecalled = false;
    private int lastMultiplierIndex = -1;
    [ContextMenu("Reset Multiplier")]
    public void ResetMultiplier ()
    {
        orderOfMultipliers.Clear();
        activeMultiplier = multiplierProgressions [currentType] [0];
        cannonIndex = 0;
        hasRecalled = false;
        for (int i =0 ;i<4 ;i++)
        {
            updateUI(true);
        }
    }
    public void SetMultiplierType ( MultiplierType type )
    {
        currentType = type;
        ResetMultiplier();
    }

    public void AdvanceMultiplier ()
    {
        var list = multiplierProgressions [currentType];
        int index = list.IndexOf(activeMultiplier);
        if (index >= 0 && index < list.Count - 1)
        {
            if (!hasRecalled) 
            {
                hasRecalled = true;
                return; 
            } 
            activeMultiplier = list [index + 1];
        }
    }
    public void ApplyCollectorEffect ()
    {
        int value = Mathf.Min((int)activeMultiplier + 1 , 20);
        activeMultiplier = (Multipliers)value;
    }

    [ContextMenu("Test")]
    public void Test ()
    {
        AdvanceMultiplier();
        
        for (int i = 0 ; i < 4 ; i++)
        {
            updateUI();
        }
    }
    /// <summary>
    /// This method is called 4 times to update the UI for each cannon.
    /// </summary>
    public void updateUI (bool isReset = false)
    {
        cannonType cannon = (cannonType)cannonIndex;
        var list = multiplierProgressions [currentType];
        Multipliers multiplier = list [cannonIndex];
        topBanner_.UpdateCanon(cannon , multiplier,activeMultiplier ,GetCannon() ,GetCanonBg() , GetSpriteAsset(),isReset);
        cannonIndex++;
        Debug.Log(cannonIndex);
        if (cannonIndex > 3)
        {
            cannonIndex = 0; // Reset cannon index if it exceeds the number of cannons
        }
    }

    private Sprite GetCannon ()
    {
        Sprite cannon = null;
        var list = multiplierProgressions [currentType];
        int activeIndex = list.IndexOf(activeMultiplier);
        bool isActive = cannonIndex == activeIndex;

        //Debug.Log($"Cannon Index: {cannonIndex}, Active Index: {activeIndex}, Is Active: {isActive}");

        int typeIndex = currentType switch
        {
            MultiplierType.Normal => 0,
            MultiplierType.Free => 1,
            MultiplierType.collector => 2,
            _ => -1
        };

        if (typeIndex < 0 || typeIndex >= multiplierCanonInfos.Length)
            return null;

        var info = multiplierCanonInfos [typeIndex];

        if (cannonIndex == 0 || cannonIndex == 3) // Cannon 1 & 4
        {
            cannon = isActive ? info.canon_1_active : info.canon_1_inactive;
        }
        else if (cannonIndex == 1 || cannonIndex == 2) // Cannon 2 & 3
        {
            cannon = isActive ? info.canon_2_active : info.canon_2_inactive;
        }

        return cannon;
    }

    public Sprite GetCanonBg ()
    {
        Sprite cannon = null;
        int typeIndex = currentType switch
        {
            MultiplierType.Normal => 0,
            MultiplierType.Free => 1,
            MultiplierType.collector => 2,
            _ => -1
        };
        var info = multiplierCanonInfos [typeIndex];
        if (cannonIndex == 0 || cannonIndex == 3) // Cannon 1 & 4
        {
            cannon =  info.canon_1_inactive;
        }
        else if (cannonIndex == 1 || cannonIndex == 2) // Cannon 2 & 3
        {
            cannon =  info.canon_2_inactive;
        }
        return cannon;
    }


    private TMP_SpriteAsset GetSpriteAsset ()
    {
        TMP_SpriteAsset asset = null;
        var list = multiplierProgressions [currentType];
        int activeIndex = list.IndexOf(activeMultiplier);
        bool isActive = cannonIndex == activeIndex;
        if (isActive)
        {
            asset = spriteAsset_active;
        }
        else
        {
           asset = spriteAsset_inactive;
        }
        return asset;
    }
    public Multipliers GetNextMultiplier ()
    {
        var list = multiplierProgressions [currentType];
        int index = list.IndexOf(activeMultiplier);
        if (index >= 0 && index < list.Count - 1)
        {
            return list [index + 1];
        }
        return activeMultiplier;
    }

}
