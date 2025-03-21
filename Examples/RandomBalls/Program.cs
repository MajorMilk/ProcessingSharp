using OpenTK.Graphics.Vulkan;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using P5CS;
using P5CSLIB;
using P5CSLIB.Shapes;
using static P5CSLIB.PFuncs;
class Program
{
    static void CheckBalls(Dictionary<int, Ball> balls, Canvas canvas)
    {
        Parallel.ForEach(balls, pairA =>
        {
            foreach (var pairB in balls)
            {
                // Process each unique pair only once
                if (pairA.Key >= pairB.Key) continue;

                Ball a = canvas.shapes[pairA.Key] as Ball;
                Ball b = canvas.shapes[pairB.Key] as Ball;

                Vector2 delta = b.Center() - a.Center();
                float distance = delta.Length;
                float minDistance = (a.Diameter / 2f) + (b.Diameter / 2f) - 10;

                if (distance < minDistance && distance > 0) // Collision detected
                {
                    // Unit normal vector (direction of collision)
                    Vector2 normal = delta / distance;

                    // Calculate the overlap distance
                    float overlap = minDistance - distance;

                    // Separate the balls by half the overlap distance to remove overlap
                    Vector2 correction = normal * overlap;

                    // Move balls away from each other
                    a.Move(-correction / 2);
                    b.Move(correction / 2);

                    // Apply elastic collision physics for velocity update
                    // Unit tangent vector (perpendicular to normal)
                    Vector2 tangent = new Vector2(-normal.Y, normal.X);

                    // Project the velocities onto the normal and tangent directions
                    float vAn = Vector2.Dot(a.Velocity, normal);
                    float vAt = Vector2.Dot(a.Velocity, tangent);
                    float vBn = Vector2.Dot(b.Velocity, normal);
                    float vBt = Vector2.Dot(b.Velocity, tangent);

                    // Swap the normal velocities for elastic collision
                    float vAnAfter = vBn;
                    float vBnAfter = vAn;

                    // Recombine the normal and tangential components of the velocities
                    Vector2 newVA = (vAnAfter * normal) + (vAt * tangent);
                    Vector2 newVB = (vBnAfter * normal) + (vBt * tangent);

                    a.Velocity = newVA;
                    b.Velocity = newVB;
                }
            }
        });
    }



    
    static void Main()
    {
        Dictionary<int, Ball> balls = [];
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
        int j = 0;
        using var canvas = new Canvas(gameWindowSettings, nativeWindowSettings);
        for (int i = 25; i < 1800; i += 25)
        {
            var b = new Ball(new Vector2(Rand(25, 725), Rand(100, 750)), 25);
            b.FillColor = new Vector4i((int)Rand(0, 255), (int)Rand(0, 255), (int)Rand(0, 255), 255);
            canvas.AddShape(j, b);
            balls[j] = b;
            canvas.OnDraw += (canvas.shapes[j++] as Ball).Update;
        }
        canvas.OnDraw += () => CheckBalls(balls, canvas);
        
        
        canvas.Run();
    }
}
