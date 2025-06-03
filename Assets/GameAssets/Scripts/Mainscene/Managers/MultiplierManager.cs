using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Multipliers
{
    X1 = 1,
    X2 = 2,
    X3 = 3,
    X4 = 4,
    X5 = 5,
    X6 = 6,
    X7 = 7,
    X8 = 8,
    X9 = 9,
    X10 = 10,
    X11 = 11,
    X12 = 12,
    X13 = 13,
    X14 = 14,
    X15 = 15,
    X16 = 16,
    X17 = 17,
    X18 = 18,
    X19 = 19,
    X20 = 20
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
    public Sprite canon_1;
    [Tooltip("used for canon 2 & 3")]
    public Sprite canon_2;
}

public class MultiplierManager : MonoBehaviour
{
    [SerializeField] private Multipliers currentMultiplier = Multipliers.X1;
    [SerializeField] private MultiplierType currentType = MultiplierType.Normal;
    [SerializeField] private MultiplierCanonInfo[] multiplierCanonInfos;
    [SerializeField] private TopBanner topBanner_;
    private static readonly Dictionary<MultiplierType , List<Multipliers>> multiplierProgressions =
        new Dictionary<MultiplierType , List<Multipliers>>()
        {
            { MultiplierType.Normal,   new List<Multipliers> { Multipliers.X1, Multipliers.X2, Multipliers.X3, Multipliers.X5 } },
            { MultiplierType.Free, new List<Multipliers> { Multipliers.X2, Multipliers.X4, Multipliers.X6, Multipliers.X10 } }
        };

    public Multipliers GetCurrentMultiplier () => currentMultiplier;
    public MultiplierType GetCurrentType () => currentType;

    private int cannonIndex = 0;
    private int lastMultiplierIndex = -1;
    public void ResetMultiplier ()
    {
        currentMultiplier = multiplierProgressions [currentType] [0];
    }

    public void AdvanceMultiplier ()
    {
        var list = multiplierProgressions [currentType];
        int index = list.IndexOf(currentMultiplier);
        if (index >= 0 && index < list.Count - 1)
        {
            currentMultiplier = list [index + 1];
        }
    }

    public void ApplyCollectorEffect ()
    {
        int value = Mathf.Min((int)currentMultiplier + 1 , 20);
        currentMultiplier = (Multipliers)value;
    }

    public void SetMultiplierType ( MultiplierType type )
    {
        currentType = type;
        ResetMultiplier();
    }

    public Multipliers GetNextMultiplier ()
    {
        var list = multiplierProgressions [currentType];
        int index = list.IndexOf(currentMultiplier);
        if (index >= 0 && index < list.Count - 1)
        {
            return list [index + 1];
        }
        return currentMultiplier;
    }

    public void updateUI ()
    {

        if (cannonIndex > 3)
        {
            cannonIndex = 0; // Reset cannon index if it exceeds the number of cannons
        }

        cannonType cannon = (cannonType)cannonIndex;

        // Determine which sprite to use based on cannon type
        Sprite cannonSprite = ( cannon == cannonType.cannon1 || cannon == cannonType.cannon4 )
            ? multiplierCanonInfos [0].canon_1
            : multiplierCanonInfos [0].canon_2;

        //canon1 = current multiplier
        //canon2 = next multipler according to logic
        //canon3 = next multiplier after cannon2 multiplier according to logic
        //canon4 = next multiplier after cannon3 multiplier according to logic
        // also aplies to cannon multiplier
        string multiplier = currentMultiplier.ToString();

        Multipliers cannonMultiplier = currentMultiplier++;

        topBanner_.UpdateCanon(cannon , currentMultiplier , multiplier , cannonSprite);
        cannonIndex++;
    }
}
