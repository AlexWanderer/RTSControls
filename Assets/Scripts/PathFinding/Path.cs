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

    private List<Vector3> path;
    private int currentIndex = 0;

    void Start()
    {
        List<Vector3> emptyPath = new List<Vector3>();
        emptyPath.Add( Vector3.zero );
        SetPath( emptyPath );
    }

    public void SetPath( List<Vector3> path )
    {
        this.path = path;

        Debug.Log( "Path created from " + StartPos + " to " + EndPos );
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
}
