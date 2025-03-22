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
    public Dictionary<int, Shape> shapes = new(); //maps an int id to a shape as to maintain a reference to it for external use
    
    private Vector2i originalCanvasSize;
    
    public delegate void OnDrawDelegate();
    public event OnDrawDelegate OnDraw;

    public Canvas(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        originalCanvasSize = Globals.CanvasSize;
        OnDraw = null;
        
    }

    public void AddShape(int id, Shape shape)
    {
        if(!shapes.TryAdd(id, shape)) throw new Exception("Shape id already exists in shape dictionary");
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
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);


        var color = Globals.NormalizedBackgroundColor;
        
        GL.ClearColor(color.X, color.Y, color.Z, color.W);
        GL.Viewport(0, 0, Globals.CanvasSize.X, Globals.CanvasSize.Y);
    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        Globals.FrameCount++;
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.DepthBufferBit);
        if (!Globals.FrameSmearMode)
            GL.Clear(ClearBufferMask.ColorBufferBit); //Uncomment this line to remove color blending
        Globals.UpdateProjectionMatrix();
        Globals.DeltaTime = (float)args.Time;
        
        float scaleX = 1f;
        float scaleY = 1f;
        

        foreach (var shape in shapes) // I get errors when trying to parallelize this
        {
            shape.Value.Draw(scaleX, scaleY);
        }
        Parallel.ForEach(OnDraw.GetInvocationList(), d => d.DynamicInvoke()); 
        
        SwapBuffers();
    }


}