using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace P5CSLIB.Shapes;

public class Ellipse : Rect
{
    public Ellipse(float xPos, float yPos, float xDiameter, float yDiameter) : base(xPos, yPos, xDiameter, yDiameter)
    {
        
    }

    public Ellipse(Vector2 position, Vector2 radius) : base(position, radius)
    {
        
    }

    
    private Vector2[] localVertices = new Vector2[4];
    float[] rotatedVertices = new float[8];
    public override void Draw(float scalex, float scaley)
    {
        GL.UseProgram(shaderProgram);

        Vector2 scaledSize = Size * new Vector2(scalex, scaley);
        Vector2 scaledPosition = Position * new Vector2(scalex, scaley);

        Vector4 fillColor = Globals.NormalizeColor(FillColor);
        Vector4 strokeColor = Globals.NormalizeColor(StrokeColor);

        int projLocation = GL.GetUniformLocation(shaderProgram, "projection");
        Globals.UpdateProjectionMatrix();

        GL.UniformMatrix4f(projLocation, 1, false, ref Globals.ProjectionMatrix);

        int canvasWidthLocation = GL.GetUniformLocation(shaderProgram, "canvasWidth");
        GL.Uniform1f(canvasWidthLocation, Globals.CanvasSize.X);

        GL.Uniform2f(GL.GetUniformLocation(shaderProgram, "size"), scaledSize.X, scaledSize.Y);
        GL.Uniform2f(GL.GetUniformLocation(shaderProgram, "position"), scaledPosition.X, scaledPosition.Y);
        GL.Uniform1i(GL.GetUniformLocation(shaderProgram, "circleMode"), 1);
        GL.Uniform4f(GL.GetUniformLocation(shaderProgram, "fillColor"), fillColor.X, fillColor.Y, fillColor.Z, fillColor.W);
        GL.Uniform4f(GL.GetUniformLocation(shaderProgram, "strokeColor"), strokeColor.X, strokeColor.Y, strokeColor.Z, strokeColor.W);

        GL.Uniform1f(GL.GetUniformLocation(shaderProgram, "strokeWidth"), StrokeWeight / scaledSize.X);
        GL.Uniform1i(GL.GetUniformLocation(shaderProgram, "toggleFill"), Globals.Fill ? 1 : 0);
        GL.Uniform1i(GL.GetUniformLocation(shaderProgram, "toggleStroke"), Globals.Stroke ? 1 : 0);

        float radians = RotationAngle;
        float cosTheta = MathF.Cos(radians);
        float sinTheta = MathF.Sin(radians);

        localVertices[0] = new Vector2(-xDiameter, -yDiameter);
        localVertices[1] = new Vector2(xDiameter, -yDiameter);
        localVertices[2] = new Vector2(xDiameter, yDiameter);
        localVertices[3] = new Vector2(-xDiameter, yDiameter);
        

        

        // Apply rotation transformation
        for (int i = 0; i < localVertices.Length; i++)
        {
            Vector2 local = localVertices[i];

            float rotatedX = local.X * cosTheta - local.Y * sinTheta;
            float rotatedY = local.X * sinTheta + local.Y * cosTheta;

            rotatedVertices[i * 2] = (Position.X + rotatedX) * scalex;
            rotatedVertices[i * 2 + 1] = (Position.Y + rotatedY) * scaley;
        }

        // Update buffer data with rotated vertices
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, rotatedVertices.Length * sizeof(float), rotatedVertices, BufferUsage.DynamicDraw);

        // Draw the rotated shape
        GL.BindVertexArray(vao);
        GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
    }
}