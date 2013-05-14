using UnityEngine;
using System.Collections.Generic;

public class UnitFormationManager : MonoBehaviour
{
    public UnitSelectionManager selectionMgr;
    public float UnitSpacingInFormation = 3.0f;
    public KeyCode LineFormationKey = KeyCode.Alpha1;
    public KeyCode StaggeredFormationKey = KeyCode.Alpha2;

    void Update()
    {
        if ( Input.GetKeyDown( LineFormationKey ) ) {
            ArrangeUnitsInLineFormation();
        }

        if ( Input.GetKeyDown( StaggeredFormationKey ) ) {
            ArrangeUnitsInStaggeredFormation();
        }
    }

    private void ArrangeUnitsInLineFormation()
    {
        List<UnitScript> units = selectionMgr.SelectedUnits;
        Vector3 unitCenter = selectionMgr.SelectedUnitsCenter;
        int numUnits = units.Count;

        float leftmostXOffset = -1 * (numUnits / 2.0f) * UnitSpacingInFormation;
        for ( int i = 0; i < numUnits; ++i ) {
            float xOffset = leftmostXOffset + i * UnitSpacingInFormation;

            Vector3 targetPos = unitCenter;
            targetPos.x += xOffset;
            units[i].TargetPos = targetPos;
        }
    }

    private void ArrangeUnitsInStaggeredFormation()
    {
        List<UnitScript> units = selectionMgr.SelectedUnits;
        Vector3 unitCenter = selectionMgr.SelectedUnitsCenter;
        int numUnits = units.Count;

        float leftmostXOffset = -1 * (numUnits / 2.0f) * UnitSpacingInFormation;
        for ( int i = 0; i < numUnits; ++i ) {
            float xOffset = leftmostXOffset + i * UnitSpacingInFormation;
            float zOffset = 0;
            if ( IsOdd( i ) ) {
                zOffset = UnitSpacingInFormation;
            }

            Vector3 targetPos = unitCenter;
            targetPos.x += xOffset;
            targetPos.z += zOffset;
            units[i].TargetPos = targetPos;
        }
    }

    private bool IsOdd( int val )
    {
        return (val % 2) == 1;
    }
}
