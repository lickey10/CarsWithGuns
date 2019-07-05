using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class demo controls : MonoBehaviour
{
    public MeshFilter[] closedBarrels;
    public Mesh[] meshTypes;
    public GameObject slime;
    public Light greenLight;
    private int alternator;
    public bool greenStrobo;
    public virtual void Start()
    {
        this.alternator = 3;
    }

    public virtual void Update()
    {
        float randomLightForce = Mathf.PingPong(Time.time / this.alternator, 0.25f);
        if (randomLightForce > 0.24f)
        {
            this.alternator = Random.Range(1, 4);
        }
        if (this.greenStrobo == true)
        {
            this.greenLight.intensity = randomLightForce;
        }
        else
        {
            this.greenLight.intensity = 0.23f;
        }
    }

    public virtual void OnGUI()
    {
        GUILayout.BeginArea(new Rect(20, 20, 600, 100));
        GUILayout.BeginHorizontal("box");
        //GUILayout.Label("Scene info:\n4 pixel lights with shadows\n\n2 Shared Materials with Specular and Normal maps.\n");
        if (GUILayout.Button("SET:\nNORMAL"))
        {
            this.StartCoroutine(this.SetNormal());
        }
        if (GUILayout.Button("SET:\nOPENED"))
        {
            this.StartCoroutine(this.SetOpened());
        }
        if (GUILayout.Button("SET:\nDEFORMED"))
        {
            this.StartCoroutine(this.SetBending());
        }
        if (GUILayout.Button("SET:\nMASHED"))
        {
            this.StartCoroutine(this.SetFlat());
        }
        if (GUILayout.Button("TOGGLE:\nSLIME"))
        {
            if (this.slime.activeSelf == true)
            {
                this.slime.SetActive(false);
            }
            else
            {
                this.slime.SetActive(true);
            }
        }
        if (GUILayout.Button("TOGGLE:\nSTROBE"))
        {
            if (this.greenStrobo == true)
            {
                this.greenStrobo = false;
            }
            else
            {
                this.greenStrobo = true;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    public virtual IEnumerator SetNormal()
    {
        foreach (MeshFilter closedBarrel in this.closedBarrels)
        {
            closedBarrel.GetComponent<AudioSource>().Stop();
            closedBarrel.GetComponent<AudioSource>().pitch = Random.Range(0.4f, 0.7f);
            closedBarrel.GetComponent<AudioSource>().Play();
            closedBarrel.mesh = this.meshTypes[0];
            //closedBarrel.transform.localEulerAngles.y = Random.Range(0,360);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public virtual IEnumerator SetOpened()
    {
        foreach (MeshFilter closedBarrel in this.closedBarrels)
        {
            closedBarrel.GetComponent<AudioSource>().Stop();
            closedBarrel.GetComponent<AudioSource>().pitch = Random.Range(0.4f, 0.7f);
            closedBarrel.GetComponent<AudioSource>().Play();
            closedBarrel.mesh = this.meshTypes[5];
            //closedBarrel.transform.localEulerAngles.y = Random.Range(0,360);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public virtual IEnumerator SetBending()
    {
        foreach (MeshFilter closedBarrel in this.closedBarrels)
        {
            closedBarrel.GetComponent<AudioSource>().Stop();
            closedBarrel.GetComponent<AudioSource>().pitch = Random.Range(0.4f, 0.7f);
            //closedBarrel.audio.volume = Random.Range(0.5, 1.0);
            closedBarrel.GetComponent<AudioSource>().Play();
            closedBarrel.mesh = this.meshTypes[Random.Range(1, 4)];
            closedBarrel.transform.localEulerAngles.y = Random.Range(0, 360);
            yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
        }
    }

    public virtual IEnumerator SetFlat()
    {
        foreach (MeshFilter closedBarrel in this.closedBarrels)
        {
            closedBarrel.GetComponent<AudioSource>().Stop();
            closedBarrel.GetComponent<AudioSource>().pitch = Random.Range(0.4f, 0.7f);
            //closedBarrel.audio.volume = Random.Range(0.5, 1.0);
            closedBarrel.GetComponent<AudioSource>().Play();
            closedBarrel.mesh = this.meshTypes[4];
            closedBarrel.transform.localEulerAngles.y = Random.Range(0, 360);
            yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
        }
    }

    public demo controls()
    {
        this.greenStrobo = true;
    }

}