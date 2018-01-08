using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/State")]
public class State : ScriptableObject {

    [Tooltip("All the actions to perform every update")]
    public Action[] Actions;
    [Tooltip("The transitions that will send it to another state")]
    public Transition[] Transitions;
    [Tooltip("Actions to be performed only when the State is starting up")]
    public Action[] InitialActions;
    [Tooltip("Actions to be performed only when the State is exiting")]
    public Action[] ExitActions;
    public Color sceneGizmoColor = Color.grey;

    public void UpdateState(StateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    public void InitializeState(StateController controller)
    {
        foreach (Action act in InitialActions)
            act.Act(controller);
    }

    public void ExitState(StateController controller)
    {
        foreach (Action act in ExitActions)
            act.Act(controller);
    }

    private void DoActions(StateController controller)
    {
        foreach(Action act in Actions)
        {
            act.Act(controller);
        }
    }

    /// <summary>
    /// Checks decsion and transitions to the coresponding state if that state isn't null.
    /// </summary>
    /// <param name="controller"></param>
    private void CheckTransitions(StateController controller)
    {
        foreach(Transition trans in Transitions)
        {
            bool decisionSucceeded = trans.decision.Decide(controller);

            if (decisionSucceeded)
            {
                if(trans.trueState != null)
                    controller.TransitionToState(trans.trueState);
            }
            else
            {
                if (trans.falseState != null)
                    controller.TransitionToState(trans.falseState);
            }
        }
    }

    

}
