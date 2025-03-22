using OpenTK.Mathematics;
using P5CSLIB;
using P5CSLIB.Shapes;

namespace P5CS;

public class Particle : Circle
{
    public Particle(float xPos, float yPos, float diameter) : base(xPos, yPos, diameter)
    {
    }

    public Particle(Vector2 position, float diameter) : base(position, diameter)
    {
    }
    
    public void Update(FlowField2D field)
    {
        Move(field.Sample(Position) * 5000 * Globals.DeltaTime);
    }
}