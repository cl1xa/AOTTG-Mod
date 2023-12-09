using System;
using UnityEngine;

namespace Utility;

internal class SingletonFactory : MonoBehaviour
{
	public static T CreateSingleton<T>(T instance) where T : Component
	{
		if (instance != null)
		{
			Type typeFromHandle = typeof(T);
			throw new Exception($"Attempting to create duplicate singleton of {typeFromHandle.Name}");
		}
		GameObject obj = new GameObject();
		instance = obj.AddComponent<T>();
		UnityEngine.Object.DontDestroyOnLoad(obj);
		return instance;
	}
}
