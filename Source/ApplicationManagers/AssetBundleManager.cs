using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ApplicationManagers;

public class AssetBundleManager : MonoBehaviour
{
	public static AssetBundle MainAssetBundle;

	public static AssetBundleStatus Status = AssetBundleStatus.Loading;

	public static bool CloseFailureBox = false;

	private static AssetBundleManager _instance;

	private static Dictionary<string, Object> _cache = new Dictionary<string, Object>();

	private static readonly string RootDataPath = Application.dataPath;

	private static readonly string LocalAssetBundlePath = "file:///" + RootDataPath + "/RCAssets.unity3d";

	private static readonly string BackupAssetBundleURL = AutoUpdateManager.PlatformUpdateURL + "/RCAssets.unity3d";

	public static void Init()
	{
		_instance = SingletonFactory.CreateSingleton(_instance);
		LoadAssetBundle();
	}

	public static void LoadAssetBundle()
	{
		_instance.StartCoroutine(_instance.LoadAssetBundleCoroutine());
	}

	public static Object LoadAsset(string name, bool cached = false)
	{
		if (cached)
		{
			if (!_cache.ContainsKey(name))
			{
				_cache.Add(name, MainAssetBundle.Load(name));
			}
			return _cache[name];
		}
		return MainAssetBundle.Load(name);
	}

	public static T InstantiateAsset<T>(string name) where T : Object
	{
		return (T)Object.Instantiate(MainAssetBundle.Load(name));
	}

	public static T InstantiateAsset<T>(string name, Vector3 position, Quaternion rotation) where T : Object
	{
		return (T)Object.Instantiate(MainAssetBundle.Load(name), position, rotation);
	}

	private IEnumerator LoadAssetBundleCoroutine()
	{
		Status = AssetBundleStatus.Loading;
		while (AutoUpdateManager.Status == AutoUpdateStatus.Updating || !Caching.ready)
		{
			yield return null;
		}
		using WWW wwwLocal = new WWW(LocalAssetBundlePath);
		yield return wwwLocal;
		if (wwwLocal.error != null)
		{
			Debug.Log("Failed to load local asset bundle, trying backup URL at " + BackupAssetBundleURL + ": " + wwwLocal.error);
			using WWW wwwBackup = WWW.LoadFromCacheOrDownload(BackupAssetBundleURL, 20211122);
			yield return wwwBackup;
			if (wwwBackup.error != null)
			{
				Debug.Log("The backup asset bundle failed too: " + wwwBackup.error);
				Status = AssetBundleStatus.Failed;
				yield break;
			}
			OnAssetBundleLoaded(wwwBackup);
		}
		else
		{
			OnAssetBundleLoaded(wwwLocal);
		}
	}

	private void OnAssetBundleLoaded(WWW www)
	{
		FengGameManagerMKII.RCassets = www.assetBundle;
		FengGameManagerMKII.isAssetLoaded = true;
		MainAssetBundle = FengGameManagerMKII.RCassets;
		MainApplicationManager.FinishLoadAssets();
		Status = AssetBundleStatus.Ready;
	}
}
