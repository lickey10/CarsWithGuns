using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WbTexturing 
{
    [SerializeField]
    public Texture2D texture;
    [SerializeField]
    public bool useBump = false;
    [SerializeField]
    public Texture2D bumpmap;
    [SerializeField]
    public Vector2 tilesize = new Vector2(50, 50);
    [SerializeField]
    public bool enableGrass = false;


    public Texture2D emptyBump;

    [SerializeField]
    public Color color = Color.white;

    [SerializeField]
    public AnimationCurve heightCurve = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

    [SerializeField]
    public AnimationCurve angleCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

    [SerializeField]
    private string[] options = { "Height", "Angle" };
    [SerializeField]
    public int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
