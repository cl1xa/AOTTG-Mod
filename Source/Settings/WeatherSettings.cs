namespace Settings;

internal class WeatherSettings : PresetSettingsContainer
{
	public SetSettingsContainer<WeatherSet> WeatherSets = new SetSettingsContainer<WeatherSet>();

	protected override string FileName => "Weather.json";
}
