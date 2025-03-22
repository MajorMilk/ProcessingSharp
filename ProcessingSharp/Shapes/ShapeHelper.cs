using OpenTK.Graphics.OpenGL;
namespace P5CSLIB.Shapes;

// This logic will eventually be replaced to allow custom shaders
public static class ShaderHelper
{
    public static int CreateShaderProgram()
    {
        string vertexShaderSource = @"
            #version 330 core
            layout (location = 0) in vec2 aPos;

            uniform mat4 projection;
            uniform vec2 position; // Shape center
            uniform vec2 canvasSize;

            void main()
            {
                vec2 worldPos = aPos;  // Apply transformation
                gl_Position = projection * vec4(worldPos, 0.0, 1.0);
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
        uniform bool circleMode;
        uniform vec2 size;  
        uniform vec2 position;

        void main()
        {
            // Normalize fragment position relative to the shape center
            vec2 uv = (gl_FragCoord.xy - position) / (size * 0.5);
            uv = uv * 2.0 - 1.0;

            float normalizedStrokeWidth = strokeWidth;

            if (circleMode)
            {
                float dist = length(uv); // Distance from center
                if (dist > 1.0) discard; // Outside the circle

                float edge = 1.0 - normalizedStrokeWidth;
                bool isStroke = dist >= edge;

                if (toggleFill && !isStroke)
                    FragColor = fillColor;
                else if (toggleStroke && isStroke)
                    FragColor = strokeColor;
                else
                    discard;
            }
            else
            {
                bool isInside = abs(uv.x) <= 1.0 && abs(uv.y) <= 1.0;
                bool isStroke = abs(uv.x) >= (1.0 - normalizedStrokeWidth) || abs(uv.y) >= (1.0 - normalizedStrokeWidth);

                if (toggleFill && isInside && !isStroke)
                    FragColor = fillColor;
                else if (toggleStroke && isStroke)
                    FragColor = strokeColor;
                else
                    discard;
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