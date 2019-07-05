using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SmartCrosshair : MonoBehaviour
{
    /*
 FPS Constructor - Weapons
 CopyrightÃ¯Â¿Â½ Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
  For additional information contact us info@dastardlybanana.com.
*/
    public float length1;
    public float width1;
    public bool scale;
    private Texture textu;
    private GUIStyle lineStyle;
    public bool debug;
    public static bool displayWhenAiming;
    public bool useTexture;
    public static bool ownTexture;
    public GameObject crosshairObj;
    public static GameObject cObj;
    public static bool scl;
    public static float cSize;
    public static float sclRef;
    public static bool draw;
    public float crosshairSize;
    public float minimumSize;
    public float maximumSize;
    public Texture2D crosshairTexture;
    public Texture2D friendTexture;
    public Texture2D foeTexture;
    public Texture2D otherTexture;
    public bool colorFoldout;
    public float colorDist;
    private bool hitEffectOn;
    public Texture2D hitEffectTexture;
    private float hitEffectTime;
    public float hitLength;
    public float hitWidth;
    public Vector2 hitEffectOffset;
    public AudioClip hitSound;
    public bool hitEffectFoldout;
    public bool displayHealth;
    public EnemyDamageReceiver enemyDamageReceiver;
    public int crosshairRange;
    private Shader currentShader;// = ((Renderer)GetComponent.<Renderer>()).sh;
    public float outlineSize;//0.01f;
    public Color outlineColor;
    public Color meshColor;
    //public var outlineColor:Color = Color(1f,1f,0f,1f);
    public static bool crosshair;
    public LayerMask RaycastsIgnore; //Layers that gun raycasts hit
    public virtual void Awake()
    {
        this.DefaultCrosshair();
        SmartCrosshair.sclRef = 1;
        SmartCrosshair.crosshair = true;
        this.lineStyle = new GUIStyle();
        this.lineStyle.normal.background = this.crosshairTexture;
    }

    //Right now this script fires a raycast every frame
    //This might impact performance, and is an area to consider when optimizing
    public virtual void Update()
    {
        float temp = 0.0f;
        float temp2 = 0.0f;
        RaycastHit hit = default(RaycastHit);
        if (!PlayerWeapons.playerActive)
        {
            if (SmartCrosshair.cObj)
            {
                SmartCrosshair.cObj.GetComponent<Renderer>().enabled = false;
            }
            return;
        }
        else
        {
            if (SmartCrosshair.cObj)
            {
                SmartCrosshair.cObj.GetComponent<Renderer>().enabled = true;
            }
        }
        if (SmartCrosshair.cObj != null)
        {
            if (SmartCrosshair.crosshair && SmartCrosshair.ownTexture)
            {
                SmartCrosshair.cObj.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                SmartCrosshair.cObj.GetComponent<Renderer>().enabled = false;
            }
        }
        if (!SmartCrosshair.scl)
        {
            temp = 1;
            temp2 = 1 / Screen.width;
        }
        else
        {
            temp = GunScript.crosshairSpread;
            temp = temp / 180;
            temp = temp * GunScript.weaponCam.GetComponent<Camera>().fieldOfView;
            temp = temp / Screen.height;
            temp = temp / SmartCrosshair.sclRef;
            temp2 = SmartCrosshair.cSize * temp;
        }
        if (SmartCrosshair.cObj != null)
        {
            if (SmartCrosshair.scl)
            {
                SmartCrosshair.cObj.transform.localScale = new Vector3(Mathf.Clamp(temp2, this.minimumSize, this.maximumSize), 1, Mathf.Clamp(temp2, this.minimumSize, this.maximumSize));
            }
            else
            {
                SmartCrosshair.cObj.transform.localScale = new Vector3(SmartCrosshair.cSize, 1, SmartCrosshair.cSize);
            }
        }
        int layerMask = 1 << PlayerWeapons.playerLayer;
        //layerMask |= (1 << 2);
        layerMask = layerMask | Physics.IgnoreRaycastLayer;
        layerMask = ~layerMask;
        Vector3 direction = this.transform.TransformDirection(new Vector3(0, 0, 1));
        if (Physics.Raycast(this.transform.position, direction, out hit, this.crosshairRange, layerMask))
        {
            if ((hit.collider && (((CrosshairColor) hit.transform.gameObject.GetComponent(typeof(CrosshairColor))) != null)) && ((hit.distance <= this.colorDist) || (this.colorDist < 0)))
            {
                CrosshairColor colorScript = (CrosshairColor) hit.transform.gameObject.GetComponent(typeof(CrosshairColor));
                if (colorScript.crosshairType == crosshairTypes.Friend)
                {
                    this.ChangeColor("Friend");
                    //display the current health of "Friend"
                    this.enemyDamageReceiver = (EnemyDamageReceiver) hit.transform.gameObject.GetComponent(typeof(EnemyDamageReceiver));
                    this.displayHealth = true;
                }
                else
                {
                    //HighlightGameObject(hit.transform.gameObject);
                    if (colorScript.crosshairType == crosshairTypes.Foe)
                    {
                        this.ChangeColor("Foe");
                        this.displayHealth = false;
                    }
                    else
                    {
                        if (colorScript.crosshairType == crosshairTypes.Other)
                        {
                            this.ChangeColor("Other");
                            this.displayHealth = false;
                        }
                    }
                }
            }
            else
            {
                this.ChangeColor(""); //Any string not recognized by ChangeColor is the default color
                this.displayHealth = false;
            }
        }
        else
        {
            this.ChangeColor("");
            this.displayHealth = false;
        }
        if (this.hitEffectTime <= 0)
        {
            this.hitEffectOn = false;
        }
    }

    public virtual void OnGUI()
    {
        if (!PlayerWeapons.playerActive)
        {
            return;
        }
        GUI.color = Color.white;
        if (!SmartCrosshair.ownTexture)
        {
            float distance1 = GunScript.crosshairSpread;
            if (!(distance1 > (Screen.height / 2)) && ((SmartCrosshair.crosshair || this.debug) || SmartCrosshair.displayWhenAiming))
            {
                GUI.Box(new Rect(((Screen.width - distance1) / 2) - this.length1, (Screen.height - this.width1) / 2, this.length1, this.width1), this.textu, this.lineStyle);
                GUI.Box(new Rect((Screen.width + distance1) / 2, (Screen.height - this.width1) / 2, this.length1, this.width1), this.textu, this.lineStyle);
                GUI.Box(new Rect((Screen.width - this.width1) / 2, ((Screen.height - distance1) / 2) - this.length1, this.width1, this.length1), this.textu, this.lineStyle);
                GUI.Box(new Rect((Screen.width - this.width1) / 2, (Screen.height + distance1) / 2, this.width1, this.length1), this.textu, this.lineStyle);
                if (this.displayHealth && (this.enemyDamageReceiver != null))
                {
                    GUI.Box(new Rect((Screen.width - 100) / 2, ((Screen.height + this.width1) / 2) - 75, 100, 50), (this.enemyDamageReceiver.transform.name + " Health: ") + Mathf.Round(this.enemyDamageReceiver.hitPoints));
                }
            }
        }
        if (this.hitEffectOn)
        {
            this.hitEffectTime = this.hitEffectTime - (Time.deltaTime * 0.5f);
            GUI.color = new Color(1, 1, 1, this.hitEffectTime);
            GUI.DrawTexture(new Rect(((Screen.width - this.hitEffectOffset.x) / 2) - (this.hitLength / 2), ((Screen.height - this.hitEffectOffset.y) / 2) - (this.hitWidth / 2), this.hitLength, this.hitWidth), this.hitEffectTexture);
        }
    }

    public virtual void HighlightGameObject(GameObject objectToHighlight)//Invoke("HighlightGameObjectRemove",3);
    {
        // Set the transparent material to this object
        MeshRenderer meshRenderer = ((MeshRenderer) GameObject.FindObjectOfType(typeof(MeshRenderer))) as MeshRenderer;
        Material[] materials = meshRenderer.materials;
        int materialsNum = materials.Length;
        int i = 0;
        while (i < materialsNum)
        {
            materials[i].shader = Shader.Find("Outline/Transparent");
            materials[i].SetColor("_color", this.meshColor);
            i++;
        }
        // Create copy of this object, this will have the shader that makes the real outline
        GameObject outlineObj = new GameObject();
        outlineObj.transform.position = objectToHighlight.transform.position;
        outlineObj.transform.rotation = objectToHighlight.transform.rotation;
        outlineObj.AddComponent(typeof(MeshFilter));
        outlineObj.AddComponent(typeof(MeshRenderer));
        Mesh mesh = null;
        mesh = UnityEngine.Object.Instantiate((((MeshFilter) GameObject.FindObjectOfType(typeof(MeshFilter))) as MeshFilter).mesh);
        ((MeshFilter) outlineObj.GetComponent(typeof(MeshFilter))).mesh = mesh;
        outlineObj.transform.parent = objectToHighlight.transform;
        //materials = Material[materialsNum];//create empty materials array
        i = 0;
        while (i < materialsNum)
        {
            materials[i] = new Material(Shader.Find("Self-Illumin/Outlined Diffuse"));
            materials[i].SetColor("_OutlineColor", this.outlineColor);
            i++;
        }
        ((MeshRenderer) outlineObj.GetComponent(typeof(MeshRenderer))).materials = materials;
    }

    public virtual void HighlightGameObjectRemove()
    {
        this.GetComponent<Renderer>().material.shader = this.currentShader;
    }

    public virtual void ChangeColor(string targetStatus)
    {
        if (targetStatus == "Friend")
        {
            this.lineStyle.normal.background = this.friendTexture;
        }
        else
        {
            if (targetStatus == "Foe")
            {
                this.lineStyle.normal.background = this.foeTexture;
            }
            else
            {
                if (targetStatus == "Other")
                {
                    this.lineStyle.normal.background = this.otherTexture;
                }
                else
                {
                    this.lineStyle.normal.background = this.crosshairTexture;
                }
            }
        }
    }

    public virtual void Aiming()
    {
        SmartCrosshair.crosshair = false;
    }

    public virtual void NormalSpeed()
    {
        SmartCrosshair.crosshair = true;
    }

    public virtual void Sprinting()
    {
        SmartCrosshair.crosshair = false;
    }

    public virtual void SetCrosshair()
    {
        if (SmartCrosshair.cObj != null)
        {
            SmartCrosshair.cObj.GetComponent<Renderer>().enabled = false;
        }
    }

    public virtual void DefaultCrosshair()
    {
        if (SmartCrosshair.cObj != null)
        {
            SmartCrosshair.cObj.GetComponent<Renderer>().enabled = false;
        }
        SmartCrosshair.ownTexture = this.useTexture;
        if (this.crosshairObj != null)
        {
            SmartCrosshair.cObj = this.crosshairObj;
        }
        if (this.scale)
        {
            SmartCrosshair.cSize = this.maximumSize;
        }
        else
        {
            SmartCrosshair.cSize = this.crosshairSize;
        }
        SmartCrosshair.scl = this.scale;
    }

    public virtual void HitEffect()
    {
        this.hitEffectOn = true;
        this.hitEffectTime = 1;
        if (this.GetComponent<AudioSource>() && !this.GetComponent<AudioSource>().isPlaying)
        {
            this.GetComponent<AudioSource>().clip = this.hitSound;
            this.GetComponent<AudioSource>().Play();
        }
    }

    public SmartCrosshair()
    {
        this.colorDist = 40;
        this.hitEffectOffset = new Vector2(0, 0);
        this.crosshairRange = 200;
        this.outlineSize = 2;
        this.outlineColor = Color.red;
        this.meshColor = new Color(1f, 1f, 1f, 0.5f);
    }

    static SmartCrosshair()
    {
        SmartCrosshair.displayWhenAiming = true;
        SmartCrosshair.crosshair = true;
    }

}