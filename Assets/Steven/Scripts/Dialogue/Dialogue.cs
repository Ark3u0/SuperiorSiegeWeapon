using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public string name;
    public List<string> sentences;
    public List<DialogueOption> options;

    public override string ToString() 
    {
        string sentencesSerialized = sentences == null ? "<Empty>" : String.Join(", ", sentences);
        string optionsSerialized = options == null ? "<Empty>" : String.Join(", ", options.ConvertAll<string>(opt => opt.ToString())); 
        return $"{{sentences: {sentencesSerialized}, options: {optionsSerialized}}}";
    }
}
