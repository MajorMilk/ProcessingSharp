namespace P5CSLIB.Shapes;

public static class PFuncs
{
    public static float Map(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (value - start1) * (stop2 - start2) / (stop1 - start1);
    }
    
    //ToDo: implement the rest lol
}