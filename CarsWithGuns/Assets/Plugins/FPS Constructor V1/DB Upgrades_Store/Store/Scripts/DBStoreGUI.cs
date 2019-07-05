using UnityEngine;
using System.Collections;

// Class to hold common GUI Panel Configuration Information
[System.Serializable]
public class DBStorePanel : object
{
    public float x;
    public float y;
    public float width;
    public float height;
    public Rect r;
    public float buttonWidth;
    public float buttonHeight;
    public float navButtonWidth;
    public float navButtonHeight;
    public Texture nextIcon;
    public Texture prevIcon;
    public Texture checkedIcon;
    public Texture lockedIcon;
    public float titleHeight;
    public float xPad;
    public float yPad;
    public string title;
    public Texture titleIcon;
    public string[] content;
    public WeaponInfo[] wi;
    public Texture[] contentIcon;
    public int columns;
    public int rows;
    public int buttonsPerPage;
    public int page;
    public int nPages;
    public int selection;
    public DBStoreController store;
    public DBStorePanel()
    {
        this.page = 1;
        this.nPages = 1;
    }

}
// Store State Variables
public enum HeaderModes
{
    Buy = 0,
    Equip = 1
}

[System.Serializable]
public partial class DBStoreGUI : MonoBehaviour
{
    public GUISkin skin;
    public Texture checkedIcon;
    public Texture lockedIcon;
    private WeaponClassIcons wcIcons;
    private Texture[] slotIcons;
    private DBStoreController store;
    //Position of the upper left corner of the store, if negative the store will be centered
    public int sPosX;
    public int sPosY;
    //configuration parameters fpr the Header (H) GUI Component
    public int HWidth;
    public int HHeight;
    public int HYpad;
    public int HButtonWidth;
    public int HButtonHeight;
    public int HTitleHeight;
    public string HTitle;
    public Texture HTitleImage;
    //Configuration for Left Selection (LS) GUI Component
    public float LSHeight;
    public float LSWidth;
    public float LSButtonHeight;
    public float LSButtonWidth;
    public float LSxPad;
    public float LSyPad;
    public Texture LSNextIcon;
    public Texture LSPrevIcon;
    // Configuration for Main Display (MD) GUI Component
    public float MDButtonWidth;
    public float MDButtonHeight;
    public float MDxPad;
    public float MDyPad;
    private DBStorePanel header;
    private DBStorePanel lsBuy; // Buy Weapons Left Selection GUI (LS) Component
    private DBStorePanel lsEquip; // Equip Slots Left Selection (LS) GUI Componenent
    private DBStorePanel mdBuy; // Buy Weapons Main Display (MD)GUI Component
    private DBStorePanel mdEquip; // Equip Slot Main Display (MD) GUI Componenet
    private Rect popupRect;
    private string[] MDContent;
    private int MDSelection;
    private int clicked; // temp variable used to track selections
    private bool viewUpgrades;
    //variables used for the Weapon Info/Buy/Upgrade Popup Window
    public float popupUpgradeWidth;
    public float popupUpgradeHeight;
    private Rect popupUpgradeRect; //Expanded Rectangle for the popup Buy/Upgrade Window
    private bool popupActive;
    private bool popupBuyActive;
    private Vector2 popupBuyScroll1;
    private bool[] upgradeSelected; //allows a maximum of 20 upgrades per weapon
    private Rect popupLockedRect;
    private bool popupLockedActive;
    public Texture maskTexture; //Texture that's drawn over the scene when the store is active
    public Texture maskTexturePopup; //Texture that's drawn behind the Buy/Sell/Upgrade window
    private bool upgradeDisplayBuy;
    private bool upgradeDisplayEquip;
    public Texture upgradeBuyIcon;
    public Texture upgradeInfoIcon;
    public virtual void Start()
    {
        this.store = (DBStoreController) UnityEngine.Object.FindObjectOfType(typeof(DBStoreController));
        this.store.Initialize();
        if ((this.sPosX < 0) || (this.sPosY < 0)) // center the store
        {
            this.sPosX = (int) ((Screen.width / 2) - ((this.HWidth + this.LSWidth) / 2));
            this.sPosY = (int) ((Screen.height / 2) - ((this.HHeight + this.LSHeight) / 2));
        }
        this.popupUpgradeRect = new Rect(this.sPosX + 30, this.sPosY + 60, this.popupUpgradeWidth, this.popupUpgradeHeight);
        this.popupLockedRect = new Rect((this.sPosX + this.LSWidth) + 50, (this.sPosY + this.HHeight) + 50, 200, 200);
        this.wcIcons = (WeaponClassIcons) UnityEngine.Object.FindObjectOfType(typeof(WeaponClassIcons));
        this.setupLS(this.lsBuy);
        this.setuplsBuyContent();
        this.setupLS(this.lsEquip);
        this.setuplsEquipContent();
        this.setupHeader();
        this.setupMD(this.mdBuy);
        this.setupMD(this.mdEquip);
    }

