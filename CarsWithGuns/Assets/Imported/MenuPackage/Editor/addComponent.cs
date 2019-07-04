using UnityEngine;
using UnityEditor;
using System.Collections;

public class addComponent : ScriptableWizard
{
	public bool attachScript;
	public MonoScript scriptToBeAttached;
	static Transform[] transforms;
	public bool attachAudioSource=false;
	public audioComp AudioProp;
	public bool attachCollider=false;
	public colliderComp ColliderProp;
	public bool attachRigidbody=false;
	public addRigidbody AddRigidbody;
	
	
	[MenuItem("Tools/Ravinder/Add Component",false)]
	static void CreateWindow()
	{
		transforms= Selection.GetTransforms(SelectionMode.OnlyUserModifiable);
		ScriptableWizard.DisplayWizard("Add Different Components",typeof(addComponent),"Add");
	}
	
	void OnWizardCreate ()
	{
       foreach(Transform transform in transforms)
		{
			if(attachAudioSource)
			{
				transform.gameObject.AddComponent(typeof(AudioSource));
				//Properties set for AudioSource
				#region
				AudioSource source=transform.gameObject.GetComponent(typeof(AudioSource))as AudioSource;
				source.priority=AudioProp.priority;
				source.mute=AudioProp.mute;
				source.minDistance=AudioProp.minDistance;
				source.maxDistance=AudioProp.maxDistance;
				source.volume=AudioProp.volume;
				source.playOnAwake=AudioProp.playOnAwake;
				source.clip=AudioProp.clipToBePlayed;
				source.bypassEffects=AudioProp.byPassEffects;
				source.pitch=AudioProp.pitch;
				#endregion
			}
			if(attachCollider)
			{
				UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(transform.gameObject, "Assets/MenuPackage/Editor/addComponent.cs (48,5)", ColliderProp.typeOfCollider.ToString());
				transform.GetComponent<Collider>().isTrigger=ColliderProp.isTrigger;
				
				//Properties set for Individual Colliders
				#region
				switch(ColliderProp.typeOfCollider.ToString())
				{
					case "BoxCollider": BoxCollider bc= transform.GetComponent(typeof(BoxCollider))as BoxCollider;
										bc.center=ColliderProp.center;
										bc.size=ColliderProp.boxColProp.size;
										break;
					case "CapsuleCollider": CapsuleCollider cap= transform.GetComponent(typeof(CapsuleCollider))as CapsuleCollider;
										cap.center=ColliderProp.center;
										cap.radius=ColliderProp.radius;
										cap.height=ColliderProp.height;
										break;
					case "SphereCollider": SphereCollider sph= transform.GetComponent(typeof(SphereCollider))as SphereCollider;
										sph.center=ColliderProp.center;
										sph.radius=ColliderProp.radius;
										break;
					case "MeshCollider": MeshCollider mc= transform.GetComponent(typeof(MeshCollider))as MeshCollider;
										mc.smoothSphereCollisions=ColliderProp.meshColProp.smoothSphereCollisions;
										mc.convex=ColliderProp.meshColProp.convex;
										if(ColliderProp.meshColProp.mesh)
											mc.sharedMesh=ColliderProp.meshColProp.mesh;
										break;
				}
				#endregion
			}
			
			if(attachRigidbody)
			{
				//Properties set for rigidbody
				#region
				transform.gameObject.AddComponent(typeof(Rigidbody));
				transform.GetComponent<Rigidbody>().mass=AddRigidbody.mass;
				transform.GetComponent<Rigidbody>().drag=AddRigidbody.drag;
				transform.GetComponent<Rigidbody>().angularDrag= AddRigidbody.angularDrag;
				transform.GetComponent<Rigidbody>().useGravity=AddRigidbody.useGravity;
				transform.GetComponent<Rigidbody>().isKinematic=AddRigidbody.isKinematic;
				transform.GetComponent<Rigidbody>().interpolation=AddRigidbody.interpolate;
				transform.GetComponent<Rigidbody>().collisionDetectionMode=AddRigidbody.collisionDetection;
				transform.GetComponent<Rigidbody>().constraints=AddRigidbody.constraints;
				#endregion
			}
			
			if(attachScript)
			{
				UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(transform.gameObject, "Assets/MenuPackage/Editor/addComponent.cs (96,5)", scriptToBeAttached.name);
			}
		}
	}
	
	
	
	
	
	void OnWizardUpdate()
	{
	}
	
	
}

//This class exposes various properties to be set for a Rigidbody.
// The properties exposed here are those which are often used. Ofcourse you could add any property you want to be exposed
//and then assign that property in OnWizard Create under attachRigidbody.


[System.Serializable]
public class addRigidbody
{
	public float mass=1.0f;
	public float drag=0;
	public float angularDrag=0.05f;
	public bool useGravity=true;
	public bool isKinematic=false;
	public RigidbodyInterpolation interpolate=RigidbodyInterpolation.None;
	public CollisionDetectionMode collisionDetection=CollisionDetectionMode.Discrete;
	public RigidbodyConstraints constraints=RigidbodyConstraints.None;
}


//This class exposes various properties to be set for an AudioSource.
// The properties exposed here are those which are often used. Ofcourse you could add any property you want to be exposed
//and then assign that property in OnWizard Create under attachAudioSource.


[System.Serializable]
public class audioComp
{
	public AudioClip clipToBePlayed;
	public bool mute=false;
	public int priority=128;
	public int minDistance=1;
	public int maxDistance=500;
	public bool playOnAwake=true;
	public bool byPassEffects=false;
	public float volume=1.0f;
	public float pitch=1.0f;
}
//This class exposes various properties to be set for a Collider.
// The properties exposed here are those which are often used. Ofcourse you could add any property you want to be exposed
//and then assign that property in OnWizard Create under attachCollider.

[System.Serializable]
public class colliderComp
{
	
	public enum collider
	{
		BoxCollider=0,
		SphereCollider=1,
		CapsuleCollider=2,
		MeshCollider=3,
		WheelCollider=4,
	};
	
	public collider typeOfCollider=collider.BoxCollider;
	
	public PhysicMaterial material;
	public bool isTrigger;
	public float radius;
	public float height;
	public Vector3 center;
	
	
	public BoxColliderProp boxColProp;//Individual Properties exposed for BoxCollider
	public MeshColliderProp meshColProp;//Individual Properties exposed for MeshCollider
	public WheelColliderProp wheelColProp;//Individual Properties exposed for WheelCollider
	
}

[System.Serializable]
public class BoxColliderProp
{
	public Vector3 size=new Vector3(1,1,1);
}

[System.Serializable]
public class WheelColliderProp
{
	public Vector3 size=new Vector3(1,1,1);
}

[System.Serializable]
public class MeshColliderProp
{
	public bool smoothSphereCollisions=false;
	public bool convex=false;
	public Mesh mesh;
}
