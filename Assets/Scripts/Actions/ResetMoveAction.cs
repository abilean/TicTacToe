using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Action/ResetMoveAction")]
public class ResetMoveAction : Action
{

    public override void Act(StateController controller)
    {
        controller.playedMove = false;
    }
}