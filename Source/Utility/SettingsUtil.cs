using Settings;
using UnityEngine;

namespace Utility;

internal class SettingsUtil
{
	public static void SetSettingValue(BaseSetting setting, SettingType type, object value)
	{
		switch (type)
		{
		case SettingType.Bool:
			((BoolSetting)setting).Value = (bool)value;
			break;
		case SettingType.Color:
			((ColorSetting)setting).Value = (Color)value;
			break;
		case SettingType.Float:
			((FloatSetting)setting).Value = (float)value;
			break;
		case SettingType.Int:
			((IntSetting)setting).Value = (int)value;
			break;
		case SettingType.String:
			((StringSetting)setting).Value = (string)value;
			break;
		default:
			Debug.Log("Attempting to set invalid setting value.");
			break;
		}
	}

	public static object DeserializeValueFromJson(SettingType type, string json)
	{
		BaseSetting baseSetting = CreateBaseSetting(type);
		if (baseSetting == null)
		{
			return baseSetting;
		}
		baseSetting.DeserializeFromJsonString(json);
		return baseSetting;
	}

	public static BaseSetting CreateBaseSetting(SettingType type)
	{
		return type switch
		{
			SettingType.Bool => new BoolSetting(), 
			SettingType.Int => new IntSetting(), 
			SettingType.Float => new FloatSetting(), 
			SettingType.Color => new ColorSetting(), 
			SettingType.String => new StringSetting(), 
			_ => null, 
		};
	}
}
