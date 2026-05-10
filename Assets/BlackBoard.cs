using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BlackboardValueType { Int, Float, Bool, String, Transform, GameObject }

[Serializable]
public class BlackboardEntry
{
    public string key;
    public BlackboardValueType valueType;

    public string primitiveValue;
    public UnityEngine.Object objectValue;
}


[Serializable]
[CreateAssetMenu()]
public class Blackboard : ScriptableObject
{
    [SerializeField]
    private List<BlackboardEntry> entries = new();

    private Dictionary<string, object> _runtimeData = new();

    private void Awake() => PopulateFromEntries();

    public void PopulateFromEntries()
    {
        _runtimeData.Clear();
        foreach (var e in entries)
        {
            object value = e.valueType switch
            {
                BlackboardValueType.Int        => int.TryParse(e.primitiveValue, out int i)       ? i     : 0,
                
                //to make both coma and dot valid
                BlackboardValueType.Float => float.TryParse(e.primitiveValue,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float f) ? f : 0f,

                //to make it case-insensitive along with accepting 0 and 1
                BlackboardValueType.Bool =>
                e.primitiveValue.Trim().ToLower() switch
                {
                    "true" or "1"  => true,
                    "false" or "0" => false,
                    _ => false
                },

                BlackboardValueType.String     => e.primitiveValue,
                BlackboardValueType.Transform  => e.objectValue as Transform,
                BlackboardValueType.GameObject => e.objectValue as GameObject,
                _ => null
            };
            _runtimeData[e.key] = value;
        }
    }

    public object this[string key]
    {
        get => _runtimeData.TryGetValue(key, out var v) ? v : null;
        set => _runtimeData[key] = value;
    }

    public T TryGetValue<T>(string key, T fallback = default)
    {
        if (_runtimeData.TryGetValue(key, out var v) && v is T t)
            return t;
        return fallback;
    }

    public bool ContainsKey(string key) => _runtimeData.ContainsKey(key);

    public Blackboard Clone()
    {
        Blackboard blackboard = Instantiate(this);
        blackboard._runtimeData = new Dictionary<string, object>();
        blackboard.PopulateFromEntries();
        return blackboard;
    }
}
