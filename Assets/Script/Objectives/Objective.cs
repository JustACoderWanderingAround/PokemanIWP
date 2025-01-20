using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// taken from https://www.jonathanyu.xyz/2023/11/29/dynamic-objective-system-tutorial-for-unity/

public class Objective
{
    // Invoked when the objective is completed
    public Action OnComplete;
    // Invoked when the objective's progress changes
    public Action OnValueChange;

    // Used to AddProgress from ObjectiveManager.
    // Can be empty if objective progress is managed elsewhere.
    public string EventTrigger { get; }
    public bool IsComplete { get; private set; }
    public int MaxValue { get; }
    public int CurrentValue { get; private set; }

    public string ComparisonStr { get { return _comparisonStr;  } }
    private readonly string _comparisonStr;

    private readonly string _statusText;

    // Status text can have 2 parameters {0} and {1} for current and max value
    // Example: "Kill {0} of {1} enemies"
    public Objective(string eventTrigger, string statusText, int maxValue, string comparisonStr = "")
    {
        EventTrigger = eventTrigger;
        _statusText = statusText;
        MaxValue = maxValue;
        this._comparisonStr = comparisonStr;
    }

    public Objective(string statusText, int maxValue) : this("", statusText, maxValue) { }

    private void CheckCompletion()
    {
        if (CurrentValue >= MaxValue)
        {
            IsComplete = true;
            OnComplete?.Invoke();
        }
    }

    public void AddProgress(int value)
    {
        if (IsComplete)
        {
            return;
        }
        CurrentValue += value;
        if (CurrentValue > MaxValue)
        {
            CurrentValue = MaxValue;
        }
        OnValueChange?.Invoke();
        CheckCompletion();
    }

    public string GetStatusText()
    {
        return string.Format(_statusText, CurrentValue, MaxValue);
    }
}

