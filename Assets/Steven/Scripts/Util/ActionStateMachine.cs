using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionStateMachine
{
    public class Empty : Action
    {
        public bool CheckForActionChange()
        {
            return false;
        }

        public void PostAction()
        {
            // No op
        }

        public void PreAction()
        {
            // No op
        }

        public void PreAction(Dictionary<string, object> preParams)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            // No op
        }

        public string Name()
        {
            return "empty";
        }
    }

    public Dictionary<string, Func<Action>> actions;
    public Action current;

    public ActionStateMachine() 
    {
        actions = new Dictionary<string, Func<Action>>();
        current = new Empty();
    }

    public void Initialize(string initial, Dictionary<string, Func<Action>> actions) {
        this.actions = actions;
        current = actions[initial].Invoke();
        current.PreAction(new Dictionary<string, object>());
    }

    public void Change(string actionName, Dictionary<string, object> changeParams)
    {
        if (!actions.ContainsKey(actionName)) {
            throw new System.Exception("Action not defined in ActionStateMachine");
        }

        current.PostAction();
        current = actions[actionName].Invoke();
        current.PreAction(changeParams);
    }

    // Update is called once per frame
    public void Update()
    {
        current.Update();
    }
}