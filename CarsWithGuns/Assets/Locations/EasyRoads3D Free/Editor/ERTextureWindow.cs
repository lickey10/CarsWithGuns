using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class ERTextureWindow : EditorWindow
{
    public static ERTextureWindow instance;
    private Texture2D roadTexture;
    private Vector2 scrollPosition;
    private int selectedItem;
    private string[] shaders1;
    private string[] shaders2;
    private string[] shaders3;
    private Texture2D[] roadTextures;
    private string[] texturePaths;
    private int[] shaderOptions;
    private int[] selInt;
    private System.Collections.Generic.List<string> dtextures;
    private System.Collections.Generic.List<string> btextures;
    private System.Collections.Generic.List<string> stextures;
    private string[] ext;
    public static ERTextureWindow window;
    private static RoadObjectScript roadscript;
    public static int test;
    public static void Init(RoadObjectScript scr)
    {
        ERTextureWindow.roadscript = scr;
        ERTextureWindow.window = EditorWindow.GetWindow(typeof(ERTextureWindow));
    }

    public ERTextureWindow()
    {
        this.dtextures = new System.Collections.Generic.List<string>();
        this.btextures = new System.Collections.Generic.List<string>();
        this.stextures = new System.Collections.Generic.List<string>();
        this.ext = new string[] {".PSD", ".TIFF", ".JPG", ".TGA", ".PNG", ".GIF", ".BMP", ".IFF", ".PICT"};
        ERTextureWindow.instance = this;
        this.title = "Road Materials";
        this.GetFiles();
    }

    public virtual void OnDestroy()
    {
        ERTextureWindow.instance = null;
    }

    public virtual void OnGUI()
    {
        if (this.shaders1 == null)
        {
            this.shaders1 = new string[1];
            this.shaders1[0] = "Diffuse";
            this.shaders2 = new string[2];
            this.shaders2[0] = "Diffuse";
            this.shaders2[1] = "Bumpmap Diffuse";
            this.shaders3 = new string[3];
            this.shaders3[0] = "Diffuse";
            this.shaders3[1] = "Bumpmap Diffuse";
            this.shaders3[2] = "Bumped Specular";
        }
        Rect r = this.position;
        int cols = (int) Mathf.Floor(r.width / 150f);
        this.scrollPosition = EditorGUILayout.BeginScrollView(this.scrollPosition);
        GUILayout.Space(10);
        int k = 0;
        int l = 0;
        i = 0;
        while (i < 5)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            j = 0;
            while (j < cols)
            {
                if (GUILayout.Button(this.roadTextures[k], GUILayout.Width(125), GUILayout.Height(125)))
                {
                    this.SetMaterial(this.roadTextures[k], this.selInt[k], this.texturePaths[k], k);
                }
                GUILayout.Space(25);
                k++;
                if (k >= this.roadTextures.Length)
                {
                    break;
                }
                j++;
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(1);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            j = 0;
            while (j < cols)
            {
                int oldItem = this.selInt[l];
                if (this.shaderOptions[l] == 1)
                {
                    this.selInt[l] = EditorGUILayout.Popup(this.selInt[l], this.shaders1, EditorStyles.toolbarPopup, GUILayout.Width(125));
                }
                else
                {
                    if (this.shaderOptions[l] == 2)
                    {
                        this.selInt[l] = EditorGUILayout.Popup(this.selInt[l], this.shaders2, EditorStyles.toolbarPopup, GUILayout.Width(125));
                    }
                    else
                    {
                        if (this.shaderOptions[l] == 3)
                        {
                            this.selInt[l] = EditorGUILayout.Popup(this.selInt[l], this.shaders3, EditorStyles.toolbarPopup, GUILayout.Width(125));
                        }
                    }
                }
                if ((oldItem != this.selInt[l]) && (this.selectedItem == l))
                {
                    this.SetMaterial(this.roadTextures[l], this.selInt[l], this.texturePaths[l], l);
                }
                GUILayout.Space(29);
                l++;
                if (l >= this.roadTextures.Length)
                {
                    break;
                }
                j++;
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
            if (k >= this.roadTextures.Length)
            {
                break;
            }
            i++;
        }
        EditorGUILayout.EndScrollView();
    }

    public virtual void GetFiles()
    {
        System.Collections.Generic.List<string> textures = new System.Collections.Generic.List<string>();
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/EasyRoads3D/Textures/Road Textures/");
        string[] extStrings = new string[1];
        extStrings[0] = "*.*";
        foreach (string ext in extStrings)
        {
            foreach (FileInfo f in dir.GetFiles(ext))
            {
                string name = f.Name;
                if (this.InArray(f.Extension))
                {
                    textures.Add(name);
                }
            }
        }
        this.dtextures.Clear();
        this.btextures.Clear();
        this.stextures.Clear();
        foreach (string file in textures)
        {
            if (file.IndexOf("_d.") >= 0)
            {
                this.dtextures.Add(file);
            }
            else
            {
                if (file.IndexOf("_b.") >= 0)
                {
                    this.btextures.Add(file);
                }
                else
                {
                    if (file.IndexOf("_s.") >= 0)
                    {
                        this.stextures.Add(file);
                    }
                    else
                    {
                        this.dtextures.Add(file);
                    }
                }
            }
        }
        this.roadTextures = new Texture2D[this.dtextures.Count];
        this.texturePaths = new string[this.dtextures.Count];
        this.shaderOptions = new int[this.dtextures.Count];
        this.selInt = new int[this.dtextures.Count];
        int i = 0;
        foreach (string file in this.dtextures)
        {
            //		Debug.Log("/Assets/EasyRoads3D/Textures/Road Textures/" + file);
            this.roadTextures[i] = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/EasyRoads3D/Textures/Road Textures/" + file, typeof(Texture2D));
            string[] arr = file.Split("_"[0]);
            string extr = "";
            if (arr.Length > 2)
            {
                l = 0;
                while (l < (arr.Length - 1))
                {
                    extr = extr + (arr[l] + "_");
                    l++;
                }
                extr = extr.Substring(0, extr.length - 1);
            }
            else
            {
                extr = arr[0];
            }
            int option = 1;
            foreach (string fb in this.btextures)
            {
                if (fb.IndexOf(extr) >= 0)
                {
                    option++;
                    option++;
                    foreach (string fs in this.stextures)
                    {
                        if (fs.IndexOf(extr) >= 0)
                        {
                            break;
                        }
                    }
                    break;
                }
            }
            this.shaderOptions[i] = option;
            this.texturePaths[i] = file;
            i++;
        }
    }

    public virtual bool InArray(object file)
    {
        bool flag = false;
        i = 0;
        while (i < this.ext.Length)
        {
            if (this.ext[i] == file.ToUpper())
            {
                flag = true;
                break;
            }
            i++;
        }
        return flag;
    }

    public virtual void SetMaterial(object tex, object shader, object file, object item)
    {
        //		Debug.Log(shader +" "+file);
        this.selectedItem = item;
        object arr = file.Split("_"[0]);
        string extr = "";
        if (arr.length > 2)
        {
            l = 0;
            while (l < (arr.length - 1))
            {
                extr = extr + (arr[l] + "_");
                l++;
            }
            extr = extr.Substring(0, extr.length - 1);
        }
        else
        {
            extr = arr[0];
        }
        mat = (Material) Resources.Load("roadMaterial", typeof(Material));
        matEdit = (Material) Resources.Load("roadMaterialEdit", typeof(Material));
        if (shader == 0)
        {
            mat.shader = Shader.Find("EasyRoads3D/Diffuse");
        }
        else
        {
            if (shader == 1)
            {
                mat.shader = Shader.Find("EasyRoads3D/Bumped Diffuse");
                string btex = "";
                foreach (string fb in this.btextures)
                {
                    if (fb.IndexOf(extr) >= 0)
                    {
                        btex = fb;
                        break;
                    }
                }
                mat.SetTexture("_BumpMap", (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/EasyRoads3D/Textures/Road Textures/" + btex, typeof(Texture2D)));
            }
            else
            {
                if (shader == 2)
                {
                    mat.shader = Shader.Find("EasyRoads3D/Bumped Specular");
                    btex = "";
                    foreach (string fb in this.btextures)
                    {
                        if (fb.IndexOf(extr) >= 0)
                        {
                            btex = fb;
                            break;
                        }
                    }
                }
            }
        }
        ERTextureWindow.roadscript.roadTexture = tex;
        mat.mainTexture = tex;
        matEdit.mainTexture = tex;
        GameObject road = ERTextureWindow.roadscript.OQCODQCQOC.road;
        if (road != null)
        {
            if (road.transform.childCount > 0)
            {
                foreach (object child in road.transform)
                {
                    child.gameObject.renderer.material = mat;
                }
            }
            else
            {
                road.GetComponent<Renderer>().material = mat;
            }
        }
    }

}