using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace P5CSLIB.Shapes;




public class Rect : Shape
{
    protected int vbo, vao, ebo;
    protected int shaderProgram;
    
    public Vector2 Position;
    public Vector2 Size;
    
    public override Vector4i FillColor { get; set; }
    public override Vector4i StrokeColor { get; set; }
    public override int StrokeWeight { get; set; }
    
    public int xDiameter => (int)Size.X;
    public int yDiameter => (int)Size.Y;
    
    public override float RotationAngle { get; set; }
    
    private Vector2 CenterOffset => Position - Size / 4;
    
    public Vector2 TopLeft => Position;
    public Vector2 TopRight => new(Position.X + Size.X, Position.Y);
    public Vector2 BottomLeft => new(Position.X, Position.Y + Size.Y);
    public Vector2 BottomRight => new(Position.X + Size.X, Position.Y + Size.Y);
    //public Vector2 Center => Position - Size / 4;
    

    public Rect(float centerX, float centerY, float xDiameter, float yDiameter)
    {
        Position = new Vector2(centerX, centerY);
        Size = new Vector2(xDiameter * 2, yDiameter * 2); // Full width/height
        Position = CenterOffset;
        FillColor = Globals.FillColor;
        StrokeColor = Globals.StrokeColor;
        StrokeWeight = Globals.StrokeWeight;
        float[] vertices = {
            centerX - xDiameter, centerY - yDiameter, // Bottom-left
            centerX + xDiameter, centerY - yDiameter, // Bottom-right
            centerX + xDiameter, centerY + yDiameter, // Top-right
            centerX - xDiameter, centerY + yDiameter  // Top-left
        };

        uint[] indices = { 0, 1, 2, 2, 3, 0 };

        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsage.StaticDraw);

        ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsage.StaticDraw);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        shaderProgram = ShaderHelper.CreateShaderProgram();
    }

    public Rect(Vector2 position, Vector2 radius) : this(position.X, position.Y, radius.X, radius.Y)
    {
        
    }

    public Rect(Vector2 position, float xDiameter, float yDiameter) : this(position.X, position.Y, xDiameter, yDiameter)
    {
        
    }

    public override void Rotate(float angle, bool rads = false)
    {
        RotationAngle += rads ? angle : MathHelper.DegreesToRadians(angle);
    }

    public override void Move(Vector2 displacement)
    {
        var tmp = new Vector2(20 * displacement.X / Globals.CanvasSize.X, 20 * displacement.Y / Globals.CanvasSize.Y);
        Position += tmp;
    }

    public override void Draw(float scalex, float scaley)
    {
        GL.UseProgram(shaderProgram);

        Vector2 scaledSize = Size * new Vector2(scalex, scaley);
        Vector2 scaledPosition = Position * new Vector2(scalex, scaley);

        Vector4 fillColor = Globals.NormalizeColor(FillColor);
        Vector4 strokeColor = Globals.NormalizeColor(StrokeColor);

        int projLocation = GL.GetUniformLocation(shaderProgram, "projection");
        GL.UniformMatrix4f(projLocation, 1, false, ref Globals.ProjectionMatrix);

        int canvasWidthLocation = GL.GetUniformLocation(shaderProgram, "canvasWidth");
        GL.Uniform1f(canvasWidthLocation, Globals.CanvasSize.X);
        
        GL.Uniform2f(GL.GetUniformLocation(shaderProgram, "canvasSize"), Globals.CanvasSize.X, Globals.CanvasSize.Y);

        GL.Uniform2f(GL.GetUniformLocation(shaderProgram, "size"), scaledSize.X, scaledSize.Y);
        GL.Uniform2f(GL.GetUniformLocation(shaderProgram, "position"), scaledPosition.X, scaledPosition.Y);
        GL.Uniform1i(GL.GetUniformLocation(shaderProgram, "circleMode"), 0);
        GL.Uniform4f(GL.GetUniformLocation(shaderProgram, "fillColor"), fillColor.X, fillColor.Y, fillColor.Z, fillColor.W);
        GL.Uniform4f(GL.GetUniformLocation(shaderProgram, "strokeColor"), strokeColor.X, strokeColor.Y, strokeColor.Z, strokeColor.W);

        GL.Uniform1f(GL.GetUniformLocation(shaderProgram, "strokeWidth"), StrokeWeight / scaledSize.X);
        GL.Uniform1i(GL.GetUniformLocation(shaderProgram, "toggleFill"), Globals.Fill ? 1 : 0);
        GL.Uniform1i(GL.GetUniformLocation(shaderProgram, "toggleStroke"), Globals.Stroke ? 1 : 0);

        // Apply rotation matrix
        float radians = RotationAngle;
        float cosTheta = MathF.Cos(radians);
        float sinTheta = MathF.Sin(radians);

        // Define the original rectangle corners relative to center
        Vector2[] localVertices = new Vector2[]
        {
            new Vector2(Position.X, Position.Y),               // Bottom-left
            new Vector2(Position.X + Size.X, Position.Y),      // Bottom-right
            new Vector2(Position.X + Size.X, Position.Y + Size.Y), // Top-right
            new Vector2(Position.X, Position.Y + Size.Y)       // Top-left
        };

        float[] rotatedVertices = new float[8];

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

