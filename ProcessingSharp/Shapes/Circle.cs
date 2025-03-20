using OpenTK.Mathematics;

namespace P5CSLIB.Shapes;

public class Circle : Ellipse
{
    public Circle(float xPos, float yPos, float radius) : base(xPos, yPos, radius, radius)
    {
        
    }

    public Circle(Vector2 position, float radius) : base(position, new Vector2(radius, radius))
    {
        
    }
}