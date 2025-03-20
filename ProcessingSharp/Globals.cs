using OpenTK.Mathematics;

namespace P5CSLIB;

public static class Globals
{
    public static float DeltaTime = 0;
    public static bool Stroke = true;
    public static bool Fill = true;
    public static int StrokeWeight = 10; // Stroke weight in pixels
    public static Vector4i FillColor = new(255,255,255,255);
    public static Vector4i StrokeColor = new(0,0,0,255);
    public static Vector4 NormalizedFillColor => new(FillColor.X / 255f, FillColor.Y / 255f, FillColor.Z / 255f, FillColor.W / 255f);
    public static Vector4 NormalizedStrokeColor => new(StrokeColor.X / 255f, StrokeColor.Y / 255f, StrokeColor.Z / 255f, StrokeColor.W / 255f);
    public static Vector2i CanvasOffset = new(0,0); // used for changing the center point of the canvas
    public static Vector2i CanvasSize = new(800,800);
    
    
    public static float FrameRate = 144f; // Target frame rate
    
    //Not implemented 
    public static int MaxShapes = 1000; 
    public static bool DebugMode = false; 
    
    public static Matrix4 ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(
        CanvasOffset.X, CanvasSize.X,  
        CanvasOffset.Y,  CanvasSize.Y,          // Bottom, Top (inverted Y axis for OpenGL)
        -1, 1); 

    public static void UpdateProjectionMatrix()
    {
        float left = CanvasOffset.X;
        float right = Globals.CanvasSize.X;
        float bottom = CanvasOffset.Y;
        float top = Globals.CanvasSize.Y; // Flip y to match OpenGL's expectations

        ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, -1, 1);
    }

    public static Vector2 cavasOffset(float x)
    {
        return new(CanvasCenter.X - x, CanvasCenter.Y - x);
    }
    
    public static Vector4 NormalizeColor(Vector4 color) => new(color.X / 255f, color.Y / 255f, color.Z / 255f, color.W / 255f);

    public static Vector2i CanvasCenter => new(CanvasSize.X / 2, CanvasSize.Y / 2);
    
    public static Random RND = new();
}