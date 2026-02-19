using System.Collections.Generic;
using UnityEngine;

public static class IListExtensions
{
	public static void Shuffle<T>(this IList<T> ts, int seed = -1)
	{
		if (seed != -1)
		{
			Random.InitState(seed);
		}
		int count = ts.Count;
		int num = count - 1;
		for (int i = 0; i < num; i++)
		{
			int index = Random.Range(i, count);
			T value = ts[i];
			ts[i] = ts[index];
			ts[index] = value;
		}
	}
}
