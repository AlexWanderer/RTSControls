using UnityEngine;
using System.Collections.Generic;

public class PathGenerator : MonoBehaviour
{
    public CollisionGridGenerator gridGenerator;

    public Path FindPath( Vector3 startPos, Vector3 endPos )
    {
        // Check if endPos is valid
        if ( gridGenerator.Grid[Mathf.FloorToInt( endPos.x ), Mathf.FloorToInt( endPos.z )] ) {
            // End pos is not available; return a path with only startPos
            Path p = new Path();
            p.AddNode( startPos );
            return p;
        }

        // Find a path
        Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();

        List<Vector3> closedSet = new List<Vector3>();
        List<Vector3> openSet = new List<Vector3>();
        openSet.Add( startPos );

        Dictionary<Vector3, float> gScores = new Dictionary<Vector3, float>(); // Cost from start along best-known path
        gScores[startPos] = 0;

        Dictionary<Vector3, float> fScores = new Dictionary<Vector3, float>(); // Estimated total cost
        fScores[startPos] = gScores[startPos] + GuessDistanceHeuristic( startPos, endPos );

        while ( openSet.Count > 0 ) {
            Vector3 currentNode = GetLowestFScore( openSet, fScores );
            if ( CalcDist( currentNode, endPos ) < 1.0f ) {
                return ReconstructPath( cameFrom, currentNode );
            }

            openSet.Remove( currentNode );
            closedSet.Add( currentNode );

            foreach ( Vector3 neighbor in GetNeigbors( currentNode ) ) {
                float tempGScore = gScores[currentNode] + Vector3.Distance( currentNode, neighbor );
                if ( closedSet.Contains( neighbor ) ) {
                    if ( tempGScore >= gScores[neighbor] ) {
                        continue;
                    }
                }

                if ( !openSet.Contains( neighbor ) || tempGScore < gScores[neighbor] ) {
                    cameFrom[neighbor] = currentNode;
                    gScores[neighbor] = tempGScore;
                    fScores[neighbor] = gScores[neighbor] + GuessDistanceHeuristic( neighbor, endPos );
                    if ( !openSet.Contains( neighbor ) ) {
                        openSet.Add( neighbor );
                    }
                }
            }
        }

        // If we reached here, we failed to find a path. Return a straight line.
        return PlaceholderPath( startPos, endPos );
    }

    private List<Vector3> GetNeigbors( Vector3 currentNode )
    {
        bool[,] collisionGrid = gridGenerator.Grid;
        // Create list of neigbors going clockwise
        List<Vector3> neigbors = new List<Vector3>();
        /*
        neigbors.Add( new Vector3( currentNode.x - 1, currentNode.y, currentNode.z + 1 ) );
        neigbors.Add( new Vector3( currentNode.x, currentNode.y, currentNode.z + 1 ) );
        neigbors.Add( new Vector3( currentNode.x + 1, currentNode.y, currentNode.z + 1 ) );
        neigbors.Add( new Vector3( currentNode.x + 1, currentNode.y, currentNode.z ) );
        neigbors.Add( new Vector3( currentNode.x + 1, currentNode.y, currentNode.z - 1 ) );
        neigbors.Add( new Vector3( currentNode.x, currentNode.y, currentNode.z - 1 ) );
        neigbors.Add( new Vector3( currentNode.x - 1, currentNode.y, currentNode.z - 1 ) );
        neigbors.Add( new Vector3( currentNode.x - 1, currentNode.y, currentNode.z ) );
         */

        // TODO: Less janky version of this
        int xPos = Mathf.FloorToInt( currentNode.x - 1 );
        int zPos = Mathf.FloorToInt( currentNode.z );
        if ( !collisionGrid[xPos, zPos] ) {
            neigbors.Add( new Vector3( xPos, currentNode.y, zPos ) );
        }

        xPos = Mathf.FloorToInt( currentNode.x + 1 );
        if ( !collisionGrid[xPos, zPos] ) {
            neigbors.Add( new Vector3( xPos, currentNode.y, zPos ) );
        }

        xPos = Mathf.FloorToInt( currentNode.x );
        zPos = Mathf.FloorToInt( currentNode.z - 1 );
        if ( !collisionGrid[xPos, zPos] ) {
            neigbors.Add( new Vector3( xPos, currentNode.y, zPos ) );
        }

        zPos = Mathf.FloorToInt( currentNode.z + 1 );
        if ( !collisionGrid[xPos, zPos] ) {
            neigbors.Add( new Vector3( xPos, currentNode.y, zPos ) );
        }

        return neigbors;
    }

    private Path ReconstructPath( Dictionary<Vector3, Vector3> cameFrom, Vector3 currentNode )
    {
        if ( cameFrom.ContainsKey( currentNode ) ) {
            Path p = ReconstructPath( cameFrom, cameFrom[currentNode] );
            p.AddNode( currentNode );
            return p;
        } else {
            Path p = new Path();
            p.AddNode( currentNode );
            return p;
        }
    }

    private Vector3 GetLowestFScore( List<Vector3> openSet, Dictionary<Vector3, float> fScores )
    {
        Vector3 closestVector = openSet[0];
        float lowestFScore = fScores[closestVector];

        foreach ( Vector3 vector in openSet ) {
            float fScore = fScores[vector];
            if ( fScore < lowestFScore ) {
                lowestFScore = fScore;
                closestVector = vector;
            }
        }

        return closestVector;
    }

    private float GuessDistanceHeuristic( Vector3 startPos, Vector3 endPos )
    {
        // TODO: More robust heuristic
        return CalcDist( startPos, endPos );
    }

    private Path PlaceholderPath( Vector3 startPos, Vector3 endPos )
    {
        Debug.Log( "Failed to find a path; creating placeholder" );

        List<Vector3> path = new List<Vector3>();
        path.Add( startPos );
        path.Add( endPos );

        Path returnVal = new Path();
        returnVal.SetPath( path );
        return returnVal;
    }

    private float CalcDist( Vector3 a, Vector3 b )
    {
        float distX = Mathf.Abs( b.x - a.x );
        float distZ = Mathf.Abs( b.z - a.z );

        return distX + distZ;
    }
}
