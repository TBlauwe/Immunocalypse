using System;

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