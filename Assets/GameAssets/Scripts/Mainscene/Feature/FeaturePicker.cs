using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class FeaturePicker : MonoBehaviour
{
    [SerializeField] private Feature_A_Sequence feature_A_sequence;
    [SerializeField] private Feature_B_Sequence feature_B_sequence;
    [SerializeField] private Feature_C_Sequence feature_C_sequence;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Feature_A_Sequence GetSequence_A ()
    {
        return feature_A_sequence;
    }

    public Feature_B_Sequence GetSequence_B ()
    {
        return feature_B_sequence;
    }

    public Feature_C_Sequence GetSequence_C ()
    {
        return feature_C_sequence;
    }


    public List<GridInfo> spinData ( Features features ,int counter)
    {
        switch (features)
        {
            case Features.Feature_A:

                var sequence = GetSequence_A();

                if (counter >= 0 && counter < sequence.AllSpinSequences.Count)
                {
                    return sequence.AllSpinSequences [counter];
                }
                break;
            case Features.Feature_B:
                switch (counter)
                {
                    case 0:
                       return GetSequence_B().Spin_1;
                        
                }
                break;
            case Features.Feature_C:
                switch (counter)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                }
                break;
        }

        return null;
    }

    public List<GridInfo> refillData ( Features features ,int counter)
    {
        switch (features)
        {
            case Features.Feature_A:

                var sequence = GetSequence_A();

                if (counter >= 0 && counter < sequence.AllRefillSequences.Count)
                    return sequence.AllRefillSequences [counter];
                break;

            case Features.Feature_B:
                switch (counter)
                {
                    case 0:
                       return GetSequence_B().refill_1;
                    case 1:
                        return GetSequence_B().refill_2;
                }
                break;
            case Features.Feature_C:
                switch (counter)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
                break;
        }

        return null;
    }

    public CardDatas featureA_spin ( int refillCounter , int col , int row )
    {
        var sequence = GetSequence_A();

        if (refillCounter >= 0 && refillCounter < sequence.AllSpinSequences.Count)
        {
            return sequence.AllSpinSequences [refillCounter][col].List [row];
        }

        return null;
    }

    public CardDatas featureA_refill (int refillCounter,int col,int row)
    {
        var sequence = GetSequence_A();

        if (refillCounter >= 0 && refillCounter < sequence.AllRefillSequences.Count)
            return sequence.AllRefillSequences [refillCounter] [col].List [row];
        return null;
    } 
    public CardDatas featureB_spin ( int refillCounter , int col , int row )
    {
        switch (refillCounter)
        {
            case 0:
                return GetSequence_B().Spin_1 [col].List [row];
        }
        return null;
    }

    public CardDatas featureB_refill ( int refillCounter,int col,int row)
    {
        switch (refillCounter)
        {
            case 0:
                return GetSequence_B().refill_1 [col].List [row];
            case 1:
                return GetSequence_B().refill_2 [col].List [row];
        }
        return null;
    } 
    public CardDatas featureC_spin ( int refillCounter , int col , int row )
    {
        switch (refillCounter)
        {
            case 0:
                return null;
            case 1:
                return null;
        }
        return null;
    }

    public CardDatas featureC_refill ( int refillCounter,int col,int row)
    {
        switch (refillCounter)
        {
            case 0:
                return null;
            case 1:
                return null;
        }
        return null;
    }

}
