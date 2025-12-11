using System;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Draws a random entry from a list of entries with weights.
/// 
/// Author: William Min
/// Date: 12/11/25
/// </summary>
/// <typeparam name="T">Type of entry</typeparam>
[Serializable]
public class WeightProcesser<T>
{
    #region Serialized Fields


    [SerializeField] private RandomOrderMode _orderMode;    // Mode of giving out entries
    [SerializeField] private WeightEntry<T>[] _entries;     // List of entries for processor


    #endregion

    #region Properties


    /// <summary>
    /// 
    /// </summary>
    public int EntryCount { get => _entries != null ? _entries.Length : 0; }


    #endregion

    #region Enums


    // 
    private enum RandomOrderMode
    {
        Random,
        Ordered,
        RandomUnique
    }


    #endregion

    #region Public Methods


    /// <summary>
    /// Resets the entry trackers.
    /// </summary>
    public void Reset()
    {
        foreach (WeightEntry<T> entry in _entries)
        {
            entry.ResetEntry();
        }
    }

    /// <summary>
    /// Returns the next entry object based on order mode.
    /// </summary>
    /// <returns>Next selected entry object</returns>
    public T GetNextEntry()
    {
        int totalWeight = _getWeightSum();

        if (totalWeight <= 0)
        {
            Reset();
            totalWeight = _getWeightSum();
        }

        int roll = UnityEngine.Random.Range(0, totalWeight);

        switch (_orderMode)
        {
            case RandomOrderMode.Random:

                foreach (WeightEntry<T> entry in _entries)
                {
                    roll -= entry.BaseWeight;

                    if (roll < 0)
                    {
                        return entry.Object;
                    }
                }

                return default(T);


            case RandomOrderMode.Ordered:
                    
                foreach (WeightEntry<T> entry in _entries)
                {
                    if (entry.Weight > 0)
                    {
                        entry.ShrinkWeight();
                        return entry.Object;
                    }
                }

                return default(T);


            case RandomOrderMode.RandomUnique:

                foreach (WeightEntry<T> entry in _entries)
                {
                    roll -= entry.Weight;

                    if (roll < 0)
                    {
                        entry.ShrinkWeight();
                        return entry.Object;
                    }
                }

                return default(T);


            default:

                return default(T);
        }
    }


    #endregion

    #region Private Methods


    // Returns the total weight of the entries
    private int _getWeightSum()
    {
        int totalWeight = 0;

        foreach (WeightEntry<T> weight in _entries)
        {
            totalWeight += weight.Weight;
        }

        return totalWeight;
    }


    #endregion
}

/// <summary>
/// Represents an entry and its weight.
/// 
/// Author: William Min
/// Date: 12/11/25
/// </summary>
/// <typeparam name="T">Type of entry</typeparam>
[Serializable]
public class WeightEntry<T>
{
    #region Serialized Fields


    [SerializeReference, SubclassSelector] private T _object;   // Object to pass when selected
    [SerializeField] private int _weight = 1;                   // Weight of entry


    #endregion

    #region Private Fields


    private int _currentWeight = 0; // 


    #endregion

    #region Properties


    /// <summary>
    /// 
    /// </summary>
    public T Object { get => _object; }

    /// <summary>
    /// 
    /// </summary>
    public int BaseWeight { get => _weight; }

    /// <summary>
    /// 
    /// </summary>
    public int Weight { get => _currentWeight; }


    #endregion

    #region Public Methods


    /// <summary>
    /// 
    /// </summary>
    public void ShrinkWeight()
    {
        _currentWeight--;

        if (_currentWeight < 0)
        {
            _currentWeight = 0;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetEntry()
    {
        _currentWeight = _weight;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tracker"></param>
    public void AddToTracker(Dictionary<WeightEntry<T>, int> tracker)
    {
        tracker.Add(this, _weight);
    }


    #endregion
}
