using UnityEngine;
using System.Collections;

/// <summary>
/// Dimensions.
/// This class can be attached to any prefab for which you need dimension information.
/// NOTE: There are two ways to calculate radius used here. You don't need both. 
/// Choose the one that works for your geometry.
/// </summary>
/// 
public class Dimensions : MonoBehaviour
{

	private float radius;
	private float height;
	public bool useMeshRadius = true;
	
	public float Radius {
		get { return radius; }
	}
	public float Height {
		get { return height; }
	}
	
	
	// Use this for initialization
	void Start ()
	{
		float x; 
		float z;
		
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		height = mesh.bounds.size.y;
		
		if (useMeshRadius)
		{
			x = mesh.bounds.extents.x * transform.localScale.x;
			z = mesh.bounds.extents.z * transform.localScale.z;
			float meshRadius = Mathf.Sqrt (x * x + z * z);
			//Debug.Log ("meshRadius = " + meshRadius);	
			radius = meshRadius;
		}
		else 
		{
			x = renderer.bounds.extents.x;
			z = renderer.bounds.extents.z;
			float renderRadius = Mathf.Sqrt (x * x + z * z);
			//Debug.Log ("renderRadius = " + renderRadius);
			radius = renderRadius;
		}
	}

	
}
