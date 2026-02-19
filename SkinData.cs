using UnityEngine;

[CreateAssetMenu(fileName = "New Skin Data", menuName = "Skin Data", order = 64)]
public class SkinData : ScriptableObject
{
	[SerializeField]
	private string skinId;

	[SerializeField]
	private string skinName;

	[SerializeField]
	private SubClassData skinSubclass;

	[Header("Base skins for characters")]
	[SerializeField]
	private bool baseSkin;

	[Header("Order in charpopup")]
	[SerializeField]
	private int skinOrder;

	[Header("Skin Requeriments")]
	[SerializeField]
	private int perkLevel;

	[Header("DLC Requeriment")]
	[SerializeField]
	private string sku;

	[Header("SteamStat Requeriment")]
	[SerializeField]
	private string steamStat;

	[Header("Prefab")]
	[SerializeField]
	private GameObject skinGo;

	[Header("Sprites")]
	[SerializeField]
	private Sprite spriteSilueta;

	[SerializeField]
	private Sprite spriteSiluetaGrande;

	[SerializeField]
	private Sprite spritePortrait;

	[SerializeField]
	private Sprite spritePortraitGrande;

	[Header("Text Id")]
	[SerializeField]
	private string skinTextId;

	[Header("Size Changes For Different Screen")]
	[SerializeField]
	private float heroSelectionScreenScale = 1f;

	[SerializeField]
	private float heroSelectionScreenOffset_X;

	public string SkinId
	{
		get
		{
			return skinId;
		}
		set
		{
			skinId = value;
		}
	}

	public string SkinName
	{
		get
		{
			return skinName;
		}
		set
		{
			skinName = value;
		}
	}

	public SubClassData SkinSubclass
	{
		get
		{
			return skinSubclass;
		}
		set
		{
			skinSubclass = value;
		}
	}

	public bool BaseSkin
	{
		get
		{
			return baseSkin;
		}
		set
		{
			baseSkin = value;
		}
	}

	public int PerkLevel
	{
		get
		{
			return perkLevel;
		}
		set
		{
			perkLevel = value;
		}
	}

	public GameObject SkinGo
	{
		get
		{
			return skinGo;
		}
		set
		{
			skinGo = value;
		}
	}

	public Sprite SpriteSilueta
	{
		get
		{
			return spriteSilueta;
		}
		set
		{
			spriteSilueta = value;
		}
	}

	public Sprite SpriteSiluetaGrande
	{
		get
		{
			return spriteSiluetaGrande;
		}
		set
		{
			spriteSiluetaGrande = value;
		}
	}

	public Sprite SpritePortrait
	{
		get
		{
			return spritePortrait;
		}
		set
		{
			spritePortrait = value;
		}
	}

	public Sprite SpritePortraitGrande
	{
		get
		{
			return spritePortraitGrande;
		}
		set
		{
			spritePortraitGrande = value;
		}
	}

	public int SkinOrder
	{
		get
		{
			return skinOrder;
		}
		set
		{
			skinOrder = value;
		}
	}

	public string Sku
	{
		get
		{
			return sku;
		}
		set
		{
			sku = value;
		}
	}

	public string SteamStat
	{
		get
		{
			return steamStat;
		}
		set
		{
			steamStat = value;
		}
	}

	public string SkinTextId
	{
		get
		{
			return skinTextId;
		}
		set
		{
			skinTextId = value;
		}
	}

	public float HeroSelectionScreenScale
	{
		get
		{
			return heroSelectionScreenScale;
		}
		set
		{
			heroSelectionScreenScale = value;
		}
	}

	public float HeroSelectionScreenOffset_X
	{
		get
		{
			return heroSelectionScreenOffset_X;
		}
		set
		{
			heroSelectionScreenOffset_X = value;
		}
	}
}
