using UnityEngine;
using System.Collections;

public delegate void MouseClickHandler( Vector3 position );

public class MouseInputHandler : MonoBehaviour
{
    public Terrain terrain;
    public event MouseClickHandler OnRightClick;
    public UnitSelectionManager unitManager;

    private KeyCode RightClickAlternate = KeyCode.Space;

    void Update()
    {
        if ( Input.GetMouseButtonDown( 1 ) || Input.GetKeyDown( RightClickAlternate ) ) {
            HandleRightClick();
        }
    }

    private void HandleRightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        Plane groundPlane = new Plane( Vector3.up, 0 );
        float distance = 0f;
        if ( groundPlane.Raycast( ray, out distance ) ) {
            Vector3 hitPos = ray.GetPoint( distance );
            Vector3 selectedUnitCenter = unitManager.SelectedUnitsCenter;
            Vector3 displacement = hitPos - selectedUnitCenter;

            if ( OnRightClick != null ) {
                OnRightClick( displacement );
            }
        }
    }
}
