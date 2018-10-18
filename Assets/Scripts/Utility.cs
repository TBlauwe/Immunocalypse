using System;
using UnityEngine;

[Serializable]
public class Pair<T, E> : IEquatable<Pair<T, E>>
{
    public T a;
    public E b;

    public Pair(T a, E b)
    {
        this.a = a; 
        this.b = b; 
    }

    public override string ToString()
    {
        return "(" + a + ", " + b + ")";
    }

    public bool Equals(Pair<T, E> pair)
    {
        if ( a.Equals(pair.a) && b.Equals(pair.b) ) { return true; }
        return false;
    }
}

[Serializable]
public class ForceSpec
{
    private static readonly float DEFAULT_N = 1.5f;
    private static readonly float DEFAULT_A = 100;
    private static readonly float DEFAULT_B = 500;
    private static readonly float DEFAULT_M = 2.5f;
    private static readonly int DEFAULT_MASK = 0;

    [SerializeField] public float N { get; set; }
    [SerializeField] public float M { get; set; }
    [SerializeField] public float A { get; set; }
    [SerializeField] public float B { get; set; }
    [SerializeField] public int Mask { get; set; }

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