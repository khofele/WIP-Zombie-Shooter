using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentExample : MonoBehaviour
{
    private NavMeshAgent navAgent = null;
    public int CurrentIndex = 0; // current Waypoint
    public bool HasPath = false;
    public bool PathPending = false;
    public bool PathStale = false;
    public NavMeshPathStatus PathStatus = NavMeshPathStatus.PathInvalid;

    public AIWaypointNetwork WayPointNetwork = null;

    // Start is called before the first frame update
    void Start()
    {
        // NavMeshAgent Referenz
        navAgent = GetComponent<NavMeshAgent>();

        if(WayPointNetwork == null)
        {
            return;
        }

        SetNextDestination(false);
    }

    void SetNextDestination(bool increment)
    {
        if(!WayPointNetwork)
        {
            return;
        }

        // incStep = IncrementStep
        int incStep = increment ? 1 : 0;

        int nextWaypoint = (CurrentIndex + incStep >= WayPointNetwork.Waypoints.Count) ? 0 : CurrentIndex + incStep;
        Transform nextWayPointTransform = WayPointNetwork.Waypoints[nextWaypoint];

        if(nextWayPointTransform != null) 
        {
            CurrentIndex = nextWaypoint;
            navAgent.destination = nextWayPointTransform.position;
            return;
        }

        CurrentIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        HasPath = navAgent.hasPath;
        PathPending = navAgent.pathPending;
        PathStale = navAgent.isPathStale;
        PathStatus = navAgent.pathStatus;

        if((!HasPath && !PathPending) || PathStatus == NavMeshPathStatus.PathInvalid)
        {
            SetNextDestination(true);
        }
        else if(navAgent.isPathStale)
        {
            SetNextDestination(false);
        }
    }
}