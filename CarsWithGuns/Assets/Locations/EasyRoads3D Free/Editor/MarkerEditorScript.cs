using EasyRoads3D;
using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
[UnityEditor.CanEditMultipleObjects]
public class MarkerEditorScript : Editor
{
    public Vector3 oldPos;
    public Vector3 pos;
    public GUISkin OODCOCOOCC;
    public GUISkin OQQQCCOOOQ;
    public int showGui;
    public bool OCOCQOQDQQ;
    public int count;
    public virtual void OnEnable()
    {
        if (this.target.objectScript == null)
        {
            this.target.SetObjectScript();
        }
        else
        {
            this.target.GetMarkerCount();
        }
    }

    public override void OnInspectorGUI()
    {
        this.showGui = this.EasyRoadsGUIMenu(false, false, this.target.objectScript);
        if ((this.showGui != -1) && (this.target.objectScript.ODODDQOO == null))
        {
            Selection.activeGameObject = this.target.transform.parent.parent.gameObject;
        }
        else
        {
            if ((this.target.objectScript.ODQDODOODQs.length <= 1) && (this.target.objectScript.ODODDDOO == null))
            {
                this.ERMarkerGUI(this.target);
            }
            else
            {
                if ((this.target.objectScript.ODQDODOODQs.length == 2) && (this.target.objectScript.ODODDDOO == null))
                {
                    this.MSMarkerGUI(this.target);
                }
                else
                {
                    if (this.target.objectScript.ODODDDOO != null)
                    {
                        this.TRMarkerGUI(this.target);
                    }
                }
            }
        }
    }

    public virtual void OnSceneGUI()
    {
        if ((this.target.objectScript.OQCODQCQOC == null) || (this.target.objectScript.erInit == ""))
        {
            Selection.activeGameObject = this.target.transform.parent.parent.gameObject;
        }
        else
        {
            this.MarkerOnScene(this.target);
        }
    }

