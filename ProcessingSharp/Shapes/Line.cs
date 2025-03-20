using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace P5CSLIB.Shapes;

public class Line : Shape
{
    public override Vector4i FillColor { get; set; }
    public override Vector4i StrokeColor { get; set; }
    public override int StrokeWeight { get; set; }
    
    public Vector2 Start { get; set; }
    public Vector2 End { get; set; }
    
    protected int shaderProgram;

    public Line(float x1, float y1, float x2, float y2)
    {
        Start = new(x1, y1);
        End = new(x2, y2);
        FillColor = Globals.FillColor;
        StrokeColor = Globals.StrokeColor;
        StrokeWeight = Globals.StrokeWeight;
        shaderProgram = ShaderHelper.CreateShaderProgram();
    }

    public Line(Vector2 x1, Vector2 y1) : this(x1.X, x1.Y, y1.X, y1.Y)
    {

    }

    public override void Draw(float scalex, float scaley)
    {
        // Use OpenGL to draw the line
        GL.UseProgram(shaderProgram);
        
        // Scale the start and end points of the line
        Vector2 scaledStart = Start * new Vector2(scalex, scaley);
        Vector2 scaledEnd = End * new Vector2(scalex, scaley);

        // Set the color and stroke width for the line
        Vector4 strokeColor = new Vector4(
            StrokeColor.X / 255f,
            StrokeColor.Y / 255f,
            StrokeColor.Z / 255f,
            StrokeColor.W / 255f
        );

        GL.Uniform4f(GL.GetUniformLocation(shaderProgram, "strokeColor"), strokeColor.X, strokeColor.Y, strokeColor.Z, strokeColor.W);
        GL.Uniform1f(GL.GetUniformLocation(shaderProgram, "strokeWidth"), StrokeWeight);

        // Create the vertex data for the line
        float[] vertices = {
            scaledStart.X, scaledStart.Y, // Start point
            scaledEnd.X, scaledEnd.Y      // End point
        };

        // Set up the vertex buffer
        int vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsage.StaticDraw);

        // Set up the vertex array
        int vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // Draw the line
        GL.DrawArrays(PrimitiveType.Lines, 0, 2);

        // Cleanup
        GL.DeleteBuffer(vbo);
        GL.DeleteVertexArray(vao);
    }

    public override void Move(Vector2 offset)
    {
        Start += offset;
        End += offset;
    }
}