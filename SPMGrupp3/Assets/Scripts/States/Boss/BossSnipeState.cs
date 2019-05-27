using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossSnipeState")]
public class BossSnipeState : BossBaseState
{
    private float countdown;
    [SerializeField] private float attackSpeed;
    private float originalAttackSpeed;

    public override void Enter()
    {
        base.Enter();
        originalAttackSpeed = owner.attackSpeed;
        owner.attackSpeed = attackSpeed;
        owner.renderColor.material.color = Color.red;
        owner.ToggleActiveWeapon();
        

    }

    public override void Update()
    {
        owner.LaserSightRenderer.SetPosition(0, owner.LaserSightRenderer.gameObject.transform.position);
        lookAt();
        countdown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.J))
        {
            toggleLaserSight();
        }

        RaycastHit hit;
        if(Physics.Raycast(owner.ActiveWeapon.transform.position, owner.LaserSightRenderer.gameObject.transform.forward, out hit))
        {
            if (countdown <= 0 && hit.collider.tag.Equals("Player"))
            {
                attack();
                countdown = attackSpeed;

            }

            if (hit.collider)
            {
                owner.LaserSightRenderer.SetPosition(1, hit.point);
            }
            else
            {
                owner.LaserSightRenderer.SetPosition(1, owner.LaserSightRenderer.gameObject.transform.forward * 5000);
            }
        }
        //owner.LaserSightRenderer.SetPosition(1, )
        
        
        
    }

    private void toggleLaserSight()
    {
        if(owner.LaserSightRenderer.isVisible == true)
        {
            owner.LaserSightRenderer.enabled = false;
        }
        else
        {
            owner.LaserSightRenderer.enabled = true;
        }
    }

    public override void Leave()
    {
        base.Leave();
        owner.attackSpeed = originalAttackSpeed;
        owner.bulletsShotSinceReload = 0;
        owner.renderColor.material.color = Color.white;
        owner.ToggleActiveWeapon();
    }
}
