using UnityEngine;
using UnityEditor;
using System.Collections;

//THIS IS THE EDITOR WINDOW. DON'T TOUCH THIS :)
[System.Serializable]
public class InventoryEditor : EditorWindow
{
    public string playerName;
    public bool groupEnabled;
    public bool myBool;
    public float myFloat;
    public GUISkin invGUI;
    public GUISkin cSheetGUI;
    public bool includeCSheet;
    public Transform WeaponHolder;
    private Transform WeaponHolderBack;
    public int pauseGameInt;
    public string[] pauseGameOptions;
    public Transform theCamera;
    public int taskTBInt;
    public string[] taskTBStrings;
    public Vector2 scrollPos;
    public Transform selectedObject;
    public string selected;
    // Add menu named "My Window" to the Window menu
    [UnityEditor.MenuItem("Window/Inventory %i")]
    public static void Init()
    {
         // Get existing open window or if none, make a new one:
        InventoryEditor window = ScriptableObject.CreateInstance<InventoryEditor>();
        window.title = "Inventory";
        window.Show();
    }

    public virtual void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos);
        this.taskTBInt = GUILayout.Toolbar(this.taskTBInt, this.taskTBStrings);
        if (this.taskTBInt == 0)
        {
            if (Selection.transforms.Length > 0)
            {
                this.selectedObject = Selection.activeTransform;
            }
            else
            {
                EditorGUILayout.Space();
                GUILayout.Label("---> Select the Player in the scene to get started! <---", EditorStyles.wordWrappedLabel);
                EditorGUILayout.Space();
                GUILayout.Label("Learn how", EditorStyles.boldLabel);
                GUILayout.Label("Visit:  http://brackeys.com/inventory", EditorStyles.wordWrappedLabel);
                EditorGUILayout.Space();
                if (GUILayout.Button("Close"))
                {
                    this.Close();
                }
                GUILayout.Label("Current version:  1.2.2", EditorStyles.wordWrappedLabel);
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
                return;
            }
            EditorGUILayout.Space();
            GUILayout.Label("This section sets up the Inventory system automatically and as recommended. Just make sure to select the Player, fill in the fields below and hit the 'Set it up!' button!", EditorStyles.wordWrappedLabel);
            EditorGUILayout.Space();
            this.includeCSheet = EditorGUILayout.BeginToggleGroup("Include Character Sheet (Highly recommended)", this.includeCSheet);
            GUILayout.Label("This is where you can add a custom skin or use the one included (CSheetSkin).", EditorStyles.wordWrappedLabel);
            this.cSheetGUI = EditorGUILayout.ObjectField("Character Sheet Skin", this.cSheetGUI, typeof(GUISkin), false);
            if (this.cSheetGUI == null)
            {
                this.cSheetGUI = (GUISkin) Resources.Load("CSheetSkin", typeof(GUISkin));
            }
            GUILayout.Label("This is the object which the equipped weapons are going to be parented to.", EditorStyles.wordWrappedLabel);
            this.WeaponHolder = EditorGUILayout.ObjectField("Weapon Holder ", this.WeaponHolder, typeof(Transform), true);
            EditorGUILayout.EndToggleGroup();
            EditorGUILayout.Space();
            GUILayout.Label("This is where you can add a custom skin for the Inventory or use the one included (InventorySkin).", EditorStyles.wordWrappedLabel);
            this.invGUI = EditorGUILayout.ObjectField("Inventory Skin", this.invGUI, typeof(GUISkin), false);
            if (this.invGUI == null)
            {
                this.invGUI = (GUISkin) Resources.Load("InventorySkin", typeof(GUISkin));
            }
            EditorGUILayout.Space();
            GUILayout.Label("Choose what happens when a window is open. Options that disable the mouse will deactive the 'MouseLook' component included in the standard First Person Controller.", EditorStyles.wordWrappedLabel);
            this.pauseGameInt = EditorGUILayout.Popup(this.pauseGameInt, this.pauseGameOptions);
            EditorGUI.BeginDisabledGroup((this.pauseGameInt == 3) || (this.pauseGameInt == 1));
            this.theCamera = EditorGUILayout.ObjectField("Player Camera ", this.theCamera, typeof(Transform), true);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();
            if (Selection.transforms.Length <= 1)
            {
                this.selected = this.selected + (this.selectedObject.name + " ");
                EditorGUILayout.LabelField("Selected Object: ", this.selected);
                this.selected = "";
            }
            else
            {
                EditorGUILayout.LabelField("More than one object selected, please correct this.");
            }
            GUI.color = new Color(0.5f, 1, 0.5f, 1);
            if (GUILayout.Button("Set it up!", GUILayout.Height(30)))
            {
                if (Selection.transforms.Length <= 1)
                {
                    this.InventorySetUp();
                }
                else
                {
                    Debug.LogError("Select only one gameobject");
                }
            }
            GUI.color = Color.white;
            GUI.color = new Color(1, 0.5f, 0.5f, 1);
            if (GUILayout.Button("Remove Inventory"))
            {
                if (Selection.transforms.Length <= 1)
                {
                    this.InventoryDelete();
                }
                else
                {
                    Debug.LogError("Select only one gameobject");
                }
            }
            GUI.color = Color.white;
        }
        else
        {
            if (this.taskTBInt == 1)
            {
                EditorGUILayout.Space();
                GUILayout.Label("This is done manually. Read the 'Guide' to learn how to create Items.", EditorStyles.wordWrappedLabel);
            }
        }
        EditorGUILayout.Space();
        GUILayout.Label("Learn how", EditorStyles.boldLabel);
        GUILayout.Label("Visit:  http://brackeys.com/inventory", EditorStyles.wordWrappedLabel);
        EditorGUILayout.Space();
        if (GUILayout.Button("Close"))
        {
            this.Close();
        }
        GUILayout.Label("Current version:  1.2.2", EditorStyles.wordWrappedLabel);
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    public virtual void InventorySetUp()
    {
        this.InventoryDelete();
        this.selected = this.selected + (this.selectedObject.name + " ");
        if (this.selectedObject.Find("Inventory") != null)
        {
            Debug.LogError(((("An object called 'Inventory' was already found on " + this.selected) + ". If the Inventory has previously been set up on the selected object (") + this.selected) + ") then make sure to delete it by pressing 'Remove Inventory'.");
            return;
        }
        GameObject invObject = new GameObject("Inventory");
        invObject.transform.parent = this.selectedObject;
        invObject.transform.position = this.selectedObject.position;
        invObject.transform.rotation = this.selectedObject.rotation;
        invObject.transform.position.y = invObject.transform.position.y + 0.5f;
        invObject.AddComponent(Inventory);
        invObject.AddComponent(InventoryDisplay);
        if (this.includeCSheet)
        {
            invObject.AddComponent(Character);
            if (this.selectedObject.GetComponent(Character) != null)
            {
                Debug.LogWarning(("The Character Sheet (Character) script was found on the selected object (" + this.selected) + "). Make sure to delete all traces of any previously installed Inventory. There should only be an 'Inventory', 'InventoryDisplay' and 'Character' script on an Inventory gameobject parented to the Player. The Inventory was still set up though.");
            }
        }
        //Set up the cSheet variables
        error csheet = invObject.GetComponent(Character);
        if (this.cSheetGUI != null)
        {
            csheet.cSheetSkin = this.cSheetGUI;
        }
        else
        {
            Debug.LogWarning("Assign the CSheetSkin");
        }
        if (this.WeaponHolder != null)
        {
            csheet.WeaponSlot = this.WeaponHolder;
        }
        else
        {
            Debug.LogError("The Weapon Holder variable wasn't assigned. Make sure to assign it manually under the Character script or remove the inventory and add it again with the variable assigned.");
        }
        //Set up ArmorSlotNames
        csheet.ArmorSlotName = new string[6];
        csheet.ArmorSlotName[0] = "Head";
        csheet.ArmorSlotName[1] = "Chest";
        csheet.ArmorSlotName[2] = "Leg";
        csheet.ArmorSlotName[3] = "Weapon";
        csheet.ArmorSlotName[4] = "Weapon";
        csheet.ArmorSlotName[5] = "Weapon";
        //Set up buttonPositions
        csheet.buttonPositions = new Rect[6];
        csheet.buttonPositions[0].x = (error) 12;
        csheet.buttonPositions[0].y = (error) 34;
        csheet.buttonPositions[0].width = (error) 80;
        csheet.buttonPositions[0].height = (error) 80;
        csheet.buttonPositions[1].x = (error) 12;
        csheet.buttonPositions[1].y = (error) 120;
        csheet.buttonPositions[1].width = (error) 80;
        csheet.buttonPositions[1].height = (error) 80;
        csheet.buttonPositions[2].x = (error) 12;
        csheet.buttonPositions[2].y = (error) 206;
        csheet.buttonPositions[2].width = (error) 80;
        csheet.buttonPositions[2].height = (error) 80;
        csheet.buttonPositions[3].x = (error) 99;
        csheet.buttonPositions[3].y = (error) 34;
        csheet.buttonPositions[3].width = (error) 80;
        csheet.buttonPositions[3].height = (error) 80;
        csheet.buttonPositions[4].x = (error) 99;
        csheet.buttonPositions[4].y = (error) 120;
        csheet.buttonPositions[4].width = (error) 80;
        csheet.buttonPositions[4].height = (error) 80;
        csheet.buttonPositions[5].x = (error) 99;
        csheet.buttonPositions[5].y = (error) 206;
        csheet.buttonPositions[5].width = (error) 80;
        csheet.buttonPositions[5].height = (error) 80;
        //Set up the InventoryDisplay variables
        error invDisp = invObject.GetComponent(InventoryDisplay);
        invDisp.invSkin = this.invGUI;
        //Set up the InvPauseGame (if any)
        if (this.pauseGameInt != 3)
        {
            error pauseGame = invObject.AddComponent(InvPauseGame);
            pauseGame.ThePlayer = this.selectedObject;
            pauseGame.TheCamera = this.theCamera;
        }
        switch (this.pauseGameInt)
        {
            case 1:
                pauseGame.disableMouseLookComponent = false;
                break;
            case 2:
                pauseGame.pauseGame = false;
                break;
        }
        //Set up the InvAudio
        AudioSource theSource = (AudioSource) invObject.AddComponent(typeof(AudioSource));
        theSource.GetComponent<AudioSource>().volume = 0.75f;
        theSource.GetComponent<AudioSource>().playOnAwake = false;
        invObject.AddComponent(InvAudio);
        //Handle errors and complications (if any) to avoid compiler errors
        if (this.selectedObject.GetComponent(Inventory) != null)
        {
            Debug.LogWarning(("The 'Inventory' script was found on the selected object (" + this.selected) + "). Make sure to delete all traces of any previously installed Inventory. There should only be an 'Inventory', 'InventoryDisplay' and 'Character' script on an Inventory gameobject parented to the Player. The Inventory was still set up though.");
        }
        if (this.selectedObject.GetComponent(InventoryDisplay) != null)
        {
            Debug.LogWarning(("The 'InventoryDisplay' script was found on the selected object (" + this.selected) + "). Make sure to delete all traces of any previously installed Inventory. There should only be an 'Inventory', 'InventoryDisplay' and 'Character' script on an Inventory gameobject parented to the Player. The Inventory was still set up though.");
        }
        if (this.includeCSheet)
        {
            Debug.Log(("Inventory and Character Sheet has been set up on " + this.selected) + " under a new GameObject called 'Inventory'");
        }
        else
        {
            Debug.Log(("Inventory has been set up on " + this.selected) + " under a new GameObject called 'Inventory'");
        }
        this.selected = "";
    }

    public virtual void InventoryDelete()
    {
        Undo.RegisterSceneUndo("PlayersInv");
        this.selected = this.selected + (this.selectedObject.name + " ");
        if (this.selectedObject.Find("Inventory") != null)
        {
            UnityEngine.Object.DestroyImmediate(this.selectedObject.Find("Inventory").gameObject);
        }
        if (this.selectedObject.GetComponent(Character) != null)
        {
            DestroyImmediate(this.selectedObject.gameObject.GetComponent(Character));
        }
        if (this.selectedObject.GetComponent(Inventory) != null)
        {
            DestroyImmediate(this.selectedObject.gameObject.GetComponent(Inventory));
        }
        if (this.selectedObject.GetComponent(InventoryDisplay) != null)
        {
            DestroyImmediate(this.selectedObject.gameObject.GetComponent(InventoryDisplay));
        }
        Debug.Log(("Inventory has been removed from " + this.selected) + ". This is always done automatically before the Inventory get's set up.");
        this.selected = "";
    }

    public virtual void OnInspectorUpdate()
    {
        this.Repaint();
    }

    public InventoryEditor()
    {
        this.playerName = "Player";
        this.myBool = true;
        this.myFloat = 1.23f;
        this.includeCSheet = true;
        this.pauseGameOptions = new string[] {"Pause Game + Disable Mouse", "Pause Game", "Disable Mouse", "Keep playing"};
        this.taskTBStrings = new string[] {"Set up Inventory", "Make Items"};
        this.selected = "";
    }

}