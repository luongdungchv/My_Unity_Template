using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunBehaviour : StateBehaviour
{
    public override void OnStateEnter(StateController stateController)
    {
        Debug.Log("state endter");
    }   

    public override void OnStateExit(StateController stateController)
    {
    }

    public override void OnStateFixedUpdate(StateController stateController)
    {
    }

    public override void OnStateUpdate(StateController stateController)
    {
    }
}