    public virtual int EasyRoadsGUIMenu(bool flag, bool senderIsMain, RoadObjectScript nRoadScript)
    {
        if ((((this.target.objectScript.OQDDCQCCDQ == null) || (this.target.objectScript.OODQCQCDQO == null)) || (this.target.objectScript.ODOOCQDQCD == null)) && (this.target.objectScript.OQCODQCQOC != null))
        {
            this.target.objectScript.OQDDCQCCDQ = new bool[5];
            this.target.objectScript.OODQCQCDQO = new bool[5];
            this.target.objectScript.ODOOCQDQCD = nRoadScript;
            this.target.objectScript.ODODQCCDOC = this.target.objectScript.OQCODQCQOC.OQQCCOQDQO();
            this.target.objectScript.ODODQOQO = this.target.objectScript.OQCODQCQOC.ODDDCOQDCC();
            this.target.objectScript.ODODQOQOInt = this.target.objectScript.OQCODQCQOC.OCDDCDCCQD();
        }
        else
        {
            if (this.target.objectScript.OQCODQCQOC == null)
            {
                return;
            }
        }
        if (this.target.objectScript.OODCOCOOCC == null)
        {
            this.target.objectScript.OODCOCOOCC = (GUISkin) Resources.Load("ER3DSkin", typeof(GUISkin));
            this.target.objectScript.OQQOCCQOOC = (Texture2D) Resources.Load("ER3DLogo", typeof(Texture2D));
        }
        if (!flag)
        {
            this.target.objectScript.OODODDQDOQ();
        }
        GUI.skin = this.target.objectScript.OODCOCOOCC;
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        this.target.objectScript.OQDDCQCCDQ[0] = GUILayout.Toggle(this.target.objectScript.OQDDCQCCDQ[0], new GUIContent("", " Add road markers. "), "AddMarkers", GUILayout.Width(40), GUILayout.Height(22));
        if ((this.target.objectScript.OQDDCQCCDQ[0] == true) && (this.target.objectScript.OODQCQCDQO[0] == false))
        {
            this.target.objectScript.OODODDQDOQ();
            this.target.objectScript.OQDDCQCCDQ[0] = true;
            this.target.objectScript.OODQCQCDQO[0] = true;
            Selection.activeGameObject = this.target.transform.parent.parent.gameObject;
        }
        this.target.objectScript.OQDDCQCCDQ[1] = GUILayout.Toggle(this.target.objectScript.OQDDCQCCDQ[1], new GUIContent("", " Insert road markers. "), "insertMarkers", GUILayout.Width(40), GUILayout.Height(22));
        if ((this.target.objectScript.OQDDCQCCDQ[1] == true) && (this.target.objectScript.OODQCQCDQO[1] == false))
        {
            this.target.objectScript.OODODDQDOQ();
            this.target.objectScript.OQDDCQCCDQ[1] = true;
            this.target.objectScript.OODQCQCDQO[1] = true;
            Selection.activeGameObject = this.target.transform.parent.parent.gameObject;
        }
        this.target.objectScript.OQDDCQCCDQ[2] = GUILayout.Toggle(this.target.objectScript.OQDDCQCCDQ[2], new GUIContent("", " Process the terrain and create road geometry. "), "terrain", GUILayout.Width(40), GUILayout.Height(22));
        if ((this.target.objectScript.OQDDCQCCDQ[2] == true) && (this.target.objectScript.OODQCQCDQO[2] == false))
        {
            if (this.target.objectScript.markers < 2)
            {
                EditorUtility.DisplayDialog("Alert", "A minimum of 2 road markers is required before the terrain can be leveled!", "Close");
                this.target.objectScript.OQDDCQCCDQ[2] = false;
            }
            else
            {
                this.target.objectScript.OQDDCQCCDQ[2] = false;
                Selection.activeGameObject = this.target.transform.parent.parent.gameObject;
            }
        }
        if ((this.target.objectScript.OQDDCQCCDQ[2] == false) && (this.target.objectScript.OODQCQCDQO[2] == true))
        {
            this.target.objectScript.OODQCQCDQO[2] = false;
            Selection.activeGameObject = this.target.transform.parent.parent.gameObject;
        }
        this.target.objectScript.OQDDCQCCDQ[3] = GUILayout.Toggle(this.target.objectScript.OQDDCQCCDQ[3], new GUIContent("", " General settings. "), "settings", GUILayout.Width(40), GUILayout.Height(22));
        if ((this.target.objectScript.OQDDCQCCDQ[3] == true) && (this.target.objectScript.OODQCQCDQO[3] == false))
        {
            this.target.objectScript.OODODDQDOQ();
            this.target.objectScript.OQDDCQCCDQ[3] = true;
            this.target.objectScript.OODQCQCDQO[3] = true;
            Selection.activeGameObject = this.target.transform.parent.parent.gameObject;
        }
        this.target.objectScript.OQDDCQCCDQ[4] = GUILayout.Toggle(this.target.objectScript.OQDDCQCCDQ[4], new GUIContent("", "Version and Purchase Info"), "info", GUILayout.Width(40), GUILayout.Height(22));
        if ((this.target.objectScript.OQDDCQCCDQ[4] == true) && (this.target.objectScript.OODQCQCDQO[4] == false))
        {
            this.target.objectScript.OODODDQDOQ();
            this.target.objectScript.OQDDCQCCDQ[4] = true;
            this.target.objectScript.OODQCQCDQO[4] = true;
            Selection.activeGameObject = this.target.transform.parent.parent.gameObject;
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        GUI.skin = null;
        this.target.objectScript.ODDDQCOOQD = -1;
        int i = 0;
        while (i < 5)
        {
            if (this.target.objectScript.OQDDCQCCDQ[i] != null)
            {
                this.target.objectScript.ODDDQCOOQD = i;
                this.target.objectScript.OODQCOQQQQ = i;
            }
            i++;
        }
        if (this.target.objectScript.ODDDQCOOQD == -1)
        {
            this.target.objectScript.OODODDQDOQ();
        }
        int markerMenuDisplay = 1;
        if ((this.target.objectScript.ODDDQCOOQD == 0) || (this.target.objectScript.ODDDQCOOQD == 1))
        {
            markerMenuDisplay = 0;
        }
        else
        {
            if (((this.target.objectScript.ODDDQCOOQD == 2) || (this.target.objectScript.ODDDQCOOQD == 3)) || (this.target.objectScript.ODDDQCOOQD == 4))
            {
                markerMenuDisplay = 0;
            }
        }
        if (((this.target.objectScript.OOCQCCQOQQ != null) && (this.target.objectScript.OQDDCQCCDQ[2] == null)) && (this.target.objectScript.ODODDQOO == null))
        {
            this.target.objectScript.OOCQCCQOQQ = false;
            if (!(this.target.objectScript.objectType == 2))
            {
                this.target.objectScript.OQCODQODOQ();
            }
            if ((this.target.objectScript.objectType == 2) && (this.target.objectScript.OOCQCCQOQQ != null))
            {
                Debug.Log("restore");
                this.target.objectScript.OQCODQCQOC.OCCODDODOO(this.target.transform, true);
            }
        }
        GUI.skin.box.alignment = TextAnchor.UpperLeft;
        if ((this.target.objectScript.ODDDQCOOQD >= 0) && !(this.target.objectScript.ODDDQCOOQD == 4))
        {
            if ((this.target.objectScript.ODODQCCDOC == null) || (this.target.objectScript.ODODQCCDOC.Length == 0))
            {
                this.target.objectScript.ODODQCCDOC = this.target.objectScript.OQCODQCQOC.OQQCCOQDQO();
                this.target.objectScript.ODODQOQO = this.target.objectScript.OQCODQCQOC.ODDDCOQDCC();
                this.target.objectScript.ODODQOQOInt = this.target.objectScript.OQCODQCQOC.OCDDCDCCQD();
            }
            EditorGUILayout.BeginHorizontal();
            GUILayout.Box(this.target.objectScript.ODODQCCDOC[this.target.objectScript.ODDDQCOOQD], GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(50));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }
        return this.target.objectScript.ODDDQCOOQD;
    }

    public virtual void ERMarkerGUI(MarkerScript markerScript)
    {
        EditorGUILayout.Space();
        GUILayout.Box(" Marker: " + (this.target.markerNum + 1).ToString(), GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(20));
        if ((this.target.distance == "-1") && !(this.target.objectScript.OQCODQCQOC == null))
        {
            object arr = this.target.objectScript.OQCODQCQOC.ODQQQDDDCQ(this.target.markerNum);
            this.target.distance = arr[0];
            this.target.ODODQDQDDC = arr[1];
            this.target.OCOCCDDCOD = arr[2];
        }
        GUILayout.Label((" Total Distance to Marker: " + this.target.distance.ToString()) + " km");
        GUILayout.Label((" Segment Distance to Marker: " + this.target.ODODQDQDDC.ToString()) + " km");
        GUILayout.Label((" Marker Distance: " + this.target.OCOCCDDCOD.ToString()) + " m");
        EditorGUILayout.Space();
        GUILayout.Box(" Marker Settings", GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(20));
        bool oldss = markerScript.OQCQOQCQCC;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("    Soft Selection", "When selected, the settings of other road markers within the selected distance will change according their distance to this marker."), GUILayout.Width(105));
        GUI.SetNextControlName("OQCQOQCQCC");
        markerScript.OQCQOQCQCC = EditorGUILayout.Toggle(markerScript.OQCQOQCQCC);
        EditorGUILayout.EndHorizontal();
        if (markerScript.OQCQOQCQCC)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("         Distance", "The soft selection distance within other markers should change too."), GUILayout.Width(105));
            markerScript.OQDOCODDQO = EditorGUILayout.Slider(markerScript.OQDOCODDQO, 0, 500);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }
        if (oldss != markerScript.OQDOCODDQO)
        {
            this.target.objectScript.ResetMaterials(markerScript);
        }
        GUI.enabled = false;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("    Left Indent", "The distance from the left side of the road to the part of the terrain levelled at the same height as the road"), GUILayout.Width(105));
        GUI.SetNextControlName("ri");
        oldfl = markerScript.ri;
        markerScript.ri = EditorGUILayout.Slider(markerScript.ri, this.target.objectScript.indent, 100);
        EditorGUILayout.EndHorizontal();
        if (oldfl != markerScript.ri)
        {
            this.target.objectScript.ODDQODCQDQ("ri", markerScript);
            markerScript.OOQOQQOO = markerScript.ri;
        }
        GUI.enabled = true;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("    Right Indent", "The distance from the right side of the road to the part of the terrain levelled at the same height as the road"), GUILayout.Width(105));
        oldfl = markerScript.li;
        markerScript.li = EditorGUILayout.Slider(markerScript.li, this.target.objectScript.indent, 100);
        EditorGUILayout.EndHorizontal();
        if (oldfl != markerScript.li)
        {
            this.target.objectScript.ODDQODCQDQ("li", markerScript);
            markerScript.ODODQQOO = markerScript.li;
        }
        GUI.enabled = false;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("    Left Surrounding", "This represents the distance over which the terrain will be gradually leveled on the left side of the road to the original terrain height"), GUILayout.Width(105));
        oldfl = markerScript.rs;
        GUI.SetNextControlName("rs");
        markerScript.rs = EditorGUILayout.Slider(markerScript.rs, this.target.objectScript.indent, 100);
        EditorGUILayout.EndHorizontal();
        if (oldfl != markerScript.rs)
        {
            this.target.objectScript.ODDQODCQDQ("rs", markerScript);
            markerScript.ODOQQOOO = markerScript.rs;
        }
        GUI.enabled = true;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("    Right Surrounding", "This represents the distance over which the terrain will be gradually leveled on the right side of the road to the original terrain height"), GUILayout.Width(105));
        oldfl = markerScript.ls;
        markerScript.ls = EditorGUILayout.Slider(markerScript.ls, this.target.objectScript.indent, 100);
        EditorGUILayout.EndHorizontal();
        if (oldfl != markerScript.ls)
        {
            this.target.objectScript.ODDQODCQDQ("ls", markerScript);
            markerScript.DODOQQOO = markerScript.ls;
        }
        if (this.target.objectScript.objectType == 0)
        {
            GUI.enabled = false;
            EditorGUILayout.BeginHorizontal();
            oldfl = markerScript.rt;
            GUILayout.Label(new GUIContent("    Left Tilting", "Use this setting to tilt the road on the left side (m)."), GUILayout.Width(105));
            markerScript.qt = EditorGUILayout.Slider(markerScript.qt, 0, this.target.objectScript.roadWidth * 0.5f);
            EditorGUILayout.EndHorizontal();
            if (oldfl != markerScript.rt)
            {
                this.target.objectScript.ODDQODCQDQ("rt", markerScript);
                markerScript.ODDQODOO = markerScript.rt;
            }
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("    Right Tilting", "Use this setting to tilt the road on the right side (cm)."), GUILayout.Width(105));
            oldfl = markerScript.lt;
            markerScript.lt = EditorGUILayout.Slider(markerScript.lt, 0, this.target.objectScript.roadWidth * 0.5f);
            EditorGUILayout.EndHorizontal();
            if (oldfl != markerScript.lt)
            {
                this.target.objectScript.ODDQODCQDQ("lt", markerScript);
                markerScript.ODDOQOQQ = markerScript.lt;
            }
            GUI.enabled = true;
            if (this.target.markerInt < 2)
            {
                GUILayout.Label(new GUIContent("    Bridge Objects are disabled for the first two markers!", ""));
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("    Bridge Object", "When selected this road segment will be treated as a bridge segment."), GUILayout.Width(105));
                GUI.SetNextControlName("bridgeObject");
                markerScript.bridgeObject = EditorGUILayout.Toggle(markerScript.bridgeObject);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if (markerScript.bridgeObject)
                {
                    GUILayout.Label(new GUIContent("      Distribute Heights", "When selected the terrain, the terrain will be gradually leveled between the height of this road segment and the current terrain height of the inner bridge segment."), GUILayout.Width(135));
                    GUI.SetNextControlName("distHeights");
                    markerScript.distHeights = EditorGUILayout.Toggle(markerScript.distHeights);
                }
                EditorGUILayout.EndHorizontal();
            }
            GUI.enabled = true;
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("    Floor Depth", "Use this setting to change the floor depth for this marker."), GUILayout.Width(105));
            oldfl = markerScript.floorDepth;
            markerScript.floorDepth = EditorGUILayout.Slider(markerScript.floorDepth, 0, 50);
            EditorGUILayout.EndHorizontal();
            if (oldfl != markerScript.floorDepth)
            {
                this.target.objectScript.ODDQODCQDQ("floorDepth", markerScript);
                markerScript.oldFloorDepth = markerScript.floorDepth;
            }
        }
        EditorGUILayout.Space();
        GUI.enabled = false;
        if (this.target.objectScript.objectType == 0)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("    Start New LOD Segment", "Use this to split the road or river object in segments to use in LOD system."), GUILayout.Width(170));
            markerScript.newSegment = EditorGUILayout.Toggle(markerScript.newSegment);
            EditorGUILayout.EndHorizontal();
        }
        GUI.enabled = true;
        EditorGUILayout.Space();
        if (!markerScript.autoUpdate)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Refresh Surface", GUILayout.Width(225)))
            {
                this.target.objectScript.OQQOCCDODC();
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        if (GUI.changed && (this.target.objectScript.OQDDDDCCQC == null))
        {
            this.target.objectScript.OQDDDDCCQC = true;
        }
        else
        {
            if (this.target.objectScript.OQDDDDCCQC != null)
            {
                this.target.objectScript.ODCCCDCCDQ(markerScript);
                this.target.objectScript.OQDDDDCCQC = false;
                SceneView.lastActiveSceneView.Repaint();
            }
        }
        oldfl = markerScript.rs;
    }

    public virtual void MSMarkerGUI(MarkerScript markerScript)
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent(" Align XYZ", "Click to distribute the x, y and z values of all markers in between the selected markers in a line between the selected markers."), GUILayout.Width(150)))
        {
            Undo.RegisterUndo(this.target.transform.parent.GetComponentsInChildren(typeof(Transform)), "Marker align");
            this.target.objectScript.OQCODQCQOC.OOCQCDCCDD(this.target.objectScript.ODQDODOODQs, 0);
            this.target.objectScript.OQQOCCDODC();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent(" Align XZ", "Click to distribute the x and z values of all markers in between the selected markers in a line between the selected markers."), GUILayout.Width(150)))
        {
            Undo.RegisterUndo(this.target.transform.parent.GetComponentsInChildren(typeof(Transform)), "Marker align");
            this.target.objectScript.OQCODQCQOC.OOCQCDCCDD(this.target.objectScript.ODQDODOODQs, 1);
            this.target.objectScript.OQQOCCDODC();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent(" Align XZ  Snap Y", "Click to distribute the x and z values of all markers in between the selected markers in a line between the selected markers and snap the y value to the terrain height at the new position."), GUILayout.Width(150)))
        {
            Undo.RegisterUndo(this.target.transform.parent.GetComponentsInChildren(typeof(Transform)), "Marker align");
            this.target.objectScript.OQCODQCQOC.OOCQCDCCDD(this.target.objectScript.ODQDODOODQs, 2);
            this.target.objectScript.OQQOCCDODC();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent(" Average Heights ", "Click to distribute the heights all markers in between the selected markers."), GUILayout.Width(150)))
        {
            Undo.RegisterUndo(this.target.transform.parent.GetComponentsInChildren(typeof(Transform)), "Marker align");
            this.target.objectScript.OQCODQCQOC.OOCQCDCCDD(this.target.objectScript.ODQDODOODQs, 3);
            this.target.objectScript.OQQOCCDODC();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }

    public virtual void TRMarkerGUI(MarkerScript markerScript)
    {
        EditorGUILayout.Space();
    }

    public virtual void MarkerOnScene(MarkerScript markerScript)
    {
        Event cEvent = Event.current;
        if ((this.target.objectScript.ODODDDOO == null) || (this.target.objectScript.objectType == 2))
        {
            if (cEvent.shift && ((this.target.objectScript.OODQCOQQQQ == 0) || (this.target.objectScript.OODQCOQQQQ == 1)))
            {
                Selection.activeGameObject = markerScript.transform.parent.parent.gameObject;
            }
            else
            {
                if (cEvent.shift && !(this.target.objectScript.ODQDODOODQ == this.target.transform))
                {
                    this.target.objectScript.OQCDCQOQDO(markerScript);
                    Selection.objects = this.target.objectScript.ODQDODOODQs;
                }
                else
                {
                    if (!(this.target.objectScript.ODQDODOODQ == this.target.transform) && (this.count == 0))
                    {
                        if (this.target.InSelected() == null)
                        {
                            this.target.objectScript.ODQDODOODQs = new GameObject[0];
                            this.target.objectScript.OQCDCQOQDO(markerScript);
                            Selection.objects = this.target.objectScript.ODQDODOODQs;
                            this.count++;
                        }
                    }
                    else
                    {
                        if (cEvent.control)
                        {
                            this.target.snapMarker = true;
                        }
                        else
                        {
                            this.target.snapMarker = false;
                        }
                        this.pos = markerScript.oldPos;
                        if ((this.pos != this.oldPos) && !markerScript.changed)
                        {
                            this.oldPos = this.pos;
                            if (!cEvent.shift)
                            {
                                this.target.objectScript.ODOODCODQC(markerScript);
                            }
                        }
                    }
                }
            }
            if (cEvent.shift && markerScript.changed)
            {
                this.OCOCQOQDQQ = true;
            }
            markerScript.changed = false;
            if (!cEvent.shift && this.OCOCQOQDQQ)
            {
                this.target.objectScript.ODOODCODQC(markerScript);
                this.OCOCQOQDQQ = false;
            }
        }
    }

}