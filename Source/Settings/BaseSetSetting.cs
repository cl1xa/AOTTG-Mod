namespace Settings;

internal abstract class BaseSetSetting : BaseSettingsContainer
{
	public StringSetting Name = new StringSetting("Set 1");

	public BoolSetting Preset = new BoolSetting(defaultValue: false);
}
