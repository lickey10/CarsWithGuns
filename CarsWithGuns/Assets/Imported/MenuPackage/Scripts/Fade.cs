using UnityEngine;
using System.Collections;

public enum EaseType
{
    None = 0,
    In = 1,
    Out = 2,
    InOut = 3
}

[System.Serializable]
public partial class Fade : MonoBehaviour
{
    public static Fade use;
    public virtual void Awake()
    {
        if (Fade.use)
        {
            Debug.LogWarning("Only one instance of the Fade script in a scene is allowed");
            return;
        }
        Fade.use = this;
    }

    public virtual IEnumerator Alpha(object @object, float start, float end, float timer)
    {
        yield return this.StartCoroutine(this.Alpha(@object, start, end, timer, EaseType.None));
    }

    public virtual IEnumerator Alpha(object @object, float start, float end, float timer, EaseType easeType)
    {
        if (!this.CheckType(@object))
        {
            yield break;
        }
        float t = 0f;
        System.Type objectType = GetTypeOf(@object);
        while (t < 1f)
        {
            t = t + (Time.deltaTime * (1f / timer));
            if (objectType == GUITexture)
            {
                (@object as GUITexture).color.a = Mathf.Lerp(start, end, this.Ease(t, easeType)) * 0.5f;
            }
            else
            {
                (@object as Material).color.a = Mathf.Lerp(start, end, this.Ease(t, easeType));
            }
            yield return null;
        }
    }

    public virtual IEnumerator Colors(object @object, Color start, Color end, float timer)
    {
        yield return this.StartCoroutine(this.Colors(@object, start, end, timer, EaseType.None));
    }

    public virtual IEnumerator Colors(object @object, Color start, Color end, float timer, EaseType easeType)
    {
        if (!this.CheckType(@object))
        {
            yield break;
        }
        float t = 0f;
        System.Type objectType = GetTypeOf(@object);
        while (t < 1f)
        {
            t = t + (Time.deltaTime * (1f / timer));
            if (objectType == GUITexture)
            {
                (@object as GUITexture).color = Color.Lerp(start, end, this.Ease(t, easeType)) * 0.5f;
            }
            else
            {
                (@object as Material).color = Color.Lerp(start, end, this.Ease(t, easeType));
            }
            yield return null;
        }
    }

    public virtual IEnumerator Colors(object @object, Color[] colorRange, float timer, bool repeat)
    {
        if (!this.CheckType(@object))
        {
            yield break;
        }
        if (colorRange.Length < 2)
        {
            Debug.LogError("Error: color array must have at least 2 entries");
            yield break;
        }
        timer = timer / colorRange.Length;
        int i = 0;
        System.Type objectType = GetTypeOf(@object);
        while (true)
        {
            float t = 0f;
            while (t < 1f)
            {
                t = t + (Time.deltaTime * (1f / timer));
                if (objectType == GUITexture)
                {
                    (@object as GUITexture).color = Color.Lerp(colorRange[i], colorRange[(i + 1) % colorRange.Length], t) * 0.5f;
                }
                else
                {
                    (@object as Material).color = Color.Lerp(colorRange[i], colorRange[(i + 1) % colorRange.Length], t);
                }
                yield return null;
            }
            i = ++i % colorRange.Length;
            if (!repeat && (i == 0))
            {
                break;
            }
        }
    }

    private float Ease(float t, EaseType easeType)
    {
        if (easeType == EaseType.None)
        {
            return t;
        }
        else
        {
            if (easeType == EaseType.In)
            {
                return Mathf.Lerp(0f, 1f, 1f - Mathf.Cos((t * Mathf.PI) * 0.5f));
            }
            else
            {
                if (easeType == EaseType.Out)
                {
                    return Mathf.Lerp(0f, 1f, Mathf.Sin((t * Mathf.PI) * 0.5f));
                }
                else
                {
                    return Mathf.SmoothStep(0f, 1f, t);
                }
            }
        }
    }

    private bool CheckType(object @object)
    {
        if ((GetTypeOf(@object) != GUITexture) && (GetTypeOf(@object) != Material))
        {
            Debug.LogError(("Error: object is a " + GetTypeOf(@object)) + ". It must be a GUITexture or a Material");
            return false;
        }
        return true;
    }

}