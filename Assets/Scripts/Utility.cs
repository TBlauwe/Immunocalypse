using System;

public class Pair<T> : IEquatable<Pair<T>>
{
    public T a;
    public T b;

    public Pair(T a, T b)
    {
        this.a = a; 
        this.b = b; 
    }

    public override string ToString()
    {
        return "(" + a + ", " + b + ")";
    }

    public bool Equals(Pair<T> pair)
    {
        if ( a.Equals(pair.a) && b.Equals(pair.b) ) { return true; }
        return false;
    }
}