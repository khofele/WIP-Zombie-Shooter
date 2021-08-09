using System.Collections.Generic;
using UnityEngine;

public enum PathDisplayMode { None, Connections, Paths } // Enum für die Modes, wie man die Verbindungen zwischen den Wegpunkten eines Networks darstelllen kann

public class AIWaypointNetwork : MonoBehaviour
{
    [HideInInspector]
    public PathDisplayMode DisplayMode = PathDisplayMode.Connections;

    [HideInInspector]
    public int UIStart = 0; // Start- und Endwegpunkt
    [HideInInspector]
    public int UIEnd = 0;

    public List<Transform> Waypoints = new List<Transform>();

}