using NUnit.Framework;
using System;
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
    Free,
    ExtraBet,
    Collector,
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
    private static readonly Dictionary<MultiplierType , List<Multipliers>> multiplierProgressions =
        new Dictionary<MultiplierType , List<Multipliers>>()
        {
            { MultiplierType.Normal,   new List<Multipliers> { Multipliers.x1, Multipliers.x2, Multipliers.x3, Multipliers.x5 } },
            { MultiplierType.Free, new List<Multipliers> { Multipliers.x2, Multipliers.x4, Multipliers.x6, Multipliers.x10 } },
            { MultiplierType.ExtraBet, new List<Multipliers> { Multipliers.x2, Multipliers.x3, Multipliers.x4, Multipliers.x6 } },
            { MultiplierType.Collector, new List<Multipliers> { Multipliers.x2, Multipliers.x3, Multipliers.x4, Multipliers.x6 } }
        };

    [SerializeField] private Multipliers activeMultiplier = Multipliers.x1;
    [SerializeField] private MultiplierType currentType = MultiplierType.Normal;
    [SerializeField] private TopBanner topBanner_;
    [SerializeField] private TMP_SpriteAsset spriteAsset_active;
    [SerializeField] private TMP_SpriteAsset spriteAsset_inactive;
    [SerializeField] private List<Multipliers> orderOfMultipliers = new List<Multipliers>();
    [SerializeField] private MultiplierCanonInfo[] multiplierCanonInfos;
    [SerializeField] private bool hasRecalled = false;
    [SerializeField] private bool isInUpgradeMode = false;
    [SerializeField] private int collectorCount = 0;
    [SerializeField] private int upgradeRoundsRemaining = 0;
    private const int MAX_FREE_SPIN_UPGRADES = 10;
    [SerializeField] private int freeSpinUpgradeCount = 0;
    [SerializeField] private Baseboard baseboard;
    private int cannonIndex = 0;
    public Action OnComplete;
    public bool isTriggerCollectorDone = false;
    [ContextMenu("Reset Multiplier")]
    public void ResetMultiplier ()
    {
        Debug.Log("reseting multiplier");
        if (collectorCount >= 5 && 
            !isInUpgradeMode && 
            !CommandCenter.Instance.freeSpinManager_.IsFreeGame())
        {
            EnterUpgradeMode();
        }
        else
        {
            if (isInUpgradeMode)
            {
                if (upgradeRoundsRemaining <= 0)
                {
                    ExitUpgradeMode();
                }
                else
                {
                    orderOfMultipliers = multiplierProgressions [currentType]; //  sync the list
                    activeMultiplier = orderOfMultipliers [0];
                    cannonIndex = 0;
                    hasRecalled = false;
                    for (int i = 0 ; i < 4 ; i++)
                    {
                        updateUI(true);
                    }
                }
            }
            else
            {
                orderOfMultipliers = multiplierProgressions [currentType]; //  sync the list
                activeMultiplier = orderOfMultipliers [0];
                cannonIndex = 0;
                hasRecalled = false;
                for (int i = 0 ; i < 4 ; i++)
                {
                    updateUI(true);
                }
            }
        }
    }

    private void ResetUI ()
    {
        cannonIndex = 0;
        for (int i = 0 ; i < 4 ; i++)
            updateUI(true);
    }



    public void SetMultiplierType ( MultiplierType type )
    {
        currentType = type;
        ResetMultiplier();
    }
    public Multipliers GetActiveMultiplier () => activeMultiplier;
    public MultiplierType GetCurrentType () => currentType;

    public void AdvanceMultiplier ()
    {
        if (!multiplierProgressions.TryGetValue(currentType , out var list))
            return;
       // Debug.Log(list.Count);
        int index = list.IndexOf(activeMultiplier);
        if (index >= 0 && index < list.Count - 1)
        {
            if (!hasRecalled) 
            {
                hasRecalled = true;
                Debug.Log($"IsinUpgradeMode : {isInUpgradeMode}");
                if (isInUpgradeMode)
                {
                    Debug.Log($"active Multiplier index: {(int)activeMultiplier} \n, order of multipliers multiplier: {(int)orderOfMultipliers [0]}");
                    if ((int)activeMultiplier > (int)orderOfMultipliers [0])
                    {
                        return;
                    }

                    if(upgradeRoundsRemaining <= 0)
                    {
                        return;
                    }

                    upgradeRoundsRemaining--;
                    baseboard.UseTicket(); // Use a ticket for the upgrade effect
                                           //show used effect
                }
                return; 
            } 
            activeMultiplier = list [index + 1];
            // If in upgrade mode, decrement rounds
            
        }
    }
    [ContextMenu("Trigger collector")]
    public void TriggerCollector ()
    {
        SetIsTriggerCollectorDone(false);
        if (currentType == MultiplierType.Normal)
        {
            collectorCount++;
        }
        else if (currentType == MultiplierType.Free)
        {
            if (freeSpinUpgradeCount >= MAX_FREE_SPIN_UPGRADES)
                return;

            freeSpinUpgradeCount++;

            // Apply +1 to all multipliers
            var upgradedList = multiplierProgressions [currentType]
                .Select(m => (Multipliers)Mathf.Min((int)m + 1 , 20))
                .ToList();

            multiplierProgressions [currentType] = upgradedList;
            orderOfMultipliers = upgradedList;
            int nextValue = Mathf.Min((int)activeMultiplier + 1 , (int)Multipliers.x20);
            activeMultiplier = (Multipliers)nextValue;
            CommandCenter.Instance.soundManager_.PlaySound("Free_MultBarUpgrade");
            UpgradeAnim(() =>
            {
                //reset
                for (int i = 0 ; i < 4 ; i++)
                {
                    updateUI(true);
                }
                SetIsTriggerCollectorDone(true);
            });
        }
    }

    public void SetIsTriggerCollectorDone ( bool value )
    {
        isTriggerCollectorDone = value;
    }

    private void EnterUpgradeMode ()
    {
        currentType = MultiplierType.Collector;
        CommandCenter.Instance.cardManager_.SetSuperJokerChance(0f);
        baseboard.ActivateUpgradeMultipliers();
        CommandCenter.Instance.soundManager_.PlaySound("Base_MultBarUpgrade");
        isInUpgradeMode = true;
        upgradeRoundsRemaining = 3;
        collectorCount = 0;

        multiplierProgressions [currentType] = new List<Multipliers>
        {
            Multipliers.x2,
            Multipliers.x3,
            Multipliers.x4,
            Multipliers.x6
        };

        orderOfMultipliers = multiplierProgressions [currentType]; //  sync the list
        activeMultiplier = orderOfMultipliers [0];
        cannonIndex = 0;
        hasRecalled = false;
        ResetUI();
    }

    private void ExitUpgradeMode ()
    {
        //test - remove 0.3f
        CommandCenter.Instance.cardManager_.SetSuperJokerChance();
        currentType = MultiplierType.Normal;
        baseboard.DeactivateUpgradeMultipliers();
        isInUpgradeMode = false;

        multiplierProgressions [currentType] = new List<Multipliers>
        {
            Multipliers.x1,
            Multipliers.x2,
            Multipliers.x3,
            Multipliers.x5
        };

        orderOfMultipliers = multiplierProgressions [currentType]; //  sync the list
        activeMultiplier = orderOfMultipliers [0];
        cannonIndex = 0;
        hasRecalled = false;
        ResetUI();
    }

    public void ApplyCollectorEffect ()
    {
        int value = Mathf.Min((int)activeMultiplier + 1 , 20);
        activeMultiplier = (Multipliers)value;
    }

    [ContextMenu("Test")]
    public void ShowMultiplier ()
    {
        Debug.Log("show multiplier");
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
       // Debug.Log(cannonIndex);
        if (cannonIndex > 3)
        {
            cannonIndex = 0; // Reset cannon index if it exceeds the number of cannons
        }
    }

    [ContextMenu("Test extraBetAnim")]
    public void ExtraBetAnim ()
    {
        currentType = MultiplierType.ExtraBet;
        StartCoroutine(RunExtraBetAnimsSequentially());
    }

    public void UpgradeAnim (Action OnComplete = null)
    {
        StartCoroutine(RunUpgradeAnimSequentially(OnComplete));
    }
    [ContextMenu("Test Free Spin Anim")]
    public void FreeGameEnter () 
    {
        StartCoroutine(runFreeGameEnter());
    }

    private IEnumerator runFreeGameEnter ()
    {
        for (int i = 0 ; i < 4 ; i++)
        {
            yield return StartCoroutine(FreeSpinAnim());
        }
    }

    private IEnumerator RunUpgradeAnimSequentially (Action OnComplete = null)
    {
        int activeCoroutines = 4;
        for (int i = 0 ; i < 4 ; i++)
        {
            yield return StartCoroutine(extraBetAnim());
            activeCoroutines--;
        }

        yield return new WaitUntil(() => activeCoroutines <= 0);
        OnComplete?.Invoke();
    }

    private IEnumerator RunExtraBetAnimsSequentially ()
    {
        for (int i = 0 ; i < 4 ; i++)
        {
            yield return StartCoroutine(extraBetAnim());
        }
        yield return new WaitForSeconds(0.5f); 
        ResetMultiplier();
    }

    private IEnumerator extraBetAnim ()
    {
        Sprite cannonSprite = null;
        Sprite cannonBgSprite = null;
        cannonType cannon = (cannonType)cannonIndex;
        var list = multiplierProgressions [currentType];
        Multipliers multiplier = list [cannonIndex];

        int typeIndex = currentType switch
        {
            MultiplierType.Normal => 0,
            MultiplierType.Free => 1,
            MultiplierType.ExtraBet => 2,
            MultiplierType.Collector => 3,
            _ => -1
        };

        var info = multiplierCanonInfos [typeIndex];

        if (cannonIndex == 0 || cannonIndex == 3)
        {
            cannonSprite = info.canon_1_active;
            cannonBgSprite = info.canon_1_active;
        }
        else if (cannonIndex == 1 || cannonIndex == 2)
        {
            cannonSprite = info.canon_2_active;
            cannonBgSprite = info.canon_2_active;
        }
        // Wait for the animation coroutine to finish
        yield return StartCoroutine(topBanner_.ExtraBetAnim(cannonIndex , multiplier , spriteAsset_active , cannonBgSprite, cannonSprite));
        yield return new WaitForSeconds(0.25f); // Wait a bit before proceeding to the next cannon
        cannonIndex++;
        if (cannonIndex > 3)
            cannonIndex = 0;
        Debug.Log($"{cannonIndex} | Multiplier: {multiplier}");
    }

    public IEnumerator FreeSpinAnim ()
    {
        cannonType cannon = (cannonType)cannonIndex;
        var list = multiplierProgressions [currentType];
        Multipliers multiplier = list [cannonIndex];
        yield return StartCoroutine(topBanner_.FreeSpinAnim(cannonIndex,multiplier,activeMultiplier,GetSpriteAsset(),GetCannon(),GetCanonBg()));
        yield return new WaitForSeconds(0.25f); // Wait a bit before proceeding to the next cannon
        cannonIndex++;
        if (cannonIndex > 3)
            cannonIndex = 0;
        Debug.Log($"{cannonIndex} | Multiplier: {multiplier}");
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
            MultiplierType.ExtraBet => 2,
            MultiplierType.Collector => 3,
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
            MultiplierType.ExtraBet => 2,
            MultiplierType.Collector => 3,
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

    public int GetMultiplier ()
    {
        string multiplierString = activeMultiplier.ToString();
        string digits = string.Empty;

        foreach (char c in multiplierString)
        {
            if (char.IsDigit(c)) // Fix 1: Use static method correctly
            {
                digits += c;
            }
        }

        if (int.TryParse(digits , out int result))
            return result;
        Debug.LogWarning("fallback multiplier!");
        return 1; // Fix 2: Provide a fallback value
    }

    public int GetFreeSpinUpgradeCount ()
    {
        return freeSpinUpgradeCount;
    }

    public int GetMaxFreeSpinUpgradeCount ()
    {
        return MAX_FREE_SPIN_UPGRADES;
    }

    public int GetCollectorCount ()
    {
        return collectorCount;
    }

    public bool IsInUpgradeMode ()
    {
        return isInUpgradeMode;
    }

    public bool IsTriggerCollectorDone ()
    {
        return isTriggerCollectorDone;
    }

    public int GetCannonIndex ()
    {
        return cannonIndex;
    }
}
