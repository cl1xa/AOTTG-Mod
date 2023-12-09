using Settings;

namespace UI;

internal class SettingsSkinsForestPanel : SettingsCategoryPanel
{
	protected override float VerticalSpacing => 20f;

	protected override bool ScrollBar => true;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		SettingsSkinsPanel obj = (SettingsSkinsPanel)parent;
		SettingsPopup obj2 = (SettingsPopup)obj.Parent;
		ForestCustomSkinSet forestCustomSkinSet = (ForestCustomSkinSet)SettingsManager.CustomSkinSettings.Forest.GetSelectedSet();
		string localeCategory = obj2.LocaleCategory;
		string subCategory = "Skins.Forest";
		obj.CreateCommonSettings(DoublePanelLeft, DoublePanelRight);
		ElementFactory.CreateToggleSetting(DoublePanelRight, new ElementStyle(24, 200f, ThemePanel), forestCustomSkinSet.RandomizedPairs, UIManager.GetLocale(localeCategory, "Skins.Common", "RandomizedPairs"), UIManager.GetLocale(localeCategory, "Skins.Common", "RandomizedPairsTooltip"));
		CreateHorizontalDivider(DoublePanelLeft);
		CreateHorizontalDivider(DoublePanelRight);
		ElementFactory.CreateInputSetting(DoublePanelRight, new ElementStyle(24, 140f, ThemePanel), forestCustomSkinSet.Ground, UIManager.GetLocale(localeCategory, "Skins.Common", "Ground"), "", 260f);
		obj.CreateSkinListStringSettings(forestCustomSkinSet.TreeTrunks, DoublePanelLeft, UIManager.GetLocale(localeCategory, subCategory, "TreeTrunks"));
		obj.CreateSkinListStringSettings(forestCustomSkinSet.TreeLeafs, DoublePanelRight, UIManager.GetLocale(localeCategory, subCategory, "TreeLeafs"));
	}
}
