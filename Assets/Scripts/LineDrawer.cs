using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// © 2018 TheFlyingKeyboard and released under MIT License
// theflyingkeyboard.net
public class LineDrawer : MonoBehaviour {
  private LineRenderer line;
  private Vector2 mousePosition;
  [SerializeField] private bool simplifyLine = false;
  [SerializeField] private float simplifyTolerance = 0.02f;
  [SerializeField] private Material material;
  private void Start () {
    line = GetComponent<LineRenderer>();
	line.SetWidth(0.1f, 0.1f);
  }
  private void Update () {
    if (Input.GetMouseButton(0)) //Or use GetKey with key defined with mouse button
    {
      mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      line.positionCount++;
      line.SetPosition(line.positionCount - 1, mousePosition);
    }
    if (Input.GetMouseButtonUp(0))
    {
        Vector2 startingPoint = line.GetPosition(0);
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, startingPoint);
      if (simplifyLine)
      {
        line.Simplify(simplifyTolerance);
      }
      triangulateLine();
      enabled = false; //Making this script stop
    }
  }

  void triangulateLine () {
      // Create Vector2 vertices
      Vector2[] testVertices2D = new Vector2[] {
          new Vector2(0,0),
          new Vector2(0,50),
          new Vector2(50,50),
          new Vector2(50,100),
          new Vector2(0,100),
          new Vector2(0,150),
          new Vector2(150,150),
          new Vector2(150,100),
          new Vector2(100,100),
          new Vector2(100,50),
          new Vector2(150,50),
          new Vector2(150,0),
      };
      for (int i=0; i<testVertices2D.Length; i++) {
          testVertices2D[i].Set( testVertices2D[i].x / 100,  testVertices2D[i].y / 100);
          // Logger.Log("testVert: (" + testVertices2D[i].x + ", " + testVertices2D[i].y + ")");
      }

      int pointCount = line.positionCount - 1; // skip last point which is the same as the first
      Vector2[] lineVertices2D = new Vector2[pointCount];
      Vector2 firstPoint = line.GetPosition(0);
      // Logger.Log("firstPoint: (" + firstPoint.x + ", " + firstPoint.y + ")");
      for (int i=0; i<pointCount; i++) {
          Vector2 position = line.GetPosition(i);
          // position.x -= firstPoint.x;
          // position.y -= firstPoint.y;
          // position.x /= 10;
          // position.y /= 10;
          lineVertices2D[i].Set(position.x, position.y);
          Logger.Log("lineVert: (" + lineVertices2D[i].x + ", " + lineVertices2D[i].y + ")");
      }

      Vector2[] vertices2D = lineVertices2D;
      // Use the triangulator to get indices for creating triangles
      Triangulator tr = new Triangulator(vertices2D);
      int[] indices = tr.Triangulate();

      // Create the Vector3 vertices
      Vector3[] vertices = new Vector3[vertices2D.Length];
      for (int i=0; i<vertices.Length; i++) {
          vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
      }

      // Create the mesh
      Mesh msh = new Mesh();
      msh.vertices = vertices;
      msh.triangles = indices;
      msh.RecalculateNormals();
      msh.RecalculateBounds();

      Vector2[] uv = new Vector2[vertices.Length];
      for (int i=0; i<vertices.Length; i++) {
          Vector2 vertex = vertices[i];
          float x = (vertex.x + 5.12f) / 10.24f;
          float y = 1.0f - (vertex.y - 3.84f) / -07.68f;
          Logger.Log("uv: (" + x + ", " + y + ")");
          uv[i] = new Vector2(x, y);
      }

      msh.uv = uv;

      // Set up game object with mesh;
      MeshRenderer meshRenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
      meshRenderer.material = material;
      MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
      filter.mesh = msh;
  }
}
