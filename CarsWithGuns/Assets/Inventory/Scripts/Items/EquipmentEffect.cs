using UnityEngine;
using System.Collections;

[System.Serializable]
//This script allows you to create equipment effects that will be called either OnEquip or WhileEquipped. This is usefull for magic effects and stat handling.
[UnityEngine.AddComponentMenu("Inventory/Items/Equipment Effect")]
[UnityEngine.RequireComponent(typeof(Item))]
public partial class EquipmentEffect : MonoBehaviour
{
    private bool effectActive;
    public virtual void Update() //-----> THIS IS WHERE YOU INSERT CODE YOU WANT TO EXECUTE AS LONG AS THE ITEM IS EQUIPPED. <-----
    {
        if (this.effectActive == true)
        {
        }
    }

    public virtual void EquipmentEffectToggle(bool effectIs)//-----> THIS IS WHERE YOU INSERT CODE YOU WANT TO EXECUTE JUST WHEN THE ITEM IS UNEQUIPPED. <-----
    {
        if (effectIs == true)
        {
            this.effectActive = true;
            Debug.LogWarning(("Remember to insert code for the EquipmentEffect script you have attached to " + this.transform.name) + ".");
        }
        else
        {
            //-----> THIS IS WHERE YOU INSERT CODE YOU WANT TO EXECUTE JUST WHEN THE ITEM IS EQUIPPED. <-----
            this.effectActive = false;
        }
    }

}