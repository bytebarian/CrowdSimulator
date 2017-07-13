using System.Collections.Generic;

[System.Serializable]
public struct IntVector2
{

	public int x, z;

	public IntVector2 (int x, int z)
    {
		this.x = x;
		this.z = z;
	}

	public static IntVector2 operator + (IntVector2 a, IntVector2 b)
    {
		a.x += b.x;
		a.z += b.z;
		return a;
	}

    public static IntVector2 operator * (IntVector2 a, IntVector2 b)
    {
        a.x *= b.x;
        a.z *= b.z;
        return a;
    }

    public static IEnumerable<IntVector2> Range(IntVector2 size)
    {
        for (var i = 0; i < size.x; i++)
            for (var j = 0; j < size.z; j++)
                yield return new IntVector2(i, j);
    }
}