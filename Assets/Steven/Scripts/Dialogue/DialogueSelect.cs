using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueSelect
{
    public string yes;
    public string no;

    public override string ToString()
    {
        return string.Format($"{{yes: {yes}, no: {no}}}");
    }
}
