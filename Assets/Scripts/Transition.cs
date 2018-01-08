using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "AI/Transition")]
public class Transition : ScriptableObject {

    public Decision decision;
    public State trueState;
    public State falseState;
}

