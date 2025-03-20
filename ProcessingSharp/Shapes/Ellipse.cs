using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace P5CSLIB.Shapes;

public class Ellipse : Rect
{
    public Ellipse(float xPos, float yPos, float xRadius, float yRadius) : base(xPos, yPos, xRadius, yRadius)
    {
        
    }

    public Ellipse(Vector2 position, Vector2 radius) : base(position, radius)
    {
        
    }

    public override void Draw(float scalex, float scaley)
    {
        GL.UseProgram(shaderProgram);
        
        Vector2 scaledSize = Size * new Vector2(scalex, scaley);
        Vector2 scaledPosition = Position * new Vector2(scalex, scaley);  // Scale the position as well if necessary

        
        Vector4 fillColor = Globals.NormalizeColor(FillColor);
        Vector4 strokeColor = Globals.NormalizeColor(StrokeColor);
        
        int projLocation = GL.GetUniformLocation(shaderProgram, "projection");
        Globals.UpdateProjectionMatrix();
        
        GL.UniformMatrix4f(projLocation, 1, false, ref Globals.ProjectionMatrix);
        
        int canvasWidthLocation = GL.GetUniformLocation(shaderProgram, "canvasWidth");
        GL.Uniform1f(canvasWidthLocation, Globals.CanvasSize.X);
        
        // Pass the size (width and height) as uniforms
        GL.Uniform2f(GL.GetUniformLocation(shaderProgram, "size"), scaledSize.X , scaledSize.Y);
        GL.Uniform2f(GL.GetUniformLocation(shaderProgram, "position"), scaledPosition.X, scaledPosition.Y);
        GL.Uniform1i(GL.GetUniformLocation(shaderProgram, "circleMode"), 1);
        GL.Uniform4f(GL.GetUniformLocation(shaderProgram, "fillColor"), fillColor.X, fillColor.Y, fillColor.Z, fillColor.W);
        GL.Uniform4f(GL.GetUniformLocation(shaderProgram, "strokeColor"), strokeColor.X, strokeColor.Y, strokeColor.Z, strokeColor.W);

        GL.Uniform1f(GL.GetUniformLocation(shaderProgram, "strokeWidth"), StrokeWeight / scaledSize.X);
        GL.Uniform1i(GL.GetUniformLocation(shaderProgram, "toggleFill"), Globals.Fill ? 1 : 0);
        GL.Uniform1i(GL.GetUniformLocation(shaderProgram, "toggleStroke"), Globals.Stroke ? 1 : 0);

        GL.BindVertexArray(vao);
        GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
    }
}