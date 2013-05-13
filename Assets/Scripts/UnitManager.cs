using UnityEngine;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
    List<GameObject> units = new List<GameObject>();

    private Vector3 dragStartPos = Vector3.zero;

    void Start()
    {
        GameObject[] placedUnits = GameObject.FindGameObjectsWithTag( "Unit" );
        foreach ( GameObject unit in placedUnits ) {
            units.Add( unit );
        }
    }

    void Update()
    {
        HandleLeftMouseDown();
    }

    private void HandleLeftMouseDown()
    {
        bool mouseDown = Input.GetMouseButton( 0 );
        bool dragStartedThisFrame = Input.GetMouseButtonDown( 0 );

        if ( dragStartedThisFrame ) {
            dragStartPos = Input.mousePosition;
        } else {
            Vector3 currentPos = Input.mousePosition;

        }
    }
}