using UnityEngine;
using System.Collections.Generic;

public class UnitSelectionManager : MonoBehaviour
{
    public Texture2D WhiteTexture;
    public Color DragRectColor = Color.green;
    public float DragRectAlpha = 0.5f;

    public Vector3 SelectedUnitsCenter 
    {
        get
        {
            Vector3 center = Vector3.zero;
            foreach ( UnitScript unit in _selectedUnits ) {
                center += unit.transform.position;
            }
            center /= _selectedUnits.Count;

            return center;
        }
    }

    public List<UnitScript> SelectedUnits
    {
        get { return _selectedUnits; }
    }

    private List<UnitScript> units = new List<UnitScript>();
    private List<UnitScript> _selectedUnits = new List<UnitScript>();

    private Vector3 dragStartPos = Vector3.zero;
    private bool isDragging = false;

    void Start()
    {
        DragRectColor.a = DragRectAlpha;

        GameObject[] placedUnits = GameObject.FindGameObjectsWithTag( "Unit" );
        foreach ( GameObject unit in placedUnits ) {
            units.Add( unit.GetComponent<UnitScript>() );
        }
        Debug.Log( "numUnits: " + units.Count );
    }

    void Update()
    {
        HandleLeftMouse();
    }

    void OnGUI()
    {
        if ( isDragging ) {
            DrawSelectionBox( dragStartPos, Input.mousePosition );
        }
    }

    private void HandleLeftMouse()
    {
        // TODO: handle select with just a mouse click rather than a drag
        bool mouseDown = Input.GetMouseButton( 0 );
        bool dragStartedThisFrame = Input.GetMouseButtonDown( 0 );

        if ( dragStartedThisFrame ) {
            dragStartPos = Input.mousePosition;
            isDragging = true;
        }

        if ( isDragging && !mouseDown ) {
            isDragging = false;
            Vector3 dragEndPos = Input.mousePosition;
            SelectUnits( dragStartPos, dragEndPos );
        }
    }

    private void SelectUnits( Vector3 dragStartPos, Vector3 dragEndPos )
    {
        Vector3 dragStartWorld = Vector3.zero;
        Vector3 dragEndWorld = Vector3.zero;
        ConvertDragToWorldSpace( dragStartPos, dragEndPos, out dragStartWorld, out dragEndWorld );

        float minX = Mathf.Min( dragStartWorld.x, dragEndWorld.x );
        float maxX = Mathf.Max( dragStartWorld.x, dragEndWorld.x );
        float minZ = Mathf.Min( dragStartWorld.z, dragEndWorld.z );
        float maxZ = Mathf.Max( dragStartWorld.z, dragEndWorld.z );

        _selectedUnits = new List<UnitScript>();
        foreach ( UnitScript unit in units ) {
            Vector3 pos = unit.transform.position;
            if ( pos.x > minX && pos.x < maxX && pos.z > minZ && pos.z < maxZ ) {
                _selectedUnits.Add( unit );
                unit.IsSelected = true;
            } else {
                unit.IsSelected = false;
            }
        }

        Debug.Log( "Selected " + _selectedUnits.Count + " units." );
    }

    // Converts mouse coords to world coords
    private void ConvertDragToWorldSpace( Vector3 dragStartPos, Vector3 dragEndPos, out Vector3 dragStartWorld, out Vector3 dragEndWorld )
    {
        Ray dragStartRay = Camera.main.ScreenPointToRay( dragStartPos );
        Ray dragEndRay = Camera.main.ScreenPointToRay( dragEndPos );

        Plane groundPlane = new Plane( Vector3.up, 0 );
        float dist = 0f;
        groundPlane.Raycast( dragStartRay, out dist );
        dragStartWorld = dragStartRay.GetPoint( dist );
        groundPlane.Raycast( dragEndRay, out dist );
        dragEndWorld = dragEndRay.GetPoint( dist );
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