using UnityEngine;
using System.Collections;

public class UnitScript : MonoBehaviour
{
    public float speed = 50.0f;

    public bool IsSelected { get; set; }

    private const float CloseDist = 1.0f;
    private MouseInputHandler mouseInputHandler;
    private Vector3 targetPos;

    void Start()
    {
        mouseInputHandler = (MouseInputHandler)GameObject.FindObjectOfType( typeof( MouseInputHandler ) );
        mouseInputHandler.OnRightClick += new MouseClickHandler( mouseInputHandler_OnRightClick );
    }

    void mouseInputHandler_OnRightClick( Vector3 position )
    {
        if ( IsSelected ) {
            targetPos = position;
            targetPos.y = transform.position.y;
        }
    }

    void Update()
    {
        if ( targetPos != Vector3.zero ) // TODO: more robust check
        {
            transform.LookAt( targetPos );
            transform.Translate( Vector3.forward * speed * Time.deltaTime );
            transform.position = new Vector3( transform.position.x,
                Terrain.activeTerrain.SampleHeight( transform.position ),
                transform.position.z );

            if ( Vector3.Distance( transform.position, targetPos ) <= CloseDist ) {
                targetPos = Vector3.zero;
            }
        }
    }
}
