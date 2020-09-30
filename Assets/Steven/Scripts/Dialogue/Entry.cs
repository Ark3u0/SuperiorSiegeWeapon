using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Entry {
    public string key;
    public Dialogue value;

    public override string ToString() 
    {
        return $"{{key: {key}, value: {value.ToString()}}}";
    }
}
