namespace Weather;

public enum WeatherAction
{
	BeginSchedule,
	EndSchedule,
	RepeatNext,
	BeginRepeat,
	EndRepeat,
	SetDefaultAll,
	SetDefault,
	SetValue,
	SetTargetDefaultAll,
	SetTargetDefault,
	SetTargetValue,
	SetTargetTimeAll,
	SetTargetTime,
	Wait,
	Goto,
	Label,
	Return,
	LoadSkybox
}
