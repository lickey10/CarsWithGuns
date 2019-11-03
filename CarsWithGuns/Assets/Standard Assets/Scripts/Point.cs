using UnityEngine;
using System.Collections;

[System.Serializable]
public class Point : MonoBehaviour
{
    public float point;
    private float GetHitEffect;
    private float targY;
    private Vector3 PointPosition;
    public GUISkin PointSkin;
    public GUISkin PointSkinShadow;

    public virtual void Start()
    {
        this.point = Mathf.Round(Random.Range(this.point / 2, this.point * 2));
        this.PointPosition = this.transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        this.targY = Screen.height / 2;
        this.BroadcastMessage("AddToScore", this.point, SendMessageOptions.DontRequireReceiver);
    }

    public virtual void OnGUI()
    {
        Vector3 screenPos2 = Camera.main.GetComponent<Camera>().WorldToScreenPoint(this.PointPosition);
        this.GetHitEffect = this.GetHitEffect + (Time.deltaTime * 30);
        GUI.color = new Color(1f, 1f, 1f, 1f - ((this.GetHitEffect - 50) / 7));
        GUI.skin = this.PointSkinShadow;
        GUI.Label(new Rect(screenPos2.x + 8, this.targY - 2, 160, 140), "+" + this.point.ToString());
        GUI.skin = this.PointSkin;
        GUI.Label(new Rect(screenPos2.x + 10, this.targY, 240, 240), "+" + this.point.ToString());
    }

    public virtual void Update()
    {
        this.targY = this.targY - (Time.deltaTime * 200);
    }

}