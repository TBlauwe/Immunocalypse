using System;

[Serializable]
public class ForceSpec
{
    public static readonly float DEFAULT_N = 1.5f;
    public static readonly float DEFAULT_A = 100;
    public static readonly float DEFAULT_B = 500;
    public static readonly float DEFAULT_M = 2.5f;
    public static readonly int DEFAULT_MASK = 0;

    public float N;
    public float M;
    public float A;
    public float B;
    public int Mask;

    public ForceSpec()
    {
        N = DEFAULT_N;
        M = DEFAULT_M;
        A = DEFAULT_A;
        B = DEFAULT_B;
        Mask = DEFAULT_MASK;
    }

    public ForceSpec(int mask, float n, float m, float a, float b)
    {
        N = n;
        M = m;
        A = a;
        B = b;
        Mask = mask;
    }

    public override string ToString()
    {
        return "(" + Mask + "," + N + "," + M + "," + A + "," + B + ")";
    }

    public override bool Equals(object obj)
    {
        if (obj != null && obj.GetType().Equals(this.GetType()))
        {
            ForceSpec spec = (ForceSpec)obj;
            return spec.Mask == Mask && spec.N == N && spec.M == M && spec.A == A && spec.B == B;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Mask << (int)N + (int)(A - B) >> (int)M;
    }
}
