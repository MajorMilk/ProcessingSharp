using OpenTK.Mathematics;

namespace P5CSLIB;
public static class PFuncs
{
    public static float Map(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (value - start1) * (stop2 - start2) / (stop1 - start1);
    }
    
    public static float Sq(float x) => x * x;
    public static float Abs(float x) => x < 0 ? -x : x;
    public static float Min(float x, float y) => x < y ? x : y;
    public static float Max(float x, float y) => x > y ? x : y;
    
    public static float Lerp(float x, float y, float a) => x * (1 - a) + y * a;
    public static Vector2 Lerp(Vector2 x, Vector2 y, float a) => x * (1 - a) + y * a;
    
    public static float Rand(float a, float b) => a + ((b-a) * (float)Globals.RND.NextDouble());
    
    public static int Floor(float x) => (int)Math.Floor(x);
    public static int Ceil(float x) => (int)Math.Ceiling(x);
    
    public static float Sqrt(float x) => MathF.Sqrt(x);
    public static float Pow(float x, float y) => MathF.Pow(x, y);
    public static float Exp(float x) => MathF.Exp(x);
    public static float Log(float x) => MathF.Log(x);
    public static float Log10(float x) => MathF.Log10(x);
    public static float Sin(float x) => MathF.Sin(x);
    public static float Cos(float x) => MathF.Cos(x);
    
    public static float Dist(float x1, float y1, float x2, float y2) => MathF.Sqrt(Sq(x1 - x2) + Sq(y1 - y2));
    public static float Dist(Vector2 a, Vector2 b) => Dist(a.X, a.Y, b.X, b.Y);
    
    public static float GetHeading(Vector2 center, Vector2 point)
    {
        Vector2 direction = point - center;
        float angleInRadians = MathF.Atan2(direction.Y, direction.X);

        return angleInRadians;
    }
    
    public static float VLength(Vector2 v) => (float) Math.Sqrt(v.X * v.X + v.Y * v.Y);


    //Later:
    //Perlin Noise 1,2 D
}