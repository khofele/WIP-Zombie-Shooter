using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

[CustomEditor(typeof(AIWaypointNetwork))]  // Objecttype für das das Editorscript verwendet werden soll
public class AIWaypointNetworkEditor : Editor // Editorscripts erben von Editor --> im Namespace UnityEditor
{

    public override void OnInspectorGUI()
    {
        AIWaypointNetwork network = (AIWaypointNetwork) target;

        network.DisplayMode = (PathDisplayMode) EditorGUILayout.EnumPopup("Display Mode", network.DisplayMode);

        if(network.DisplayMode == PathDisplayMode.Paths)
        {
            network.UIStart = EditorGUILayout.IntSlider("Waypoint Start", network.UIStart, 0, network.Waypoints.Count - 1);
            network.UIEnd = EditorGUILayout.IntSlider("Waypoint End", network.UIEnd, 0, network.Waypoints.Count - 1);
        }

        DrawDefaultInspector();
    }

    void OnSceneGUI()
    {
        AIWaypointNetwork network = (AIWaypointNetwork) target; // über network kann nun auf die Waypoints zugegriffen werden

        for(int i=0; i<network.Waypoints.Count; i++)
        {
            if(network.Waypoints[i] != null)
            {
                Handles.Label(network.Waypoints[i].position, "Waypoint " + i.ToString());
            }
        }

        if(network.DisplayMode == PathDisplayMode.Connections) // wenn im Inspector Connections ausgewählt ist, sollen die Linien zwischen den Wegpunkten gezeigt werden
        {
            // Array mit den Koordinaten für jeden Wegpunkt
            // --> damit der letzte Wegpunkt mit dem ersten verbunden wird, muss das Array 1 größer sein als es Wegpunkte gibt
            Vector3[] linePoints = new Vector3[network.Waypoints.Count + 1];

            for (int i = 0; i <= network.Waypoints.Count; i++)
            {
                int index = i != network.Waypoints.Count ? i : 0; // wenn der index kein index innerhalb des Arrays ist (hier 0 bis 5), wird er auf 0 gesetzt --> erster index
                if (network.Waypoints[index] != null)
                {
                    linePoints[i] = network.Waypoints[index].position;
                }
                else
                {
                    linePoints[i] = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity); // wenn der index nicht valid ist, wird ihm folgende Position zugewiesen --> größter Wert, der in einem float gespeichert werden kann
                }
            }

            Handles.color = Color.cyan;
            Handles.DrawPolyLine(linePoints);
        } 
        else if (network.DisplayMode == PathDisplayMode.Paths)
        {
            // Allocate (= Zuweisen) a new NavMeshPath
            NavMeshPath path = new NavMeshPath();

            if(network.Waypoints[network.UIStart] != null && network.Waypoints[network.UIEnd] != null)
            {
                Vector3 from = network.Waypoints[network.UIStart].position;
                Vector3 to = network.Waypoints[network.UIEnd].position;

                // Request a path search on the nav mesh. This will return the path between from and to vectors
                NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);

                Handles.color = Color.yellow;

                // Draw a polyline passing int he path's corner points
                Handles.DrawPolyLine(path.corners);
            }
        }
    }
}