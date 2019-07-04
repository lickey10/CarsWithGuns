using System.Collections.Generic;
using System.IO;
using EasyRoads3D;
using EasyRoads3DEditor;
using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
[UnityEditor.CustomEditor(typeof(error))]
public class RoadObjectEditorScript : Editor
{
    public int counter;
    public float pe;
    public bool tv;
    public bool tvDone;
    public bool debugDone;
    public bool res;
    public Collider col;
    public virtual void OnEnable()
    {
        this.target.backupLocation = EditorPrefs.GetInt("ER3DbckLocation", 0);
        if (this.target.OQCODQCQOC == null)
        {
            RoadObjectEditorScript.ODCDDDDDDQ();
            this.target.OCOOCQODQD(null, null, null);
        }
        this.target.ODODQOQO = this.target.OQCODQCQOC.ODDDCOQDCC();
        this.target.ODODQOQOInt = this.target.OQCODQCQOC.OCDDCDCCQD();
        if (this.target.splatmapLayer >= this.target.ODODQOQO.Length)
        {
            this.target.splatmapLayer = 4;
        }
        if (!(this.target.customMesh == null))
        {
            if (this.target.customMesh.GetComponent(typeof(Collider)) != null)
            {
                this.col = this.target.customMesh.GetComponent(typeof(Collider));
            }
            else
            {
                if (OQCDOODQQQ.terrain != null)
                {
                    this.col = OQCDOODQQQ.terrain.GetComponent(typeof(TerrainCollider));
                }
            }
        }
        else
        {
            if (OQCDOODQQQ.terrain != null)
            {
                this.col = OQCDOODQQQ.terrain.GetComponent(typeof(TerrainCollider));
            }
        }
        if (RoadObjectEditorScript.ODCDDDDDDQ())
        {
            OQCDOODQQQ.OQOOCOCDDQ();
        }
        this.target.ODQDODOODQs = new GameObject[0];
    }

    public override void OnInspectorGUI()
    {
        this.EasyRoadsGUIMenu(true, true, this.target);
    }

    public virtual void OnSceneGUI()
    {
        if (this.target.OQCODQCQOC == null)
        {
            RoadObjectEditorScript.ODCDDDDDDQ();
            this.target.OCOOCQODQD(null, null, null);
            if (!(this.target.OCDQCCOCOC == EditorApplication.currentScene) && (this.target.OQCODQCQOC == null))
            {
                OCQCDCCDOC.terrainList.Clear();
                this.target.OCDQCCOCOC = EditorApplication.currentScene;
            }
        }
        this.OnScene();
    }

