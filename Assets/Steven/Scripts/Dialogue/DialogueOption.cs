using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueOption
{
     public string text;
     public Dialogue result;

    public override string ToString()
    {
        return string.Format($"{{text: {text}, result: {result.ToString()}}}");
    }
}
