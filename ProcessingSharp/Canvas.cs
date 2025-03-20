using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using P5CSLIB.Shapes;

namespace P5CSLIB;

public class Canvas : GameWindow
{
    //Just a list for now, maybe a dict in the future
    private List<Shape> shapes = new List<Shape>();
    
    private Vector2i originalCanvasSize;

    public Canvas(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        originalCanvasSize = Globals.CanvasSize;
    }

    public void AddShape(Shape shape)
    {
        shapes.Add(shape);
    }
    
    
    //very crude and bad, considering disabling resize
    protected override void OnResize(ResizeEventArgs e)
    {
        Globals.CanvasSize = new Vector2i(e.Width, e.Height);
        Globals.UpdateProjectionMatrix();
        GL.Viewport(0, 0, e.Width, e.Height);
        base.OnResize(e);
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.Enable(EnableCap.Multisample);
        
        GL.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);
        GL.Viewport(0, 0, Globals.CanvasSize.X, Globals.CanvasSize.Y);
    }
    private float time = 0;
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        
        Globals.UpdateProjectionMatrix();
        

        GL.Clear(ClearBufferMask.ColorBufferBit);


        float scaleX = Globals.CanvasSize.X / originalCanvasSize.X;
        float scaleY = Globals.CanvasSize.Y / originalCanvasSize.Y;

        foreach (var shape in shapes)
        {
            Vector2 offset = new((float)Math.Cos(time) *50, (float)Math.Sin(time) * 50);
            time += 0.01f;
            //shape.Move(offset);
            shape.Draw(scaleX, scaleY);
        }

        SwapBuffers();
    }


}