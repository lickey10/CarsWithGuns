using UnityEngine;
using UnityEditor;
using System.Collections;

public class changeDepth : ScriptableWizard
{
	
	public float new_Z_Position=0;
	static Transform[] transforms;
	
	[MenuItem("Tools/Ravinder/Change Depth",false)]
	static void CreateWindow()
	{
		//finding all the transform from the selection
		transforms= Selection.GetTransforms(SelectionMode.OnlyUserModifiable);
		ScriptableWizard.DisplayWizard("Change Z of the objects",typeof(changeDepth),"ChangeDepth");
	}
	
	void OnWizardCreate ()
	{
        foreach(Transform transform in transforms)
		{
			Transform thisObject = transform;
			Vector3 temp=thisObject.position;
			temp.z=new_Z_Position;//changing the value of z for each transform selected
			thisObject.position=temp;
		}
	}
	void OnWizardUpdate()
	{	
		
	}		
}