using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	// The singleton instance.
	public static GameController instance = null;

	void Awake()
    {
        // Enforce singleton pattern.
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Logger.Log("duplicate GameController, destroying");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
	// Use this for initialization
	void Start () {
		Logger.Log("Hello!");
        Logger.Log("Screen (width, height): (" + Screen.width + ", " + Screen.height + ")");

		// transform.position = new Vector3(-5.12f, -3.84f, 0.0f);
		// createQuadMesh();
	}

	// Update is called once per frame
	void Update () {

	}

	void createQuadMesh()
	{
		float width = 10.24f;
		float height = 7.68f;

		Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

	    Vector3[] vertices  = new Vector3[4];

	    vertices[0] = new Vector3(0, 0, 0);
	    vertices[1] = new Vector3(width, 0, 0);
	    vertices[2] = new Vector3(0, height, 0);
	    vertices[3] = new Vector3(width, height, 0);

	    mesh.vertices = vertices;

		int[] tri = new int[6]  { 0, 2, 1, 2, 3, 1 };

	    mesh.triangles = tri;

	    Vector3[] normals = new Vector3[4];

	    normals[0] = -Vector3.forward;
	    normals[1] = -Vector3.forward;
	    normals[2] = -Vector3.forward;
	    normals[3] = -Vector3.forward;

	    mesh.normals = normals;

	    Vector2[] uv = new Vector2[4];

	    uv[0] = new Vector2(0, 0);
	    uv[1] = new Vector2(1, 0);
	    uv[2] = new Vector2(0, 1);
	    uv[3] = new Vector2(1, 1);

	    mesh.uv = uv;
	}
}
