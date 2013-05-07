using UnityEngine;
using System.Collections;

public class UnitScript : MonoBehaviour 
{
    public float speed = 50.0f;

    private const float CloseDist = 1.0f;
    private MouseInputHandler mouseInputHandler;
    private Vector3 targetPos;

	void Start ()
    {
        mouseInputHandler = (MouseInputHandler)GameObject.FindObjectOfType(typeof(MouseInputHandler));
        mouseInputHandler.OnRightClick += new MouseClickHandler(mouseInputHandler_OnRightClick);
	}

    void mouseInputHandler_OnRightClick(Vector3 position)
    {
        targetPos = position;
        targetPos.y = transform.position.y;
    }
	
	void Update ()
    {
        if (targetPos != Vector3.zero) // TODO: more robust check
        {
            transform.LookAt(targetPos);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) <= CloseDist)
            {
                targetPos = Vector3.zero;
            }
        }
	}
}
