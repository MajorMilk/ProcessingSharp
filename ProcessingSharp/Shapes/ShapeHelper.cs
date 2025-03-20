using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace P5CSLIB.Shapes;

public static class ShaderHelper
{
    public static int CreateShaderProgram()
    {
        string vertexShaderSource = @"
            #version 330 core
            layout (location = 0) in vec2 aPos;
            uniform vec2 offset;
            uniform mat4 projection;
            void main()
            {
                gl_Position = projection * vec4(aPos + offset, 0.0, 1.0);
            }
        ";

        string fragmentShaderSource = @"
             #version 330 core
out vec4 FragColor;

uniform vec4 fillColor;
uniform vec4 strokeColor;
uniform float strokeWidth;
uniform bool toggleFill;
uniform bool toggleStroke;
uniform bool circleMode; // Enables circle rendering
uniform vec2 position;  // Shape center
uniform vec2 size;      // Shape size (width, height)
uniform float canvasWidth; // Canvas width (for scaling stroke width)

void main()
{
    // Normalize fragment position relative to the shape center
    vec2 uv = (gl_FragCoord.xy - position) / (size * 0.5); // Convert to range -1 to 1

    // Normalize stroke width in NDC space
    float normalizedStrokeWidth = strokeWidth / size.x; // Normalize based on shape's width

    if (circleMode)
    {
        float dist = length(uv); // Distance from center
        if (dist > 1.0) discard; // Discard pixels outside the circle

        // Calculate the stroke edge for circle
        float edge = 1.0 + normalizedStrokeWidth;
        bool isStroke = dist >= edge;

        if (toggleFill && !isStroke)
        {
            FragColor = fillColor;
        }
        else if (toggleStroke && isStroke)
        {
            FragColor = strokeColor;
        }
        else
        {
            discard;
        }
    }
    else
    {
        // Rectangular rendering
        float left = position.x - size.x / 2.0;
        float right = position.x + size.x / 2.0;
        float bottom = position.y - size.y / 2.0;
        float top = position.y + size.y / 2.0;

        bool isInside = gl_FragCoord.x >= left && gl_FragCoord.x <= right && 
                        gl_FragCoord.y >= bottom && gl_FragCoord.y <= top;

        // Check if the fragment is near the edge to render stroke
        bool isNearEdge = (gl_FragCoord.x >= left - normalizedStrokeWidth && gl_FragCoord.x <= left + normalizedStrokeWidth) || 
                          (gl_FragCoord.x >= right - normalizedStrokeWidth && gl_FragCoord.x <= right + normalizedStrokeWidth) ||
                          (gl_FragCoord.y >= bottom - normalizedStrokeWidth && gl_FragCoord.y <= bottom + normalizedStrokeWidth) ||
                          (gl_FragCoord.y >= top - normalizedStrokeWidth && gl_FragCoord.y <= top + normalizedStrokeWidth);

        if (toggleFill && isInside && !isNearEdge)
        {
            FragColor = fillColor;
        }
        else if (toggleStroke && isNearEdge)
        {
            FragColor = strokeColor;
        }
        else
        {
            discard;
        }
    }
}
        ";


        int vertexShader = CompileShader(ShaderType.VertexShader, vertexShaderSource);
        int fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentShaderSource);

        int shaderProgram = GL.CreateProgram();
        GL.AttachShader(shaderProgram, vertexShader);
        GL.AttachShader(shaderProgram, fragmentShader);
        GL.LinkProgram(shaderProgram);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        return shaderProgram;
    }

    private static int CompileShader(ShaderType type, string source)
    {
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        GL.GetShaderInfoLog(shader, out string infoLog);
        if (!string.IsNullOrEmpty(infoLog))
        {
            Console.WriteLine($"Shader Compilation Error ({type}): {infoLog}");
        }

        return shader;
    }
}