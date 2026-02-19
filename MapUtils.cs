using Zone;

public static class MapUtils
{
	public static string GetNodeName(MapType mapType)
	{
		return mapType switch
		{
			MapType.Tutorial => "tutorial_0", 
			MapType.Sen => "sen_0", 
			MapType.Secta => "secta_0", 
			MapType.Velka => "velka_0", 
			MapType.Aqua => "aqua_0", 
			MapType.Spider => "spider_0", 
			MapType.Voidlow => "voidlow_0", 
			MapType.Faen => "faen_0", 
			MapType.Forge => "forge_0", 
			MapType.Sewers => "sewers_0", 
			MapType.Wolf => "wolf_0", 
			MapType.Ulmin => "ulmin_0", 
			MapType.Pyr => "pyr_0", 
			MapType.Uprising => "uprising_0", 
			MapType.Sahti => "sahti_0", 
			MapType.Dread => "dread_0", 
			MapType.Sunken => "sunken_0", 
			MapType.Dream => "dream_0", 
			_ => null, 
		};
	}
}
