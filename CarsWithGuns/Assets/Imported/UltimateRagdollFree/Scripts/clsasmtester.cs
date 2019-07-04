using UnityEngine;
using System.Collections;
using clsurgutils = U_r_g_utils.clsurgutils;

/// <summary>
/// 2015-04-01
/// ULTIMATE RAGDOLL GENERATOR V4.6
/// © THE ARC GAMES STUDIO 2015
///
/// special class for the ASM Tester scene. implements prefab spawning, mecanim to ragdoll and ragdoll to mecanim calls
/// REFER TO THE README FILE FOR USAGE SPECIFICATIONS
/// <para>Basic class to test a controllable mecanim character against the animation states manager utility, to determine if compilation and asm transition complete correctly.</para>
/// <para>Please refer to the in-scene information in play mode for usage specifications.</para>
/// </summary>
public class clsasmtester : MonoBehaviour {
	public GameObject vargamsource = null;
	public float vargamtransitiontime = 0.5f;
	private bool vartargetisragdoll = false;
	/// <summary>
	/// Spawn position for the reinstantiation
	/// </summary>
	public Vector3 vargamspawnposition = new Vector3(0,0,0);
	
	/// <summary>
	/// The rigidbody array that's used to detach ragdolled parts
	/// </summary>
	private Rigidbody[] varDbodies = new Rigidbody[0];
	/// <summary>
	/// The instanced model
	/// </summary>
	private GameObject vargamasmtarget;
	
	void Start () {
		if (vargamsource != null) {
			if(vargamsource.transform.root == transform.root) {
				Debug.LogError("Can't host the tester on the target.\nPlease host the tester in a persistent scene object (for example the main camera).", gameObject);
				return;
			}
			metinstantiatemodel();
			varDbodies = vargamasmtarget.GetComponentsInChildren<Rigidbody>();
			if (varDbodies.Length == 0) {
				Debug.LogError("There's no rigidbodies to test in the chosen target: make sure it's ragdolled and prefabbed.");
			}
			
		}
		else {
			Debug.LogError("Please assign a model to be able to test its ASM transitions.", transform);
		}
	}
	
	Animator varanimator = null;
	clsurganimationstatesmanager varasm = null;
	void OnGUI() {
		if(GUILayout.Button("Go ragdoll")) {
			if (vargamasmtarget == null) {
				Debug.Log("Please assign a mecanim game character to the vargamMecanimASMTarget slot of this script", gameObject);
				return;
			}
			varanimator = vargamasmtarget.GetComponentInChildren<Animator>();
			if (varanimator == null) {
				Debug.Log("No animator found on source. Need an Animator component to perform Mecanim operations.", vargamasmtarget);
				return;
			}
			varasm = vargamasmtarget.GetComponentInChildren<clsurganimationstatesmanager>();
			if (varasm == null) {
				Debug.Log("No animationn states manager found on source. Need the ASM to perform ragdoll to Mecanim transition.", vargamasmtarget);
				return;
			}
			CharacterController varcontroller = vargamasmtarget.GetComponent<CharacterController>();
			if (varcontroller == null) {
				Debug.LogWarning("No controller found on source. Need the controller's speed to perform proper Mecanim to ragdoll transition.", vargamasmtarget);
			}
			
			//disable help GUI
			clscameratarget varscenegui = GetComponent<Camera>().GetComponentInChildren<clscameratarget>();
			if (varscenegui != null) {
				varscenegui.vargamcurrentscenario = 1;
			}
			
			Vector3 varspeed = Vector3.zero;
			if (varcontroller != null) {
				varspeed = varcontroller.velocity;
			}
			//go-ragdoll: Disable the animator so that the ragdoll can take over
			varanimator.enabled = false;
			clsurgutils.metgodriven(vargamasmtarget.transform, varspeed);
			
			//Set the ragdoll flag to true (merely necessary for this scripts' GUI)
			vartargetisragdoll = true;
		}
		if(GUILayout.Button("Reload scene")) {
			Application.LoadLevel (Application.loadedLevelName);
		}
		
		if (vartargetisragdoll == true) {
			foreach (string varstatename in varasm.vargamstatenames) {
				if(GUILayout.Button(varstatename)) {
					vartargetisragdoll = false;
					//the actual ragdoll to mecanim function call
					//PLEASE NOTE: for special, post transition actions, a special overload to metinterpolatetoanimationstate is available, which takes an ACTION as a parameter
					//an example action function is available below, in the metasmaction function
					StartCoroutine(clsurgutils.metinterpolatetoanimationstate(vargamasmtarget.transform, varstatename, vargamtransitiontime, true));
				}
			}
		}
		else {
			GUILayout.Label("Need ragdoll state for ASM");
		}
	}
	
	/// <summary>
	/// Instantiate the model
	/// </summary>
	private void metinstantiatemodel() {
		vargamasmtarget = Instantiate(vargamsource, transform.position, Quaternion.identity) as GameObject;
		Transform varinstancetransform = vargamasmtarget.transform;
		varinstancetransform.position = vargamspawnposition;
		varinstancetransform.rotation = Quaternion.identity;
		varinstancetransform.parent = null;
		clscameratarget varcamtarget = Camera.main.GetComponentInChildren<clscameratarget>();
		varcamtarget.vargamtarget = varinstancetransform;
		varcamtarget.vargamscenarios[0].proptarget = varinstancetransform;
	}
	
	/// <summary>
	/// Can be used as a parameter for the ragdoll to mecanim function 
	/// </summary>
	/// <param name="varptarget">Varptarget.</param>
	/// <param name="varstatename">Varstatename.</param>
	private void metasmaction(Transform varptarget, string varstatename) {
		varanimator.Play(varstatename);
	}
}