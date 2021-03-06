﻿using FYFY;
using System;
using System.Collections.Generic;
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
public class PairELevelBool : Pair<ELevel, bool>
{
    public PairELevelBool(ELevel a, bool b) : base(a, b)
    {
    }
}

[Serializable]
public class PairEGalleryModelBool : Pair<EGalleryModel, bool>
{
    public PairEGalleryModelBool(EGalleryModel a, bool b) : base(a, b)
    {
    }
}

[Serializable]
public class PairEStatTrackedEntityInt : Pair<EStatTrackedEntity, int>
{
    public PairEStatTrackedEntityInt(EStatTrackedEntity a, int b) : base(a, b)
    {
    }
}

[Serializable]
public class Triple<K, T, E> : IEquatable<Triple<K, T, E>>
{
    public K key;
    public T a;
    public E b;

    public Triple(K key, T a, E b)
    {
        this.key = key;
        this.a = a; 
        this.b = b; 
    }

    public override string ToString()
    {
        return key + " (" + a + ", " + b + ")";
    }

    public bool Equals(Triple<K, T, E> triple)
    {
        if ( a.Equals(triple.a) && b.Equals(triple.b) ) { return true; }
        return false;
    }
}

[Serializable]
public class TripleEEffectFloatBool : Triple<EEffect, float, bool>
{
    public TripleEEffectFloatBool(EEffect key, float a, bool b) : base(key, a, b)
    {
    }
}

[Serializable]
public class TripleEEffectFloatFloat : Triple<EEffect, float, float>
{
    public TripleEEffectFloatFloat(EEffect key, float a, float b) : base(key, a, b)
    {
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

    public static GameObject clone(GameObject go, GameObject parent)
    {
        GameObject clone = UnityEngine.Object.Instantiate(go);
        clone.transform.SetParent(parent.transform);
        GameObjectManager.bind(clone);
        return clone;
    }
}
 