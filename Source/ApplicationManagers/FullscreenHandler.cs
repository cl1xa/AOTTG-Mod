using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using Settings;
using UI;
using UnityEngine;
using Utility;

namespace ApplicationManagers;

internal class FullscreenHandler : MonoBehaviour
{
	private static FullscreenHandler _instance;

	private static bool _exclusiveFullscreen;

	private static bool _fullscreen;

	private static int WindowedWidth;

	private static int WindowedHeight;

	private static int FullscreenWidth;

	private static int FullscreenHeight;

	private static readonly string RootPath = Application.dataPath + "/FullscreenFix";

	private static readonly string BorderlessPath = RootPath + "/mainDataBorderless";

	private static readonly string ExclusivePath = RootPath + "/mainDataExclusive";

	private static readonly string MainDataPath = Application.dataPath + "/mainData";

	[DllImport("user32.dll")]
	private static extern int GetActiveWindow();

	[DllImport("user32.dll")]
	private static extern bool ShowWindow(int hWnd, int nCmdShow);

	public static void Init()
	{
		_instance = SingletonFactory.CreateSingleton(_instance);
		_fullscreen = Screen.fullScreen;
		_exclusiveFullscreen = SettingsManager.GraphicsSettings.ExclusiveFullscreen.Value;
		if (_fullscreen)
		{
			WindowedWidth = 960;
			WindowedHeight = 600;
			FullscreenWidth = Screen.width;
			FullscreenHeight = Screen.height;
		}
		else
		{
			WindowedWidth = Screen.width;
			WindowedHeight = Screen.height;
			FullscreenWidth = Screen.currentResolution.width;
			FullscreenHeight = Screen.currentResolution.height;
		}
	}

	public static void ToggleFullscreen()
	{
		SetFullscreen(!_fullscreen);
		_fullscreen = !_fullscreen;
	}

	private static void SetFullscreen(bool fullscreen)
	{
		bool num = fullscreen != Screen.fullScreen;
		if (fullscreen && !Screen.fullScreen)
		{
			Screen.SetResolution(FullscreenWidth, FullscreenHeight, fullscreen: true);
		}
		else if (!fullscreen && Screen.fullScreen)
		{
			Screen.SetResolution(WindowedWidth, WindowedHeight, fullscreen: false);
		}
		if (num)
		{
			_instance.StartCoroutine(_instance.WaitAndRefreshHUD());
			CursorManager.RefreshCursorLock();
			if (UIManager.CurrentMenu != null)
			{
				UIManager.CurrentMenu.ApplyScale();
			}
		}
	}

	public void OnApplicationFocus(bool hasFocus)
	{
		if (!Supported())
		{
			return;
		}
		if (_exclusiveFullscreen)
		{
			if (hasFocus)
			{
				SetFullscreen(_fullscreen);
			}
			else
			{
				SetFullscreen(fullscreen: false);
				ShowWindow(GetActiveWindow(), 2);
			}
		}
		else if (hasFocus)
		{
			_instance.StartCoroutine(WaitAndRefreshMinimap());
		}
		CursorManager.RefreshCursorLock();
	}

	private IEnumerator WaitAndRefreshHUD()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		IN_GAME_MAIN_CAMERA.needSetHUD = true;
		Minimap.OnScreenResolutionChanged();
		GameObject gameObject = GameObject.Find("Stylish");
		if (gameObject != null)
		{
			gameObject.GetComponent<StylishComponent>().OnResolutionChange();
		}
	}

	private IEnumerator WaitAndRefreshMinimap()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Minimap.OnScreenResolutionChanged();
	}

	public static void SetMainData(bool trueFullscreen)
	{
		if (!Supported())
		{
			return;
		}
		try
		{
			if (trueFullscreen)
			{
				File.Copy(ExclusivePath, MainDataPath, overwrite: true);
			}
			else
			{
				File.Copy(BorderlessPath, MainDataPath, overwrite: true);
			}
		}
		catch (Exception ex)
		{
			Debug.Log("FullscreenHandler error setting main data: " + ex.Message);
		}
	}

	private static bool Supported()
	{
		return Application.platform == RuntimePlatform.WindowsPlayer;
	}
}
