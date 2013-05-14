using UnityEngine;
using System.Collections;

public class CollisionGridGenerator : MonoBehaviour
{
    // TODO: public float CellSize = 1.0f; // Size of each "cell" in the grid, in world coordinates

    private bool[,] _collisionGrid;
    public bool[,] Grid
    {
        get { return _collisionGrid; }
        private set { _collisionGrid = value; }
    }

    private const float RaycastStartY = 200;

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        Vector3 terrainSize = Terrain.activeTerrain.terrainData.size;
        _collisionGrid = new bool[(int)terrainSize.x, (int)terrainSize.z];

        // TODO: Handle terrain at non-zero position
        int obstacleLayerMask = 1 << LayerMask.NameToLayer("Obstacle");
        for ( int xPos = 0; xPos < terrainSize.x; ++xPos ) {
            for ( int zPos = 0; zPos < terrainSize.z; ++zPos ) {
                // Raycast at point to see if we hit an obstacle
                Vector3 rayOrigin = new Vector3( xPos, RaycastStartY, zPos );
                Ray ray = new Ray( rayOrigin, Vector3.down );
                if ( Physics.Raycast( ray, 2 * RaycastStartY, obstacleLayerMask ) ) {
                    Grid[xPos, zPos] = true;
                } else {
                    Grid[xPos, zPos] = false;
                }
            }
        }
    }
}
