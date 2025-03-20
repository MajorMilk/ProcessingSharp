using OpenTK.Mathematics;

namespace P5CSLIB.Shapes;

public abstract class Shape
{
    public abstract Vector4i FillColor { get; set; }
    public abstract Vector4i StrokeColor { get; set; }
    public abstract int StrokeWeight { get; set; }
    public abstract void Draw(float scalex, float scaley);
    public abstract void Move(Vector2 offset);
}
