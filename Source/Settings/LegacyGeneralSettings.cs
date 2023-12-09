namespace Settings;

internal class LegacyGeneralSettings : BaseSettingsContainer
{
	public BoolSetting SpecMode = new BoolSetting(defaultValue: false);

	public BoolSetting LiveSpectate = new BoolSetting(defaultValue: true);
}
