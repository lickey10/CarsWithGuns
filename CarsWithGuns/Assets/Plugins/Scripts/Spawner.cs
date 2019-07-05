using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Spawner : MonoBehaviour
{
    public int curWave;
    public Waypoint[] waypoints;
    public Wave[] waves;
    public Transform[] spawners;
    public float spawnDelay;
    public float spawnTime;
    private bool spawning;
    private float nextSpawnTimme;
    public virtual IEnumerator Spawn()
    {
        Wave w = null;
        CubeSet cs = null;
        while (this.curWave < this.waves.Length)
        {
            w = this.waves[this.curWave];
            int i = 0;
            while (i < w.cubeSets.Length)
            {
                cs = w.cubeSets[i];
                this.StartCoroutine(cs.SpawnCS(this.spawners[i], this.waypoints[i], this.spawnTime));
                i++;
            }
            while (EnemyMovement.enemies > 0)
            {
                yield return typeof(WaitForFixedUpdate);
            }
            this.curWave++;
            yield return new WaitForSeconds(this.spawnDelay + (1 * this.curWave));
            if (this.curWave >= this.waves.Length)
            {
                this.curWave = 0;
            }
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        //if(other.tag == "Player")
        if (!this.spawning)
        {
            this.spawning = true;
            this.StartCoroutine(this.Spawn());
        }
    }

    public Spawner()
    {
        this.spawnDelay = 3;
        this.spawnTime = 0.2f;
    }

}