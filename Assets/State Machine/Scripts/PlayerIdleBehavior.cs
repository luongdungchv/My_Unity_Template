using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleBehavior : StateBehaviour
{
    public override void OnStateEnter(StateController stateController)
    {
        Debug.Log("state enter");
    }

    public override void OnStateExit(StateController stateController)
    {
        Debug.Log("state exit");
    }

    public override void OnStateFixedUpdate(StateController stateController)
    {
        Debug.Log("state fixed update");
    }

    public override void OnStateUpdate(StateController stateController)
    {
        Debug.Log("state update");
    }
}
