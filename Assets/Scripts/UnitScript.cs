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
            transform.LookAt( TargetPos );
            transform.Translate( Vector3.forward * speed * Time.deltaTime );
            transform.position = new Vector3( transform.position.x,
                Terrain.activeTerrain.SampleHeight( transform.position ) + 1,
                transform.position.z );

            if ( Vector3.Distance( transform.position, TargetPos ) <= CloseDist ) {
                if ( currentPath != null ) {
                    TargetPos = currentPath.GetNextPosition();
                } else {
                    TargetPos = Vector3.zero;
                }
            }
        }
    }
}
