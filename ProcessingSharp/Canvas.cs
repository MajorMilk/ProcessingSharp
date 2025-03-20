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
    
    public delegate void OnDrawDelegate();
    public OnDrawDelegate OnDraw;

    public Canvas(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        originalCanvasSize = Globals.CanvasSize;
        OnDraw = null;
        
    }

    public void AddShape(Shape shape)
    {
        shapes.Add(shape);
    }
    
    
    //very crude and bad, considering disabling resize
    protected override void OnResize(ResizeEventArgs e)
    {
        Globals.CanvasSize = new Vector2i(e.Width, e.Height);
        GL.Viewport(0, 0, Globals.CanvasSize.X, Globals.CanvasSize.Y);
        base.OnResize(e);
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.Enable(EnableCap.Multisample);
        GL.Disable(EnableCap.DepthTest);
        
        GL.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);
        GL.Viewport(0, 0, Globals.CanvasSize.X, Globals.CanvasSize.Y);
    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        Globals.UpdateProjectionMatrix();
        Globals.DeltaTime = (float)args.Time;


        float scaleX = 1;
        float scaleY = 1;

        foreach (var shape in shapes)
        {
            shape.Draw(scaleX, scaleY);
        }
        OnDraw?.Invoke();

        
        

        SwapBuffers();
    }


}