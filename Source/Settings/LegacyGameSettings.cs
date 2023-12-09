namespace Settings;

internal class LegacyGameSettings : SaveableSettingsContainer
{
	public IntSetting TitanSpawnCap = new IntSetting(20, 0, 100);

	public IntSetting GameType = new IntSetting(0, 0);

	public StringSetting LevelScript = new StringSetting(string.Empty);

	public StringSetting LogicScript = new StringSetting(string.Empty);

	public BoolSetting TitanNumberEnabled = new BoolSetting(defaultValue: false);

	public IntSetting TitanNumber = new IntSetting(10, 0, 100);

	public BoolSetting TitanSpawnEnabled = new BoolSetting(defaultValue: false);

	public FloatSetting TitanSpawnNormal = new FloatSetting(20f, 0f, 100f);

	public FloatSetting TitanSpawnAberrant = new FloatSetting(20f, 0f, 100f);

	public FloatSetting TitanSpawnJumper = new FloatSetting(20f, 0f, 100f);

	public FloatSetting TitanSpawnCrawler = new FloatSetting(20f, 0f, 100f);

	public FloatSetting TitanSpawnPunk = new FloatSetting(20f, 0f, 100f);

	public BoolSetting TitanSizeEnabled = new BoolSetting(defaultValue: false);

	public FloatSetting TitanSizeMin = new FloatSetting(1f, 0f, 100f);

	public FloatSetting TitanSizeMax = new FloatSetting(3f, 0f, 100f);

	public IntSetting TitanHealthMode = new IntSetting(0, 0);

	public IntSetting TitanHealthMin = new IntSetting(100, 0);

	public IntSetting TitanHealthMax = new IntSetting(200, 0);

	public BoolSetting TitanArmorEnabled = new BoolSetting(defaultValue: false);

	public BoolSetting TitanArmorCrawlerEnabled = new BoolSetting(defaultValue: false);

	public IntSetting TitanArmor = new IntSetting(1000, 0);

	public BoolSetting TitanExplodeEnabled = new BoolSetting(defaultValue: false);

	public IntSetting TitanExplodeRadius = new IntSetting(30, 0);

	public BoolSetting RockThrowEnabled = new BoolSetting(defaultValue: true);

	public BoolSetting PointModeEnabled = new BoolSetting(defaultValue: false);

	public IntSetting PointModeAmount = new IntSetting(25, 1);

	public BoolSetting BombModeEnabled = new BoolSetting(defaultValue: false);

	public BoolSetting BombModeCeiling = new BoolSetting(defaultValue: true);

	public BoolSetting BombModeInfiniteGas = new BoolSetting(defaultValue: true);

	public BoolSetting BombModeDisableTitans = new BoolSetting(defaultValue: true);

	public IntSetting TeamMode = new IntSetting(0, 0);

	public BoolSetting InfectionModeEnabled = new BoolSetting(defaultValue: false);

	public IntSetting InfectionModeAmount = new IntSetting(1, 1);

	public BoolSetting FriendlyMode = new BoolSetting(defaultValue: false);

	public IntSetting BladePVP = new IntSetting(0, 0);

	public BoolSetting AHSSAirReload = new BoolSetting(defaultValue: true);

	public BoolSetting CannonsFriendlyFire = new BoolSetting(defaultValue: false);

	public BoolSetting TitanPerWavesEnabled = new BoolSetting(defaultValue: false);

	public IntSetting TitanPerWaves = new IntSetting(10, 0, 100);

	public BoolSetting TitanMaxWavesEnabled = new BoolSetting(defaultValue: false);

	public IntSetting TitanMaxWaves = new IntSetting(20, 1);

	public BoolSetting PunksEveryFive = new BoolSetting(defaultValue: true);

	public BoolSetting GlobalMinimapDisable = new BoolSetting(defaultValue: false);

	public BoolSetting PreserveKDR = new BoolSetting(defaultValue: false);

	public BoolSetting RacingEndless = new BoolSetting(defaultValue: false);

	public FloatSetting RacingStartTime = new FloatSetting(20f, 1f);

	public BoolSetting EndlessRespawnEnabled = new BoolSetting(defaultValue: false);

	public IntSetting EndlessRespawnTime = new IntSetting(0, 5);

	public BoolSetting KickShifters = new BoolSetting(defaultValue: false);

	public BoolSetting AllowHorses = new BoolSetting(defaultValue: false);

	public StringSetting Motd = new StringSetting(string.Empty, 1000);

	public BoolSetting GlobalHideNames = new BoolSetting(defaultValue: false);

	protected override string FileName => "LegacyGameSettings.json";
}
