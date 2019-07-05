using UnityEngine;
using UnityEditor;
using System.Collections;

/*
 FPS Constructor - Weapons
 Copyright� Dastardly Banana Productions 2011-2012
 This script, and all others contained within the Dastardly Banana Weapons Package are licensed under the terms of the
 Unity Asset Store End User License Agreement at http://download.unity3d.com/assetstore/customer-eula.pdf 
 
 For additional information contact us info@dastardlybanana.com.
*/
[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class WizardEditor : Editor
{
    public object[] cArray;
    public object[][] NameArrays;
    public object[] Names;
    public bool[] boolArray;
    public int[] setArray;
    public int[] setArrayDummy;
    public string[] setNameArrayDummy;
    public int[] penetrationArray;
    public object script;
    public error sScript;
    public object prevScript; //Used to see if the user changed which script they were viewing
    public GUIStyle penStyle;
    public string effectTypeLabel;
    public string effectTypeName;
    public virtual void OnEnable()//	penStyle.normal.textColor = Color(.7, .7, .7, 1);
    {
        this.target.effectsManager = GameObject.FindWithTag("Manager").GetComponent(EffectsManager);
        if (this.target.effectsManager == null)
        {
            Debug.Log("Effects Manager Script must be attached to the Manager Object");
        }
        this.setArrayDummy = new int[1];
        this.setNameArrayDummy = new string[1];
        this.setNameArrayDummy[0] = "None";
        this.penStyle = new GUIStyle();
        this.penStyle.alignment = (TextAnchor) 1;
    }

    public virtual void SortAndScanColliders()
    {
        this.cArray.Clear();
        this.Names.Clear();
        string n = null;
        bool nameIsInArray = false;
        this.cArray = (Collider[]) UnityEngine.Object.FindObjectsOfType(typeof(Collider));
        int i = 0;
        while (i < this.cArray.Length)
        {
            nameIsInArray = false;
            n = this.cArray[i].name;
            int j = 0;
            while (j < this.Names.Length)
            {
                if (this.Names[j] == n)
                {
                    nameIsInArray = true;
                    j = this.Names.Length;
                }
                j++;
            }
            if (!nameIsInArray)
            {
                this.Names.Add(n);
            }
            i++;
        }
        this.Names.Sort();
        this.NameArrays = (object[][]) new object[this.Names.length].ToBuiltin(typeof(object[]));
        this.boolArray = new bool[this.Names.Length];
        this.setArray = new int[this.Names.Length];
        this.penetrationArray = new int[this.Names.Length];
        i = 0;
        while (i < this.cArray.Length)
        {
            n = this.cArray[i].name;
            j = 0;
            while (j < this.Names.Length)
            {
                if (this.Names[j] == n)
                {
                    if (this.NameArrays[j] == null)
                    {
                        this.NameArrays[j] = new object[0];
                    }
                    this.NameArrays[j].Add(this.cArray[i]);
                    if (!(this.cArray[i].GetComponent(this.script) == null))
                    {
                        this.boolArray[j] = true;
                    }
                }
                j++;
            }
            i++;
        }
    }

    public override void OnInspectorGUI()
    {
        //EditorGUIUtility.LookLikeInspector();
        this.target.selectedScript = EditorGUILayout.EnumPopup("   Effect: ", this.target.selectedScript);
        this.sScript = this.target.selectedScript;
        if (this.sScript == wizardScripts.UseEffects)
        {
            if (this.target.effectsManager == null)
            {
                EditorGUILayout.LabelField("   Effects Manager Script must be attached to Manager Object", "");
                if (GUILayout.Button("Add Script Now"))
                {
                    this.target.effectsManager = GameObject.FindWithTag("Manager").AddComponent(EffectsManager);
                }
                return;
            }
            this.effectTypeLabel = "Enable?";
            this.effectTypeName = "Effect Set";
            this.script = UseEffects;
        }
        else
        {
            if (this.sScript == wizardScripts.BulletPenetration)
            {
                this.script = BulletPenetration;
                this.effectTypeLabel = "Enable?";
                this.effectTypeName = "Resistance";
            }
        }
        if (!(this.prevScript == this.script))
        {
            this.SortAndScanColliders();
        }
        if (this.Names.Length > 0)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("   Collider", "");
            EditorGUILayout.LabelField("", this.effectTypeLabel);
            EditorGUILayout.LabelField("", this.effectTypeName);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
        }
        bool changed = false;
        EditorGUILayout.BeginVertical("textField");
        int k = 0;
        while (k < this.Names.Length)
        {
            if (!this.NameArrays[k][0])
            {
                Debug.Log("null, k = " + k);
                this.SortAndScanColliders();
                break;
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(((object) "     ") + this.Names[k], "");
            bool prevBool = this.boolArray[k];
            this.boolArray[k] = EditorGUILayout.Toggle("", this.boolArray[k]);
            if (this.boolArray[k] != prevBool)
            {
                changed = true;
            }
            //Use Effects options
            if (this.script == UseEffects)
            {
                if (this.boolArray[k])
                {
                    if (!this.NameArrays[k][0].GetComponent(UseEffects))
                    {
                        // Selected but script not found on object
                        changed = true;
                    }
                    else
                    {
                        //						ApplyChanges();
                        if (this.setArray[k] == 0)
                        {
                            if (this.NameArrays[k][0].GetComponent(UseEffects))
                            {
                                this.setArray[k] = (int) this.NameArrays[k][0].GetComponent(UseEffects).setIndex;
                            }
                            else
                            {
                                this.setArray[k] = 0;
                            }
                        }
                        int prev = this.setArray[k];
                        this.setArray[k] = EditorGUILayout.Popup("", this.setArray[k], this.target.effectsManager.setNameArray);
                        if (this.setArray[k] != prev)
                        {
                            changed = true;
                        }
                    }
                }
                else
                {
                    // Effect not selected
                    if (this.NameArrays[k][0].GetComponent(UseEffects))
                    {
                        changed = true;
                    }
                    EditorGUILayout.Popup("", this.setArrayDummy[0], this.setNameArrayDummy);
                }
            }
            else
            {
                if (this.script == BulletPenetration)
                {
                    if (this.penetrationArray[k] == 0)
                    {
                        if (this.NameArrays[k][0].GetComponent(BulletPenetration))
                        {
                            this.penetrationArray[k] = (int) this.NameArrays[k][0].GetComponent(BulletPenetration).penetrateValue;
                        }
                        else
                        {
                            this.penetrationArray[k] = 0;
                        }
                    }
                    prev = this.penetrationArray[k];
                    if (this.boolArray[k])
                    {
                        this.penetrationArray[k] = EditorGUILayout.IntField("", this.penetrationArray[k], this.penStyle);
                        if (this.penetrationArray[k] != prev)
                        {
                            changed = true;
                        }
                    }
                    else
                    {
                        int tmp = EditorGUILayout.IntField("", 0, this.penStyle);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(this.target);
            }
            k++;
        }
        if (changed)
        {
            this.ApplyChanges();
            changed = false;
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        //		if (GUILayout.Button(new GUIContent("Apply Changes", "Adds the selected script script to all Coliders selected, removes it from those that are unselected. Changes decal sets if applicable"), "miniButton")){
        //			ApplyChanges();
        //		}
        EditorGUILayout.EndVertical();
        this.prevScript = this.script;
    }

    public virtual void ApplyChanges()
    {
        i = 0;
        while (i < this.Names.Length)
        {
            if (this.boolArray[i])
            {
                j = 0;
                while (j < this.NameArrays[i].Length)
                {
                    if (this.NameArrays[i][j].GetComponent(this.script) == null)
                    {
                        this.NameArrays[i][j].gameObject.AddComponent(this.script);
                    }
                    //Script specific
                    if (this.script == UseEffects)
                    {
                        this.NameArrays[i][j].GetComponent(UseEffects).setIndex = (error) this.setArray[i];
                    }
                    else
                    {
                        if (this.script == BulletPenetration) //Don't just use 'else' in case additional scripts are added to the wizard
                        {
                            this.NameArrays[i][j].GetComponent(BulletPenetration).penetrateValue = (error) this.penetrationArray[i];
                        }
                    }
                    j++;
                }
            }
            else
            {
                j = 0;
                while (j < this.NameArrays[i].Length)
                {
                    if (!(this.NameArrays[i][j].GetComponent(this.script) == null))
                    {
                        UnityEngine.Object.DestroyImmediate(this.NameArrays[i][j].gameObject.GetComponent(this.script));
                    }
                    j++;
                }
            }
            i++;
        }
    }

    public WizardEditor()
    {
        this.cArray = new object[0];
        this.Names = new object[0];
        this.sScript = wizardScripts.UseEffects;
        this.effectTypeLabel = "";
        this.effectTypeName = "";
    }

}