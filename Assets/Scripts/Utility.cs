using System;
using System.Collections.Generic;

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

// https://forum.unity.com/threads/clever-way-to-shuffle-a-list-t-in-one-line-of-c-code.241052/#post-1596795
public static class IListExtensions {
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static T Pop<T>(this IList<T> ts)
    {
        int index = ts.Count - 1;
        if(index >= 0)
        {
            T obj = ts[ts.Count - 1];
            ts.RemoveAt(ts.Count - 1);
            return obj;
        }
        else
        {
            return default(T);
        }
    }
}

public static class Utility
{
     public static int Mod(int a, int b)
     {
         return (a % b + b) % b;
     }
}
 