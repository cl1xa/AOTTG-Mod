using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Utility;

namespace ApplicationManagers;

public class AutoUpdateManager : MonoBehaviour
{
	public static AutoUpdateStatus Status = AutoUpdateStatus.Updating;

	public static bool CloseFailureBox = false;

	private static AutoUpdateManager _instance;

	private static readonly string RootDataPath = Application.dataPath;

	private static readonly string Platform = File.ReadAllText(RootDataPath + "/PlatformInfo");

	private static readonly string RootUpdateURL = "http://aottgrc.com/Patch";

	private static readonly string LauncherVersionURL = RootUpdateURL + "/LauncherVersion.txt";

	public static readonly string PlatformUpdateURL = RootUpdateURL + "/" + Platform;

	private static readonly string ChecksumURL = PlatformUpdateURL + "/Checksum.txt";

	public static void Init()
	{
		//Autoupdate bypass without mass commenting code lol

        Status = AutoUpdateStatus.Updated;
		return;

		//_instance = SingletonFactory.CreateSingleton(_instance);
		//StartUpdate();
	}

	public static void StartUpdate()
	{
		if (ApplicationConfig.DevelopmentMode)
		{
			Status = AutoUpdateStatus.Updated;
		}
		else
		{
			_instance.StartCoroutine(_instance.StartUpdateCoroutine());
		}
	}

	private IEnumerator StartUpdateCoroutine()
	{
		Status = AutoUpdateStatus.Updating;
		bool downloadedFile = false;
		if (Application.platform == RuntimePlatform.OSXPlayer && !RootDataPath.Contains("Applications"))
		{
			Status = AutoUpdateStatus.MacTranslocated;
			yield break;
		}
		using (WWW wWW = new WWW(LauncherVersionURL))
		{
			yield return wWW;
			if (wWW.error != null)
			{
				OnUpdateFail("Error fetching launcher version", wWW.error);
				yield break;
			}
			if (!float.TryParse(wWW.text, out var _))
			{
				OnUpdateFail("Received an invalid launcher version", wWW.text);
				yield break;
			}
			if (wWW.text != "1.0")
			{
				OnLauncherOutdated();
				yield break;
			}
		}
		List<string> list;
		using (WWW wWW = new WWW(ChecksumURL))
		{
			yield return wWW;
			if (wWW.error != null)
			{
				OnUpdateFail("Error fetching checksum", wWW.error);
				yield break;
			}
			list = wWW.text.Split('\n').ToList();
		}
		foreach (string item in list)
		{
			string[] array = item.Split(':');
			string fileName = array[0].Trim();
			string text = array[1].Trim();
			string filePath = RootDataPath + "/" + fileName;
			string text2;
			if (File.Exists(filePath))
			{
				try
				{
					text2 = GenerateMD5(filePath);
				}
				catch (Exception ex)
				{
					OnUpdateFail("Error generating checksum for " + fileName, ex.Message);
					yield break;
				}
			}
			else
			{
				text2 = string.Empty;
			}
			if (!(text2 != text))
			{
				continue;
			}
			Debug.Log("File diff found, downloading " + fileName);
			downloadedFile = true;
			using WWW wWW = new WWW(PlatformUpdateURL + "/" + fileName);
			yield return wWW;
			if (wWW.error != null)
			{
				OnUpdateFail("Error fetching file " + fileName, wWW.error);
				yield break;
			}
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(filePath));
				File.WriteAllBytes(filePath, wWW.bytes);
			}
			catch (Exception ex2)
			{
				OnUpdateFail("Error writing file " + fileName, ex2.Message);
				yield break;
			}
		}
		if (downloadedFile)
		{
			Status = AutoUpdateStatus.NeedRestart;
		}
		else
		{
			Status = AutoUpdateStatus.Updated;
		}
	}

	private void OnUpdateFail(string message, string error)
	{
		Debug.Log(message + ": " + error);
		Status = AutoUpdateStatus.FailedUpdate;
	}

	private void OnLauncherOutdated()
	{
		Status = AutoUpdateStatus.LauncherOutdated;
	}

	private string GenerateMD5(string filePath)
	{
		byte[] buffer = File.ReadAllBytes(filePath);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array = MD5.Create().ComputeHash(buffer);
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}
}
