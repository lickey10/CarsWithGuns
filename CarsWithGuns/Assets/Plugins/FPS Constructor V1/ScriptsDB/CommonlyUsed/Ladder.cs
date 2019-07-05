using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Ladder : MonoBehaviour
{
    public float sensitivity;
    public float moveSpeed;
    public Transform exitPos;
    public Transform enterPos;
    public float minHeight;
    public float soundInterval;
    private bool ladder;
    private bool temp;
    private float soundAmt;
    private Transform player;
    private bool belowHeight;
    private float val;
    public virtual IEnumerator MoveTo(Transform what, Vector3 where, float time)
    {
        float i = 0;
        Vector3 pos = what.position;
        while (i <= 1)
        {
            i = i + (Time.deltaTime / time);
            what.position = Vector3.Lerp(pos, where, Mathf.SmoothStep(0, 1, i));
            yield return typeof(WaitForFixedUpdate);
        }
    }

    public virtual IEnumerator RotateTo(Transform what, Vector3 where, float time, bool local)
    {
        Vector3 pos = default(Vector3);
        float i = 0;
        if (local)
        {
            pos = what.localEulerAngles;
        }
        else
        {
            pos = what.eulerAngles;
        }
        while (i <= 1)
        {
            i = i + (Time.deltaTime / time);
            if (local)
            {
                what.localEulerAngles.x = Mathf.LerpAngle(pos.x, where.x, Mathf.SmoothStep(0, 1, i));
                what.localEulerAngles.y = Mathf.LerpAngle(pos.y, where.y, Mathf.SmoothStep(0, 1, i));
                what.localEulerAngles.z = Mathf.LerpAngle(pos.z, where.z, Mathf.SmoothStep(0, 1, i));
            }
            else
            {
                what.eulerAngles.x = Mathf.LerpAngle(pos.x, where.x, Mathf.SmoothStep(0, 1, i));
                what.eulerAngles.y = Mathf.LerpAngle(pos.y, where.y, Mathf.SmoothStep(0, 1, i));
                what.eulerAngles.z = Mathf.LerpAngle(pos.z, where.z, Mathf.SmoothStep(0, 1, i));
            }
            yield return typeof(WaitForFixedUpdate);
        }
        ((MouseLookDBJS) what.GetComponent(typeof(MouseLookDBJS))).UpdateIt();
    }

    public virtual IEnumerator MoveToStart(Transform what, Vector3 where, float time)
    {
        float i = 0;
        Vector3 pos = what.position;
        while (i <= 1)
        {
            i = i + (Time.deltaTime / time);
            Vector3 targetVect = Vector3.Lerp(pos, where, Mathf.SmoothStep(0, 1, i));
            what.position.x = targetVect.x;
            what.position.z = targetVect.z;
            yield return typeof(WaitForFixedUpdate);
        }
    }

    public virtual void Update()
    {
        if (this.player == null)
        {
            return;
        }
        if (this.ladder)
        {
            if (Input.GetButtonDown("Jump"))
            {
                this.ReleasePlayer();
            }
            float lastVal = this.val;
            this.val = this.val + ((InputDB.GetAxis("Vertical") * this.sensitivity) * Time.deltaTime);
            this.val = Mathf.Clamp01(this.val);
            this.soundAmt = this.soundAmt - Mathf.Abs(lastVal - this.val);
            if (this.soundAmt <= 0)
            {
                this.soundAmt = this.soundInterval;
                this.GetComponent<AudioSource>().Play();
            }
            this.player.position.y = Mathf.Lerp(this.transform.position.y, this.exitPos.position.y, this.val);
            if (((this.val == 1) && !GunScript.takingOut) && !GunScript.puttingAway)
            {
                this.StartCoroutine(this.ExitLadder());
            }
            if ((((this.val < this.minHeight) && ((lastVal > this.val) || (this.val == 0))) && !GunScript.takingOut) && !GunScript.puttingAway)
            {
                this.ReleasePlayer();
            }
        }
    }

    //Locks player to ladder
    public virtual IEnumerator LockPlayer()
    {
        if ((GunScript.takingOut || GunScript.puttingAway) || this.ladder)
        {
            yield break;
        }
        if (PlayerWeapons.PW.weapons[PlayerWeapons.PW.selectedWeapon])
        {
            this.temp = ((GunScript) PlayerWeapons.PW.weapons[PlayerWeapons.PW.selectedWeapon].GetComponent(typeof(GunScript))).gunActive;
        }
        else
        {
            this.temp = true;
        }
        this.player.SendMessage("StandUp");
        this.belowHeight = true;
        CharacterMotorDB.paused = true;
        SmartCrosshair.draw = false;
        PlayerWeapons.playerActive = false;
        DBStoreController.canActivate = false;
        PlayerWeapons.HideWeapon();
        this.val = (this.player.position.y - this.transform.position.y) / (this.exitPos.position.y - this.transform.position.y);
        this.StartCoroutine(this.RotateTo(this.player, this.enterPos.eulerAngles, this.moveSpeed, false));
        this.StartCoroutine(this.RotateTo(PlayerWeapons.weaponCam.transform, new Vector3(0, 0, 0), this.moveSpeed, true));
        ((MouseLookDBJS) this.player.GetComponent(typeof(MouseLookDBJS))).individualFreeze = true;
        ((MouseLookDBJS) PlayerWeapons.weaponCam.GetComponent(typeof(MouseLookDBJS))).individualFreeze = true;
        yield return this.StartCoroutine(this.MoveToStart(this.player, this.enterPos.position, this.moveSpeed));
        ((MouseLookDBJS) this.player.GetComponent(typeof(MouseLookDBJS))).LockIt(60, 60);
        ((MouseLookDBJS) PlayerWeapons.weaponCam.GetComponent(typeof(MouseLookDBJS))).LockItSpecific(-40, 80);
        ((MouseLookDBJS) this.player.GetComponent(typeof(MouseLookDBJS))).individualFreeze = false;
        ((MouseLookDBJS) PlayerWeapons.weaponCam.GetComponent(typeof(MouseLookDBJS))).individualFreeze = false;
        PlayerWeapons.canLook = true;
        this.ladder = true;
        this.soundAmt = this.soundInterval;
    }

    //Removes player from ladder to exit position
    public virtual IEnumerator ExitLadder()
    {
        this.ladder = false;
        yield return this.StartCoroutine(this.MoveTo(this.player, this.exitPos.position, this.moveSpeed));
        this.ReleasePlayer();
    }

    //Reactivates player to normal function
    public virtual void ReleasePlayer()
    {
        ((MouseLookDBJS) this.player.GetComponent(typeof(MouseLookDBJS))).UnlockIt();
        ((MouseLookDBJS) PlayerWeapons.weaponCam.GetComponent(typeof(MouseLookDBJS))).UnlockIt();
        this.ladder = false;
        SmartCrosshair.draw = true;
        DBStoreController.canActivate = true;
        PlayerWeapons.playerActive = true;
        CharacterMotorDB.paused = false;
        if (this.temp)
        {
            PlayerWeapons.ShowWeapon();
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (((other.tag == "Player") && !GunScript.takingOut) && !GunScript.puttingAway)
        {
            this.player = other.transform;
            this.StartCoroutine(this.LockPlayer());
        }
    }

    public Ladder()
    {
        this.sensitivity = 0.05f;
        this.moveSpeed = 1;
        this.minHeight = 0.1f;
    }

}