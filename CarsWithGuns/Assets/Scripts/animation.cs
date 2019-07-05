using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class animation : MonoBehaviour
{
    public virtual IEnumerator OnMouseEnter()
    {
        this.GetComponent<Animation>().Play("open");
        yield return new WaitForSeconds(1.75f);
        this.GetComponent<Animation>().Play("close");
    }

}