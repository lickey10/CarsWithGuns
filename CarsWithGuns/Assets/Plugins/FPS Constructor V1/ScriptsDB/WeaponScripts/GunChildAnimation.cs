using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class GunChildAnimation : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 CopyrightÂ© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public string fireAnim;
    public string emptyFireAnim;
    public string reloadAnim;
    public string emptyReloadAnim;
    public string takeOutAnim;
    public string putAwayAnim;
    public string enterSecondaryAnim;
    public string exitSecondaryAnim;
    public string reloadIn;
    public string reloadOut;
    public string walkAnimation;
    public string secondaryWalkAnim;
    public string secondarySprintAnim;
    public float walkSpeedModifier;
    public bool walkWhenAiming;
    public string sprintAnimation;
    public string nullAnim;
    public string secondaryNullAnim;
    public string idleAnim;
    public string secondaryIdleAnim;
    public string chargeAnim;
    private float stopAnimTime;
    public bool aim;
    private CharacterMotorDB CM;
    private bool idle;
    private bool secondary;
    private string walkAnim;
    private string sprintAnim;
    private string nullAnimation;
    public bool hasSecondary;
    public string secondaryReloadAnim;
    public string secondaryReloadEmpty;
    public string secondaryFireAnim;
    public string secondaryEmptyFireAnim;
    //melee
    public int animCount;
    public string[] fireAnims;
    public string[] reloadAnims;
    public int index;
    public int lastIndex;
    public bool melee;
    public bool random;
    public float lastSwingTime;
    public float resetTime;
    public GunScript gs;
    private Vector3 dir;
    private float moveWeight;
    private float nullWeight;
    private bool useStrafe;
    public virtual void PlayAnim(string name)
    {
        this.idle = false;
        if ((this.GetComponent<Animation>()[name] == null) || !this.gs.gunActive)
        {
            return;
        }
        this.GetComponent<Animation>().Stop(name);
        this.GetComponent<Animation>().Rewind(name);
        this.GetComponent<Animation>().CrossFade(name, 0.2f);
        this.stopAnimTime = Time.time + this.GetComponent<Animation>()[name].length;
    }

    public virtual void PlayAnim(string name, float time)
    {
        this.idle = false;
        if ((this.GetComponent<Animation>()[name] == null) || !this.gs.gunActive)
        {
            return;
        }
        this.GetComponent<Animation>().Stop(name);
        this.GetComponent<Animation>().Rewind(name);
        this.GetComponent<Animation>()[name].speed = this.GetComponent<Animation>()[name].clip.length / time;
        this.GetComponent<Animation>().CrossFade(name, 0.2f);
        this.stopAnimTime = Time.time + this.GetComponent<Animation>()[name].length;
    }

    public virtual void Update()// 	}
    {
        Vector3 veloc = default(Vector3);
        float strafeSpeed = 0.0f;
        if (this.gs != null)
        {
            if (!this.gs.gunActive)
            {
                return;
            }
        }
        if (this.GetComponent<Animation>()[this.nullAnim] == null)
        {
            return;
        }
        if (this.GetComponent<Animation>()[this.walkAnimation] == null)
        {
            return;
        }
        CharacterMotorDB CM = PlayerWeapons.CM;
        if ((CM != null) && !CM.grounded)
        {
            this.nullWeight = Mathf.Lerp(this.nullWeight, 1, Time.deltaTime * 5);
            this.moveWeight = 0;
        }
        if (Time.time > (this.stopAnimTime + 0.1f))
        {
            this.moveWeight = Mathf.Lerp(this.moveWeight, 1, Time.deltaTime * 5);
            this.nullWeight = Mathf.Lerp(this.nullWeight, 1, Time.deltaTime * 5);
        }
        else
        {
            this.moveWeight = 0;
            this.nullWeight = 0;
        }
        this.GetComponent<Animation>()[this.nullAnim].weight = this.nullWeight;
        if (PlayerWeapons.CM != null)
        {
            veloc = PlayerWeapons.CM.movement.velocity;
        }
        Transform trans = PlayerWeapons.player.transform;
        this.dir = Vector3.Lerp(this.dir, trans.InverseTransformDirection(veloc), Time.deltaTime * 6);
        Vector3 dirN = this.dir.normalized;
        float forwardWeight = dirN.z;
        float rightWeight = dirN.x;
        //Weight and speed from direction
        this.GetComponent<Animation>()[this.walkAnimation].weight = Mathf.Abs(forwardWeight) * this.moveWeight;
        if (CM != null)
        {
            this.GetComponent<Animation>()[this.walkAnimation].speed = this.dir.z / CM.movement.maxForwardSpeed;
        }
        float strafeWeight = Mathf.Abs(rightWeight) * this.moveWeight;
        if (CM != null)
        {
            strafeSpeed = (this.dir.x / CM.movement.maxSidewaysSpeed) * this.moveWeight;
        }
        //Apply to strafe animation
        /* if(useStrafe){
    	animation[strafeRightAnimation].weight = strafeWeight;
   		animation[strafeRightAnimation].speed = strafeSpeed;
   	} else {*/
        //Handle if we don't have a strafe animation by applying to walk animation
        this.GetComponent<Animation>()[this.walkAnimation].weight = Mathf.Max(this.GetComponent<Animation>()[this.walkAnimation].weight, strafeWeight);
        if (Mathf.Abs(strafeSpeed) > Mathf.Abs(this.GetComponent<Animation>()[this.walkAnimation].speed))
        {
            this.GetComponent<Animation>()[this.walkAnimation].speed = strafeSpeed;
        }
    }

    /*function LateUpdate(){
	if(gs)
		if(!gs.gunActive)
			return;
	if(animation[walkAnim] != null){
		var temp : boolean = animation[walkAnim].enabled;
	} else {
		temp = false;
	}
	
	if(animation[sprintAnim] != null){
		var temp2 : boolean = animation[sprintAnim].enabled;
	} else {
		temp2 = false;
	}

	/*if(!animation.IsPlaying(nullAnim))
		animation.CrossFade(nullAnim, .4);
}*/
    public virtual void ReloadAnim(float reloadTime)
    {
        this.idle = false;
        if (this.GetComponent<Animation>()[this.reloadAnim] == null)
        {
            return;
        }
        //animation.Stop(reloadAnim);
        this.GetComponent<Animation>().Rewind(this.reloadAnim);
        this.GetComponent<Animation>()[this.reloadAnim].speed = this.GetComponent<Animation>()[this.reloadAnim].clip.length / reloadTime;
        this.GetComponent<Animation>().Play(this.reloadAnim);
        this.stopAnimTime = Time.time + reloadTime;
    }

    public virtual void ReloadEmpty(float reloadTime)
    {
        this.idle = false;
        if (this.GetComponent<Animation>()[this.emptyReloadAnim] == null)
        {
            return;
        }
        this.GetComponent<Animation>().Rewind(this.emptyReloadAnim);
        this.GetComponent<Animation>()[this.emptyReloadAnim].speed = this.GetComponent<Animation>()[this.emptyReloadAnim].clip.length / reloadTime;
        this.GetComponent<Animation>().Play(this.emptyReloadAnim);
        this.stopAnimTime = Time.time + reloadTime;
    }

    public virtual void FireAnim()
    {
        this.idle = false;
        if (this.GetComponent<Animation>()[this.fireAnim] == null)
        {
            return;
        }
        this.GetComponent<Animation>().Rewind(this.fireAnim);
        this.GetComponent<Animation>().CrossFade(this.fireAnim, 0.05f);
        this.stopAnimTime = Time.time + this.GetComponent<Animation>()[this.fireAnim].clip.length;
    }

    public virtual void SecondaryReloadEmpty(float reloadTime)
    {
        this.idle = false;
        if (this.GetComponent<Animation>()[this.secondaryReloadEmpty] == null)
        {
            return;
        }
        this.GetComponent<Animation>()[this.secondaryReloadEmpty].speed = this.GetComponent<Animation>()[this.secondaryReloadEmpty].clip.length / reloadTime;
        this.GetComponent<Animation>().Rewind(this.secondaryReloadEmpty);
        this.GetComponent<Animation>().CrossFade(this.secondaryReloadEmpty, 0.2f);
        this.stopAnimTime = Time.time + reloadTime;
    }

    public virtual void SecondaryReloadAnim(float reloadTime)
    {
        this.idle = false;
        if (this.GetComponent<Animation>()[this.secondaryReloadAnim] == null)
        {
            return;
        }
        this.GetComponent<Animation>()[this.secondaryReloadAnim].speed = this.GetComponent<Animation>()[this.secondaryReloadAnim].clip.length / reloadTime;
        this.GetComponent<Animation>().Rewind(this.secondaryReloadAnim);
        this.GetComponent<Animation>().CrossFade(this.secondaryReloadAnim, 0.2f);
        this.stopAnimTime = Time.time + reloadTime;
    }

    public virtual void SecondaryFireAnim()
    {
        this.idle = false;
        if (this.GetComponent<Animation>()[this.secondaryFireAnim] == null)
        {
            return;
        }
        this.GetComponent<Animation>().Rewind(this.secondaryFireAnim);
        this.GetComponent<Animation>().CrossFade(this.secondaryFireAnim, 0.2f);
        this.stopAnimTime = Time.time + this.GetComponent<Animation>()[this.secondaryFireAnim].clip.length;
    }

    public virtual void TakeOutAnim(float takeOutTime)
    {
        this.idle = false;
        if (takeOutTime <= 0)
        {
            return;
        }
        if (this.GetComponent<Animation>()[this.takeOutAnim] == null)
        {
            return;
        }
        this.GetComponent<Animation>().Stop(this.putAwayAnim);
        this.GetComponent<Animation>().Stop(this.takeOutAnim);
        this.GetComponent<Animation>().Rewind(this.takeOutAnim);
        this.GetComponent<Animation>()[this.takeOutAnim].speed = this.GetComponent<Animation>()[this.takeOutAnim].clip.length / takeOutTime;
        this.GetComponent<Animation>().Play(this.takeOutAnim);
        this.stopAnimTime = Time.time + takeOutTime;
    }

    public virtual void PutAwayAnim(float putAwayTime)
    {
        this.idle = false;
        this.secondary = false;
        this.nullAnimation = this.nullAnim;
        if (putAwayTime <= 0)
        {
            return;
        }
        if (this.GetComponent<Animation>()[this.putAwayAnim] == null)
        {
            return;
        }
        this.GetComponent<Animation>().Stop(this.putAwayAnim);
        this.GetComponent<Animation>().Rewind(this.putAwayAnim);
        this.GetComponent<Animation>()[this.putAwayAnim].speed = this.GetComponent<Animation>()[this.putAwayAnim].clip.length / putAwayTime;
        this.GetComponent<Animation>().CrossFade(this.putAwayAnim, 0.1f);
        this.stopAnimTime = Time.time + putAwayTime;
    }

    public virtual void SingleFireAnim(float fireRate)
    {
        this.idle = false;
        if (this.GetComponent<Animation>()[this.fireAnim] == null)
        {
            return;
        }
        this.GetComponent<Animation>().Stop(this.fireAnim);
        this.GetComponent<Animation>()[this.fireAnim].speed = this.GetComponent<Animation>()[this.fireAnim].clip.length / fireRate;
        this.GetComponent<Animation>().Rewind(this.fireAnim);
        this.GetComponent<Animation>().CrossFade(this.fireAnim, 0.05f);
        this.stopAnimTime = Time.time + fireRate;
    }

    public virtual void EmptyFireAnim()
    {
        this.idle = false;
        if (this.GetComponent<Animation>()[this.emptyFireAnim] == null)
        {
            return;
        }
        this.GetComponent<Animation>().Stop(this.emptyFireAnim);
        this.GetComponent<Animation>().Rewind(this.emptyFireAnim);
        this.GetComponent<Animation>().CrossFade(this.emptyFireAnim, 0.05f);
        this.stopAnimTime = Time.time + this.GetComponent<Animation>()[this.emptyFireAnim].length;
    }

    public virtual void SecondaryEmptyFireAnim()
    {
        this.idle = false;
        if (this.GetComponent<Animation>()[this.secondaryEmptyFireAnim] == null)
        {
            return;
        }
        this.GetComponent<Animation>().Stop(this.secondaryEmptyFireAnim);
        this.GetComponent<Animation>().Rewind(this.secondaryEmptyFireAnim);
        this.GetComponent<Animation>().CrossFade(this.secondaryEmptyFireAnim, 0.05f);
        this.stopAnimTime = Time.time + this.GetComponent<Animation>()[this.secondaryEmptyFireAnim].length;
    }

    public virtual void EnterSecondary(float t)
    {
        if (this.GetComponent<Animation>()[this.secondaryNullAnim] != null)
        {
            this.nullAnimation = this.secondaryNullAnim;
        }
        this.idle = false;
        this.secondary = true;
        if (this.GetComponent<Animation>()[this.enterSecondaryAnim] == null)
        {
            return;
        }
        this.GetComponent<Animation>().Stop(this.enterSecondaryAnim);
        this.GetComponent<Animation>()[this.enterSecondaryAnim].speed = this.GetComponent<Animation>()[this.enterSecondaryAnim].clip.length / t;
        this.GetComponent<Animation>().Rewind(this.enterSecondaryAnim);
        this.GetComponent<Animation>().CrossFade(this.enterSecondaryAnim, 0.2f);
        this.stopAnimTime = Time.time + t;
    }

    public virtual void ExitSecondary(float t)
    {
        this.nullAnimation = this.nullAnim;
        this.idle = false;
        this.secondary = false;
        if (this.GetComponent<Animation>()[this.exitSecondaryAnim] == null)
        {
            return;
        }
        this.GetComponent<Animation>().Stop(this.exitSecondaryAnim);
        this.GetComponent<Animation>()[this.exitSecondaryAnim].speed = this.GetComponent<Animation>()[this.exitSecondaryAnim].clip.length / t;
        this.GetComponent<Animation>().Rewind(this.exitSecondaryAnim);
        this.GetComponent<Animation>().CrossFade(this.exitSecondaryAnim, 0.2f);
        this.stopAnimTime = Time.time + t;
    }

    public virtual void SingleSecFireAnim(float fireRate)
    {
        this.idle = false;
        if (this.GetComponent<Animation>()[this.secondaryFireAnim] == null)
        {
            return;
        }
        this.GetComponent<Animation>().Stop(this.secondaryFireAnim);
        this.GetComponent<Animation>()[this.secondaryFireAnim].speed = this.GetComponent<Animation>()[this.secondaryFireAnim].clip.length / fireRate;
        this.GetComponent<Animation>().Rewind(this.secondaryFireAnim);
        this.GetComponent<Animation>().CrossFade(this.secondaryFireAnim, 0.05f);
        this.stopAnimTime = Time.time + fireRate;
    }

    public virtual void ReloadIn(float reloadTime)
    {
        this.idle = false;
        if (this.GetComponent<Animation>()[this.reloadIn] == null)
        {
            return;
        }
        this.GetComponent<Animation>()[this.reloadIn].speed = this.GetComponent<Animation>()[this.reloadIn].clip.length / reloadTime;
        this.GetComponent<Animation>().Rewind(this.reloadIn);
        this.GetComponent<Animation>().Play(this.reloadIn);
        this.stopAnimTime = Time.time + reloadTime;
    }

    public virtual void ReloadOut(float reloadTime)
    {
        this.idle = false;
        if (this.GetComponent<Animation>()[this.reloadOut] == null)
        {
            return;
        }
        this.GetComponent<Animation>()[this.reloadOut].speed = this.GetComponent<Animation>()[this.reloadOut].clip.length / reloadTime;
        this.GetComponent<Animation>().Rewind(this.reloadOut);
        this.GetComponent<Animation>().Play(this.reloadOut);
        this.stopAnimTime = Time.time + reloadTime;
    }

    public virtual IEnumerator IdleAnim()
    {
        if (((this.GetComponent<Animation>()[this.idleAnim] == null) || this.idle) || (Time.time < this.stopAnimTime))
        {
            yield break;
        }
        if (!PlayerWeapons.doesIdle)
        {
            this.idle = true;
            yield break;
        }
        this.idle = true;
        if (this.secondary)
        {
            this.GetComponent<Animation>().Stop(this.secondaryIdleAnim);
            this.GetComponent<Animation>().Rewind(this.secondaryIdleAnim);
            this.GetComponent<Animation>().CrossFade(this.secondaryIdleAnim, 0.2f);
            this.stopAnimTime = Time.time + this.GetComponent<Animation>()[this.secondaryIdleAnim].clip.length;
            yield break;
        }
        this.GetComponent<Animation>().Stop(this.idleAnim);
        this.GetComponent<Animation>().Rewind(this.idleAnim);
        this.GetComponent<Animation>().CrossFade(this.idleAnim, 0.2f);
        this.stopAnimTime = Time.time + this.GetComponent<Animation>()[this.idleAnim].clip.length;
        yield return new WaitForSeconds(this.GetComponent<Animation>()[this.idleAnim].clip.length);
        this.idle = false;
    }

    public virtual void Start()
    {
        this.idle = false;
        this.CM = PlayerWeapons.CM;
        this.stopAnimTime = 10;
        this.aim = false;
        this.nullAnimation = this.nullAnim;
        /*for (s : AnimationState in animation) {
    	s.layer = 1;
	}*/
        if (this.GetComponent<Animation>()[this.nullAnim] != null)
        {
            this.GetComponent<Animation>()[this.nullAnim].layer = -2;
            this.GetComponent<Animation>()[this.nullAnim].enabled = true;
        }
        if (this.GetComponent<Animation>()[this.walkAnimation] != null)
        {
            this.GetComponent<Animation>()[this.walkAnimation].layer = -1;
            this.GetComponent<Animation>()[this.walkAnimation].enabled = true;
        }
        /*	if(animation[strafeRightAnimation] != null){
		animation[strafeRightAnimation].layer = -1;
		animation[strafeRightAnimation].enabled = true;
	} else {
		useStrafe = false;
	}*/
        if (this.GetComponent<Animation>()[this.sprintAnim] != null)
        {
            this.GetComponent<Animation>()[this.sprintAnim].layer = -1;
        }
        this.GetComponent<Animation>().SyncLayer(-1);
        this.stopAnimTime = -1;
    }

    public virtual void Aiming()
    {
        this.idle = false;
        this.aim = true;
        bool temp = false;
        bool temp2 = false;
        if ((this.GetComponent<Animation>()[this.walkAnim] != null) && !this.walkWhenAiming)
        {
            this.GetComponent<Animation>().Stop(this.walkAnim);
        }
        if (this.GetComponent<Animation>()[this.sprintAnim] != null)
        {
            this.GetComponent<Animation>().Stop(this.sprintAnim);
        }
        if (this.GetComponent<Animation>()[this.nullAnim] != null)
        {
            this.GetComponent<Animation>().CrossFade(this.nullAnimation, 0.2f);
        }
    }

    public virtual void StopAiming()
    {
        this.aim = false;
    }

    public virtual void FireMelee(float fireRate)
    {
        string temp = null;
        if (this.random)
        {
            this.lastIndex = this.index;
            this.index = Mathf.Round(Random.Range(0, this.animCount - 1));
            if (this.index == this.lastIndex)
            {
                if (this.index == (this.animCount - 1))
                {
                    this.index = Mathf.Clamp(this.index - 1, 0, this.animCount - 1);
                }
                else
                {
                    this.index = this.index + 1;
                }
            }
        }
        else
        {
            if (Time.time > (this.lastSwingTime + this.resetTime))
            {
                this.index = 0;
            }
            else
            {
                this.index = this.index + 1;
            }
            if (this.index == this.animCount)
            {
                this.index = 0;
            }
            this.lastSwingTime = Time.time;
        }
        temp = this.fireAnims[this.index];
        this.idle = false;
        if ((temp == "") || (this.GetComponent<Animation>()[temp] == null))
        {
            return;
        }
        //animation.Stop(temp);
        this.GetComponent<Animation>()[temp].speed = this.GetComponent<Animation>()[temp].clip.length / fireRate;
        //animation.Rewind(temp);
        this.GetComponent<Animation>().CrossFade(temp, 0.05f);
        this.stopAnimTime = Time.time + fireRate;
    }

    public virtual void ReloadMelee(float fireRate)
    {
        string temp = null;
        temp = this.reloadAnims[this.index];
        this.idle = false;
        if (this.GetComponent<Animation>()[temp] == null)
        {
            return;
        }
        this.GetComponent<Animation>().Stop(this.fireAnims[this.index]);
        this.GetComponent<Animation>()[temp].speed = this.GetComponent<Animation>()[temp].clip.length / fireRate;
        //animation.Rewind(temp);
        this.GetComponent<Animation>().CrossFadeQueued(temp, 0.05f);
        this.stopAnimTime = Time.time + fireRate;
    }

    public GunChildAnimation()
    {
        this.fireAnim = "Fire";
        this.emptyFireAnim = "";
        this.reloadAnim = "Reload";
        this.emptyReloadAnim = "Reload";
        this.takeOutAnim = "TakeOut";
        this.putAwayAnim = "PutAway";
        this.enterSecondaryAnim = "EnterSecondary";
        this.exitSecondaryAnim = "ExitSecondary";
        this.reloadIn = "ReloadIn";
        this.reloadOut = "ReloadOut";
        this.walkAnimation = "Walk";
        this.secondaryWalkAnim = "";
        this.secondarySprintAnim = "";
        this.walkSpeedModifier = 20;
        this.sprintAnimation = "Sprint";
        this.nullAnim = "Null";
        this.secondaryNullAnim = "";
        this.idleAnim = "Idle";
        this.secondaryIdleAnim = "";
        this.chargeAnim = "Charge";
        this.walkAnim = "";
        this.sprintAnim = "";
        this.nullAnimation = "";
        this.secondaryReloadAnim = "";
        this.secondaryReloadEmpty = "";
        this.secondaryFireAnim = "";
        this.secondaryEmptyFireAnim = "";
        this.animCount = 2;
        this.fireAnims = new string[15];
        this.reloadAnims = new string[15];
        this.index = -1;
        this.lastIndex = -1;
        this.moveWeight = 1;
        this.nullWeight = 1;
        this.useStrafe = true;
    }

}