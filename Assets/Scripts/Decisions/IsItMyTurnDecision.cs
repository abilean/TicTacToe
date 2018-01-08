using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decision/IsitMyTurnDecision")]
public class IsItMyTurnDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        if (controller.MyMark == controller.curTurn)
            return true;
        else
            return false;
                   
    }
}