    public virtual int EasyRoadsGUIMenu(bool flag, bool senderIsMain, RoadObjectScript nRoadScript)
    {
        if ((((this.target.OQDDCQCCDQ == null) || (this.target.OODQCQCDQO == null)) || (this.target.ODOOCQDQCD == null)) || (this.target.OQDDCQCCDQ.Length == 0))
        {
            this.target.OQDDCQCCDQ = new bool[5];
            this.target.OODQCQCDQO = new bool[5];
            this.target.ODOOCQDQCD = nRoadScript;
            this.target.ODODQCCDOC = this.target.OQCODQCQOC.OQQCCOQDQO();
            this.target.ODODQOQO = this.target.OQCODQCQOC.ODDDCOQDCC();
            this.target.ODODQOQOInt = this.target.OQCODQCQOC.OCDDCDCCQD();
        }
        origAnchor = GUI.skin.box.alignment;
        if (this.target.OODCOCOOCC == null)
        {
            this.target.OODCOCOOCC = (GUISkin) Resources.Load("ER3DSkin", typeof(GUISkin));
            this.target.OQQOCCQOOC = (Texture2D) Resources.Load("ER3DLogo", typeof(Texture2D));
        }
        if (!flag)
        {
            this.target.OODODDQDOQ();
        }
        if (this.target.ODDDQCOOQD == -1)
        {
            this.target.ODQDODOODQ = null;
        }
        GUISkin origSkin = GUI.skin;
        GUI.skin = this.target.OODCOCOOCC;
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        this.target.OQDDCQCCDQ[0] = GUILayout.Toggle(this.target.OQDDCQCCDQ[0], new GUIContent("", " Add road markers. "), "AddMarkers", GUILayout.Width(40), GUILayout.Height(22));
        if ((this.target.OQDDCQCCDQ[0] == true) && (this.target.OODQCQCDQO[0] == false))
        {
            this.target.OODODDQDOQ();
            this.target.OQDDCQCCDQ[0] = true;
            this.target.OODQCQCDQO[0] = true;
        }
        this.target.OQDDCQCCDQ[1] = GUILayout.Toggle(this.target.OQDDCQCCDQ[1], new GUIContent("", " Insert road markers. "), "insertMarkers", GUILayout.Width(40), GUILayout.Height(22));
        if ((this.target.OQDDCQCCDQ[1] == true) && (this.target.OODQCQCDQO[1] == false))
        {
            this.target.OODODDQDOQ();
            this.target.OQDDCQCCDQ[1] = true;
            this.target.OODQCQCDQO[1] = true;
        }
        this.target.OQDDCQCCDQ[2] = GUILayout.Toggle(this.target.OQDDCQCCDQ[2], new GUIContent("", " Process the terrain and create road geometry. "), "terrain", GUILayout.Width(40), GUILayout.Height(22));
        if ((this.target.OQDDCQCCDQ[2] == true) && ((this.target.OODQCQCDQO[2] == false) || (this.target.doTerrain != null)))
        {
            if (this.target.markers <= 2)
            {
                EditorUtility.DisplayDialog("Alert", "A minimum of 2 road markers is required before the terrain can be leveled!", "Close");
                this.target.OQDDCQCCDQ[2] = false;
            }
            else
            {
                if (this.target.disableFreeAlerts != null)
                {
                    EditorUtility.DisplayDialog("Alert", "Switching back to 'Edit Mode' is not supported in the free version.\n\nClick Close to generate the road mesh and deform the terrain. This process can take some time depending on the terrains heightmap resolution and the optional vegetation removal, please be patient!\n\nYou can always restore the terrain using the EasyRoads3D terrain restore option in the main menu.\n\nNote: you can disable displaying this message in General Settings.", "Close");
                }
                if (!flag)
                {
                    EditorUtility.DisplayDialog("Alert", "The Unity Terrain Object does not accept height values < 0. The river floor will be equal or higher then the water level. Position all markers higher above the terrain!", "Close");
                    this.target.OQDDCQCCDQ[2] = false;
                }
                else
                {
                    this.tvDone = false;
                    this.target.OODODDQDOQ();
                    this.target.OQDDCQCCDQ[2] = true;
                    this.target.OODQCQCDQO[2] = true;
                    this.target.OOCQCCQOQQ = true;
                    this.target.doTerrain = false;
                    this.target.markerDisplayStr = "Show Markers";
                    if (this.target.objectType < 2)
                    {
                        Undo.RegisterUndo(OQCDOODQQQ.terrain.terrainData, "EasyRoads3D Terrain leveling");
                        if (this.target.displayRoad == null)
                        {
                            this.target.displayRoad = true;
                            this.target.OQCODQCQOC.OQOCOCCQOC(true, this.target.OCCQOQDDDO);
                        }
                        OCQCDCCDOC.ODQODCODOD = false;
                        RoadObjectEditorScript.OOQCOOQQDO(this.target);
                        if (this.target.OOQDOOQQ != null)
                        {
                            this.target.OQCCOODCDO();
                        }
                    }
                    else
                    {
                        this.target.OQCODQCQOC.OCCODDODOO(this.target.transform, false);
                    }
                }
                if (this.target.disableFreeAlerts != null)
                {
                    EditorUtility.DisplayDialog("Finished!", "The terrain data has been updated.\n\nIf you want to keep these changes and add more road objects it is recommended to update the terrain backup data using the EasyRoads3D terrain backup options in the main menu. By doing this you will not loose the current terrain changes if later in the development process you want to restore the terrain back to the current status.\n\nYou can also duplicate the terrain object in the project panel and keep that as the terrain backup.\n\nNote: you can disable displaying this message in General Settings.", "Close");
                }
            }
        }
        this.target.OQDDCQCCDQ[3] = GUILayout.Toggle(this.target.OQDDCQCCDQ[3], new GUIContent("", " General settings. "), "settings", GUILayout.Width(40), GUILayout.Height(22));
        if ((this.target.OQDDCQCCDQ[3] == true) && (this.target.OODQCQCDQO[3] == false))
        {
            this.target.OODODDQDOQ();
            this.target.OQDDCQCCDQ[3] = true;
            this.target.OODQCQCDQO[3] = true;
        }
        this.target.OQDDCQCCDQ[4] = GUILayout.Toggle(this.target.OQDDCQCCDQ[4], new GUIContent("", "Version and Purchase Info"), "info", GUILayout.Width(40), GUILayout.Height(22));
        if ((this.target.OQDDCQCCDQ[4] == true) && (this.target.OODQCQCDQO[4] == false))
        {
            this.target.OODODDQDOQ();
            this.target.OQDDCQCCDQ[4] = true;
            this.target.OODQCQCDQO[4] = true;
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        GUI.skin = null;
        GUI.skin = origSkin;
        this.target.ODDDQCOOQD = -1;
        int i = 0;
        while (i < 5)
        {
            if (this.target.OQDDCQCCDQ[i] != null)
            {
                this.target.ODDDQCOOQD = i;
                this.target.OODQCOQQQQ = i;
            }
            i++;
        }
        if (this.target.ODDDQCOOQD == -1)
        {
            this.target.OODODDQDOQ();
        }
        int markerMenuDisplay = 1;
        if ((this.target.ODDDQCOOQD == 0) || (this.target.ODDDQCOOQD == 1))
        {
            markerMenuDisplay = 0;
        }
        else
        {
            if (((this.target.ODDDQCOOQD == 2) || (this.target.ODDDQCOOQD == 3)) || (this.target.ODDDQCOOQD == 4))
            {
                markerMenuDisplay = 0;
            }
        }
        if (((this.target.OOCQCCQOQQ != null) && (this.target.OQDDCQCCDQ[2] == null)) && Application.isPlaying)
        {
            EditorPrefs.SetBool("ERv2isPlaying", true);
        }
        if ((this.target.OOCQCCQOQQ != null) && (this.target.OQDDCQCCDQ[2] == null))
        {
            this.target.OQDDCQCCDQ[2] = true;
            this.target.OODQCQCDQO[2] = true;
            if (this.target.disableFreeAlerts != null)
            {
                EditorUtility.DisplayDialog("Alert", "Switching back to 'Edit Mode' to add markers or change other settings is not supported in the free version.\n\nDrag the road mesh to the root of the hierarchy and delete the EasyRoads3D editor object once the road is ready!\n\nYou can use Undo to restore the terrain.", "Close");
            }
        }
        GUI.skin.box.alignment = TextAnchor.UpperLeft;
        if ((this.target.ODDDQCOOQD >= 0) && !(this.target.ODDDQCOOQD == 4))
        {
            if ((this.target.ODODQCCDOC == null) || (this.target.ODODQCCDOC.Length == 0))
            {
                this.target.ODODQCCDOC = this.target.OQCODQCQOC.OQQCCOQDQO();
                this.target.ODODQOQO = this.target.OQCODQCQOC.ODDDCOQDCC();
                this.target.ODODQOQOInt = this.target.OQCODQCQOC.OCDDCDCCQD();
            }
            EditorGUILayout.BeginHorizontal();
            GUILayout.Box(this.target.ODODQCCDOC[this.target.ODDDQCOOQD], GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(50));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }
        if ((this.target.ODDDQCOOQD == -1) && !(this.target.ODQDODOODQ == null))
        {
            Selection.activeGameObject = this.target.ODQDODOODQ.gameObject;
        }
        GUI.skin.box.alignment = origAnchor;
        if ((this.target.erInit == "") || (OCQCDCCDOC.debugFlag && !this.debugDone))
        {
            this.debugDone = true;
            this.target.erInit = OCDQQCOQOD.OQOCQOQCOO(this.target.version);
            this.target.OQCODQCQOC.erInit = this.target.erInit;
            this.Repaint();
        }
        if (!(this.target.erInit == "") && this.res)
        {
            this.target.OQDODQOODQ(this.target.geoResolution, false, false);
            this.res = false;
        }
        if (this.target.erInit.Length == 0)
        {
        }
        else
        {
            if ((this.target.ODDDQCOOQD == 0) || (this.target.ODDDQCOOQD == 1))
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Refresh Surfaces", GUILayout.Width(200)))
                {
                    this.target.ODDCCCQCOC();
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                if (this.target.ODDDQCOOQD == 3)
                {
                    GUI.skin.box.alignment = TextAnchor.MiddleLeft;
                    GUILayout.Box(" General Settings", GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(20));
                    if (!(this.target.objectType == 2))
                    {
                        GUILayout.Space(10);
                        bool oldDisplay = this.target.displayRoad;
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("    Display object", "This will activate/deactivate the road object transforms"), GUILayout.Width(125));
                        this.target.displayRoad = EditorGUILayout.Toggle(this.target.displayRoad);
                        EditorGUILayout.EndHorizontal();
                        if (!(oldDisplay == this.target.displayRoad))
                        {
                            this.target.OQCODQCQOC.OQOCOCCQOC(this.target.displayRoad, this.target.OCCQOQDDDO);
                        }
                    }
                    if (this.target.materialStrings == null)
                    {
                        this.target.materialStrings = new string[2];
                        this.target.materialStrings[0] = "Diffuse Shader";
                        this.target.materialStrings[1] = "Transparent Shader";
                    }
                    if (this.target.materialStrings.Length == 0)
                    {
                        this.target.materialStrings = new string[2];
                        this.target.materialStrings[0] = "Diffuse Shader";
                        this.target.materialStrings[1] = "Transparent Shader";
                    }
                    int cm = this.target.materialType;
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("    Surface Material", "The material type used for the road surfaces."), GUILayout.Width(125));
                    this.target.materialType = EditorGUILayout.Popup(this.target.materialType, this.target.materialStrings, GUILayout.Width(115));
                    EditorGUILayout.EndHorizontal();
                    if (!(cm == this.target.materialType))
                    {
                        this.target.OQCODQCQOC.ODODDDCCOQ(this.target.materialType);
                    }
                    if (this.target.materialType == 1)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("        Surface Opacity", "This controls the transparacy level of the surface objects."), GUILayout.Width(125));
                        float so = this.target.surfaceOpacity;
                        this.target.surfaceOpacity = EditorGUILayout.Slider(this.target.surfaceOpacity, 0, 1, GUILayout.Width(150));
                        EditorGUILayout.EndHorizontal();
                        if (!(so == this.target.surfaceOpacity))
                        {
                            this.target.OQCODQCQOC.OOCQQCCCDO(this.target.surfaceOpacity);
                        }
                    }
                    EditorGUILayout.Space();
                    if (this.target.objectType < 2)
                    {
                        bool od = this.target.multipleTerrains;
                    }
                    GUI.enabled = true;
                    GUI.enabled = false;
                    object cl = this.target.backupLocation;
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("    Backup Location", "Use outside Assets folder unless you are using the asset server."), GUILayout.Width(125));
                    this.target.backupLocation = EditorGUILayout.Popup(this.target.backupLocation, this.target.backupStrings, GUILayout.Width(115));
                    EditorGUILayout.EndHorizontal();
                    if (!(this.target.backupLocation == cl))
                    {
                        if (this.target.backupLocation == 1)
                        {
                            if (EditorUtility.DisplayDialog("Backup Location", "Changing the backup location to inside the assets folder is only recommended when you want to synchronize EasyRoads3D backup files with the assetserver.\n\nWould you like to continue?", "Yes", "No"))
                            {
                                EditorPrefs.SetInt("ER3DbckLocation", this.target.backupLocation);
                                OOQDDDCQDD.SwapFiles(this.target.backupLocation);
                                EditorUtility.DisplayDialog("Confirmation", "The backup location has been updated, all backup folders and files have been copied to the new location.\n\nUse CTRL+R to update the assets folder!", "Close");
                            }
                            else
                            {
                                this.target.backupLocation = 0;
                            }
                        }
                        else
                        {
                            if (EditorUtility.DisplayDialog("Backup Location", "The backup location will be changed to outside the assets folder.\n\nWould you like to continue?", "Yes", "No"))
                            {
                                EditorPrefs.SetInt("ER3DbckLocation", this.target.backupLocation);
                                OOQDDDCQDD.SwapFiles(this.target.backupLocation);
                                EditorUtility.DisplayDialog("Confirmation", "The backup location has been updated, all backup folders and files have been copied to the new location.\n\nUse CTRL+R to update the assets folder!", "Close");
                            }
                            else
                            {
                                this.target.backupLocation = 1;
                            }
                        }
                    }
                    GUI.enabled = true;
                    od = OCQCDCCDOC.debugFlag;
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("    Enable Debugging", "This will enable debugging."), GUILayout.Width(125));
                    OCQCDCCDOC.debugFlag = EditorGUILayout.Toggle(OCQCDCCDOC.debugFlag);
                    EditorGUILayout.EndHorizontal();
                    if ((od != OCQCDCCDOC.debugFlag) && OCQCDCCDOC.debugFlag)
                    {
                        this.debugDone = false;
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("    Free version alerts", "Uncheck to disable free version alerts."), GUILayout.Width(125));
                    this.target.disableFreeAlerts = EditorGUILayout.Toggle(this.target.disableFreeAlerts);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    GUILayout.Box(" Object Settings", GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(20));
                    EditorGUILayout.BeginHorizontal();
                    float wd = this.target.roadWidth;
                    if (this.target.objectType == 0)
                    {
                        GUILayout.Label(new GUIContent("    Road width", "The width of the road"), GUILayout.Width(125));
                    }
                    else
                    {
                        GUILayout.Label(new GUIContent("    River Width", "The width of the river"), GUILayout.Width(125));
                    }
                    this.target.roadWidth = EditorGUILayout.FloatField(this.target.roadWidth, GUILayout.Width(40));
                    EditorGUILayout.EndHorizontal();
                    if (!(wd == this.target.roadWidth))
                    {
                        this.target.OQDODQOODQ(this.target.geoResolution, false, false);
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("    Default Indent", "The distance from the left and right side of the road to the part of the terrain levelled at the same height as the road"), GUILayout.Width(125));
                    this.target.indent = EditorGUILayout.FloatField(this.target.indent, GUILayout.Width(40));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("    Raise Markers", "This will raise the position of the markers (m)."), GUILayout.Width(125));
                    this.target.raiseMarkers = EditorGUILayout.FloatField(this.target.raiseMarkers, GUILayout.Width(40));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("    Force Y Position", "When toggled on, ne road markers will inherit the y position of the previous marker."), GUILayout.Width(125));
                    this.target.forceY = EditorGUILayout.Toggle(this.target.forceY);
                    EditorGUILayout.EndHorizontal();
                    if (this.target.forceY != null)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("        Y Change", "The marker will be raised / lowered according this amount for every 100 meters."), GUILayout.Width(125));
                        this.target.yChange = EditorGUILayout.FloatField(this.target.yChange, GUILayout.Width(40));
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("    Surrounding", "This represents the distance over which the terrain will be gradually leveled to the original terrain height"), GUILayout.Width(125));
                    this.target.surrounding = EditorGUILayout.FloatField(this.target.surrounding, GUILayout.Width(40));
                    EditorGUILayout.EndHorizontal();
                    bool OldClosedTrack = this.target.OOQDOOQQ;
                    EditorGUILayout.BeginHorizontal();
                    if (this.target.objectType == 0)
                    {
                        GUILayout.Label(new GUIContent("    Closed Track", "This will connect the 'start' and 'end' of the road"), GUILayout.Width(125));
                    }
                    else
                    {
                        if (this.target.objectType == 1)
                        {
                            GUILayout.Label(new GUIContent("    Closed River", "This will connect the 'start' and 'end' of the river"), GUILayout.Width(125));
                        }
                        else
                        {
                            GUILayout.Label(new GUIContent("    Closed Object", "This will connect the 'start' and 'end' of the object"), GUILayout.Width(125));
                        }
                    }
                    this.target.OOQDOOQQ = EditorGUILayout.Toggle(this.target.OOQDOOQQ);
                    EditorGUILayout.EndHorizontal();
                    if (!(OldClosedTrack == this.target.OOQDOOQQ))
                    {
                        this.target.ODDCCCQCOC();
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUI.enabled = false;
                    GUILayout.Label(new GUIContent("    iOS Platform", "This will prepare the road mesh for the iOS Platform"), GUILayout.Width(125));
                    this.target.iOS = EditorGUILayout.Toggle(this.target.iOS);
                    EditorGUILayout.EndHorizontal();
                    if (!(OldClosedTrack == this.target.iOS))
                    {
                    }
                    GUI.enabled = true;
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("    Geometry Resolution", "The polycount of the generated surfaces. It is recommended to use a low resolution while creating the road. Use the maximum resolution when processing the final terrain."), GUILayout.Width(125));
                    float gr = this.target.geoResolution;
                    this.target.geoResolution = EditorGUILayout.Slider(this.target.geoResolution, 0.5f, 5, GUILayout.Width(150));
                    EditorGUILayout.EndHorizontal();
                    if (!(gr == this.target.geoResolution))
                    {
                        this.target.OQDODQOODQ(this.target.geoResolution, false, false);
                    }
                    EditorGUILayout.BeginHorizontal();
                    OldClosedTrack = this.target.iOS;
                    GUI.enabled = false;
                    GUILayout.Label(new GUIContent("    Tangents", "This will automatically calculate mesh tangents data required for bump mapping. Note that this will take a little bit more preocessing time."), GUILayout.Width(125));
                    this.target.applyTangents = EditorGUILayout.Toggle(this.target.applyTangents);
                    EditorGUILayout.EndHorizontal();
                    GUI.enabled = true;
                    EditorGUILayout.Space();
                    GUILayout.Box(" Render Settings", GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(20));
                    GUI.enabled = false;
                    if (OQCDOODQQQ.selectedTerrain == null)
                    {
                        OQCDOODQQQ.OQOOCOCDDQ();
                    }
                    int st = OQCDOODQQQ.selectedTerrain;
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("    Active Terrain", "The terrain that will be updated"), GUILayout.Width(125));
                    OQCDOODQQQ.selectedTerrain = EditorGUILayout.Popup(OQCDOODQQQ.selectedTerrain, OQCDOODQQQ.terrainStrings, GUILayout.Width(115));
                    EditorGUILayout.EndHorizontal();
                    if (st != OQCDOODQQQ.selectedTerrain)
                    {
                        OQCDOODQQQ.OOODOQCOOQ();
                    }
                    GUI.enabled = true;
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("    Update Vegetation", "When toggled on tree and detail objects near the road will be removed when rendering the terrain."), GUILayout.Width(125));
                    this.target.handleVegetation = EditorGUILayout.Toggle(this.target.handleVegetation);
                    EditorGUILayout.EndHorizontal();
                    if (this.target.handleVegetation != null)
                    {
                        GUI.enabled = false;
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("      Tree Distance (m)", "The distance from the left and the right of the road up to which terrain trees should be removed."), GUILayout.Width(125));
                        float tr = this.target.OCOCODCCDD;
                        this.target.OCOCODCCDD = EditorGUILayout.Slider(this.target.OCOCODCCDD, 0, 25, GUILayout.Width(150));
                        EditorGUILayout.EndHorizontal();
                        if (!(tr == this.target.OCOCODCCDD))
                        {
                            this.target.OQCODQCQOC.OCOCODCCDD = this.target.OCOCODCCDD;
                        }
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("      Detail Distance (m)", "The distance from the left and the right of the road up to which terrain detail opbjects should be removed."), GUILayout.Width(125));
                        tr = this.target.ODDQODQCOO;
                        this.target.ODDQODQCOO = EditorGUILayout.Slider(this.target.ODDQODQCOO, 0, 25, GUILayout.Width(150));
                        EditorGUILayout.EndHorizontal();
                        if (!(tr == this.target.ODDQODQCOO))
                        {
                            this.target.OQCODQCQOC.ODDQODQCOO = this.target.ODDQODQCOO;
                        }
                        GUI.enabled = true;
                    }
                    EditorGUILayout.Space();
                }
                else
                {
                    if (this.target.ODDDQCOOQD == 2)
                    {
                        EditorGUILayout.Space();
                        if (this.target.objectType == 0)
                        {
                            GUILayout.Box(" Road Settings:", GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(20));
                        }
                        else
                        {
                            GUILayout.Box(" River Settings:", GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(20));
                        }
                        GUILayout.Space(10);
                        bool oldRoad = this.target.renderRoad;
                        float oldRoadResolution = this.target.roadResolution;
                        float oldRoadUV = this.target.tuw;
                        float oldRaise = this.target.raise;
                        int oldSegments = this.target.OdQODQOD;
                        float oldOOQQQDOD = this.target.OOQQQDOD;
                        float oldOOQQQDODOffset = this.target.OOQQQDODOffset;
                        float oldOOQQQDODLength = this.target.OOQQQDODLength;
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent(" Render", " When active, terrain matching road geometry will be created."), GUILayout.Width(105));
                        this.target.renderRoad = EditorGUILayout.Toggle(this.target.renderRoad);
                        EditorGUILayout.EndHorizontal();
                        if (this.target.renderRoad != null)
                        {
                            if (this.target.objectType == 0)
                            {
                                if (this.target.roadTexture == null)
                                {
                                    mat = (Material) Resources.Load("roadMaterial", typeof(Material));
                                    this.target.roadTexture = mat.mainTexture;
                                }
                                GUI.enabled = false;
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Label(new GUIContent(" Material", " The road texture."), GUILayout.Width(105));
                                if (GUILayout.Button(this.target.roadTexture, GUILayout.Width(75), GUILayout.Height(75)) != null)
                                {
                                }
                                EditorGUILayout.EndHorizontal();
                                GUI.enabled = true;
                            }
                            EditorGUILayout.BeginHorizontal();
                            GUI.enabled = false;
                            GUILayout.Label(new GUIContent(" Road Segments", " The number of segments over the width of the road."), GUILayout.Width(105));
                            this.target.OdQODQOD = EditorGUILayout.IntSlider(this.target.OdQODQOD, 1, 10, GUILayout.Width(175));
                            GUI.enabled = true;
                            EditorGUILayout.EndHorizontal();
                            if (this.target.OdQODQOD > 1)
                            {
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Label(new GUIContent(" Bumpyness", " The bumypness of the surface of the road."), GUILayout.Width(95));
                                this.target.OOQQQDOD = EditorGUILayout.Slider(this.target.OOQQQDOD, 0, 1, GUILayout.Width(175));
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Label(new GUIContent(" Bumpyness Offset", " The bumypness variation of the road."), GUILayout.Width(95));
                                this.target.OOQQQDODOffset = EditorGUILayout.Slider(this.target.OOQQQDODOffset, 0, 1, GUILayout.Width(175));
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Label(new GUIContent(" Bumpyness Density", " The bumypness density on the road."), GUILayout.Width(95));
                                this.target.OOQQQDODLength = EditorGUILayout.Slider(this.target.OOQQQDODLength, 0.01f, 1, GUILayout.Width(175));
                                EditorGUILayout.EndHorizontal();
                            }
                            GUI.enabled = false;
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label(new GUIContent(" Resolution", " The resolution level of the road geometry."), GUILayout.Width(95));
                            this.target.roadResolution = EditorGUILayout.IntSlider(this.target.roadResolution, 1, 10, GUILayout.Width(175));
                            EditorGUILayout.EndHorizontal();
                            GUI.enabled = true;
                            if (this.target.objectType == 0)
                            {
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Label(new GUIContent(" UV Mapping", " Use the slider to control texture uv mapping of the road geometry."), GUILayout.Width(95));
                                this.target.tuw = EditorGUILayout.Slider(this.target.tuw, 1, 30, GUILayout.Width(175));
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Label(new GUIContent(" Raise (cm)", "Optionally increase this setting when parts of the terrain stick through the road geometry. It is recommended to adjust these areas using the terrain tools!"), GUILayout.Width(95));
                                this.target.raise = EditorGUILayout.Slider(this.target.raise, 0, 100, GUILayout.Width(175));
                                EditorGUILayout.EndHorizontal();
                            }
                            else
                            {
                            }
                            GUILayout.Space(5);
                            GUI.enabled = false;
                            if (this.target.applyTangents != null)
                            {
                                GUI.enabled = false;
                            }
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("Calculate Tangents", GUILayout.Width(175)))
                            {
                            }
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                            GUI.enabled = true;
                            GUI.enabled = true;
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("Save Geometry", GUILayout.Width(175)))
                            {
                                this.target.ODCDDDOCDO();
                                Debug.Log("Road object geometry saved");
                            }
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("Finalize Object", GUILayout.Width(175)))
                            {
                                bool bflag = false;
                                i = 0;
                                while (i < this.target.ODODQQOD.Length)
                                {
                                    if (this.target.ODODQQOD[i] != null)
                                    {
                                        bflag = true;
                                        break;
                                    }
                                    i++;
                                }
                                if ((this.target.autoODODDQQO != null) || (this.target.sosBuild == true))
                                {
                                    bflag = false;
                                }
                                if (EditorUtility.DisplayDialog("Important!", "This will unlink the road from the EasyRoads3D editor object and the EasyRoads3D object will be destroyed!\n\nWould you like to continue?", "Yes", "No"))
                                {
                                    if (bflag)
                                    {
                                        if (EditorUtility.DisplayDialog("Important!", "This object includes activated side objects that have not yet been build!\n\nAre you sure you would you like to continue?", "Yes", "No"))
                                        {
                                            bflag = false;
                                        }
                                    }
                                    if (!bflag)
                                    {
                                        this.target.OQCODQCQOC.FinalizeObject(this.target.gameObject);
                                        UnityEngine.Object.DestroyImmediate(this.target.gameObject);
                                    }
                                }
                            }
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                            GUI.enabled = true;
                        }
                        EditorGUILayout.Space();
                        if (((((((!(oldRoad == this.target.renderRoad) || !(oldRoadResolution == this.target.roadResolution)) || !(oldRoadUV == this.target.tuw)) || !(oldRaise == this.target.raise)) || !(oldSegments == this.target.OdQODQOD)) || !(this.target.OOQQQDOD == oldOOQQQDOD)) || !(this.target.OOQQQDODOffset == oldOOQQQDODOffset)) || !(this.target.OOQQQDODLength == oldOOQQQDODLength))
                        {
                            this.target.OQQOCOQQOC();
                        }
                        GUILayout.Box(" Terrain Settings:", GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(20));
                        GUILayout.Space(5);
                        bool oldApplySplatmap = this.target.applySplatmap;
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent(" Apply Splatmap", " When active, the road will be added to the terrain splatmap. The quality highly depends on the terrain Control Texture Resolution size."), GUILayout.Width(125));
                        this.target.applySplatmap = EditorGUILayout.Toggle(this.target.applySplatmap);
                        EditorGUILayout.EndHorizontal();
                        if (this.target.applySplatmap != null)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label(new GUIContent(" Terrain texture", "This represents the terrain texture which will be used for the road spatmap."), GUILayout.Width(125));
                            this.target.splatmapLayer = EditorGUILayout.IntPopup(this.target.splatmapLayer, this.target.ODODQOQO, this.target.ODODQOQOInt, GUILayout.Width(120));
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label(new GUIContent(" Expand", " Use this setting to increase the size of the splatmap."), GUILayout.Width(125));
                            this.target.expand = EditorGUILayout.IntSlider(this.target.expand, 0, 3, GUILayout.Width(175));
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label(new GUIContent(" Smooth Level", " Use this setting to blur the road splatmap for smoother results."), GUILayout.Width(125));
                            this.target.splatmapSmoothLevel = EditorGUILayout.IntSlider(this.target.splatmapSmoothLevel, 0, 3, GUILayout.Width(175));
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label(new GUIContent(" Offset x", " Moves the road splatmap in the x direction."), GUILayout.Width(125));
                            this.target.offsetX = EditorGUILayout.IntField(this.target.offsetX, GUILayout.Width(50));
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label(new GUIContent(" Offset y", " Moves the road splatmap in the y direction."), GUILayout.Width(125));
                            this.target.offsetY = EditorGUILayout.IntField(this.target.offsetY, GUILayout.Width(50));
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label(new GUIContent(" Opacity", "Use this setting to blend the road splatmap with the terrain splatmap."), GUILayout.Width(125));
                            this.target.opacity = EditorGUILayout.Slider(this.target.opacity, 0, 1, GUILayout.Width(175));
                            EditorGUILayout.EndHorizontal();
                            GUILayout.Space(5);
                            GUI.enabled = this.target.OCDCOQDCDQ;
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("Apply Changes", GUILayout.Width(175)))
                            {
                                this.target.OQCCOODCDO();
                                if (this.target.OOQDOOQQ != null)
                                {
                                    this.target.OQCCOODCDO();
                                }
                                this.target.OCDCOQDCDQ = false;
                            }
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                        }
                        GUILayout.Space(5);
                        if (!(oldApplySplatmap == this.target.applySplatmap))
                        {
                            this.target.OQCCOODCDO();
                            if (this.target.OOQDOOQQ != null)
                            {
                                this.target.OQCCOODCDO();
                            }
                        }
                        GUI.enabled = true;
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent(" Terrain Smoothing:", "This will smoothen the terrain near the surface edges according the below distance."), GUILayout.Width(175));
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent(" Edges (m)", "This represents the smoothen distance."), GUILayout.Width(125));
                        this.target.smoothDistance = EditorGUILayout.Slider(this.target.smoothDistance, 0, 5, GUILayout.Width(175));
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(5);
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Update Edges", GUILayout.Width(175)))
                        {
                            Undo.RegisterUndo(OQCDOODQQQ.terrain.terrainData, "EasyRoads3D Terrain smooth");
                            this.target.OQCODQCQOC.OQQDCDQQDC(this.target.smoothDistance, 0);
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent(" Surrounding (m)", "This represents the smoothen distance."), GUILayout.Width(125));
                        this.target.smoothSurDistance = EditorGUILayout.Slider(this.target.smoothSurDistance, 0, 15, GUILayout.Width(175));
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(5);
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Update Surrounding", GUILayout.Width(175)))
                        {
                            Undo.RegisterUndo(OQCDOODQQQ.terrain.terrainData, "EasyRoads3D Terrain smooth");
                            this.target.OQCODQCQOC.OQQDCDQQDC(this.target.smoothSurDistance, 1);
                        }
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                        GUILayout.Box(" Cam Fly Over", GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(20));
                        GUI.enabled = false;
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("  Position", ""), GUILayout.Width(75));
                        float sp = this.target.splinePos;
                        this.target.splinePos = EditorGUILayout.Slider(this.target.splinePos, 0, 0.9999f);
                        EditorGUILayout.EndHorizontal();
                        if (!(sp == this.target.splinePos))
                        {
                        }
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label(new GUIContent("  Height", ""), GUILayout.Width(75));
                        sp = this.target.camHeight;
                        this.target.camHeight = EditorGUILayout.Slider(this.target.camHeight, 1, 10);
                        EditorGUILayout.EndHorizontal();
                        if (!(sp == this.target.camHeight))
                        {
                        }
                        GUI.enabled = true;
                        EditorGUILayout.Space();
                    }
                    else
                    {
                        if (this.target.ODDDQCOOQD == 4)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(this.target.OQQOCCQOOC);
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(" EasyRoads3D v" + this.target.version);
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(" Version Type: Free Version", GUILayout.Height(22));
                            if (GUILayout.Button("i", GUILayout.Width(22)))
                            {
                                Application.OpenURL("http://www.unityterraintools.com");
                            }
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("Help", GUILayout.Width(225)))
                            {
                                Application.OpenURL("http://www.unityterraintools.com/manual.php");
                            }
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                            GUI.skin = origSkin;
                            EditorGUILayout.Space();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            GUILayout.Box("Check out the full version if you had like to take advantage of all the features including the built-in paramatric modeling tool", GUILayout.Width(250));
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("Get the Full Version", GUILayout.Width(225)))
                            {
                                //	AssetStore.Open("http://u3d.as/content/anda-soft/easy-roads3d-pro/1Ch");
                                Application.OpenURL("https://www.assetstore.unity3d.com/#/content/469");
                            }
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(new GUIContent("  Newsletter Sign Up:", ""), GUILayout.Width(155));
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(new GUIContent("  Name", ""), GUILayout.Width(75));
                            this.target.uname = GUILayout.TextField(this.target.uname, GUILayout.Width(150));
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(new GUIContent("  Email", ""), GUILayout.Width(75));
                            this.target.email = GUILayout.TextField(this.target.email, GUILayout.Width(150));
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("Submit", GUILayout.Width(225)))
                            {
                                EditorUtility.DisplayDialog("Newsletter Signup", OCDDOQCCDD0.NewsletterSignUp(this.target.uname, this.target.email), "Ok");
                            }
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.EndHorizontal();
                        }
                        else
                        {
                            if (!(this.target.markers == this.target.OCCQOQDDDO.childCount))
                            {
                                this.target.ODDCCCQCOC();
                            }
                            EditorGUILayout.Space();
                            GUILayout.Box(" General Info", GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(20));
                            if (RoadObjectScript.objectStrings == null)
                            {
                                RoadObjectScript.objectStrings = new string[3];
                                RoadObjectScript.objectStrings[0] = "Road Object";
                                RoadObjectScript.objectStrings[1] = "River Object";
                                RoadObjectScript.objectStrings[2] = "Procedural Mesh Object";
                            }
                            if (this.target.distance == "-1")
                            {
                                string[] ar = this.target.OQCODQCQOC.ODQQQDDDCQ(-1);
                                this.target.distance = ar[0];
                            }
                            EditorGUILayout.Space();
                            GUILayout.Label(" Object Type: " + RoadObjectScript.objectStrings[this.target.objectType]);
                            if (this.target.objectType == 0)
                            {
                                GUILayout.Label((" Total Road Distance: " + this.target.distance.ToString()) + " km");
                            }
                        }
                    }
                }
            }
        }
        EditorGUILayout.Space();
        if (GUI.tooltip != "")
        {
            GUI.Label(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 200, 40), GUI.tooltip);
        }
        if (GUI.changed)
        {
            this.target.OCDCOQDCDQ = true;
        }
        return markerMenuDisplay;
    }

    public virtual float OOCDDQQOOD(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
        if (dir > 0f)
        {
            return 1f;
        }
        else
        {
            if (dir < 0f)
            {
                return -1f;
            }
            else
            {
                return 0f;
            }
        }
    }

    public virtual void OnScene()
    {
        RaycastHit hit = default(RaycastHit);
        Event cEvent = Event.current;
        if (!(this.target.OODQCOQQQQ == -1) && Event.current.shift)
        {
            this.target.OQDDCQCCDQ[this.target.OODQCOQQQQ] = true;
        }
        if ((this.target.OQDDCQCCDQ == null) || (this.target.OQDDCQCCDQ.Length == 0))
        {
            this.target.OQDDCQCCDQ = new bool[5];
            this.target.OODQCQCDQO = new bool[5];
        }
        if ((cEvent.shift && (cEvent.type == EventType.MouseDown)) || (this.target.OQDDCQCCDQ[1] != null))
        {
            Vector2 mPos = cEvent.mousePosition;
            mPos.y = (Screen.height - mPos.y) - 40;
            Ray ray = Camera.current.ScreenPointToRay(mPos);
            if (Physics.Raycast(Camera.current.transform.position, ray.direction, out hit, 3000))
            {
                if (this.target.OQDDCQCCDQ[0] != null)
                {
                    go = this.target.OOOCOOQCOD(hit.point);
                }
                else
                {
                    if (((this.target.OQDDCQCCDQ[1] != null) && (cEvent.type == EventType.MouseDown)) && cEvent.shift)
                    {
                        this.target.ODDCCQDCQC(hit.point, true);
                    }
                    else
                    {
                        if ((this.target.OQDDCQCCDQ[1] != null) && cEvent.shift)
                        {
                            this.target.ODDCCQDCQC(hit.point, false);
                        }
                        else
                        {
                            if (this.target.handleInsertFlag != null)
                            {
                                this.target.handleInsertFlag = this.target.OQCODQCQOC.OOOCDCQDOC();
                            }
                        }
                    }
                }
                Selection.activeGameObject = this.target.obj.gameObject;
            }
        }
        if ((cEvent.control && cEvent.alt) && (cEvent.type == EventType.MouseDown))
        {
            mPos = cEvent.mousePosition;
            mPos.y = (Screen.height - mPos.y) - 40;
            ray = Camera.current.ScreenPointToRay(mPos);
            if (Physics.Raycast(Camera.current.transform.position, ray.direction, out hit, 3000))
            {
                if (((Terrain) hit.collider.gameObject.GetComponent(typeof(Terrain))) != null)
                {
                    Terrain t = (Terrain) hit.collider.gameObject.GetComponent(typeof(Terrain));
                    i = 0;
                    while (i < OQCDOODQQQ.terrains.Length)
                    {
                        if (t == OQCDOODQQQ.terrains[i])
                        {
                            if (OQCDOODQQQ.terrains.Length > 1)
                            {
                                OQCDOODQQQ.selectedTerrain = i + 1;
                            }
                            else
                            {
                                OQCDOODQQQ.selectedTerrain = i;
                            }
                            OQCDOODQQQ.OOODOQCOOQ();
                            EditorUtility.SetDirty(this.target);
                        }
                        i++;
                    }
                }
            }
        }
        if (!(this.target.OQCQDODCQQ == this.target.obj) || !(this.target.obj.name == this.target.OQQODDQQOO))
        {
            this.target.OQCODQCQOC.OCQOQQOCCO();
            this.target.OQCQDODCQQ = this.target.obj;
            this.target.OQQODDQQOO = this.target.obj.name;
        }
        if (!(this.target.ODQDODOODQ == null))
        {
            this.target.OQCODQCQOC.OOOCDCQDOC();
        }
        if (!(this.target.transform.position == Vector3.zero))
        {
            this.target.transform.position = Vector3.zero;
        }
    }

    public static bool ODCDDDDDDQ()
    {
        bool flag = false;
        Terrain[] terrains = (Terrain[]) MonoBehaviour.FindObjectsOfType(typeof(Terrain));
        foreach (Terrain terrain in terrains)
        {
            if (!terrain.gameObject.GetComponent(EasyRoads3DTerrainID))
            {
                EasyRoads3DTerrainID terrainscript = terrain.gameObject.AddComponent<EasyRoads3DTerrainID>();
                string id = UnityEngine.Random.Range(100000000, 999999999).ToString();
                terrainscript.terrainid = id;
                flag = true;
                path = ((Directory.GetCurrentDirectory() + OOCDCOQDQC.backupFolder) + "/") + id;
                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch(System.Exception e)
                    {
                        Debug.Log((("Could not create directory: " + path) + " ") + e);
                    }
                }
                if (Directory.Exists(path))
                {
                }
            }
        }
    }

    public static void OOQCOOQQDO(object target)
    {
        EditorUtility.DisplayProgressBar("Build EasyRoads3D Object", "Initializing", 0);
        scripts = FindObjectsOfType()System.Collections.Generic.List<Transform> rObj = new List<Transform>();
        foreach (object script in scripts)
        {
            if (!(script.transform == target.transform))
            {
                rObj.Add(script.transform);
            }
        }
        if (target.ODODQOQO == null)
        {
            target.ODODQOQO = target.OQCODQCQOC.ODDDCOQDCC();
            target.ODODQOQOInt = target.OQCODQCQOC.OCDDCDCCQD();
        }
        target.OQDODQOODQ(0.5f, true, false);
        if ((OQCDOODQQQ.selectedTerrain == null) || (target.OQCODQCQOC.terrain == null))
        {
            OQCDOODQQQ.OQOOCOCDDQ();
        }
        target.OQCODQCQOC.OCDCCOCOQO();
        OQCDOODQQQ.OOCCDDOCCO(target.OQCODQCQOC.terrain, (((((Directory.GetCurrentDirectory() + OOCDCOQDQC.backupFolder) + "/") + OQCDOODQQQ.OCCQOOCOCQ(target.OQCODQCQOC.terrain)) + "/") + target.OQCODQCQOC.OQQODDQQOO) + "_splatMap");
        OOQDDDCQDD.ODQODDOOCC(target.OQCODQCQOC.terrain, (((((Directory.GetCurrentDirectory() + OOCDCOQDQC.backupFolder) + "/") + OQCDOODQQQ.OCCQOOCOCQ(target.OQCODQCQOC.terrain)) + "/") + target.OQCODQCQOC.OQQODDQQOO) + "_heightmap.backup");
        System.Collections.Generic.List<tPoint> hitOCOCCDOQQD = target.OQCODQCQOC.OQCCCOQCOO(Vector3.zero, target.raise, target.obj, target.OOQDOOQQ, rObj, target.handleVegetation);
        System.Collections.Generic.List<Vector3> changeArr = new List<Vector3>();
        float stepsf = Mathf.Floor(hitOCOCCDOQQD.Count / 10);
        int steps = Mathf.RoundToInt(stepsf);
        i = 0;
        while (i < 10)
        {
            changeArr = target.OQCODQCQOC.OOCDDDQQQD(hitOCOCCDOQQD, i * steps, steps, changeArr);
            EditorUtility.DisplayProgressBar("Build EasyRoads3D Object", "Updating Terrain", i * 10);
            i++;
        }
        changeArr = target.OQCODQCQOC.OOCDDDQQQD(hitOCOCCDOQQD, 10 * steps, hitOCOCCDOQQD.Count - (10 * steps), changeArr);
        target.OQCODQCQOC.OQCQCCCOOQ(changeArr, rObj);
        if (target.OQCODQCQOC.handleVegetation != null)
        {
            target.OQCODQCQOC.OCCDDQCOCO();
            path = ((((Directory.GetCurrentDirectory() + OOCDCOQDQC.backupFolder) + "/") + OQCDOODQQQ.OCCQOOCOCQ(target.OQCODQCQOC.terrain)) + "/") + target.OQCODQCQOC.OCDCQCDDDO.OQQODDQQOO;
            OOCDCOQDQC.OQDDODQOOQ(Directory.GetCurrentDirectory() + OOCDCOQDQC.backupFolder);
            OOCDCOQDQC.OQDDODQOOQ(((Directory.GetCurrentDirectory() + OOCDCOQDQC.backupFolder) + "/") + OQCDOODQQQ.OCCQOOCOCQ(target.OQCODQCQOC.terrain));
            OOQDDDCQDD.ODCDDCDCQD(target.OQCODQCQOC.OQQDCCOCCO.ToArray(), target.OQCODQCQOC.ODQDDQQOQQ, path);
        }
        target.OQCCOODCDO();
        target.OQCODQCQOC.ODODDCCCQC(target.transform, true);
        target.OQCODQCQOC.OOCCQQODQO();
        EditorUtility.ClearProgressBar();
    }

    public virtual void OCOCCCDDDO(object target)
    {
        EditorUtility.DisplayProgressBar("Restore EasyRoads3D Object", "Restoring terrain data", 0f);
        target.OQDODQOODQ(target.geoResolution, false, false);
        if (!(target.OQCODQCQOC.OQQQOODDCC == null) && !(target.OQCODQCQOC == null))
        {
            if ((target.OQCODQCQOC.editRestore != null) && File.Exists((((((Directory.GetCurrentDirectory() + OOCDCOQDQC.backupFolder) + "/") + OQCDOODQQQ.OCCQOOCOCQ(target.OQCODQCQOC.terrain)) + "/") + target.OQCODQCQOC.OQQODDQQOO) + "_heightmap.backup"))
            {
                OOQDDDCQDD.OCDDQCDODO(target.OQCODQCQOC.terrain, (((((Directory.GetCurrentDirectory() + OOCDCOQDQC.backupFolder) + "/") + OQCDOODQQQ.OCCQOOCOCQ(target.OQCODQCQOC.terrain)) + "/") + target.OQCODQCQOC.OQQODDQQOO) + "_heightmap.backup");
            }
            else
            {
                if (target.OQCODQCQOC.editRestore != null)
                {
                    Debug.LogWarning("The original terrain heightmap data file was not found. If necessary you may restore the terrain data by using Undo or, if the terrain backup is up to date, through the EasyRoads3D Menu");
                }
            }
        }
        if (!(target.OQCODQCQOC == null))
        {
            target.OQCODQCQOC.OQCODQODOQ();
            if ((target.OQCODQCQOC.handleVegetation != null) && (target.OQCODQCQOC.editRestore != null))
            {
                if (!(target.OQCODQCQOC.OQQDCCOCCO == null))
                {
                    if (target.OQCODQCQOC.OQQDCCOCCO.Count == 0)
                    {
                        // get treeData from file
                        path = ((((Directory.GetCurrentDirectory() + OOCDCOQDQC.backupFolder) + "/") + OQCDOODQQQ.OCCQOOCOCQ(target.OQCODQCQOC.terrain)) + "/") + target.OQCODQCQOC.OQQODDQQOO;
                        target.OQCODQCQOC.OQQDCCOCCO = OOQDDDCQDD.ODCCCCQQCQ(path);
                    }
                    if (!(target.OQCODQCQOC.OQQDCCOCCO == null))
                    {
                        target.OQCODQCQOC.OCCOCQQDOO();
                    }
                    if (target.OQCODQCQOC.ODQDDQQOQQ.Count == 0)
                    {
                        // get detailData from file
                        path = ((((Directory.GetCurrentDirectory() + OOCDCOQDQC.backupFolder) + "/") + OQCDOODQQQ.OCCQOOCOCQ(target.OQCODQCQOC.terrain)) + "/") + target.OQCODQCQOC.OQQODDQQOO;
                        target.OQCODQCQOC.ODQDDQQOQQ = OOQDDDCQDD.ODQQDOCODQ(path);
                    }
                    if (!(target.OQCODQCQOC.ODQDDQQOQQ == null))
                    {
                        target.OQCODQCQOC.OQCDDCCCOD();
                    }
                }
            }
        }
        target.ODODDDOO = false;
        EditorUtility.ClearProgressBar();
    }

}