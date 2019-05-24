using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Boss/BossReloadState")]
public class BossReloadState : BossBaseState
{
    [SerializeField] private float reloadTime;
    private Image reloadMeterBack;
    private GameObject reloadHolder;
    private BasicTimer timer;

    public override void Enter()
    {
        owner.reloadMeter.gameObject.SetActive(true);
        //reloadMeterBack = owner.reloadMeter.transform.GetComponentInChildren<Image>(true);
        reloadHolder = owner.reloadMeter.transform.Find("ReloadMeterBack").gameObject;
        reloadMeterBack = reloadHolder.GetComponent<Image>();

        Debug.Log("Reload holder: " + reloadHolder.name);
        timer = new BasicTimer(reloadTime);
        owner.renderColor.material.color = Color.blue;
        owner.agnes.isStopped = true;
        EventSystem.Current.FireEvent(new PlaySoundEvent(owner.transform.position, owner.ReloadSound, 1f, 1f, 1f));
        Debug.Log(reloadMeterBack.name);
    }

    public override void Update()
    {
        timer.Update(Time.deltaTime);

        owner.reloadMeter.fillAmount = timer.GetPercentage();
        reloadMeterBack.fillAmount = owner.reloadMeter.fillAmount;

        if (timer.IsCompleted(Time.deltaTime, false, false))
        {
            owner.Transition<BossPatrolState>();
        }
    }

    public override void Leave()
    {
        base.Leave();
        owner.renderColor.material.color = Color.white;
        owner.agnes.isStopped = false;
        owner.reloadMeter.fillAmount = 0;
        owner.reloadMeter.gameObject.SetActive(false);
    }


}
