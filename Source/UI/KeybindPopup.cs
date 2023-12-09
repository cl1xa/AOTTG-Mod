using System.Collections;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class KeybindPopup : PromptPopup
{
	private InputKey _setting;

	private Text _settingLabel;

	private Text _displayLabel;

	private InputKey _buffer;

	private bool _isDone;

	protected override string Title => UIManager.GetLocale("SettingsPopup", "KeybindPopup", "Title");

	protected override float Width => 300f;

	protected override float Height => 250f;

	protected override float VerticalSpacing => 15f;

	protected override int VerticalPadding => 30;

	protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		ElementStyle style = new ElementStyle(ButtonFontSize, 120f, ThemePanel);
		ElementFactory.CreateDefaultButton(BottomBar, style, UIManager.GetLocale("SettingsPopup", "KeybindPopup", "Unbind"), 0f, 0f, delegate
		{
			OnButtonClick("Unbind");
		});
		ElementFactory.CreateDefaultButton(BottomBar, style, UIManager.GetLocaleCommon("Cancel"), 0f, 0f, delegate
		{
			OnButtonClick("Cancel");
		});
		ElementFactory.CreateDefaultLabel(SinglePanel, style, UIManager.GetLocale("SettingsPopup", "KeybindPopup", "CurrentKey") + ":").GetComponent<Text>();
		_displayLabel = ElementFactory.CreateDefaultLabel(SinglePanel, style, string.Empty).GetComponent<Text>();
		_buffer = new InputKey();
	}

	private void Update()
	{
		if (_setting != null && !_isDone && _buffer.ReadNextInput())
		{
			_isDone = true;
			if (_buffer.ToString() == "Mouse0")
			{
				StartCoroutine(WaitAndUpdateSetting());
			}
			else
			{
				UpdateSetting();
			}
		}
	}

	private IEnumerator WaitAndUpdateSetting()
	{
		yield return new WaitForEndOfFrame();
		UpdateSetting();
	}

	private void UpdateSetting()
	{
		_setting.LoadFromString(_buffer.ToString());
		_settingLabel.text = _setting.ToString();
		base.gameObject.SetActive(value: false);
	}

	public void Show(InputKey setting, Text label)
	{
		if (!base.gameObject.activeSelf)
		{
			Show();
			_setting = setting;
			_settingLabel = label;
			_displayLabel.text = _setting.ToString();
			_isDone = false;
		}
	}

	private void OnButtonClick(string name)
	{
		if (name == "Unbind")
		{
			_setting.LoadFromString(SpecialKey.None.ToString());
			_settingLabel.text = SpecialKey.None.ToString();
		}
		_isDone = true;
		Hide();
	}
}
