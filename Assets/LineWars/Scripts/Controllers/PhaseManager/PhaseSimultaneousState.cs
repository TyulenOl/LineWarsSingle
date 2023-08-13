using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

public class PhaseSimultaneousState : Phase
{
    private int actorsLeft;
    private readonly Dictionary<IActor, bool> actorsReadiness;
    public override bool AreActorsDone => actorsLeft <= 0;

    public PhaseSimultaneousState(PhaseType phase, PhaseManager phaseManager) : base(phase, phaseManager)
    {
        actorsReadiness = new Dictionary<IActor, bool>();
        actorsLeft = manager.Actors.Count;
    }

    public override void OnEnter()
    {
        actorsLeft = 0;
        manager.ActorTurnEnded += OnActorsTurnEnded;
        foreach(var actor in manager.Actors)
        {
            actorsReadiness[actor] = true;
            if(actor.CanExecuteTurn(Type))
            {
                actor.ExecuteTurn(Type);
                actorsLeft++;
                actorsReadiness[actor] = false;
            }
            
        }
    }

    public override void OnExit()
    {
        manager.ActorTurnEnded -= OnActorsTurnEnded;
    }

    private void OnActorsTurnEnded(IActor actor, PhaseType phase)
    {
        if(actorsReadiness[actor] == true)
        {
            Debug.LogError($"{actor} ended turn {phase}; He ended {Type} earlier!");
        }

        actorsLeft--;
        actorsReadiness[actor] = true;
    }
    
}
