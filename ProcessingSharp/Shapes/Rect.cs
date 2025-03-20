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
    
    public int xRadius => (int)Size.X;
    public int yRadius => (int)Size.Y;
    

    public Rect(float centerX, float centerY, float xRadius, float yRadius)
    {
        Position = new Vector2(centerX, centerY);
        Size = new Vector2(xRadius * 2, yRadius * 2); // Full width/height
        FillColor = Globals.FillColor;
        StrokeColor = Globals.StrokeColor;
        StrokeWeight = Globals.StrokeWeight;
        float[] vertices = {
            centerX - xRadius, centerY - yRadius, // Bottom-left
            centerX + xRadius, centerY - yRadius, // Bottom-right
            centerX + xRadius, centerY + yRadius, // Top-right
            centerX - xRadius, centerY + yRadius  // Top-left
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

    public Rect(Vector2 position, float xRadius, float yRadius) : this(position.X, position.Y, xRadius, yRadius)
    {
        
    }
    
    public override void Move(Vector2 displacement)
    {
        // Update position
        var tmp = new Vector2(20 * displacement.X / Globals.CanvasSize.X, 20 * displacement.Y / Globals.CanvasSize.Y);
        Position += tmp;
        
        float[] updatedVertices = {
            Position.X - xRadius, Position.Y - yRadius, // Bottom-left
            Position.X + xRadius, Position.Y - yRadius, // Bottom-right
            Position.X + xRadius, Position.Y + yRadius, // Top-right
            Position.X - xRadius, Position.Y + yRadius  // Top-left
        };

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, updatedVertices.Length * sizeof(float), updatedVertices, BufferUsage.StaticDraw);
    }

    public override void Draw(float scalex, float scaley)
    {
        GL.UseProgram(shaderProgram);
        
        Vector2 scaledSize = Size * new Vector2(scalex, scaley);
        Vector2 scaledPosition = Position * new Vector2(scalex, scaley);  // Scale the position as well if necessary

        
        Vector4 fillColor = Globals.NormalizeColor(FillColor);
        Vector4 strokeColor = Globals.NormalizeColor(StrokeColor);
        
        int projLocation = GL.GetUniformLocation(shaderProgram, "projection");
        //Globals.UpdateProjectionMatrix();
        
        GL.UniformMatrix4f(projLocation, 1, false, ref Globals.ProjectionMatrix);

        int canvasWidthLocation = GL.GetUniformLocation(shaderProgram, "canvasWidth");
        GL.Uniform1f(canvasWidthLocation, Globals.CanvasSize.X);
        
        // Pass the size (width and height) as uniforms
        GL.Uniform2f(GL.GetUniformLocation(shaderProgram, "size"), scaledSize.X , scaledSize.Y);
        GL.Uniform2f(GL.GetUniformLocation(shaderProgram, "position"), scaledPosition.X, scaledPosition.Y);
        GL.Uniform1i(GL.GetUniformLocation(shaderProgram, "circleMode"), 0);
        GL.Uniform4f(GL.GetUniformLocation(shaderProgram, "fillColor"), fillColor.X, fillColor.Y, fillColor.Z, fillColor.W);
        GL.Uniform4f(GL.GetUniformLocation(shaderProgram, "strokeColor"), strokeColor.X, strokeColor.Y, strokeColor.Z, strokeColor.W);

        GL.Uniform1f(GL.GetUniformLocation(shaderProgram, "strokeWidth"), StrokeWeight / scaledSize.X);
        GL.Uniform1i(GL.GetUniformLocation(shaderProgram, "toggleFill"), Globals.Fill ? 1 : 0);
        GL.Uniform1i(GL.GetUniformLocation(shaderProgram, "toggleStroke"), Globals.Stroke ? 1 : 0);

        GL.BindVertexArray(vao);
        GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
    }
}

