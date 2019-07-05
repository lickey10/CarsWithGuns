using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class Outliner2 : MonoBehaviour {
	
	public Color meshColor = new Color(1f,1f,1f,0.5f);
	public Color outlineColor = new Color(1f,1f,0f,1f);
	
	// Use this for initialization
	public void Start () {
		
		// Set the transparent material to this object
		SkinnedMeshRenderer meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

		//foreach (SkinnedMeshRenderer meshRenderer in meshRenderers) {

			// Set the transparent material to this object
			Material[] materials = meshRenderer.materials;
			int materialsNum = materials.Length;
			for (int i = 0; i < materialsNum; i++) {
				materials [i].shader = Shader.Find ("Outline/Transparent");
				materials [i].SetColor ("_color", meshColor);
			}
		
			// Create copy of this object, this will have the shader that makes the real outline
			GameObject outlineObj = new GameObject ();

		outlineObj.transform.position = meshRenderer.transform.position;
		outlineObj.transform.rotation = meshRenderer.transform.rotation;
		outlineObj.transform.localScale = transform.lossyScale;
			//outlineObj.AddComponent<MeshFilter> ();
			outlineObj.AddComponent<SkinnedMeshRenderer> ();

			Mesh mesh;
//			mesh = (Mesh)Instantiate (GetComponent<MeshFilter> ().mesh);
//			outlineObj.GetComponent<MeshFilter> ().mesh = mesh;

			mesh = (Mesh)Instantiate (meshRenderer.sharedMesh);
			//outlineObj.GetComponent<MeshFilter> ().mesh = mesh;
			outlineObj.GetComponent<SkinnedMeshRenderer> ().sharedMesh = mesh;
		

			materials = new Material[materialsNum];
			for (int i = 0; i < materialsNum; i++) {
				materials [i] = new Material (Shader.Find ("Outline/Outline"));
				materials [i].SetColor ("_OutlineColor", outlineColor);
			}

		outlineObj.GetComponent<SkinnedMeshRenderer> ().materials = materials;

		outlineObj.transform.parent = this.transform;
		//}
	}
	
}