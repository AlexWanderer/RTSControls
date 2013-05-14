using UnityEngine;
using System.Collections;

public class UnitScript : MonoBehaviour
{
    public float speed = 50.0f;
    public GameObject selectionRing;
    public Vector3 TargetPos { get; set; }

    private bool _isSelected;
    public bool IsSelected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
            selectionRing.renderer.enabled = _isSelected;
        }
    }

    private const float CloseDist = 1.0f;
    private MouseInputHandler mouseInputHandler;
    private PathGenerator pathGenerator;

    private Path currentPath;

    void Start()
    {
        pathGenerator = (PathGenerator)GameObject.FindObjectOfType( typeof( PathGenerator ) );

        mouseInputHandler = (MouseInputHandler)GameObject.FindObjectOfType( typeof( MouseInputHandler ) );
        mouseInputHandler.OnRightClick += new MouseClickHandler( mouseInputHandler_OnRightClick );
    }

    void mouseInputHandler_OnRightClick( Vector3 displacement )
    {
        if ( IsSelected ) {
            Vector3 endPos = transform.position + displacement;
            endPos.y = transform.position.y;
            currentPath = pathGenerator.FindPath( transform.position, endPos );
            TargetPos = currentPath.StartPos;
        }
    }

    void Update()
    {
        FollowPath();
    }

    private void FollowPath()
    {
        if ( TargetPos != Vector3.zero ) // TODO: more robust check
        {
            Vector3 targetAtSameHeight = new Vector3( TargetPos.x, transform.position.y, TargetPos.z );

            transform.LookAt( targetAtSameHeight );
            transform.Translate( Vector3.forward * speed * Time.deltaTime );
            transform.position = new Vector3( transform.position.x,
                Terrain.activeTerrain.SampleHeight( transform.position ) + 1,
                transform.position.z );

            bool closeToTargetPos = Vector3.Distance( transform.position, targetAtSameHeight ) <= CloseDist; // Compare along same Y to prevent issues with terrain
            if ( closeToTargetPos ) {
                if ( currentPath != null ) {
                    TargetPos = currentPath.GetNextPosition();
                } else {
                    TargetPos = Vector3.zero;
                }
            }
        }
    }
}
