using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Waypoint : MonoBehaviour
{
    public Waypoint nextWaypoint;
    public static Waypoint[] Waypoints;
    public virtual void Awake()
    {
        if (Waypoint.Waypoints == null)
        {
            Waypoint.Waypoints = ((Waypoint[]) GameObject.FindObjectsOfType(typeof(Waypoint))) as Waypoint[];
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        EnemyMovement AI = (EnemyMovement) other.transform.root.GetComponent(typeof(EnemyMovement));
        if (AI != null)
        {
            AI.waypoint = this.nextWaypoint.transform;
        }
    }

    public static Transform GetClosestWaypoint(Vector3 pos)
    {
        float temp = 0.0f;
        float dist = 100000000;
        Transform closest = null;
        int i = 0;
        while (i < Waypoint.Waypoints.Length)
        {
            if (Waypoint.Waypoints[i])
            {
                temp = (Waypoint.Waypoints[i].transform.position - pos).magnitude;
                if (temp < dist)
                {
                    dist = temp;
                    closest = Waypoint.Waypoints[i].transform;
                }
            }
            i++;
        }
        return closest;
    }

}