# ProcessingSharp

A C# library that aims to bring similar functionality to the Java library 'Processing', to C# using OpenTK.

## How to:

- Clone the Repo
- Type your code within Program.cs in the P5CS project
- Global settings can be found in the Globals.cs in the ProcessingSharp project

Not all features are yet to be added as this is a work in progress

## Coming Soon™

- Perlin noise
- Lines and points
- Better code lol
- 3D? Maybe.

## Already Added:

- Circles, Ellipses, Rectangles, Squares
- Shape specific fill color, stroke color, stroke weight
- Transformations and rotations
- Static functions from Processing such as map, dist, etc.

# Why use this over Processing?

Dont... Well not yet anyway. That is unless you prefer C# over Java as I do. I'm developing this in the hope that this library becomes as functional as Processing.

This library is decently efficient so far, hopefully that will remain true in the future.

# Docs

## Hello Circle

Begin by cloning this repository. Then navigate to the P5CS project. This is where you will type your code, similar to a P5JS project.

You'll find that an example is already provided. If you wish to start from fresh, simply remove all of the example code such that you're left with...

```csharp
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using P5CS;
using P5CSLIB;
using static P5CSLIB.PFuncs; // Optional global functions similar to processing
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
            Title = "Name of your window",
            WindowBorder = WindowBorder.Fixed, // Resizeable? (very buggy if so)
            WindowState = WindowState.Normal,
        };
        using var canvas = new Canvas(gameWindowSettings, nativeWindowSettings);
        
        
        //Here is where you should place your code
        
        
        canvas.Run();
    }
}
```

In this state, nothing will be rendered. First, you'll need to create a shape object:
```csharp
Circle circle = new(Vector2 Position, float radius);
```
Please note that all Vector2's used are from OpenTK.Mathematics, not System.Numerics.

From here, you can alter the properties of your circle:
```csharp
circle.FillColor = new Vector4i(R,G,B,A); //0-255 Colors are normalized automatically
circle.StrokeColor = new Vector4i(R,G,B,A);
circle.StrokeWeight = n; //Stroke weight in pixels
```

Now you can add your shape to the rendering pipeline:
```csharp
canvas.AddShape(int ID, circle);
```
The ID here is a key in a dictionary that links said ID to a shape on the canvas. This is so that you can maintain a reference to a specific shape.

Now you should have a program like this:
```csharp
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using P5CS;
using P5CSLIB;
using static P5CSLIB.PFuncs; // Optional global functions similar to processing
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
            Title = "Name of your window",
            WindowBorder = WindowBorder.Fixed, // Resizeable? (very buggy if so)
            WindowState = WindowState.Normal,
        };
        using var canvas = new Canvas(gameWindowSettings, nativeWindowSettings);
        
        // CanvasCenter is a dynamic property that always points to the center of the window
        Circle circle = new(Globals.CanvasCenter, 200);
        circle.FillColor = new Vector4i(0,255,0,255); //0-255 Colors are normalized automatically
        circle.StrokeColor = new Vector4i(0,0,0,255);
        circle.StrokeWeight = 10; //Stroke weight in pixels
        
        canvas.AddShape(0, circle);
        
        canvas.Run();
    }
}
```
Congratulations, you now have a green circle.

### OnDraw Behavior
When you add a shape to the canvas, it is automatically drawn every frame. To run code every frame along side the drawing of the shape, you'll need to add a subscriber to the canvas' OnDraw action like so:
```csharp
canvas.OnDraw += () => circle.Move(new Vector2(50 * Cos(Globals.FrameCount / 1000), 50 * Sin(Globals.FrameCount / 1000)));
```
Here, the Vector2 passed to the Move function is a displacement vector that will be added to the shapes position. There is also a Rotate function that rotates the shape by an angle.

All together you should have:
```csharp
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using P5CSLIB.Shapes;
using P5CSLIB;
using static P5CSLIB.PFuncs; // Optional global functions similar to processing
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
            Title = "Name of your window",
            WindowBorder = WindowBorder.Fixed, // Resizeable? (very buggy if so)
            WindowState = WindowState.Normal,
        };
        using var canvas = new Canvas(gameWindowSettings, nativeWindowSettings);
        
        // CanvasCenter is a dynamic property that always points to the center of the window
        Circle circle = new(Globals.CanvasCenter, 200);
        circle.FillColor = new Vector4i(0,255,0,255); //0-255 Colors are normalized automatically
        circle.StrokeColor = new Vector4i(0,0,0,255);
        circle.StrokeWeight = 10; //Stroke weight in pixels
        
        canvas.AddShape(0, circle);
        
        canvas.OnDraw += () => circle.Move(new Vector2(100 * Cos(Globals.FrameCount / 100f), 100 * Sin(Globals.FrameCount / 100f)));
        
        canvas.Run();
    }
}
```
Congratulations, now you have a green circle moving in a circle.

## Custom Shaders
Most shapes derive from a rect, or a shape that derives from a rect, and the shaders used on it are determined by its protected property called 'shaderProgram'

