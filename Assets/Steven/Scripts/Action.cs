using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public interface Action
{
    bool CheckForActionChange();
    void PreAction(Dictionary<string, object> preParams);
    void PostAction();
    void Update();
    string Name();
}
