using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriTest : MonoBehaviour
{
    // Draws a triangle that covers the middle of the screen
    private Material lineMaterial;
    public Vector2 point1;
    public Vector2 point2;
    public Vector2 point3;
    public bool IsActive;

    void CreateLineMaterial()
    {
        // Unity has a built-in shader that is useful for drawing simple colored things
        var shader = Shader.Find("Hidden/Internal-Colored");
        lineMaterial = new Material(shader);
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        // Turn on alpha blending
        lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        // Turn backface culling off
        lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        // Turn off depth writes
        lineMaterial.SetInt("_ZWrite", 0);
    }

    void OnPostRender()
    {
        if (IsActive)
        {
            if (!lineMaterial)
            {
                CreateLineMaterial();
            }
            GL.PushMatrix();
            lineMaterial.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.TRIANGLES);
            GL.Color(Color.black);
            //GL.Vertex3(point1.x, point1.y + .01f, 0);
            //GL.Vertex3(point2.x - .01f, point2.y, 0);
            //GL.Vertex3(point3.x + .01f, point3.y, 0);
            GL.Color(Color.white);
            GL.Vertex3(point1.x, point1.y, 0);
            GL.Vertex3(point2.x, point2.y, 0);
            GL.Vertex3(point3.x, point3.y, 0);

            GL.End();
            GL.PopMatrix();
        }
        
    }

    public void SetPoint1(Vector2 point)
    {
        point1 = point;
    }
}
