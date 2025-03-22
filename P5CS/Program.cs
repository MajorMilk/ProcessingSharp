using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using P5CS;
using P5CSLIB;
using static P5CSLIB.PFuncs;
class Program
{
    static Vector2 SimpleFlowField(Vector2 point)
    {
        
        float x = point.X ;
        float y = point.Y ;
        
        float cx = point.X - Globals.CanvasCenter.X;
        float cy = point.Y - Globals.CanvasCenter.Y;
        float wStretch = x + y *Abs(GetHeading(Globals.CanvasCenter, point));

        // Noise angle calculation
        Vector2 noiseVector = point.Normalized() - Globals.CanvasCenter;
        float noiseAngle = GetHeading(Globals.CanvasCenter, noiseVector);

        // Smooth radial attraction
        Vector2 toCenter = point - Globals.CanvasCenter;

        float distToCenter = VLength(toCenter);
        float maxDist = (float)Math.Sqrt(cx * cx + cy * cy + wStretch * wStretch);
        float wOffset = wStretch * Map(VLength(toCenter), 0, maxDist, 0.0001f, 1f);
        float gravityStrength = Map(distToCenter, 0, maxDist + 500, 1000f, 0f);
    
        toCenter = toCenter.Normalized();
        float ratio = Map(gravityStrength, 0, 1, 0, (float)Math.Sqrt(wOffset));
        float finalAngle = Lerp(noiseAngle, GetHeading(Globals.CanvasCenter ,toCenter), 
            (gravityStrength * gravityStrength) / (ratio / Sq(distToCenter) / ((ratio) /  (distToCenter))) / wOffset);

    
        return new Vector2(MathF.Cos(finalAngle + MathF.PI/4), MathF.Sin(finalAngle + MathF.PI/4));
    }

    static void Main()
    {
        Globals.FrameSmearMode = true; // Enable frame smearing
        var gameWindowSettings = new GameWindowSettings()
        {
            UpdateFrequency = Globals.FrameRate,
        };
        var nativeWindowSettings = new NativeWindowSettings() 
        { 
            ClientSize = Globals.CanvasSize, 
            Title = "Processing in C#",
            WindowBorder = WindowBorder.Fixed,
            WindowState = WindowState.Normal,
        };
        var field = new FlowField2D(SimpleFlowField);
        int j = 0;
        using var canvas = new Canvas(gameWindowSettings, nativeWindowSettings);
        for (int i = 1; i < 3000; i++)
        {
            var p = new Particle(new Vector2(Rand(0, 800), Rand(0, 800)), 1);
            p.FillColor = new Vector4i(0,255,0, 50);
            p.StrokeWeight = 0;
            canvas.AddShape(i, p);

            var pRef = p;

            canvas.OnDraw += () => pRef.Update(field);
        }
        
        
        canvas.Run();
    }
}
