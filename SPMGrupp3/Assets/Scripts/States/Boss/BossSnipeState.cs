using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossSnipeState")]
public class BossSnipeState : BossBaseState
{
    private float countdown;
    //private Vector3 originalPosition;
    [SerializeField] private float attackSpeed;
    private float originalAttackSpeed;
    //private float rangeFromDestination = 3f;

    public override void Enter()
    {
        base.Enter();
        //originalPosition = owner.transform.position;
        //owner.bulletsShotSinceReload = 0;
        //owner.agnes.Warp(owner.snipeLocation.transform.position);
        //owner.agnes.isStopped = true;
        //owner.agnes.enabled = false;
        originalAttackSpeed = owner.attackSpeed;
        owner.attackSpeed = attackSpeed;
        owner.renderColor.material.color = Color.red;
        owner.ToggleActiveWeapon();

    }

    public override void Update()
    {
        lookAt();

        //if(!Helper.IsWithinDistance(owner.transform.position, owner.snipeLocation.transform.position, rangeFromDestination))
        //{
            //owner.transform.position = Vector3.Slerp(owner.transform.position, owner.snipeLocation.transform.position, 1.5f * Time.deltaTime);
        //}
        
        
        //owner.agnes.enabled = true;

        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            attack();
            countdown = attackSpeed;

        }
        
    }

    public override void Leave()
    {
        base.Leave();
        //owner.agnes.isStopped = false;
        //owner.agnes.enabled = true;
        //owner.Destination = originalPosition;
        //owner.agnes.Warp(originalPosition);
        owner.attackSpeed = originalAttackSpeed;
        owner.bulletsShotSinceReload = 0;
        owner.renderColor.material.color = Color.white;
        owner.ToggleActiveWeapon();
    }
}
