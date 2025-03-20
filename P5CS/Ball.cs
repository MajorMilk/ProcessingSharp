using OpenTK.Mathematics;
using P5CSLIB;
using P5CSLIB.Shapes;

namespace P5CS;

public class Ball : Circle
{
    public Vector2 Velocity { get; set; } = new(PFuncs.Rand(0, 0), 0);
    public float Diameter { get; set; }
    public Ball(float xPos, float yPos, float diameter) : base(xPos, yPos, diameter)
    {
        Diameter = diameter;
    }

    public Ball(Vector2 position, float diameter) : base(position, diameter)
    {
        Diameter = diameter;
    }

    public void CheckCollisionWalls()
    {
        // Check for collision with the bottom of the canvas
        if (Position.Y <= 0)
        {
            Position = new Vector2(Position.X, 0); // Snap to the bottom
            Velocity = new Vector2(Velocity.X, -Velocity.Y); // Bounce with some energy loss
        }
        else if (Position.Y >= Globals.CanvasSize.Y - Diameter)
        {
            Position = new Vector2(Position.X, Globals.CanvasSize.Y - (Diameter + 1)); // Snap to the bottom
            Velocity = new Vector2(Velocity.X, -Velocity.Y); // Bounce with some energy loss
        }
        else if (Position.X <= 0)
        {
            // Snap to the right or left
            Position = new Vector2(0, Position.Y);
            Velocity = new Vector2(-Velocity.X, Velocity.Y);
        }
        else if ( Position.X >= Globals.CanvasSize.X - Diameter)
        {
            // Snap to the right or left
            Position = new Vector2(Globals.CanvasSize.X - Diameter, Position.Y);
            Velocity = new Vector2(-Velocity.X, Velocity.Y);
        }
    }

    public void CheckCollision(ref Ball other)
    {
        var a = Center();
        var b = other.Center();
        if (PFuncs.Dist(a, b) < (Diameter / 2f) + (other.Diameter / 2f))
        {
            Console.WriteLine($"Collision detected between Ball {this} and {other}"); // Debug
            Velocity *= -0.9f;
            other.Velocity *= -0.9f;
        }
    }

    public void Update()
    {
        // Apply gravity
        Velocity += new Vector2(0, -98f * Globals.DeltaTime * 2);
        
        Move(Velocity);
        CheckCollisionWalls();
        
    }
}