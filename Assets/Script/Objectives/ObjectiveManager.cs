using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
[CreateAssetMenu]
// adapted from https://www.jonathanyu.xyz/2023/11/29/dynamic-objective-system-tutorial-for-unity/
public class ObjectiveManager : SingletonScriptableObject<ObjectiveManager>
{
    public Action<Objective> OnObjectiveAdded;
    public Action<Objective> OnObjectiveUpdated;
    public List<Objective> Objectives { get; } = new List<Objective>();

    private readonly Dictionary<string, List<Objective>> _objectiveMap = new();

    // Adds an objective to the objective manager.
    // If the objective has an EventTrigger, it's progress will be incremented
    // by AddProgress when the event is triggered. Multiple objectives can have
    // the same EventTrigger (i.e. MobKilled, ItemCollected, etc)
    public void AddObjective(Objective objective)
    {
        Objectives.Add(objective);
        if (!string.IsNullOrEmpty(objective.EventTrigger))
        {
            if (!_objectiveMap.ContainsKey(objective.EventTrigger))
            {
                _objectiveMap.Add(objective.EventTrigger,
                                  new List<Objective>());
            }

            _objectiveMap[objective.EventTrigger].Add(objective);
            Objectives.Add(objective);
        }

        OnObjectiveAdded?.Invoke(objective);
    }

    public void AddProgress(string eventTrigger, int value)
    {
        if (!_objectiveMap.ContainsKey(eventTrigger))
            return;
        foreach (var objective in _objectiveMap[eventTrigger])
        {
            objective.AddProgress(value);
            OnObjectiveUpdated.Invoke(objective);
        }

    }
    public void CheckInventory(InventoryItem newItem)
    {

       
        foreach (var objective in Objectives)
        {
            if (objective.ComparisonStr == newItem.itemName)
            {
                AddProgress(objective.EventTrigger, 1);   
            }
        }

    }
    public List<Objective> FindObjectives(string eventTrigger)
    {

        if (_objectiveMap.ContainsKey(eventTrigger))
        {
            return _objectiveMap[eventTrigger];
        }
        else return new List<Objective>();
    }
}