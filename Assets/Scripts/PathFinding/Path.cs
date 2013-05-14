using UnityEngine;
using System.Collections.Generic;

public class Path
{
    public Vector3 StartPos
    {
        get { return path[0]; }
    }
    public Vector3 EndPos
    {
        get { return path[path.Count - 1]; }
    }

    private List<Vector3> path = new List<Vector3>();
    private int currentIndex = 0;

    public void SetPath( List<Vector3> path )
    {
        this.path = path;
    }

    public void AddNode( Vector3 node )
    {
        path.Add( node );
    }

    public Vector3 GetNextPosition()
    {
        // Check if we're at the end
        if ( currentIndex == path.Count - 1 ) {
            return Vector3.zero;
        }

        // If not at end, return next point
        return path[++currentIndex];
    }

    public void PrintDebugPath()
    {
        Debug.Log( "Printing path from " + StartPos + " to " + EndPos );
        for ( int i = 0; i < path.Count; ++i ) {
            Debug.Log( i + ". " + path[i] );
        }
    }
}
