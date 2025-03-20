using OpenTK.Mathematics;

namespace P5CSLIB.Shapes;

public class Square : Rect
{
    public Square(float xPos, float yPos, float diameter) : base(xPos, yPos, diameter, diameter)
    {
        
    }

    public Square(Vector2i position, float diameter) : base(position, diameter, diameter)
    {
        
    }
}