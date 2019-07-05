using UnityEngine;
using System.Collections;

public class Point : MonoBehaviour {
   public float Points;
private float GetHitEffect;
private float targY;
private Vector3 PointPosition;

    GUISkin PointSkin;
    GUISkin PointSkinShadow;

	// Use this for initialization
	void Start () {
        Points = Mathf.Round(Random.Range(Points / 2, Points * 2));
        PointPosition = transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        targY = Screen.height / 2;
    }
	
	// Update is called once per frame
	void Update () {
        targY -= Time.deltaTime * 200;
    }

    void OnGUI()
    {
        Vector3 screenPos2 = Camera.main.GetComponent<Camera>().WorldToScreenPoint(PointPosition);
        GetHitEffect += Time.deltaTime * 30;
        GUI.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - (GetHitEffect - 50) / 7);
        GUI.skin = PointSkinShadow;
        GUI.Label(new Rect(screenPos2.x + 8, targY - 2, 80, 70), "+" + Points.ToString());
        GUI.skin = PointSkin;
        GUI.Label(new Rect(screenPos2.x + 10, targY, 120, 120), "+" + Points.ToString());
    }
}
