using System.Collections;
using Settings;

namespace UI;

internal class SettingsKeybindsDefaultPanel : SettingsCategoryPanel
{
	protected override bool ScrollBar => true;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		SettingsKeybindsPanel obj = (SettingsKeybindsPanel)parent;
		SettingsPopup settingsPopup = (SettingsPopup)obj.Parent;
		obj.CreateGategoryDropdown(DoublePanelLeft);
		string localeCategory = settingsPopup.LocaleCategory;
		ElementStyle style = new ElementStyle(24, 140f, ThemePanel);
		string text = obj.GetCurrentCategoryName().Replace(" ", "");
		BaseSettingsContainer container = (BaseSettingsContainer)SettingsManager.InputSettings.Settings[text];
		CreateKeybindSettings(container, settingsPopup.KeybindPopup, localeCategory, "Keybinds." + text, style);
		if (text == "Human")
		{
			ElementFactory.CreateToggleSetting(DoublePanelRight, style, SettingsManager.InputSettings.Human.DashDoubleTap, UIManager.GetLocale(localeCategory, "Keybinds.Human", "DashDoubleTap"));
			ElementFactory.CreateSliderSetting(DoublePanelRight, style, SettingsManager.InputSettings.Human.ReelOutScrollSmoothing, UIManager.GetLocale(localeCategory, "Keybinds.Human", "ReelOutScrollSmoothing"), UIManager.GetLocale(localeCategory, "Keybinds.Human", "ReelOutScrollSmoothingTooltip"), 130f);
		}
	}

	private void CreateKeybindSettings(BaseSettingsContainer container, KeybindPopup popup, string cat, string sub, ElementStyle style)
	{
		int num = 0;
		foreach (DictionaryEntry setting in container.Settings)
		{
			BaseSetting baseSetting = (BaseSetting)setting.Value;
			string item = (string)setting.Key;
			if (baseSetting.GetType() == typeof(KeybindSetting))
			{
				ElementFactory.CreateKeybindSetting((num < container.Settings.Count / 2) ? DoublePanelLeft : DoublePanelRight, style, baseSetting, UIManager.GetLocale(cat, sub, item), popup);
				num++;
			}
		}
	}
}