    public virtual void OnGUI()
    {
        if (!DBStoreController.inStore)
        {
            return;
        }
        //GUI.skin = skin;
        if (DBStoreController.inStore)
        {
            if (this.maskTexture)
            {
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.maskTexture);
            }
            this.clicked = DBStoreGUI.DisplayHeader(this.header); //Draw Header Area
            if (!this.popupActive && (this.clicked != -1))
            {
                this.header.selection = this.clicked; // If Popup is Active Don't respond to clicks
                this.setmdBuyContent(this.lsBuy.selection);
                this.setmdEquipContent(this.lsEquip.selection);
            }
            if (((HeaderModes) this.header.selection) == HeaderModes.Buy)
            {
                this.clicked = DBStoreGUI.DisplayLS(this.lsBuy); // Draw Left Selection Area
                if ((this.clicked != -1) && !this.popupActive)
                {
                    this.setmdBuyContent(this.lsBuy.selection);
                }
                this.clicked = DBStoreGUI.DisplayMD(this.mdBuy, this.lsBuy.selection, (HeaderModes) this.header.selection);
                if ((this.clicked != -1) && this.mdBuy.wi[this.clicked].locked)
                {
                    this.mdBuy.selection = this.clicked;
                    this.popupLockedActive = true;
                    this.popupActive = true;
                }
                else
                {
                    if ((this.clicked != -1) && !this.popupActive)
                    {
                        this.mdBuy.selection = this.clicked;
                        this.popupActive = true;
                        this.popupBuyActive = true;
                    }
                }
            }
            else
            {
                if (((HeaderModes) this.header.selection) == HeaderModes.Equip)
                {
                    this.clicked = DBStoreGUI.DisplayLS(this.lsEquip);
                    if ((this.clicked != -1) && !this.popupActive)
                    {
                        this.mdEquip.selection = this.clicked;
                        this.setmdEquipContent(this.lsEquip.selection);
                    }
                    this.clicked = DBStoreGUI.DisplayMD(this.mdEquip, this.lsEquip.selection, (HeaderModes) this.header.selection);
                    if ((this.clicked != -1) && !this.popupActive)
                    {
                        this.mdEquip.selection = this.clicked;
                        this.popupActive = true;
                        this.popupBuyActive = true;
                    }
                }
            }
            if (this.popupBuyActive)
            {
                if (this.maskTexture)
                {
                    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.maskTexturePopup);
                }
                this.popupRect = this.popupUpgradeRect;
                if (this.maskTexture)
                {
                    GUI.DrawTexture(this.popupRect, this.maskTexture);
                }
                GUI.Window(1, this.popupUpgradeRect, this.WeaponBuyWindow, "Weapon Info");
            }
            else
            {
                if (this.popupLockedActive)
                {
                    this.popupRect = this.popupLockedRect;
                    if (this.maskTexturePopup)
                    {
                        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.maskTexturePopup);
                        GUI.DrawTexture(this.popupRect, this.maskTexture);
                    }
                    GUI.Window(0, this.popupRect, this.WeaponLockedWindow, "Sorry!!!");
                }
            }
        }
    }

    public virtual void setupMD(DBStorePanel md)
    {
        md.selection = -1;
        md.buttonHeight = this.MDButtonHeight;
        md.buttonWidth = this.MDButtonWidth;
        md.xPad = this.MDxPad;
        md.yPad = this.MDyPad;
        md.checkedIcon = this.checkedIcon;
        md.lockedIcon = this.lockedIcon;
        md.r = new Rect(this.sPosX + this.LSWidth, this.sPosY + this.HHeight, this.HWidth, this.LSHeight);
        md.columns = (int) Mathf.Floor((md.r.width - md.xPad) / (md.buttonWidth + md.xPad));
        md.rows = (int) Mathf.Floor(((md.r.height - md.titleHeight) - md.yPad) / (md.buttonHeight - md.yPad));
        md.content = null;
        md.store = this.store;
    }

    //Set the content for the Buy MD panel based on weapon class selected
    public virtual void setmdBuyContent(int sel)
    {
        this.mdBuy.content = new string[this.store.WeaponInfoByClass[sel].WeaponInfoArray.Length];
        this.mdBuy.wi = new WeaponInfo[this.store.WeaponInfoByClass[sel].WeaponInfoArray.Length];
        int i = 0;
        while (i < this.mdBuy.content.Length)
        {
            this.mdBuy.content[i] = this.store.WeaponInfoByClass[sel].WeaponInfoArray[i].gunName;
            this.mdBuy.wi[i] = this.store.WeaponInfoByClass[sel].WeaponInfoArray[i];
            i++;
        }
    }

    //set the content for the Equip MD panel based on the slot selected
    //for now it's the same for all slots but we'll be adding the ability to restrict slots to different types of weapons
    public virtual void setmdEquipContent(int slot)
    {
        this.mdEquip.content = this.store.getWeaponNamesOwned(slot);
        this.mdEquip.wi = this.store.getWeaponsOwned(slot);
    }

    public virtual void setupLS(DBStorePanel ls)
    {
        ls.selection = 0;
        ls.buttonHeight = this.LSButtonHeight;
        ls.buttonWidth = this.LSButtonWidth;
        ls.xPad = this.LSxPad;
        ls.yPad = this.LSyPad;
        ls.navButtonWidth = ls.buttonWidth / 2f;
        ls.navButtonHeight = ls.buttonHeight / 2f;
        ls.nextIcon = this.LSNextIcon;
        ls.prevIcon = this.LSPrevIcon;
        ls.r = new Rect(this.sPosX, this.sPosY + this.HHeight, this.LSWidth, this.LSHeight);
        ls.buttonsPerPage = (int) Mathf.Floor((((ls.r.height - ls.titleHeight) - ls.navButtonHeight) - (2 * ls.yPad)) / (ls.buttonHeight + ls.yPad));
        ls.store = this.store;
    }

    public virtual void setuplsBuyContent()
    {
        this.lsBuy.title = "Weapon Classes";
        this.lsBuy.content = new string[this.store.WeaponInfoByClass.Length];
        this.lsBuy.contentIcon = new Texture[this.store.WeaponInfoByClass.Length];
        System.Array.Copy(this.store.weaponClassNamesPopulated, this.lsBuy.content, this.store.WeaponInfoByClass.Length);
        int i = 0;
        while (i < this.lsBuy.content.Length)
        {
            this.lsBuy.content[i] = ((this.lsBuy.content[i] + " (") + this.store.WeaponInfoByClass[i].WeaponInfoArray.Length) + ")";
            int ic = (int) this.store.WeaponInfoByClass[i].WeaponInfoArray[0].weaponClass;
            this.lsBuy.contentIcon[i] = this.wcIcons.weaponClassTextures[ic];
            i++;
        }
        this.lsBuy.nPages = (int) Mathf.Ceil(UnityScript.Lang.UnityBuiltins.parseFloat(this.lsBuy.content.Length) / UnityScript.Lang.UnityBuiltins.parseFloat(this.lsBuy.buttonsPerPage));
    }

    public virtual void setuplsEquipContent()
    {
        this.lsEquip.title = "Weapon Slots";
        this.lsEquip.content = new string[this.store.playerW.weapons.Length];
        this.lsEquip.contentIcon = new Texture[10];
        int i = 0;
        while (i < this.lsEquip.content.Length)
        {
            this.lsEquip.content[i] = this.store.slotInfo.slotName[i];
            this.lsEquip.contentIcon[i] = this.slotIcons[i];
            i++;
        }
        this.lsEquip.nPages = (int) Mathf.Ceil(UnityScript.Lang.UnityBuiltins.parseFloat(this.lsEquip.content.Length) / UnityScript.Lang.UnityBuiltins.parseFloat(this.lsEquip.buttonsPerPage));
    }

    public virtual void setupHeader()
    {
        this.header.r = new Rect(this.sPosX + this.LSWidth, this.sPosY, this.HWidth, this.HHeight);
        this.header.yPad = this.HYpad;
        this.header.content = new string[] {"Buy", "Equip"};
        this.header.title = this.HTitle;
        this.header.titleHeight = this.HTitleHeight;
        this.header.buttonHeight = this.HButtonHeight;
        this.header.buttonWidth = this.HButtonWidth;
        this.header.checkedIcon = this.checkedIcon;
        this.header.lockedIcon = this.lockedIcon;
        this.header.store = this.store;
        this.header.titleIcon = this.HTitleImage;
    }

    //Function to display the Header Panel for the store
    public static int DisplayHeader(DBStorePanel cfg)
    {
        Rect rect = default(Rect);
        int clicked = -1;
        GUIContent gc = null;
        GUILayout.BeginArea(cfg.r);
        GUI.Box(new Rect(0, 0, cfg.r.width, cfg.r.height), "");
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        gc = DBStoreGUI.setGUIContent(cfg.titleIcon, cfg.title, "");
        GUILayout.Label(gc, GUILayout.Height(cfg.titleHeight), GUILayout.Width((cfg.titleHeight / cfg.titleIcon.height) * cfg.titleIcon.width));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        int i = 0;
        while (i < cfg.content.Length)
        {
            if (GUILayout.Button(cfg.content[i], GUILayout.Width(cfg.buttonWidth), GUILayout.Height(cfg.buttonHeight)))
            {
                clicked = i;
            }
            i++;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Balance: $" + cfg.store.getBalance());
        GUILayout.Space(5);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        string msg = null;
        if (((HeaderModes) cfg.selection) == HeaderModes.Buy)
        {
            msg = "Owned";
        }
        if (((HeaderModes) cfg.selection) == HeaderModes.Equip)
        {
            msg = "Equipped";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label(cfg.checkedIcon, GUILayout.Width(40), GUILayout.Height(20));
        Rect r = GUILayoutUtility.GetLastRect();
        r.x = r.x + 25;
        r.width = r.width + 15;
        GUI.Label(r, msg);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label(cfg.lockedIcon, GUILayout.Width(40), GUILayout.Height(20));
        r = GUILayoutUtility.GetLastRect();
        r.x = r.x + 25;
        r.width = r.width + 10;
        GUI.Label(r, "Locked");
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        return clicked;
    }

    public static int DisplayLS(DBStorePanel cfg)
    {
        // function to display Left Selection Box
        int clicked = -1; //local variable to keep track of selections
        int startInt = (cfg.page - 1) * cfg.buttonsPerPage;
        int endInt = Mathf.Min(startInt + cfg.buttonsPerPage, cfg.content.Length);
        GUILayout.BeginArea(cfg.r);
        GUI.Box(new Rect(0, 0, cfg.r.width, cfg.r.height), "");
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(cfg.title, GUILayout.Height(cfg.titleHeight));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        int i = startInt;
        while (i < endInt)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (cfg.contentIcon[i])
            {
                if (GUILayout.Button(cfg.contentIcon[i], GUILayout.Width(cfg.buttonWidth), GUILayout.Height(cfg.buttonHeight)))
                {
                    clicked = i;
                    cfg.selection = i;
                }
            }
            else
            {
                if (GUILayout.Button(cfg.content[i], GUILayout.Width(cfg.buttonWidth), GUILayout.Height(cfg.buttonHeight)))
                {
                    clicked = i;
                    cfg.selection = i;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            i++;
        }
        GUILayout.EndVertical();
        float bx = (cfg.r.width - cfg.navButtonWidth) - cfg.xPad;
        float by = (cfg.r.height - cfg.navButtonHeight) - cfg.yPad;
        if (cfg.page < cfg.nPages)
        {
            if (cfg.nextIcon)
            {
                if (GUI.Button(new Rect(bx, by, cfg.navButtonWidth, cfg.navButtonHeight), cfg.nextIcon))
                {
                    cfg.page++;
                }
            }
            else
            {
                if (GUI.Button(new Rect(bx, by, cfg.navButtonWidth, cfg.navButtonHeight), "next"))
                {
                    cfg.page++;
                }
            }
        }
        if (cfg.page > 1)
        {
            bx = cfg.xPad / 2f;
            if (cfg.prevIcon)
            {
                if (GUI.Button(new Rect(bx, by, cfg.navButtonWidth, cfg.navButtonHeight), cfg.prevIcon))
                {
                    cfg.page--;
                }
            }
            else
            {
                if (GUI.Button(new Rect(bx, by, cfg.navButtonWidth, cfg.navButtonHeight), "Prev"))
                {
                    cfg.page--;
                }
            }
        }
        GUILayout.EndArea();
        return clicked;
    }

    public static int DisplayMD(DBStorePanel cfg, int sel, HeaderModes mode)
    {
        int clicked = -1;
        string msg = null;
        GUIContent gc = null; //used to hold temporary string messages
        GUILayout.BeginArea(cfg.r);
        GUI.Box(new Rect(0, 0, cfg.r.width, cfg.r.height), "");
        if (cfg.content == null)
        {
            GUILayout.EndArea();
            return clicked;
        }
        GUILayout.BeginVertical();
        GUILayout.Space(cfg.yPad);
        if (cfg.content.Length == 0)
        {
            DBStoreGUI.DrawLabelHCenter("Slot: " + cfg.store.slotInfo.slotName[sel]);
            GUILayout.Space(20);
            DBStoreGUI.DrawLabelHCenter("You Don't Own any Weapons For This Slot");
            GUILayout.EndVertical();
            GUILayout.EndArea();
            return clicked;
        }
        else
        {
            if (mode == HeaderModes.Equip)
            {
                DBStoreGUI.DrawLabelHCenter("Slot: " + cfg.store.slotInfo.slotName[sel]);
            }
        }
        int count = 0;
        int cl = -1;
        int i = 0;
        while (i < cfg.rows)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(cfg.xPad);
            int j = 0;
            while (j < cfg.columns)
            {
                if (count >= cfg.content.Length)
                {
                    break;
                }
                if ((HeaderModes.Buy != HeaderModes.Buy) && cfg.wi[count].owned)
                {
                    msg = cfg.content[count];
                }
                else
                {
                    msg = (cfg.content[count] + "\n$") + cfg.wi[count].buyPrice;
                }
                gc = DBStoreGUI.setGUIContent(cfg.wi[count].icon, msg, "");
                if (GUILayout.Button(gc, GUILayout.Width(cfg.buttonWidth), GUILayout.Height(cfg.buttonHeight)))
                {
                    clicked = count;
                }
                // now draw the overlay icons if the weapon is owned or locked when in buy mode
                // and equipped in slot in equip mode
                Rect r = GUILayoutUtility.GetLastRect();
                if (mode == HeaderModes.Equip)
                {
                    if ((cfg.store.playerW.weapons[sel] == cfg.wi[count].gameObject) && cfg.checkedIcon)
                    {
                        GUI.Label(r, cfg.checkedIcon);
                    }
                }
                else
                {
                    if (cfg.wi[count].owned && cfg.checkedIcon)
                    {
                        GUI.Label(r, cfg.checkedIcon);
                    }
                    if (cfg.wi[count].locked && cfg.lockedIcon)
                    {
                        GUI.Label(r, cfg.lockedIcon);
                    }
                }
                count++;
                j++;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            i++;
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
        return clicked;
    }

    // This function draws the popup window to buy or equip a weapon.
    public virtual void WeaponBuyWindow(int windowID)
    {
        bool before = false;
        WeaponInfo g = null;
        string msg = null;
        Rect rLeft = new Rect(5, 20, (this.popupRect.width / 2) - 7, this.popupRect.height - 25);
        Rect rRight = new Rect((this.popupRect.width / 2) + 2, 20, (this.popupRect.width / 2) - 7, this.popupRect.height - 25);
        int slot = this.lsEquip.selection;
        if (((HeaderModes) this.header.selection) == HeaderModes.Buy) // only used for equiping
        {
            g = this.mdBuy.wi[this.mdBuy.selection];
        }
        else
        {
            g = this.mdEquip.wi[this.mdEquip.selection];
        }
        Upgrade[] upgrades = g.getUpgrades();
        //var upgradeToggles : boolean[] = new boolean[upgrades.length];
        GUI.Box(rLeft, "");
        GUILayout.BeginArea(rLeft);
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginVertical();
        GUILayout.Space(5);
        if (((HeaderModes) this.header.selection) == HeaderModes.Buy)
        {
            msg = this.getBuyMsg(g);
        }
        else
        {
            msg = this.getEquipMsg(g, slot);
        }
        GUILayout.Label(msg);
        GUILayout.Label("Available Balance = $" + this.store.getBalance());
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (((HeaderModes) this.header.selection) == HeaderModes.Equip)
        {
            if (this.store.playerW.weapons[slot] != g.gameObject)
            {
                if (GUILayout.Button("Equip", GUILayout.Width(65)))
                {
                    this.store.equipWeapon(g, slot);
                }
            }
            else
            {
                if (GUILayout.Button("Un-Equip", GUILayout.Width(65)))
                {
                    this.store.unEquipWeapon(g, slot);
                }
            }
            if (!g.gun.infiniteAmmo)
            {
                if (g.gun.clips < g.gun.maxClips)
                {
                    if (this.store.getBalance() >= g.ammoPrice)
                    {
                        if (GUILayout.Button(new GUIContent(("Ammo ($" + g.ammoPrice) + ")", "Current Ammo: " + g.gun.clips), GUILayout.Width(85)))
                        {
                            this.store.BuyAmmo(g);
                        }
                    }
                    else
                    {
                        GUILayout.Button(new GUIContent(("Ammo ($" + g.ammoPrice) + ")", "Can't Afford"), GUILayout.Width(85));
                    }
                }
                else
                {
                    GUILayout.Button(new GUIContent(("Ammo ($" + g.ammoPrice) + ")", "Ammo Full: " + g.gun.clips), GUILayout.Width(85));
                }
            }
        }
        else
        {
            if (g.owned)
            {
                if (g.canBeSold)
                {
                    if (GUILayout.Button("Sell", GUILayout.Width(70)))
                    {
                        this.store.sellWeapon(g);
                    }
                }
                else
                {
                    GUILayout.Button("Can't Sell", GUILayout.Width(70));
                }
                if (!g.gun.infiniteAmmo)
                {
                    if (g.gun.clips < g.gun.maxClips)
                    {
                        if (this.store.getBalance() >= g.ammoPrice)
                        {
                            if (GUILayout.Button(new GUIContent(("Ammo ($" + g.ammoPrice) + ")", "Current Ammo: " + g.gun.clips), GUILayout.Width(85)))
                            {
                                this.store.BuyAmmo(g);
                            }
                        }
                        else
                        {
                            GUILayout.Button(new GUIContent(("Ammo ($" + g.ammoPrice) + ")", "Can't Afford"), GUILayout.Width(85));
                        }
                    }
                    else
                    {
                        GUILayout.Button(new GUIContent(("Ammo ($" + g.ammoPrice) + ")", "Ammo Full: " + g.gun.clips), GUILayout.Width(85));
                    }
                }
            }
            else
            {
                if (this.store.getBalance() >= g.buyPrice)
                {
                    if (GUILayout.Button("Buy", GUILayout.Width(70)))
                    {
                        this.store.buyWeapon(g);
                    }
                }
                else
                {
                    GUILayout.Button(new GUIContent("Buy", "Insufficient Funds"), GUILayout.Width(70));
                }
            }
        }
        if (GUILayout.Button("Close", GUILayout.Width(70)))
        {
            this.MDSelection = -1;
            this.popupActive = false;
            this.popupBuyActive = false;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label(GUI.tooltip);
        GUILayout.EndVertical();
        GUILayout.Space(5);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        // Display for upgrades in the right side of the window
        // The display changes for buying upgrades or equipping upgrades.
        GUI.Box(rRight, "");
        GUILayout.BeginArea(rRight);
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginVertical();
        GUILayout.Space(5);
        if (upgrades == null)
        {
            GUILayout.Label("No Upgrades Available for this Weapon");
        }
        else
        {
            if (upgrades.Length < 0)
            {
                GUILayout.Label("No Upgrades Available for this Weapon");
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                this.upgradeDisplayBuy = GUILayout.Toggle(this.upgradeDisplayBuy, "Buy Upgrades");
                if (this.upgradeDisplayBuy)
                {
                    this.upgradeDisplayEquip = false;
                }
                this.upgradeDisplayEquip = GUILayout.Toggle(this.upgradeDisplayEquip, "Equip Upgrades");
                if (this.upgradeDisplayEquip)
                {
                    this.upgradeDisplayBuy = false;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                if (this.upgradeDisplayBuy)
                {
                    GUILayout.Space(5);
                    string upgradeMsg = null;
                    if (g.owned)
                    {
                        upgradeMsg = "View or Buy Upgrades";
                    }
                    else
                    {
                        upgradeMsg = "You must Purchase the Weapon before buying Upgrades";
                    }
                    GUILayout.Label(upgradeMsg);
                    this.popupBuyScroll1 = GUILayout.BeginScrollView(this.popupBuyScroll1);
                    if (!(upgrades == null))
                    {
                        int i = 0;
                        while (i < upgrades.Length)
                        {
                            if (!upgrades[i].owned)
                            {
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(upgrades[i].upgradeName);
                                GUILayout.Space(5);
                                if (GUILayout.Button(DBStoreGUI.setGUIContent(this.upgradeInfoIcon, "View", ""), GUILayout.Width(40), GUILayout.Height(20)))
                                {
                                    if (this.upgradeSelected[i])
                                    {
                                        this.upgradeSelected[i] = false;
                                    }
                                    else
                                    {
                                        this.upgradeSelected[i] = true;
                                    }
                                }
                                GUILayout.Space(5);
                                GUIContent gc = null;
                                if (upgrades[i].locked)
                                {
                                    GUILayout.Label("(Locked)");
                                    upgradeMsg = upgrades[i].lockedDescription;
                                }
                                else
                                {
                                    if (g.owned)
                                    {
                                        float balance = this.store.getBalance();
                                        if (balance > upgrades[i].buyPrice)
                                        {
                                            gc = DBStoreGUI.setGUIContent(this.upgradeBuyIcon, "Buy", "");
                                        }
                                        else
                                        {
                                            gc = new GUIContent(this.upgradeBuyIcon, "Insufficient Funds");
                                        }
                                        if (GUILayout.Button(gc, GUILayout.Width(40), GUILayout.Height(20)))
                                        {
                                            if (balance > upgrades[i].buyPrice)
                                            {
                                                this.store.buyUpgrade(g, upgrades[i]);
                                            }
                                        }
                                    }
                                }
                                upgradeMsg = ("Price :\t$" + upgrades[i].buyPrice) + "\n";
                                upgradeMsg = upgradeMsg + ("Description:\t" + upgrades[i].description);
                                GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                                if (this.upgradeSelected[i])
                                {
                                    this.closeOtherSelections(i);
                                    GUILayout.BeginHorizontal();
                                    GUILayout.Space(10);
                                    GUILayout.BeginVertical();
                                    GUILayout.Label(upgradeMsg);
                                    GUILayout.EndVertical();
                                    GUILayout.EndHorizontal();
                                }
                            }
                            i++;
                        }
                    }
                     // upgrade not owned
                     // loop over upgrades
                    GUILayout.EndScrollView();
                    GUILayout.Label(GUI.tooltip);
                }
                else
                {
                     // Displaying Equip Window
                    GUILayout.Space(5);
                    GUILayout.Label("Select Upgrades To Apply - Note: Some upgrades disable others");
                    //			var upgradesApplied : boolean[] = g.getUpgradesApplied();
                    this.popupBuyScroll1 = GUILayout.BeginScrollView(this.popupBuyScroll1);
                    int j = 0;
                    while (j < upgrades.Length)
                    {
                        if (upgrades[j].owned)
                        {
                            before = g.upgradesApplied[j]; // keep track of current state
                            g.upgradesApplied[j] = GUILayout.Toggle(g.upgradesApplied[j], upgrades[j].upgradeName);
                            if (before != g.upgradesApplied[j])
                            {
                                if (before)
                                {
                                    upgrades[j].RemoveUpgrade();
                                }
                                else
                                {
                                    upgrades[j].ApplyUpgrade();
                                    PlayerWeapons.HideWeaponInstant(); //turn off graphics for applied upgrade
                                }
                                g.updateApplied();
                            }
                        }
                        j++;
                    }
                    GUILayout.EndScrollView();
                    GUILayout.Space(8);
                }
            }
        }
         // Displaying Equip Window
         //upgrades.length !=0
        GUILayout.EndVertical();
        GUILayout.Space(5);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    //This function displays popup messages, including the weapon locked message.
    public virtual void WeaponLockedWindow(int windowID)
    {
        WeaponInfo g = this.mdBuy.wi[this.mdBuy.selection];
        GUILayout.BeginArea(new Rect(5, 10, this.popupLockedRect.width - 10, this.popupLockedRect.height - 20));
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label(g.lockedDescription);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Close"))
        {
            this.MDSelection = -1;
            this.popupActive = false;
            this.popupLockedActive = false;
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    // This function is used to make the GUILayout.Toggle() function act like a togglegroup for the Buy/Equip popup window.
    public virtual void closeOtherSelections(int sel)
    {
        int i = 0;
        while (i < this.upgradeSelected.Length)
        {
            if (i != sel)
            {
                this.upgradeSelected[i] = false;
            }
            i++;
        }
    }

    //the following two functions are used just to cleanup the logic in the Buy/Upgrade popup window
    public virtual string getBuyMsg(WeaponInfo g)
    {
        string msg = null;
        msg = ("Weapon Name: " + g.gunName) + "\n\n";
        if (g.owned)
        {
            msg = msg + "Weapon Not Owned\n";
        }
        else
        {
            msg = msg + "Weapon Owned\n";
        }
        msg = msg + (("Description: " + g.gunDescription) + "\n\n");
        if (g.owned)
        {
            msg = msg + (("Sell Price: $" + g.sellPriceUpgraded) + "\n");
        }
        else
        {
            msg = msg + ("Price: $" + g.buyPrice);
        }
        return msg;
    }

    public virtual string getEquipMsg(WeaponInfo g, int slot)
    {
        string msg = null;
        msg = ("Equipping for Slot " + slot) + " \n";
        msg = msg + (("Weapon Name " + g.gunName) + "\n");
        if ((this.store.playerW.weapons[slot] as WeaponInfo) == g.gameObject)
        {
            msg = msg + "Weapon Equiped in Slot\n";
        }
        else
        {
            msg = msg + "Weapon not Equiped in Slot\n";
        }
        msg = msg + (("Description: " + g.gunDescription) + "\n");
        if (g.owned)
        {
            msg = msg + (("Sell Price: $" + g.sellPriceUpgraded) + "\n");
        }
        else
        {
            msg = msg + ("Price: $" + g.buyPrice);
        }
        return msg;
    }

    //This utility function just keeps the code a little simpler. It centers a GUILayout Label Horizontally
    public static void DrawLabelHCenter(string s)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(s);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    // Utility function to create GUIContent
    // If texture existing dont set the string value
    // If tool tip is not null set it
    public static GUIContent setGUIContent(Texture t, string label, string tip)
    {
        GUIContent gc = null;
        if (tip != "")
        {
            if (t)
            {
                gc = new GUIContent(t, tip);
            }
            else
            {
                gc = new GUIContent(label, tip);
            }
        }
        else
        {
            if (t)
            {
                gc = new GUIContent(t);
            }
            else
            {
                gc = new GUIContent(label);
            }
        }
        return gc;
    }

    public DBStoreGUI()
    {
        this.slotIcons = new Texture[10];
        this.sPosX = -1;
        this.sPosY = -1;
        this.HWidth = 400;
        this.HHeight = 100;
        this.HYpad = 10;
        this.HButtonWidth = 120;
        this.HButtonHeight = 50;
        this.HTitleHeight = 40;
        this.HTitle = "WEAPONS DEPOT";
        this.LSHeight = 300;
        this.LSWidth = 140;
        this.LSButtonHeight = 50f;
        this.LSButtonWidth = 120;
        this.LSxPad = 5;
        this.LSyPad = 5;
        this.MDButtonWidth = 120;
        this.MDButtonHeight = 50f;
        this.MDxPad = 5;
        this.MDyPad = 5;
        this.header = new DBStorePanel();
        this.lsBuy = new DBStorePanel();
        this.lsEquip = new DBStorePanel();
        this.mdBuy = new DBStorePanel();
        this.mdEquip = new DBStorePanel();
        this.viewUpgrades = true;
        this.popupUpgradeWidth = 500;
        this.popupUpgradeHeight = 275;
        this.upgradeSelected = new bool[20];
        this.upgradeDisplayBuy = true;
    }

}