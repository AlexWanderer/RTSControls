using UnityEngine;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
    public Texture2D WhiteTexture;
    public Color DragRectColor = Color.green;
    public float DragRectAlpha = 0.5f;

    private List<GameObject> units = new List<GameObject>();

    private Vector3 dragStartPos = Vector3.zero;
    private bool isDragging = false;

    void Start()
    {
        DragRectColor.a = DragRectAlpha;

        GameObject[] placedUnits = GameObject.FindGameObjectsWithTag( "Unit" );
        foreach ( GameObject unit in placedUnits ) {
            units.Add( unit );
        }
    }

    void Update()
    {
        HandleLeftMouseDown();
    }

    void OnGUI()
    {
        if ( isDragging ) {
            DrawSelectionBox( dragStartPos, Input.mousePosition );
        }
    }

    private void HandleLeftMouseDown()
    {
        bool mouseDown = Input.GetMouseButton( 0 );
        bool dragStartedThisFrame = Input.GetMouseButtonDown( 0 );

        if ( dragStartedThisFrame ) {
            dragStartPos = Input.mousePosition;
            isDragging = true;
        }

        if ( isDragging ) {
            if ( !mouseDown ) {
                Vector3 dragEndPos = Input.mousePosition;
                Debug.Log( "Dragged from " + dragStartPos + " to " + dragEndPos );
                isDragging = false;
            }
        }
    }

    private void DrawSelectionBox( Vector3 start, Vector3 end )
    {
        float minX = Mathf.Min( start.x, end.x );
        float maxX = Mathf.Max( start.x, end.x );
        float minY = Screen.height - Mathf.Min( start.y, end.y ); // Flip Y to go from Input to GUI coords
        float maxY = Screen.height - Mathf.Max( start.y, end.y );

        int width = Mathf.FloorToInt( maxX - minX );
        int height = Mathf.FloorToInt( maxY - minY );

        GUI.color = DragRectColor;
        GUI.DrawTexture( new Rect( minX, minY, width, height ), WhiteTexture );
        GUI.color = Color.white;
    }
}