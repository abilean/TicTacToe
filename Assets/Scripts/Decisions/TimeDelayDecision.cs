using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decision/TimeDelayDecision")]
public class TimeDelayDecision : Decision {

    public float DelayTime = 1;

    public override bool Decide(StateController controller)
    {
        return (DelayTime > controller.stateTimeElapsed);
    }
}
