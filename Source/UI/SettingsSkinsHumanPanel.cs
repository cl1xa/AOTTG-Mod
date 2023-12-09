using Settings;

namespace UI;

internal class SettingsSkinsHumanPanel : SettingsCategoryPanel
{
	protected override float VerticalSpacing => 20f;

	protected override bool ScrollBar => true;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		SettingsSkinsPanel obj = (SettingsSkinsPanel)parent;
		SettingsPopup settingsPopup = (SettingsPopup)obj.Parent;
		HumanCustomSkinSettings human = SettingsManager.CustomSkinSettings.Human;
		obj.CreateCommonSettings(DoublePanelLeft, DoublePanelRight);
		ElementFactory.CreateToggleSetting(DoublePanelRight, new ElementStyle(24, 200f, ThemePanel), human.GasEnabled, UIManager.GetLocale(settingsPopup.LocaleCategory, "Skins.Human", "GasEnabled"));
		ElementFactory.CreateToggleSetting(DoublePanelRight, new ElementStyle(24, 200f, ThemePanel), human.HookEnabled, UIManager.GetLocale(settingsPopup.LocaleCategory, "Skins.Human", "HookEnabled"));
		CreateHorizontalDivider(DoublePanelLeft);
		CreateHorizontalDivider(DoublePanelRight);
		obj.CreateSkinStringSettings(DoublePanelLeft, DoublePanelRight, 200f, 200f, 9);
	}
}
