using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decision/FinishedMyTurnDecision")]
public class FinishedMyTurnDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return (controller.playedMove);
    }
}
