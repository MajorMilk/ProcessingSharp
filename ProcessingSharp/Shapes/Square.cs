using OpenTK.Mathematics;

namespace P5CSLIB.Shapes;

public class Square : Rect
{
    public Square(float xPos, float yPos, float radius) : base(xPos, yPos, radius, radius)
    {
        
    }

    public Square(Vector2i position, float radius) : base(position, radius, radius)
    {
        
    }
}