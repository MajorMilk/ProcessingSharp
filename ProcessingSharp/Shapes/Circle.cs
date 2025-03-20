using OpenTK.Mathematics;

namespace P5CSLIB.Shapes;

public class Circle : Ellipse
{
    public Circle(float xPos, float yPos, float diameter) : base(xPos, yPos, diameter, diameter)
    {
        
    }

    public Circle(Vector2 position, float diameter) : base(position, new Vector2(diameter, diameter))
    {
        
    }
}