using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using P5CSLIB;
using P5CSLIB.Shapes;

class Program
{
    static void Main()
    {
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

        using var canvas = new Canvas(gameWindowSettings, nativeWindowSettings);
        for (int i = 50; i < 800; i += 50)
        {
            var shape = new Rect(new Vector2i(i, i), new Vector2i(10, 50));
            shape.FillColor = new Vector4i(255, (int)((i/1600f)*255), (int)((i/800f)*255), 255);
            shape.StrokeColor = new Vector4i((int)((i/1600f)*255), 255, (int)((i/800f)*255), 255);
            canvas.AddShape(shape);
        }
        canvas.AddShape(new Ellipse(Globals.CanvasCenter, new Vector2i(100, 100)));
        canvas.Run();
    }
}
