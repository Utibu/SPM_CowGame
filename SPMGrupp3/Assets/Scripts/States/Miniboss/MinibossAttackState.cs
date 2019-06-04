//Main Author: Sofia Kauko
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Miniboss/MinibossAttackState")]
public class MinibossAttackState : BondeBaseState
{
    // efter spec i GDD
    private float damage = 30;
    private float pushbackForce = 10;
    private float maxHits = 3; // antal ggr minibaus pallar ta en ram innan stun
    private float hitsTaken = 0;

    private float cooldown = 1.0f;

    // skriv en helt separat attack för denna miniboss. 
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Time.deltaTime % cooldown == 0)  // hope this mean "every second" but lets seee
        {
            Debug.Log("miniboss hits you");
            Attack();
        }

        // deathcheck (remove laterr)
        if (hitsTaken >= maxHits && owner.DoingKnockback == false)
        {
            owner.Transition<MinibossStunState>();
        }
    }

    private void Attack()
    {
        owner.player.playerValues.health -= 30f;
    }

    public override void Leave()
    {
        base.Leave();
    }

    

}
