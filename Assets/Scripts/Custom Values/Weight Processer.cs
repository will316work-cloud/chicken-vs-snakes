using System;

using UnityEngine;

/// <summary>
/// Draws a random entry from a list of entries with weights.
/// 
/// Author: William Min
/// Date: 12/13/25
/// </summary>
/// <typeparam name="T">Type of entry</typeparam>
/// <typeparam name="Entry">Type of weighted entry for the processor to contain</typeparam>
[Serializable]
public class WeightProcesser<T, Entry> where Entry : GenericWeightEntry<T>
{
    #region Serialized Fields


    [SerializeField] private RandomOrderMode _orderMode;    // Mode of giving out entries
    [SerializeField] private Entry[] _entries;              // List of entries for processor


    #endregion

    #region Properties


    /// <summary>
    /// Number of entries in processer.
    /// </summary>
    public int EntryCount { get => _entries != null ? _entries.Length : 0; }


    #endregion

    #region Enums


    // Mode of selecting objects from entries in the processer
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
        foreach (Entry entry in _entries)
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

                foreach (Entry entry in _entries)
                {
                    roll -= entry.BaseWeight;

                    if (roll < 0)
                    {
                        return entry.Object;
                    }
                }

                return default(T);


            case RandomOrderMode.Ordered:
                    
                foreach (Entry entry in _entries)
                {
                    if (entry.Weight > 0)
                    {
                        entry.ShrinkWeight();
                        return entry.Object;
                    }
                }

                return default(T);


            case RandomOrderMode.RandomUnique:

                foreach (Entry entry in _entries)
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

    /// <summary>
    /// Returns the processer entry on the index.
    /// </summary>
    /// <param name="index">Index of processer</param>
    /// <returns>Entry on index</returns>
    public Entry GetEntryInfo(int index)
    {
        return _entries[index];
    }


    #endregion

    #region Private Methods


    // Returns the total weight of the entries
    private int _getWeightSum()
    {
        int totalWeight = 0;

        foreach (Entry weight in _entries)
        {
            totalWeight += weight.Weight;
        }

        return totalWeight;
    }


    #endregion
}

/// <summary>
/// Represents a generic entry and its weight.
/// 
/// Author: William Min
/// Date: 12/11/25
/// </summary>
/// <typeparam name="T">Type of entry</typeparam>
[Serializable]
public abstract class GenericWeightEntry<T>
{
    #region Serialized Fields


    [SerializeField] private int _weight = 1;   // Weight of entry


    #endregion

    #region Private Fields


    private int _currentWeight = 0; // Current weight of entry during runtime


    #endregion

    #region Properties


    /// <summary>
    /// Returns the object in entry.
    /// </summary>
    public abstract T Object { get; }

    /// <summary>
    /// Default weight of entry.
    /// </summary>
    public int BaseWeight { get => _weight; }

    /// <summary>
    /// Current weight of entry.
    /// </summary>
    public int Weight { get => _currentWeight; }


    #endregion

    #region Public Methods


    /// <summary>
    /// Shrinks the runtime weight after being selected.
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
    /// Resets the entry's weight.
    /// </summary>
    public void ResetEntry()
    {
        _currentWeight = _weight;
    }


    #endregion
}

/// <summary>
/// Represents an object as an entry and its weight.
/// 
/// Author: William Min
/// Date: 12/13/25
/// </summary>
/// <typeparam name="T">Type of entry</typeparam>
[Serializable]
public class WeightEntry<T> : GenericWeightEntry<T>
{
    #region Serialized Fields


    [Space]
    [SerializeField] private T _object; // Object to pass when selected


    #endregion

    #region Generic Weight Entry Callbacks


    public override T Object { get => _object; }


    #endregion
}

/// <summary>
/// Represents a subclass object as an entry and its weight.
/// 
/// Author: William Min
/// Date: 12/13/25
/// </summary>
/// <typeparam name="T">Type of entry</typeparam>
[Serializable]
public class SubclassWeightEntry<T> : GenericWeightEntry<T>
{
    #region Serialized Fields


    [Space]
    [SerializeReference, SubclassSelector] private T _object; // Object to pass when selected


    #endregion

    #region Generic Weight Entry Callbacks


    public override T Object { get => _object; }


    #endregion
}
