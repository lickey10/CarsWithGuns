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
public class EffectsManagerEditor : Editor
{
    public string[] nameArray;
    public virtual void OnEnable()
    {
        if (this.target.highestSet == 0)
        {
            this.target.CreateSet();
        }
        this.updateNameArray();
    }

    public virtual void updateNameArray()
    {
        this.nameArray = new string[((int) this.target.setNameArray.length) + 1];
        i = 0;
        while (i < (this.nameArray.Length - 1))
        {
            this.nameArray[i] = this.target.setNameArray[i];
            i++;
        }
        this.nameArray[this.nameArray.Length - 1] = "New Set";
    }

    public override void OnInspectorGUI()
    {
        bool markForDelete = false;
        EditorGUIUtility.LookLikeInspector();
        EditorGUILayout.BeginVertical();
        this.target.maxDecals = EditorGUILayout.IntField("  Max. Decals in World: ", (int) this.target.maxDecals);
        if (this.target.highestSet > 0)
        {
            this.target.selectedSet = EditorGUILayout.Popup("  Select Set to Edit: ", (int) this.target.selectedSet, this.nameArray);
            if (this.target.selectedSet == (this.nameArray.Length - 1))
            {
                if (this.target.highestSet < this.target.maxSets)
                {
                    this.target.CreateSet();
                    this.updateNameArray();
                }
                else
                {
                    Debug.LogWarning("Effects Set not created - too many decal sets exist already!");
                }
            }
            string setName = this.target.setNameArray[this.target.selectedSet];
            //EditorGUILayout.Separator();
            setName = EditorGUILayout.TextField("  Effects Set Name:  ", setName);
            if (!(this.target.setNameArray[this.target.selectedSet] == setName))
            {
                this.target.Rename(setName);
                this.updateNameArray();
            }
            EditorGUILayout.Separator();
            if (GUILayout.Button("Delete Set"))
            {
                markForDelete = true;
            }
            EditorGUILayout.Separator();
            if (this.target.setArray.length > 0)
            {
                //Bullet Decals
                this.target.setArray[this.target.selectedSet].bulletDecalsFolded = EditorGUILayout.Foldout(this.target.setArray[this.target.selectedSet].bulletDecalsFolded, "  Bullet Decals: ");
                if (this.target.setArray[this.target.selectedSet].bulletDecalsFolded != null)
                {
                    int i = 0;
                    while (i < this.target.setArray[this.target.selectedSet].bulletDecals.length)
                    {
                        this.target.setArray[this.target.selectedSet].bulletDecals[i] = EditorGUILayout.ObjectField("  Bullet Decal: ", this.target.setArray[this.target.selectedSet].bulletDecals[i], typeof(GameObject), false);
                        if (this.target.setArray[this.target.selectedSet].bulletDecals[i] == null)
                        {
                            if (i < this.target.setArray[this.target.selectedSet].bulletDecals.length)
                            {
                                if (this.target.setArray[this.target.selectedSet].bulletDecals[i + 1] == null)
                                {
                                    this.target.setArray[this.target.selectedSet].lastBulletDecal = i;
                                    break;
                                }
                                else
                                {
                                    int m = i + 1;
                                    while (m < this.target.setArray[this.target.selectedSet].bulletDecals.length)
                                    {
                                        if (this.target.setArray[this.target.selectedSet].bulletDecals[m] == null)
                                        {
                                            this.target.setArray[this.target.selectedSet].bulletDecals[m - 1] = this.target.setArray[this.target.selectedSet].bulletDecals[m];
                                            break;
                                        }
                                        else
                                        {
                                            this.target.setArray[this.target.selectedSet].bulletDecals[m - 1] = this.target.setArray[this.target.selectedSet].bulletDecals[m];
                                        }
                                        m++;
                                    }
                                }
                            }
                        }
                        i++;
                    }
                }
                //Dent Decals
                this.target.setArray[this.target.selectedSet].dentDecalsFolded = EditorGUILayout.Foldout(this.target.setArray[this.target.selectedSet].dentDecalsFolded, "  Dent Decals: ");
                if (this.target.setArray[this.target.selectedSet].dentDecalsFolded != null)
                {
                    i = 0;
                    while (i < this.target.setArray[this.target.selectedSet].dentDecals.length)
                    {
                        this.target.setArray[this.target.selectedSet].dentDecals[i] = EditorGUILayout.ObjectField("  Dent Decal: ", this.target.setArray[this.target.selectedSet].dentDecals[i], typeof(GameObject), false);
                        if (this.target.setArray[this.target.selectedSet].dentDecals[i] == null)
                        {
                            if (i < this.target.setArray[this.target.selectedSet].dentDecals.length)
                            {
                                if (this.target.setArray[this.target.selectedSet].dentDecals[i + 1] == null)
                                {
                                    this.target.setArray[this.target.selectedSet].lastDentDecal = i;
                                    break;
                                }
                                else
                                {
                                    m = i + 1;
                                    while (m < this.target.setArray[this.target.selectedSet].dentDecals.length)
                                    {
                                        if (this.target.setArray[this.target.selectedSet].dentDecals[m] == null)
                                        {
                                            this.target.setArray[this.target.selectedSet].dentDecals[m - 1] = this.target.setArray[this.target.selectedSet].dentDecals[m];
                                            break;
                                        }
                                        else
                                        {
                                            this.target.setArray[this.target.selectedSet].dentDecals[m - 1] = this.target.setArray[this.target.selectedSet].dentDecals[m];
                                        }
                                        m++;
                                    }
                                }
                            }
                        }
                        i++;
                    }
                }
                //Hit Particles
                this.target.setArray[this.target.selectedSet].hitParticlesFolded = EditorGUILayout.Foldout(this.target.setArray[this.target.selectedSet].hitParticlesFolded, "  Hit Particles: ");
                if (this.target.setArray[this.target.selectedSet].hitParticlesFolded != null)
                {
                    i = 0;
                    while (i < this.target.setArray[this.target.selectedSet].hitParticles.length)
                    {
                        this.target.setArray[this.target.selectedSet].hitParticles[i] = EditorGUILayout.ObjectField("  Hit Particle: ", this.target.setArray[this.target.selectedSet].hitParticles[i], typeof(GameObject), false);
                        if (this.target.setArray[this.target.selectedSet].hitParticles[i] == null)
                        {
                            if (i < this.target.setArray[this.target.selectedSet].hitParticles.length)
                            {
                                if (this.target.setArray[this.target.selectedSet].hitParticles[i + 1] == null)
                                {
                                    this.target.setArray[this.target.selectedSet].lastHitParticle = i;
                                    break;
                                }
                                else
                                {
                                    m = i + 1;
                                    while (m < this.target.setArray[this.target.selectedSet].hitParticles.length)
                                    {
                                        if (this.target.setArray[this.target.selectedSet].hitParticles[m] == null)
                                        {
                                            this.target.setArray[this.target.selectedSet].hitParticles[m - 1] = this.target.setArray[this.target.selectedSet].hitParticles[m];
                                            break;
                                        }
                                        else
                                        {
                                            this.target.setArray[this.target.selectedSet].hitParticles[m - 1] = this.target.setArray[this.target.selectedSet].hitParticles[m];
                                        }
                                        m++;
                                    }
                                }
                            }
                        }
                        i++;
                    }
                }
                //Bullet Sounds
                this.target.setArray[this.target.selectedSet].bulletSoundsFolded = EditorGUILayout.Foldout(this.target.setArray[this.target.selectedSet].bulletSoundsFolded, "  Bullet Sounds: ");
                if (this.target.setArray[this.target.selectedSet].bulletSoundsFolded != null)
                {
                    i = 0;
                    while (i < this.target.setArray[this.target.selectedSet].bulletSounds.length)
                    {
                        this.target.setArray[this.target.selectedSet].bulletSounds[i] = EditorGUILayout.ObjectField("  Bullet Sound: ", this.target.setArray[this.target.selectedSet].bulletSounds[i], typeof(AudioClip), false);
                        if (this.target.setArray[this.target.selectedSet].bulletSounds[i] == null)
                        {
                            if (i < this.target.setArray[this.target.selectedSet].bulletSounds.length)
                            {
                                if (this.target.setArray[this.target.selectedSet].bulletSounds[i + 1] == null)
                                {
                                    this.target.setArray[this.target.selectedSet].lastBulletSound = i;
                                    break;
                                }
                                else
                                {
                                    m = i + 1;
                                    while (m < this.target.setArray[this.target.selectedSet].bulletSounds.length)
                                    {
                                        if (this.target.setArray[this.target.selectedSet].bulletSounds[m] == null)
                                        {
                                            this.target.setArray[this.target.selectedSet].bulletSounds[m - 1] = this.target.setArray[this.target.selectedSet].bulletSounds[m];
                                            break;
                                        }
                                        else
                                        {
                                            this.target.setArray[this.target.selectedSet].bulletSounds[m - 1] = this.target.setArray[this.target.selectedSet].bulletSounds[m];
                                        }
                                        m++;
                                    }
                                }
                            }
                        }
                        i++;
                    }
                }
                //Collision Sounds
                this.target.setArray[this.target.selectedSet].collisionSoundsFolded = EditorGUILayout.Foldout(this.target.setArray[this.target.selectedSet].collisionSoundsFolded, "  Collision Sounds: ");
                if (this.target.setArray[this.target.selectedSet].collisionSoundsFolded != null)
                {
                    i = 0;
                    while (i < this.target.setArray[this.target.selectedSet].collisionSounds.length)
                    {
                        this.target.setArray[this.target.selectedSet].collisionSounds[i] = EditorGUILayout.ObjectField("  Collision Sound: ", this.target.setArray[this.target.selectedSet].collisionSounds[i], typeof(AudioClip), false);
                        if (this.target.setArray[this.target.selectedSet].collisionSounds[i] == null)
                        {
                            if (i < this.target.setArray[this.target.selectedSet].collisionSounds.length)
                            {
                                if (this.target.setArray[this.target.selectedSet].collisionSounds[i + 1] == null)
                                {
                                    this.target.setArray[this.target.selectedSet].lastCollisionSound = i;
                                    break;
                                }
                                else
                                {
                                    m = i + 1;
                                    while (m < this.target.setArray[this.target.selectedSet].collisionSounds.length)
                                    {
                                        if (this.target.setArray[this.target.selectedSet].collisionSounds[m] == null)
                                        {
                                            this.target.setArray[this.target.selectedSet].collisionSounds[m - 1] = this.target.setArray[this.target.selectedSet].collisionSounds[m];
                                            break;
                                        }
                                        else
                                        {
                                            this.target.setArray[this.target.selectedSet].collisionSounds[m - 1] = this.target.setArray[this.target.selectedSet].collisionSounds[m];
                                        }
                                        m++;
                                    }
                                }
                            }
                        }
                        i++;
                    }
                }
                //Footstep Sounds
                this.target.setArray[this.target.selectedSet].footstepSoundsFolded = EditorGUILayout.Foldout(this.target.setArray[this.target.selectedSet].footstepSoundsFolded, "  Footstep Sounds: ");
                if (this.target.setArray[this.target.selectedSet].footstepSoundsFolded != null)
                {
                    i = 0;
                    while (i < this.target.setArray[this.target.selectedSet].footstepSounds.length)
                    {
                        this.target.setArray[this.target.selectedSet].footstepSounds[i] = EditorGUILayout.ObjectField("  Footstep Sound: ", this.target.setArray[this.target.selectedSet].footstepSounds[i], typeof(AudioClip), false);
                        if (this.target.setArray[this.target.selectedSet].footstepSounds[i] == null)
                        {
                            if (i < this.target.setArray[this.target.selectedSet].footstepSounds.length)
                            {
                                if (this.target.setArray[this.target.selectedSet].footstepSounds[i + 1] == null)
                                {
                                    this.target.setArray[this.target.selectedSet].lastFootstepSound = i;
                                    break;
                                }
                                else
                                {
                                    m = i + 1;
                                    while (m < this.target.setArray[this.target.selectedSet].footstepSounds.length)
                                    {
                                        if (this.target.setArray[this.target.selectedSet].footstepSounds[m] == null)
                                        {
                                            this.target.setArray[this.target.selectedSet].footstepSounds[m - 1] = this.target.setArray[this.target.selectedSet].footstepSounds[m];
                                            break;
                                        }
                                        else
                                        {
                                            this.target.setArray[this.target.selectedSet].footstepSounds[m - 1] = this.target.setArray[this.target.selectedSet].footstepSounds[m];
                                        }
                                        m++;
                                    }
                                }
                            }
                        }
                        i++;
                    }
                }
                //Crawl Sounds	
                this.target.setArray[this.target.selectedSet].crawlSoundsFolded = EditorGUILayout.Foldout(this.target.setArray[this.target.selectedSet].crawlSoundsFolded, "  Crawl (Prone) Sounds: ");
                if (this.target.setArray[this.target.selectedSet].crawlSoundsFolded != null)
                {
                    i = 0;
                    while (i < this.target.setArray[this.target.selectedSet].crawlSounds.length)
                    {
                        this.target.setArray[this.target.selectedSet].crawlSounds[i] = EditorGUILayout.ObjectField("  Crawl Sound: ", this.target.setArray[this.target.selectedSet].crawlSounds[i], typeof(AudioClip), false);
                        if (this.target.setArray[this.target.selectedSet].crawlSounds[i] == null)
                        {
                            if (i < this.target.setArray[this.target.selectedSet].crawlSounds.length)
                            {
                                if (this.target.setArray[this.target.selectedSet].crawlSounds[i + 1] == null)
                                {
                                    this.target.setArray[this.target.selectedSet].lastCrawlSound = i;
                                    break;
                                }
                                else
                                {
                                    m = i + 1;
                                    while (m < this.target.setArray[this.target.selectedSet].crawlSounds.length)
                                    {
                                        if (this.target.setArray[this.target.selectedSet].crawlSounds[m] == null)
                                        {
                                            this.target.setArray[this.target.selectedSet].crawlSounds[m - 1] = this.target.setArray[this.target.selectedSet].crawlSounds[m];
                                            break;
                                        }
                                        else
                                        {
                                            this.target.setArray[this.target.selectedSet].crawlSounds[m - 1] = this.target.setArray[this.target.selectedSet].crawlSounds[m];
                                        }
                                        m++;
                                    }
                                }
                            }
                        }
                        i++;
                    }
                }
            }
        }
        /*	var crawlSounds : AudioClip[] = new AudioClip[maxOfEach];
	var crawlSoundsFolded : boolean = false;
	var lastCrawlSound : int = 0;*/
        if (markForDelete)
        {
            if (this.target.highestSet == 1)
            {
                Debug.Log("Can't Delete Last Effects Set");
            }
            else
            {
                this.target.DeleteSet(this.target.selectedSet);
                this.updateNameArray();
                this.target.selectedSet = 0;
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(this.target);
        }
    }

    public EffectsManagerEditor()
    {
        this.nameArray = new string[1];
    }

}