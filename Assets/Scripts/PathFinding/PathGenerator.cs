using UnityEngine;
using System.Collections.Generic;

public class PathGenerator : MonoBehaviour
{
    public CollisionGridGenerator gridGenerator;

    public Path FindPath(Vector3 startPos, Vector3 endPos)
    {
        List<Vector3> path = new List<Vector3>();
        
        // Create placeholder path
        path.Add( startPos );
        path.Add( endPos );

        Path returnVal = new Path();
        returnVal.SetPath( path );
        return returnVal;
    }
}