For those not familiar with creating shaders, you can look through the OpenTK docs [here](https://opentk.net/learn/chapter1/4-shaders.html)

Alternatively, you could look at  *ProcessingSharp/Shapes/ShapeHelper.cs* and create a new helper function for creating your custom shader program.

*Note: shaderProgram is an int, you need to use the opengl functions provided by OpenTK to get and assign that int*

The way you would do this is as follows:
```csharp
using OpenTK.Graphics.OpenGL;
namespace P5CSLIB.Shapes;

public class SomeClass
{
    public static int CreateShaderProgram()
    {
        string vertexShaderSource = "YOUR VERTEX SHADER SOURCE CODE";

        string fragmentShaderSource = "YOUR FRAGMENT SHADERS SOURCE CODE";


        int vertexShader = CompileShader(ShaderType.VertexShader, vertexShaderSource);
        int fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentShaderSource);

        int shaderProgram = GL.CreateProgram();
        GL.AttachShader(shaderProgram, vertexShader);
        GL.AttachShader(shaderProgram, fragmentShader);
        GL.LinkProgram(shaderProgram);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        return shaderProgram;
    }

    private static int CompileShader(ShaderType type, string source)
    {
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        GL.GetShaderInfoLog(shader, out string infoLog);
        if (!string.IsNullOrEmpty(infoLog))
        {
            Console.WriteLine($"Shader Compilation Error ({type}): {infoLog}");
        }

        return shader;
    }
}
//Then in your custom shape object that inherits from Rect (including circles and ellipses)...
shaderProgram = SomeClass.CreateShaderProgram()
```
Now your shape should have a custom shader applied to it.

### IMPORTANT NOTE:
Circles and ellipses are derived from the Rect class. This is because the rendering circles and ellipses is handled by the default shader.
If you want to put your own shader on a circle or ellipse, you'll need to account for that in your shader.

## Shapes

### Shape
- The main object from which all 2D shapes derive from
### Rect
- The second most main shape that most other 2D shapes will inherit from
- Includes functions like Move and Rotate, as well as properties that define its vertices and center point
### Square
- Just a Rect with equal diameters.
### Ellipse
- Inherits from Rect
- Has its own Draw function that tells the shader to enable its circular rendering mode.
### Circle
- Just an ellipse with equal diameters.

## Globals
This library has a multitude of global variables that define default behavior in the render pipeline such as:

- float DeltaTime -- Set by OpenTK each frame
- long FrameCount -- number of frames that have been rendered
- bool Stroke -- enables or disables stroke
- bool Fill -- enables or disables fill
- int StrokeWeight -- default Stroke Weight
- Vector4i FillColor -- RGBA 0-255
- Vector4i StrokeColor -- RGBA 0-255
- Vector4i BackgroundColor -- RGBA 0-255
- bool FrameSmearMode -- if enabled, the renderer will no longer clear the canvas each frame
- float FrameRate -- Target frame rate
- Random RND -- a global random object to generate random behavior

## Static Functions
In the interest of reducing verbosity, this library includes a public static class called PFuncs that contains many useful functions such as:

```csharp
public static float Map(float value, float start1, float stop1, float start2, float stop2)
{
    return start2 + (value - start1) * (stop2 - start2) / (stop1 - start1);
}

public static float Sq(float x) => x * x;
public static float Abs(float x) => x < 0 ? -x : x;
public static float Min(float x, float y) => x < y ? x : y;
public static float Max(float x, float y) => x > y ? x : y;

public static float Lerp(float x, float y, float a) => x * (1 - a) + y * a;
public static Vector2 Lerp(Vector2 x, Vector2 y, float a) => x * (1 - a) + y * a;

public static float Rand(float a, float b) => a + ((b-a) * (float)Globals.RND.NextDouble());

public static int Floor(float x) => (int)Math.Floor(x);
public static int Ceil(float x) => (int)Math.Ceiling(x);

public static float Sqrt(float x) => MathF.Sqrt(x);
public static float Pow(float x, float y) => MathF.Pow(x, y);
public static float Exp(float x) => MathF.Exp(x);
public static float Log(float x) => MathF.Log(x);
public static float Log10(float x) => MathF.Log10(x);
public static float Sin(float x) => MathF.Sin(x);
public static float Cos(float x) => MathF.Cos(x);

public static float Dist(float x1, float y1, float x2, float y2) => MathF.Sqrt(Sq(x1 - x2) + Sq(y1 - y2));
public static float Dist(Vector2 a, Vector2 b) => Dist(a.X, a.Y, b.X, b.Y);

public static float GetHeading(Vector2 center, Vector2 point)
{
    Vector2 direction = point - center;
    float angleInRadians = MathF.Atan2(direction.Y, direction.X);

    return angleInRadians;
}

public static float VLength(Vector2 v) => (float) Math.Sqrt(v.X * v.X + v.Y * v.Y);
```

To use these functions statically, make sure to include it in your Program.cs file:
```csharp
using static P5CSLIB.PFuncs;
```