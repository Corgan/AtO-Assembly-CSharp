using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class HeroSelection : MonoBehaviour
{
	public bool blocked;

	private Vector3 destination = Vector3.zero;

	private GameObject oldBox;

	internal SubClassData subClassData;

	public Transform sprite;

	public float selectedPortraitScale = 1.3f;

	public float selectedPortraitScaleMultiplayer = 0.95f;

	public Transform spriteBackground;

	public TMP_Text nameTM;

	public TMP_Text rankTM;

	public int rankTMHidden;

	public TMP_Text nameShadow;

	public TMP_Text nameOver;

	public TMP_Text rankOver;

	private string id;

	public ParticleSystem particleFlash;

	public Transform borderTransform;

	public Transform botPopup;

	public Transform botPopupPerks;

	public TMP_Text perkPoints;

	public Transform perkPointsT;

	public Transform botPerks;

	public Transform lockIcon;

	public Transform DefaultParent;

	private Transform nameT;

	internal SpriteRenderer spriteSR;

	private SpriteRenderer spriteBackgroundSR;

	private Vector3 startMousePos;

	private Vector3 startPos;

	private Vector3 namePos;

	private bool controllerClicked;

	private bool moveThis;

	private bool isInOriPosition = true;

	private bool multiplayerBlocked;

	public bool selected;

	public bool HeroPicked;

	private bool dlcBlocked;

	public Transform whiteSquare;

	public string Id
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}

	public bool DlcBlocked
	{
		get
		{
			return dlcBlocked;
		}
		set
		{
			dlcBlocked = value;
		}
	}

	private void Awake()
	{
		spriteSR = sprite.GetComponent<SpriteRenderer>();
		spriteBackgroundSR = spriteBackground.GetComponent<SpriteRenderer>();
		nameT = nameTM.transform;
		namePos = nameT.position;
	}

	private void Start()
	{
		destination = sprite.position;
		nameOver.gameObject.SetActive(HeroPicked);
		rankOver.gameObject.SetActive(HeroPicked);
		spriteSR.enabled = HeroPicked;
		if (GameManager.Instance.IsObeliskChallenge())
		{
			botPerks.gameObject.SetActive(value: false);
		}
		if (blocked || multiplayerBlocked || dlcBlocked)
		{
			rankTM.gameObject.SetActive(value: false);
		}
		showWhiteSquare(_state: false);
	}

	private void Update()
	{
		if (moveThis)
		{
			sprite.position = destination;
		}
		if (HeroSelectionManager.Instance.controllerCurrentHS == this)
		{
			Dragging();
		}
	}

	public void SetMultiplayerBlocked(bool state)
	{
		if (state)
		{
			multiplayerBlocked = true;
			nameTM.color = Functions.HexToColor(Globals.Instance.ClassColor["warrior"]);
		}
		else
		{
			multiplayerBlocked = false;
			nameTM.color = Functions.HexToColor(Globals.Instance.ClassColor["scout"]);
		}
	}

	public void SetSubclass(SubClassData _subclassdata)
	{
		subClassData = _subclassdata;
		id = _subclassdata.Id;
		SetDlcBlock();
	}

	public void SetDlcBlock()
	{
		if (subClassData.Sku != "" && !SteamManager.Instance.PlayerHaveDLC(subClassData.Sku))
		{
			dlcBlocked = true;
		}
		else
		{
			dlcBlocked = false;
		}
	}

	public string GetSubclassName()
	{
		if (subClassData != null)
		{
			return subClassData.SubClassName;
		}
		return "";
	}

	public string GetHeroClass()
	{
		if (subClassData != null)
		{
			return subClassData.HeroClass.ToString();
		}
		return "";
	}

	public string GetHeroClassSecondary()
	{
		if (subClassData != null)
		{
			return subClassData.HeroClassSecondary.ToString();
		}
		return "";
	}

	public void SetSprite(Sprite _sprite = null, Sprite _spriteBorder = null, Sprite _spriteLocked = null)
	{
		if (_sprite != null)
		{
			spriteSR.sprite = _sprite;
		}
		if ((!blocked && !dlcBlocked) || GameManager.Instance.IsWeeklyChallenge())
		{
			if (_spriteBorder != null)
			{
				spriteBackgroundSR.sprite = _spriteBorder;
				spriteBackground.GetComponent<SpriteMask>().sprite = _spriteBorder;
			}
		}
		else
		{
			spriteBackgroundSR.sprite = _spriteBorder;
			spriteBackgroundSR.color = new Color(0.2f, 0.2f, 0.2f, 1f);
			spriteBackground.GetComponent<SpriteMask>().sprite = _spriteBorder;
		}
	}

	public void SetSpriteSilueta(Sprite _spriteBorder = null)
	{
		if (_spriteBorder != null)
		{
			spriteBackgroundSR.sprite = _spriteBorder;
			spriteBackground.GetComponent<SpriteMask>().sprite = _spriteBorder;
		}
	}

	public void SetName(string _name)
	{
		TMP_Text tMP_Text = nameTM;
		TMP_Text tMP_Text2 = nameShadow;
		string text = (nameOver.text = _name);
		string text3 = (tMP_Text2.text = text);
		tMP_Text.text = text3;
		SetRank();
	}

	public void SetRank()
	{
		TMP_Text tMP_Text = rankTM;
		string text = (rankOver.text = string.Format(Texts.Instance.GetText("rankProgress"), PlayerManager.Instance.GetPerkRank(subClassData.Id)));
		tMP_Text.text = text;
		rankTMHidden = PlayerManager.Instance.GetPerkRank(subClassData.Id);
		if (GameManager.Instance.IsObeliskChallenge())
		{
			rankTM.gameObject.SetActive(value: false);
			rankOver.gameObject.SetActive(value: false);
		}
	}

	public void SetRankBox(int _rank)
	{
		rankOver.text = string.Format(Texts.Instance.GetText("rankProgress"), _rank);
		rankTMHidden = _rank;
		if (GameManager.Instance.IsObeliskChallenge())
		{
			rankTM.gameObject.SetActive(value: false);
			rankOver.gameObject.SetActive(value: false);
		}
	}

	private void ResetSkin()
	{
		SetSkin(PlayerManager.Instance.GetActiveSkin(subClassData.Id));
	}

	public void SetSkin(string _skinId)
	{
		if (_skinId == "")
		{
			_skinId = Globals.Instance.GetSkinBaseIdBySubclass(subClassData.Id);
		}
		SkinData skinData = Globals.Instance.GetSkinData(_skinId);
		if (skinData != null)
		{
			spriteSR.sprite = skinData.SpritePortrait;
		}
	}

	public void Init()
	{
		perkPointsT.gameObject.SetActive(value: false);
		botPopup.gameObject.SetActive(value: true);
		botPopup.GetComponent<BotHeroChar>().SetSubClassData(subClassData);
		if (!blocked && !dlcBlocked)
		{
			botPopupPerks.gameObject.SetActive(value: true);
			botPopupPerks.GetComponent<BotHeroChar>().SetSubClassData(subClassData);
			lockIcon.gameObject.SetActive(value: false);
			SetPerkPoints();
			return;
		}
		botPopup.transform.localPosition = new Vector3(botPopup.transform.localPosition.x, -0.35f, botPopup.transform.localPosition.z);
		botPopupPerks.gameObject.SetActive(value: false);
		lockIcon.gameObject.SetActive(value: true);
		if (dlcBlocked)
		{
			lockIcon.GetComponent<PopupText>().id = "";
			if (subClassData.Id == Globals.Instance.GetSubClassData("queen").Id || subClassData.Id == Globals.Instance.GetSubClassData("engineer").Id || subClassData.Id == Globals.Instance.GetSubClassData("deathknight").Id || subClassData.Id == Globals.Instance.GetSubClassData("bloodmage").Id)
			{
				lockIcon.GetComponent<PopupText>().text = string.Format(Texts.Instance.GetText("requiredDLC"), SteamManager.Instance.GetDLCName(subClassData.Sku));
			}
			else
			{
				lockIcon.GetComponent<PopupText>().text = string.Format(Texts.Instance.GetText("requiredDLCandQuest"), SteamManager.Instance.GetDLCName(subClassData.Sku));
			}
		}
		nameDisabled();
	}

	public void ShowComingSoon()
	{
		TMP_Text tMP_Text = nameTM;
		string text = (nameShadow.text = Texts.Instance.GetText("comingSoon"));
		tMP_Text.text = text;
		nameTM.color = Functions.HexToColor("#572424");
		lockIcon.gameObject.SetActive(value: false);
	}

	public void SetPerkPoints()
	{
		if (GameManager.Instance.IsLoadingGame())
		{
			perkPointsT.gameObject.SetActive(value: false);
			botPerks.gameObject.SetActive(value: false);
			return;
		}
		int perkPointsAvailable = PlayerManager.Instance.GetPerkPointsAvailable(Id);
		if (perkPointsAvailable > 0)
		{
			perkPoints.text = perkPointsAvailable.ToString();
			perkPointsT.gameObject.SetActive(value: true);
		}
		else
		{
			perkPointsT.gameObject.SetActive(value: false);
		}
		if (GameManager.Instance.IsObeliskChallenge())
		{
			perkPointsT.gameObject.SetActive(value: false);
		}
	}

	public async void AssignHeroToBox(GameObject _box)
	{
		await AssignCo(_box);
	}

	private async Task AssignCo(GameObject _box)
	{
		HeroPicked = true;
		await Task.Delay(100);
		isInOriPosition = false;
		base.transform.parent = HeroSelectionManager.Instance.boxCharacters;
		sprite.position = _box.transform.position;
		sprite.transform.localScale = Vector3.one * (GameManager.Instance.IsMultiplayer() ? selectedPortraitScaleMultiplayer : selectedPortraitScale);
		spriteBackgroundSR.color = new Color(0.3f, 0.3f, 0.3f, 1f);
		spriteSR.sortingLayerName = "Characters";
		spriteSR.sortingOrder = 0;
		spriteSR.enabled = true;
		spriteSR.color = new Color(1f, 1f, 1f, 1f);
		nameOver.gameObject.SetActive(value: true);
		nameOver.GetComponent<Renderer>().sortingOrder = 1;
		rankOver.gameObject.SetActive(value: true);
		rankOver.GetComponent<Renderer>().sortingOrder = 1;
		nameDisabled();
		HeroSelectionManager.Instance.FillBox(_box, this, _state: true);
		if (GameManager.Instance.IsObeliskChallenge())
		{
			rankTM.gameObject.SetActive(value: false);
			rankOver.gameObject.SetActive(value: false);
		}
		if (!GameManager.Instance.IsMultiplayer() || (GameManager.Instance.IsMultiplayer() && HeroSelectionManager.Instance.IsYourBox(_box.name)))
		{
			HeroSelectionManager.Instance.SetDefaultMiniPopupHero();
		}
	}

	public void MoveToBox(GameObject _box, GameObject _oldBox, bool animation = true)
	{
		HeroPicked = true;
		HeroSelectionManager.Instance.ShowHeroesByFilterAsync(HeroSelectionManager.Instance.CurrentFilter);
		selected = false;
		oldBox = _oldBox;
		spriteSR.enabled = true;
		isInOriPosition = false;
		destination = _box.transform.position;
		sprite.position = destination;
		nameOver.gameObject.SetActive(value: true);
		rankOver.gameObject.SetActive(value: true);
		HeroSelectionManager.Instance.FillBox(_box, this, _state: true);
		if (GameManager.Instance.IsObeliskChallenge())
		{
			rankTM.gameObject.SetActive(value: false);
			rankOver.gameObject.SetActive(value: false);
		}
	}

	public void GoBackToOri()
	{
		selected = false;
		HeroPicked = false;
		HeroSelectionManager.Instance.ShowHeroesByFilterAsync(HeroSelectionManager.Instance.CurrentFilter);
		oldBox = null;
		spriteSR.enabled = false;
		spriteBackgroundSR.color = new Color(1f, 1f, 1f, 1f);
		nameRegular();
		nameOver.gameObject.SetActive(value: false);
		rankOver.gameObject.SetActive(value: false);
		base.transform.parent = DefaultParent;
		sprite.position = spriteBackground.position;
		isInOriPosition = true;
		base.transform.localPosition = Vector3.zero;
		if (controllerClicked)
		{
			ResetClickedController();
		}
	}

	private void nameMouseEnter()
	{
		nameTM.color = new Color(1f, 0.7f, 0f, 1f);
		spriteBackgroundSR.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
	}

	private void nameDisabled()
	{
		nameTM.color = new Color(0.3f, 0.3f, 0.3f, 1f);
		spriteBackgroundSR.transform.localScale = new Vector3(0.95f, 0.95f, 1f);
	}

	private void nameRegular()
	{
		nameTM.color = new Color(1f, 1f, 1f, 1f);
		spriteBackgroundSR.transform.localScale = new Vector3(0.95f, 0.95f, 1f);
		if (GameManager.Instance.IsObeliskChallenge() || blocked || multiplayerBlocked || dlcBlocked)
		{
			rankTM.gameObject.SetActive(value: false);
			rankOver.gameObject.SetActive(value: false);
		}
	}

	public void OnMouseOver()
	{
		if (GameManager.Instance.GetDeveloperMode() && (blocked || dlcBlocked) && Input.GetMouseButtonUp(1))
		{
			PlayerManager.Instance.HeroUnlock(id, save: true, achievement: false);
		}
		if ((!GameManager.Instance.IsWeeklyChallenge() || GameManager.Instance.GetDeveloperMode()) && !AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!GameManager.Instance.IsMultiplayer() || !GameManager.Instance.IsLoadingGame()) && (!(HeroSelectionManager.Instance.charPopup != null) || !HeroSelectionManager.Instance.charPopup.MoveThis) && !blocked && !multiplayerBlocked && !dlcBlocked && !HeroSelectionManager.Instance.dragging && Input.GetMouseButtonUp(1))
		{
			RightClick();
		}
	}

	public void RightClick()
	{
		if (HeroSelectionManager.Instance.controllerCurrentHS != null)
		{
			HeroSelectionManager.Instance.controllerCurrentHS.ResetClickedController();
			HeroSelectionManager.Instance.ResetController();
		}
		HeroSelectionManager.Instance.charPopup.Init(subClassData);
		HeroSelectionManager.Instance.charPopup.Show();
		HeroSelectionManager.Instance.MouseOverBox(null);
	}

	public void OnMouseDown()
	{
		HeroSelectionManager.Instance?.charPopupMini.SetSubClassData(subClassData);
		showWhiteSquare(_state: false);
		if ((GameManager.Instance.IsWeeklyChallenge() && !GameManager.Instance.GetDeveloperMode()) || AlertManager.Instance.IsActive() || GameManager.Instance.IsTutorialActive() || SettingsManager.Instance.IsActive() || DamageMeterManager.Instance.IsActive() || (GameManager.Instance.IsMultiplayer() && GameManager.Instance.IsLoadingGame()) || HeroSelectionManager.Instance.charPopup.MoveThis)
		{
			return;
		}
		if (!blocked && !dlcBlocked)
		{
			GameObject overBox = HeroSelectionManager.Instance.GetOverBox();
			if (overBox != null && !HeroSelectionManager.Instance.IsYourBox(overBox.name))
			{
				multiplayerBlocked = true;
				return;
			}
			multiplayerBlocked = false;
			PickHero();
		}
		if (!blocked && !multiplayerBlocked && !dlcBlocked)
		{
			Dragging();
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			HeroSelectionManager.Instance.charPopup.Close();
		}
	}

	public void OnMouseDrag()
	{
		if ((!GameManager.Instance.IsWeeklyChallenge() || GameManager.Instance.GetDeveloperMode()) && selected && !AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!GameManager.Instance.IsMultiplayer() || !GameManager.Instance.IsLoadingGame()) && !blocked && !multiplayerBlocked && !dlcBlocked)
		{
			Dragging();
		}
	}

	public void OnMouseUp()
	{
		if ((!GameManager.Instance.IsWeeklyChallenge() || GameManager.Instance.GetDeveloperMode()) && !AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!GameManager.Instance.IsMultiplayer() || !GameManager.Instance.IsLoadingGame()))
		{
			if (HeroSelectionManager.Instance.charPopup.MoveThis)
			{
				DraggingStop();
			}
			else if (!blocked && !dlcBlocked)
			{
				DraggingStop();
			}
		}
	}

	public void OnMouseExit()
	{
		if ((!GameManager.Instance.IsWeeklyChallenge() || GameManager.Instance.GetDeveloperMode()) && !AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!GameManager.Instance.IsMultiplayer() || !GameManager.Instance.IsLoadingGame()))
		{
			showWhiteSquare(_state: false);
			if (!blocked && !dlcBlocked && isInOriPosition && !HeroSelectionManager.Instance.dragging)
			{
				particleFlash.gameObject.SetActive(value: false);
				nameRegular();
			}
			if (HeroSelectionManager.Instance.charPopupGO != null)
			{
				_ = HeroSelectionManager.Instance.charPopupGO.activeSelf;
			}
			if (!blocked && !dlcBlocked)
			{
				HeroSelectionManager.Instance.ShowDrag(state: false, Vector3.zero);
			}
		}
	}

	public void OnMouseEnter()
	{
		if (AlertManager.Instance.IsActive() || (GameManager.Instance.IsWeeklyChallenge() && !GameManager.Instance.GetDeveloperMode()) || (GameManager.Instance.IsMultiplayer() && GameManager.Instance.IsLoadingGame()) || blocked || dlcBlocked || HeroSelectionManager.Instance.dragging)
		{
			return;
		}
		if (isInOriPosition)
		{
			particleFlash.gameObject.SetActive(value: true);
			GameManager.Instance.PlayLibraryAudio("castnpccardfast", 0.25f);
			nameMouseEnter();
			showWhiteSquare(_state: true);
		}
		else if (!multiplayerBlocked)
		{
			GameObject overBox = HeroSelectionManager.Instance.GetOverBox();
			if (overBox != null && HeroSelectionManager.Instance.IsYourBox(overBox.name))
			{
				HeroSelectionManager.Instance.ShowDrag(state: true, base.transform.position);
			}
		}
	}

	private void showWhiteSquare(bool _state)
	{
		if (whiteSquare.gameObject.activeSelf != _state)
		{
			whiteSquare.gameObject.SetActive(_state);
		}
	}

	public void OnClickedController()
	{
		HeroSelectionManager.Instance.dragging = false;
		if (blocked || multiplayerBlocked || dlcBlocked || GameManager.Instance.IsLoadingGame())
		{
			return;
		}
		GameObject overBox = HeroSelectionManager.Instance.GetOverBox();
		if (overBox == null || HeroSelectionManager.Instance.controllerCurrentHS == null)
		{
			HeroSelectionManager.Instance.controllerCurrentHS = this;
			controllerClicked = true;
			OnMouseDown();
			if (overBox == null)
			{
				HeroSelectionManager.Instance.MoveAbsoluteToCharactersAfterClick();
			}
		}
		else if (overBox != null)
		{
			if (HeroSelectionManager.Instance.controllerCurrentHS != null)
			{
				PickStop();
			}
			else
			{
				PickHero();
			}
		}
	}

	public void ResetClickedController()
	{
		HeroSelectionManager.Instance.controllerCurrentHS = null;
		controllerClicked = false;
		HeroSelectionManager.Instance.dragging = false;
		PickStop(-1, _removeHeroAbsolute: true);
	}

	private void Dragging()
	{
		if (blocked || multiplayerBlocked || dlcBlocked)
		{
			return;
		}
		if (HeroSelectionManager.Instance.IsHeroSelected(Id))
		{
			Reset();
			return;
		}
		Vector3 position = GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition);
		position.z = -5f;
		if (controllerClicked)
		{
			position += new Vector3(-0.3f, 0.3f, 0f);
		}
		sprite.position = position;
		if (HeroSelectionManager.Instance.GetOverBox() != null)
		{
			spriteSR.color = new Color(1f, 1f, 1f, 0.8f);
		}
		else if (spriteSR.color.a < 1f)
		{
			spriteSR.color = new Color(1f, 1f, 1f, 1f);
		}
		nameDisabled();
	}

	private void DraggingStop()
	{
		if (HeroSelectionManager.Instance.dragging)
		{
			PickStop();
		}
	}

	public void Reset()
	{
		GoBackToOri();
	}

	public void PickHero(bool _comingFromRandom = false)
	{
		selected = true;
		if (!GameManager.Instance.IsMultiplayer())
		{
			HeroSelectionManager.Instance.charPopup.Close();
		}
		moveThis = false;
		spriteSR.enabled = true;
		nameOver.gameObject.SetActive(value: false);
		rankOver.gameObject.SetActive(value: false);
		startPos = sprite.position;
		startMousePos = Input.mousePosition;
		base.transform.parent = HeroSelectionManager.Instance.boxCharacters;
		sprite.transform.localScale = Vector3.one * (GameManager.Instance.IsMultiplayer() ? selectedPortraitScaleMultiplayer : selectedPortraitScale);
		spriteSR.sortingLayerName = "UI";
		spriteSR.sortingOrder = 100;
		spriteBackgroundSR.color = new Color(0.3f, 0.3f, 0.3f, 1f);
		nameOver.GetComponent<Renderer>().sortingOrder = 101;
		rankOver.GetComponent<Renderer>().sortingOrder = 101;
		HeroSelectionManager.Instance.dragging = true;
		GameObject gameObject = HeroSelectionManager.Instance.GetOverBox();
		if (_comingFromRandom)
		{
			gameObject = null;
		}
		if (gameObject != null)
		{
			oldBox = gameObject;
			HeroSelectionManager.Instance.FillBox(gameObject, null, _state: false);
		}
		else
		{
			oldBox = null;
			if (GameManager.Instance.IsMultiplayer())
			{
				ResetSkin();
			}
		}
		HeroSelectionManager.Instance.ShowDrag(state: false, Vector3.zero);
		borderTransform.gameObject.SetActive(value: false);
		spriteSR.enabled = true;
	}

	public void PickStop(int _box = -1, bool _removeHeroAbsolute = false)
	{
		GameObject gameObject = null;
		if (!_removeHeroAbsolute)
		{
			gameObject = ((_box == -1) ? HeroSelectionManager.Instance.GetOverBox() : HeroSelectionManager.Instance.boxGO[_box]);
		}
		HeroSelectionManager.Instance.dragging = false;
		if (HeroSelectionManager.Instance.controllerCurrentHS != null)
		{
			HeroSelectionManager.Instance.controllerCurrentHS = null;
			controllerClicked = false;
		}
		if (GameManager.Instance.IsMultiplayer() && HeroSelectionManager.Instance.IsHeroSelected(Id))
		{
			return;
		}
		spriteSR.sortingLayerName = "Characters";
		spriteSR.sortingOrder = 0;
		nameOver.GetComponent<Renderer>().sortingOrder = 1;
		rankOver.GetComponent<Renderer>().sortingOrder = 1;
		spriteSR.color = new Color(1f, 1f, 1f, 1f);
		if (gameObject == null || !HeroSelectionManager.Instance.IsYourBox(gameObject.name))
		{
			if (GameManager.Instance.IsWeeklyChallenge())
			{
				MoveToBox(oldBox, oldBox);
				return;
			}
			GoBackToOri();
			if (GameManager.Instance.IsMultiplayer())
			{
				HeroSelectionManager.Instance.ResetHero(Id);
			}
			return;
		}
		if (HeroSelectionManager.Instance.IsBoxFilled(gameObject))
		{
			if (oldBox != null)
			{
				HeroSelection boxHero = HeroSelectionManager.Instance.GetBoxHero(gameObject);
				if (boxHero != null)
				{
					boxHero.MoveToBox(oldBox, gameObject);
				}
			}
			else
			{
				HeroSelectionManager.Instance.GetBoxHero(gameObject).GoBackToOri();
			}
		}
		MoveToBox(gameObject, oldBox);
	}

	public bool RandomAvailable()
	{
		if (!selected && !blocked && !multiplayerBlocked && !dlcBlocked && !HeroPicked)
		{
			return true;
		}
		return false;
	}
}
