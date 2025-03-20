using OpenTK.Mathematics;

namespace P5CSLIB;

public static class Globals
{
    public static bool Stroke = true;
    public static bool Fill = true;
    public static int StrokeWeight = 1000; // Stroke weight in pixels * ~100 --Stroke for ellipses and circles is bugged
    public static Vector4i FillColor = new(255,255,255,255);
    public static Vector4i StrokeColor = new(0,0,0,255);
    public static Vector4 NormalizedFillColor => new(FillColor.X / 255f, FillColor.Y / 255f, FillColor.Z / 255f, FillColor.W / 255f);
    public static Vector4 NormalizedStrokeColor => new(StrokeColor.X / 255f, StrokeColor.Y / 255f, StrokeColor.Z / 255f, StrokeColor.W / 255f);
    public static Vector2i CanvasOffset = new(-400,-400); // used for changing the center point of the canvas
    public static Vector2i CanvasSize = new(800,800);
    
    
    public static float FrameRate = 60f; // Target frame rate
    
    //Not implemented 
    public static int MaxShapes = 1000; 
    public static bool DebugMode = false; 
    
    public static Matrix4 ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(
        0, CanvasSize.X,  
        0,  CanvasSize.Y,          // Bottom, Top (inverted Y axis for OpenGL)
        -1, 1); 

    public static void UpdateProjectionMatrix()
    {
        float left = 0;
        float right = Globals.CanvasSize.X;
        float bottom = 0;
        float top = Globals.CanvasSize.Y; // Flip y to match OpenGL's expectations

        ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, -1, 1);
    }
    
    public static Vector4 NormalizeColor(Vector4 color) => new(color.X / 255f, color.Y / 255f, color.Z / 255f, color.W / 255f);

    public static Vector2i CanvasCenter => new(CanvasSize.X / 2, CanvasSize.Y / 2);
}