using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Painter : MonoBehaviour
{
    Texture2D texture;
    readonly int texture_size = 1024;
    public Color drawing_color;
    public int radius = 10;
    private bool updated=false;
    private Vector2Int oldPos;
    private Color defaultColor;
    private BasicRaycast rayCaster;

    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(texture_size, texture_size);
        texture.wrapMode = TextureWrapMode.Clamp;
        defaultColor = texture.GetPixel(0,0);
        Debug.Log(defaultColor);
        GetComponent<Renderer>().material.mainTexture = texture;
        rayCaster = FindObjectOfType<BasicRaycast>();
    }

    // Convert a world-space coordinate to texture coordinate
    private Vector2Int ConvertCoords(Vector3 p) {
        Vector3 local_p = transform.InverseTransformPoint(p);
        return new Vector2Int(
            (int)(texture_size - ((local_p.x + 5) / 10) * texture_size),
            (int)(texture_size - ((local_p.z + 5) / 10) * texture_size)
        );
    }

    public void ContinusDraw(Vector3 p, bool reset = false) {
        if (reset) {
            oldPos = ConvertCoords(p);
            DrawCircle(oldPos);
            return;
        }
        Vector2Int newPos=ConvertCoords(p);
        DrawLine(oldPos,newPos);
        //DrawCircle(newPos);
        oldPos=newPos;
    }

    // Draws from a 3D world-space point
    public void Draw(Vector3 p) => Draw (ConvertCoords(p));
    
    // Draws from a 2D texture point
    public void Draw(Vector2Int p) => Draw(p.x,p.y);

    public void Draw(int x, int y) {
        // If we try to paint outside the canvas, Unity will then draw back on the opposite side, and in our case we don't want that
        if (x < 0 || y < 0 || x>=texture_size || y>=texture_size) return;
        // Since we are layering transparent pixels, we need to do alpha blending
        // Before that, we do a slight optimisation: if the new color is fully opaque, we don't bother with alpha blending
        if (drawing_color.a>0.9999999)
            texture.SetPixel(x, y, drawing_color);
        else if (drawing_color.a<=0.0001)
            // If the color is full tranparent, we consider it is an eraser and we don't blend
            texture.SetPixel(x, y, defaultColor);
        else {
            // Alpha blending
            // See https://en.wikipedia.org/wiki/Alpha_compositing#Description
            Color old = texture.GetPixel(x, y);
            float alpha0 = drawing_color.a + old.a * (1.0f - drawing_color.a);
            Color newColor = (drawing_color * drawing_color.a + old * old.a * (1.0f - drawing_color.a))/ alpha0;
            newColor.a = alpha0;
            texture.SetPixel(x, y, newColor);
        }
        updated=true;
    }

    // https://stackoverflow.com/a/14976268
    public void DrawCircle(Vector2Int p) {
        
        int radius_sqr = radius * radius;
        for (int x = -radius; x < radius ; x++)
        {
            int hh = (int)System.Math.Sqrt(radius_sqr - x * x);
            int rx = p.x + x;
            int ph = p.y+hh;

            for (int y = p.y-hh; y < ph; y++)
                Draw(rx,y);
        }
    }

    // Draws a line between 2 2D texture points, using the Bresenham's line algorithm
    // See https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
    // Modified to put circles along the way
    public void DrawLine(Vector2Int p1, Vector2Int p2) {
        int dx = System.Math.Abs(p2.x - p1.x);
        int sx = p1.x < p2.x ? 1 : -1;
        int dy = -System.Math.Abs(p2.y - p1.y);
        int sy = p1.y < p2.y ? 1 : -1;
        int error = dx + dy;
        DrawCircle(p1);
        Vector2Int oldPos = p1;
        while (true){
            if (Vector2Int.Distance(oldPos,p1) > radius/2) {
                oldPos=p1;
                DrawCircle(p1);
            }
            if (p1.x == p2.x && p1.y == p2.y) break;
            int e2 = 2 * error;
            if (e2 >= dy){
                if (p1.x == p2.x) break;
                error = error + dy;
                p1.x = p1.x + sx;
            }
            if (e2 <= dx) {
                if (p1.y == p2.y) break;
                error = error + dx;
                p1.y = p1.y + sy;
            }
        }
    }
    
    /*public void DrawLine(Vector2Int p1, Vector2Int p2) {
        int dx = System.Math.Abs(p2.x - p1.x);
        int sx = p1.x < p2.x ? 1 : -1;
        int dy = -System.Math.Abs(p2.y - p1.y);
        int sy = p1.y < p2.y ? 1 : -1;
        int error = dx + dy;
        
        while (true){
            Draw(p1);
            if (p1.x == p2.x && p1.y == p2.y) break;
            int e2 = 2 * error;
            if (e2 >= dy){
                if (p1.x == p2.x) break;
                error = error + dy;
                p1.x = p1.x + sx;
            }
            if (e2 <= dx) {
                if (p1.y == p2.y) break;
                error = error + dx;
                p1.y = p1.y + sy;
            }
        }
    }*/



    void Update() {
        if (rayCaster.Held && rayCaster.RayHit.collider.gameObject == this.gameObject)
        {
            ContinusDraw(rayCaster.RayHit.point, false);
        }

        if (updated){
            texture.Apply();
            updated=false;
        }
    }


}
