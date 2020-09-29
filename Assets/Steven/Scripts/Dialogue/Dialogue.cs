using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public List<string> sentences;
    public DialogueSelect select;

    public override string ToString() 
    {
        string sentencesSerialized = sentences == null ? "<Empty>" : String.Join(", ", sentences);
        string optionsSerialized = select == null ? "<Empty>" : select.ToString(); 
        return $"{{sentences: {sentencesSerialized}, options: {optionsSerialized}}}";
    }
}
