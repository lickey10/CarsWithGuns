using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class target : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 Copyright© Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public float health;
    private float curHealth;
    private bool dead;
    public float dieTime;
    public float minimum;
    public virtual void Start()
    {
        this.curHealth = this.health;
    }

    public virtual IEnumerator ApplyDamagePlayer(float damage)
    {
        float tempFloat = 0.0f;
        if (this.dead)
        {
            yield break;
        }
        //	float.TryParse(Arr[0], tempFloat);
        this.curHealth = this.curHealth - damage;
        if (this.curHealth <= 0)
        {
            this.dead = true;
            ((HingeJoint) this.GetComponent(typeof(HingeJoint))).useSpring = false;
            ((HingeJoint) this.GetComponent(typeof(HingeJoint))).limits.min = -90;
            yield return new WaitForSeconds(this.dieTime);
            this.curHealth = this.health;
            ((HingeJoint) this.GetComponent(typeof(HingeJoint))).useSpring = true;
            ((HingeJoint) this.GetComponent(typeof(HingeJoint))).limits.min = this.minimum;
            this.dead = false;
        }
    }

}