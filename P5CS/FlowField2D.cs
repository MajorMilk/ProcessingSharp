using OpenTK.Mathematics;

namespace P5CS;

public class FlowField2D
{
    private Func<Vector2, Vector2> Field { get; }
    
    /// <summary>
    /// Simply sees a flow-field as a function that maps a vector to another vector
    /// </summary>
    /// <param name="func"></param>
    public FlowField2D(Func<Vector2, Vector2> func)
    {
        Field = func;
    }

    public Vector2 Sample(Vector2 position)
    {
        return Field(position).Normalized();
    }
}