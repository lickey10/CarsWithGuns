using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Motion;
using RAIN.Navigation;
using RAIN.Representation;
using RAIN.Entities;
using RAIN.Entities.Aspects;

[RAINAction]
public class Zombie_ApplyDamage : RAINAction
{
	public Expression AspectVariable = new Expression();//the person taking damage
	GameObject damagedPlayer;
	RAINAspect tAspect;
	//EnemyDamageReceiver enemyDamageReceiver;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);

//		tAspect = ai.WorkingMemory.GetItem<RAINAspect>(AspectVariable.VariableName);//I believe this gets the entity that is detected on the person taking damage
//
//		damagedPlayer = (GameObject)tAspect.MountPoint.gameObject;//this gets the gameobject of the player taking damage
//		enemyDamageReceiver = damagedPlayer.GetComponent<EnemyDamageReceiver> ();


//		tAspect.MountPoint.transform.GetComponent(EnemyDamageReceiver);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
//		if(int.Parse(enemyDamageReceiver.hitPoints.ToString()) == 1)
//			tAspect.IsActive = false;
//
//		damagedPlayer.SendMessage("ApplyDamage",1,SendMessageOptions.DontRequireReceiver);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}