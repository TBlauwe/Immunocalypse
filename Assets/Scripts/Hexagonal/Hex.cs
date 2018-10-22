// Class describing an hexagon using cube coordinates (x + y + z = 0)
// using the following link : https://www.redblobgames.com/grids/hexagons/#range

using System;
using System.Collections.Generic;

public class Hex : IEquatable<Hex> {

    public readonly int q;
    public readonly int r;
    public readonly int s;

    public static readonly Hex[] Directions = {
        new Hex(+1, -1, 0), new Hex(+1, 0, -1), new Hex(0, +1, -1),
        new Hex(-1, +1, 0), new Hex(-1, 0, +1), new Hex(0, -1, +1)
    };

    public Hex(int _q, int _r, int _s)
    {
        q = _q;
        r = _r;
        s = _s;
    }

    public bool Equals(Hex other)
    {
        return other.q == q && other.r == r && other.s == s;
    }

    public static Hex Add(Hex a, Hex b)
    {
        return new Hex(a.q + b.q, a.r + b.r, a.s + b.s);
    }

    public static Hex Substract(Hex a, Hex b)
    {
        return new Hex(a.q - b.q, a.r - b.r, a.s - b.s);
    }

    public static Hex Scale(Hex a, int k)
    {
        return new Hex(a.q * k, a.r * k, a.s * k);
    }

    public static Pair<int, int> ToIndex(Hex hex)
    {
        int h = hex.q + (hex.s - (hex.s&1)) / 2;
        int w = hex.s;
        return new Pair<int, int>(h, w);
    }

    public static Pair<double, double> ToCoordinate(Hex hex, float sizeX, float sizeZ)
    {
        double x = (Math.Sqrt(3.0) * hex.q + Math.Sqrt(3.0) / 2.0 * hex.r) * sizeX;
        double z = (1.5 * hex.r) * sizeZ;
        return new Pair<double, double>(x, z);
    }

    // Returns the coordinates to go towards direction - Pointy configuration
    // https://www.redblobgames.com/grids/hexagons/#neighbors-cube
    public static Hex Direction(int direction)
    {
        return Hex.Directions[direction];
    }

    public static Hex Neighbor(Hex center, int direction)
    {
        return Hex.Add(center, Hex.Direction(direction));
    }

    public static int Distance(Hex a, Hex b)
    {
        return (Math.Abs(a.q - b.q) + Math.Abs(a.r - b.r) + Math.Abs(a.s - b.s)) / 2;
    }

    // Returns list of hexes in range from the center
    public static List<Hex> Range(Hex center, int radius)
    {
        List<Hex> results = new List<Hex>();

        for(int x = -radius; x<=radius; x++)
        {
            for(int y = Math.Max(-radius, -x - radius); y <= Math.Min(radius, -x + radius); y++)
            {
                int z = -x - y;
                results.Add(Hex.Add(center, new Hex(x, y, z)));
            }
        }
        return results;
    }

    // Returns a ring of hexes in the center's specified range
    public static List<Hex> Ring(Hex center, int radius)
    {
        List<Hex> results = new List<Hex>();

        Hex hex = Hex.Add(center, Hex.Scale(Hex.Direction(4), radius));
        for(int i=0; i < 6; i++)
        {
            for(int j=0; j < radius; j++)
            {
                results.Add(hex);
                hex = Hex.Neighbor(hex, i);
            }
        }
        return results;
    }

    // Returns a list of hexes forming a hexagonal grid with the specified radius
    public static List<Hex> Spiral(Hex center, int radius)
    {
        List<Hex> results = new List<Hex>() { center };

        for(int i=1; i <= radius; i++)
        {
            results.AddRange(Hex.Ring(center, i));
        }
        return results;
    }

    // Returns a list of hexes forming a hexagonal grid with the specified radius starting from specified int
    public static List<Hex> SpiralAt(Hex center, int startAt, int radius)
    {
        List<Hex> results = new List<Hex>() {};

        for(int i=startAt; i <= startAt + radius - 1; i++)
        {
            results.AddRange(Hex.Ring(center, i));
        }
        return results;
    }

    public static int CountHexesInIsland(int radius)
    {
        return (int) Math.Floor(Math.Pow(radius+1, 3) - Math.Pow(radius, 3));
    }
}
