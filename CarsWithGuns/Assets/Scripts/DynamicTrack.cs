using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTrack : MonoBehaviour
{
    public GameObject WaypointPrefab;
    public List<WbTexturing> texClass;

    float yOffset = 0.5f;

    float terrainWidth;
    float terrainLength;

    float xTerrainPos;
    float zTerrainPos;
    public float maxDistanceBetweenWaypoints = 100;
    public float minDistanceBetweenWaypoints = 50;
    Vector3 trackPosition;

    // Start is called before the first frame update
    void Start()
    {
        generateTerrain(25, 0.1f, 1, 0, 1, 2.5f, true, "2018213383", 5, 1000);

        //select track area
        selectTrackArea();

        generateWaypoints(5);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void generateTerrain(float MountainFreq, float WaterLevel, float BumpbMultiplier, float BumpRoughness, float HeightMultiplier, float Roughness, bool randomSeed, string seed, float PixelError, float BasemapDistance)
    {
        bool useWater = false;

        if (useWater)
        {
            if (GameObject.Find("Water"))
            {
                GameObject water = GameObject.Find("Water");
                water.transform.position = new Vector3(water.transform.position.x, WaterLevel, water.transform.position.z);
            }
            else
            {
                Debug.Log("No water found!\n Name your water GameObject as 'Water'");
            }
        }

        TerrainGeneration TG = new TerrainGeneration();
        TG.SetMountainFreq = MountainFreq;
        TG.SetWaterlevel = WaterLevel;
        TG.BumpMultiplier = BumpbMultiplier;
        TG.BumbRoughness = BumpRoughness;
        TG.HeightMultiplier = HeightMultiplier;
        TG.Roughness = Roughness;
        TG.terrain = Terrain.activeTerrain;

        if (useWater && GameObject.Find("Water")) 
        { 
            TG.waterplane = GameObject.Find("Water"); 
            TerrainFoliage.waterLevel = WaterLevel; 
        }

        TG.editor = true;

        if (randomSeed)
        {
            TG.TerrainSeed = "" + (int)UnityEngine.Random.Range(0, int.MaxValue);
            seed = TG.TerrainSeed;
        }
        else
            TG.TerrainSeed = seed;

        Debug.Log("Using seed: " + TG.TerrainSeed);
        TG.makeHeightmap();

        Terrain.activeTerrain.heightmapPixelError = PixelError;
        Terrain.activeTerrain.basemapDistance = BasemapDistance;

        //Get terrain size
        terrainWidth = Terrain.activeTerrain.terrainData.size.x;
        terrainLength = Terrain.activeTerrain.terrainData.size.z;

        //Get terrain position
        xTerrainPos = Terrain.activeTerrain.transform.position.x;
        zTerrainPos = Terrain.activeTerrain.transform.position.z;

        //apply textures to terrain
        if (texClass != null && texClass.Count > 0 && texClass[texClass.Count - 1].texture != null)
        {
            //if (GUILayout.Button("Assign new textures"))
            //{
                List<TTexture> textures = new List<TTexture>();
                for (int i = 0; i < texClass.Count; i++)
                {
                    TTexture TTex = new TTexture();
                    TTex.texture = texClass[i].texture;
                    //TTex.color = texClass[i].color;
                    TTex.useBump = texClass[i].useBump;
                    if (texClass[i].useBump)
                    {
                        TTex.bumpmap = texClass[i].bumpmap;
                    }
                    else
                    {
                        TTex.bumpmap = texClass[i].emptyBump;
                    }
                    TTex.tilesize = texClass[i].tilesize;
                    TTex.index = texClass[i].index;
                    TTex.heightCurve = texClass[i].heightCurve;
                    TTex.angleCurve = texClass[i].angleCurve;
                    textures.Add(TTex);
                }
                TerrainTexturing.GenerateTexture(textures);
            //}
        }
    }

    private void selectTrackArea()
    {
        //Generate random x,z,y position on the terrain
        float adjustedWidth = (terrainWidth - terrainWidth * 0.75f) / 2;
        float adjustedLength = (terrainLength - terrainLength * 0.75f) / 2;
        float randX = UnityEngine.Random.Range(xTerrainPos + adjustedWidth, xTerrainPos + terrainWidth - adjustedWidth);
        float randZ = UnityEngine.Random.Range(zTerrainPos + adjustedLength, zTerrainPos + terrainLength - adjustedLength);
        float yVal = Terrain.activeTerrain.SampleHeight(new Vector3(randX, 0, randZ));

        //Apply Offset if needed
        yVal = yVal + yOffset;

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        SphereCollider sphereCollider = sphere.GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = maxDistanceBetweenWaypoints;

        trackPosition = new Vector3(randX, yVal, randZ);
        sphere.transform.position = trackPosition;
    }

    private void generateWaypoints(int NumberOfWaypoints)
    {
        //float radius = 5f;
        //originPoint = spawner.gameObject.transform.position;
        //originPoint.x += Random.Range(-radius, radius);
        //originPoint.z += Random.Range(-radius, radius);

        Vector3 lastWaypoint = trackPosition;

        //loop through areas on the terrain placing waypoints
        for (int x = 0; x < NumberOfWaypoints; x++)
        {
            //Generate random x,z,y position on the terrain
            //float randX = UnityEngine.Random.Range(xTerrainPos, xTerrainPos + terrainWidth);
            //float randZ = UnityEngine.Random.Range(zTerrainPos, zTerrainPos + terrainLength);
            float randX = lastWaypoint.x + Random.Range(-maxDistanceBetweenWaypoints + minDistanceBetweenWaypoints, maxDistanceBetweenWaypoints);
            float randZ = lastWaypoint.z + Random.Range(-maxDistanceBetweenWaypoints + minDistanceBetweenWaypoints, maxDistanceBetweenWaypoints);
            float yVal = Terrain.activeTerrain.SampleHeight(new Vector3(randX, 0, randZ));

            //Apply Offset if needed
            yVal = yVal + yOffset;

            lastWaypoint = new Vector3(randX, yVal, randZ);

            //Generate the Prefab on the generated position
            GameObject objInstance = (GameObject)Instantiate(WaypointPrefab, lastWaypoint, Quaternion.identity);
            
        }
    }
}
