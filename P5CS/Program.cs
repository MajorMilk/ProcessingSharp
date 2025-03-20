using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using P5CS;
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
        var ball = new Ball(Globals.CanvasCenter, 100);
        canvas.AddShape(ball);
        canvas.AddShape(new Circle(new(200,400), 50));
        canvas.AddShape(new Circle(new(600,400), 50));
        canvas.AddShape(new Circle(new(200,200), 50));
        canvas.AddShape(new Circle(new(600,200), 50));
        canvas.AddShape(new Circle(new(200,600), 50));
        canvas.AddShape(new Circle(new(600,600), 50));
        //canvas.OnDraw += () => ball.Update();
        //canvas.AddShape(new Circle(Globals.CavasOffset(-10 * MathF.Sqrt(2)), 100));
        
        canvas.Run();
    }
}
