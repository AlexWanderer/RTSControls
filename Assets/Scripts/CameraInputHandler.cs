using UnityEngine;
using System.Collections;

public class CameraInputHandler : MonoBehaviour
{
    public float speed = 1.0f;

    void Update()
    {
        if ( Input.GetKey( KeyCode.LeftArrow ) ) {
            transform.Translate( speed * -1, 0, 0, Space.World );
        }
        if ( Input.GetKey( KeyCode.RightArrow ) ) {
            transform.Translate( speed, 0, 0, Space.World );
        }

        if ( Input.GetKey( KeyCode.UpArrow ) ) {
            transform.Translate( 0, 0, speed, Space.World );
        }
        if ( Input.GetKey( KeyCode.DownArrow ) ) {
            transform.Translate( 0, 0, speed * -1, Space.World );
        }
    }
}
