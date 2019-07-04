using UnityEngine;
using UnityEditor;
using System.Collections;

public class changeTag : ScriptableWizard
{
	
	public string newTag="Write tag name";
	static Transform[] transforms;
	
	[MenuItem("Tools/Ravinder/Change Tag",false)]
	static void CreateWindow()
	{
		transforms= Selection.GetTransforms(SelectionMode.OnlyUserModifiable);
		ScriptableWizard.DisplayWizard("Change tag of the objects",typeof(changeTag),"Change Tag");
	}
	
	void OnWizardCreate ()
	{
        foreach(Transform transform in transforms)
		{
			transform.tag=newTag;
		}
	}
	void OnWizardUpdate()
	{	
		
	}		
}