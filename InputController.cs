using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
	private ActionsController actionsController;

	private PointerEventData pointerEventData;

	private List<RaycastResult> raycastResultList;

	private float _mouseSpeed = 300f;

	private float _maxSpeed = 1500f;

	private float _incrementSpeed;

	private Vector2 stickL;

	private Vector2 stickR;

	private float _duration = 0.15f;

	private float _timer;

	private Vector2 oldVector = Vector2.zero;

	private Vector2 mouseTranslation;

	private Vector2 movVector;

	private Vector2 zoomVector;

	private Vector2 keyVector;

	private void Awake()
	{
		actionsController = new ActionsController();
		pointerEventData = new PointerEventData(EventSystem.current);
		raycastResultList = new List<RaycastResult>();
		keyVector = Vector2.zero;
	}

	private void Start()
	{
		_incrementSpeed = _maxSpeed * 0.01f;
		mouseTranslation = default(Vector2);
		movVector = default(Vector2);
	}

	private IEnumerator CheckEscapeIsPressed()
	{
		int iterations = 0;
		while (iterations < 100)
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
			if (Keyboard.current.escapeKey.isPressed)
			{
				iterations++;
			}
			else
			{
				yield return null;
			}
		}
		if ((bool)LogosManager.Instance)
		{
			LogosManager.Instance.Escape(skipAll: true);
		}
	}

	private void Update()
	{
		if (Gamepad.current == null || SettingsManager.Instance.IsActive())
		{
			return;
		}
		_timer += Time.deltaTime;
		stickR = Gamepad.current.rightStick.ReadValue();
		if ((bool)KeyboardManager.Instance && KeyboardManager.Instance.IsChat())
		{
			if (Mathf.Abs(stickR.y) > 0.1f)
			{
				if (stickR.y > 0f)
				{
					KeyboardManager.Instance.DoScroll(goUp: true);
				}
				else
				{
					KeyboardManager.Instance.DoScroll(goUp: false);
				}
			}
		}
		else if (Mathf.Abs(stickR.x) > 0.25f || Mathf.Abs(stickR.y) > 0.25f)
		{
			if (_mouseSpeed < _maxSpeed)
			{
				_mouseSpeed += _incrementSpeed;
			}
			mouseTranslation.x = Mathf.Clamp(Mouse.current.position.ReadValue().x + _mouseSpeed * stickR.x * Time.deltaTime, 0f, Screen.width);
			mouseTranslation.y = Mathf.Clamp(Mouse.current.position.ReadValue().y + _mouseSpeed * stickR.y * Time.deltaTime, 0f, Screen.height);
			Mouse.current.WarpCursorPosition(mouseTranslation);
		}
		else
		{
			if (_mouseSpeed > 0f)
			{
				_mouseSpeed = 0f;
			}
			stickL = Gamepad.current.leftStick.ReadValue();
			DoMovementVector(stickL);
		}
	}

	private void OnEnable()
	{
		actionsController.Player.Movement.Enable();
		actionsController.Player.Fire.Enable();
		actionsController.Player.Escape.Enable();
		actionsController.Player.KeyBinding.Enable();
		actionsController.Player.Zoom.Enable();
		actionsController.Player.Movement.performed += DoMovement;
		actionsController.Player.Fire.performed += DoFire;
		actionsController.Player.Escape.performed += DoEscape;
		actionsController.Player.KeyBinding.performed += DoKeyBinding;
		actionsController.Player.Zoom.performed += DoZoom;
	}

	private void OnDisable()
	{
		actionsController.Player.Movement.Disable();
		actionsController.Player.Fire.Disable();
		actionsController.Player.Escape.Disable();
		actionsController.Player.KeyBinding.Disable();
		actionsController.Player.Zoom.Disable();
	}

	private void DoShoulder(bool _isRight)
	{
		if ((bool)KeyboardManager.Instance && KeyboardManager.Instance.IsActive())
		{
			KeyboardManager.Instance.DoShift();
		}
		else
		{
			if (AlertManager.Instance.IsActive())
			{
				return;
			}
			if (TomeManager.Instance.IsActive())
			{
				TomeManager.Instance.ControllerMoveShoulder(_isRight);
			}
			else if ((bool)HeroSelectionManager.Instance)
			{
				HeroSelectionManager.Instance.ControllerMoveBlock(_isRight);
			}
			else if ((bool)ChallengeSelectionManager.Instance)
			{
				CardCraftManager.Instance.ControllerMoveShoulder(_isRight);
			}
			else if ((bool)TownManager.Instance)
			{
				if (TownManager.Instance.characterWindow.IsActive())
				{
					TownManager.Instance.characterWindow.ControllerMoveShoulder(_isRight);
				}
				else if ((bool)CardCraftManager.Instance)
				{
					CardCraftManager.Instance.ControllerMoveShoulder(_isRight);
				}
				else
				{
					TownManager.Instance.ControllerMoveBlock(_isRight);
				}
			}
			else if ((bool)MapManager.Instance)
			{
				if (MapManager.Instance.characterWindow.IsActive())
				{
					MapManager.Instance.characterWindow.ControllerMoveShoulder(_isRight);
				}
				else if ((bool)CardCraftManager.Instance)
				{
					if (_isRight)
					{
						CardCraftManager.Instance.ControllerMoveShoulder(_isRight: true);
					}
					else
					{
						MapManager.Instance.characterWindow.ControllerMoveShoulder();
					}
				}
				else
				{
					MapManager.Instance.ControllerMoveBlock(_isRight);
				}
			}
			else if ((bool)MatchManager.Instance)
			{
				if (MatchManager.Instance.characterWindow.IsActive())
				{
					MatchManager.Instance.characterWindow.ControllerMoveShoulder(_isRight);
				}
				else
				{
					MatchManager.Instance.ControllerMoveShoulder(_isRight);
				}
			}
			else if ((bool)LootManager.Instance)
			{
				if (LootManager.Instance.characterWindowUI.IsActive())
				{
					LootManager.Instance.characterWindowUI.ControllerMoveShoulder(_isRight);
				}
				else
				{
					LootManager.Instance.ControllerMoveShoulder(_isRight);
				}
			}
			else if ((bool)RewardsManager.Instance)
			{
				Debug.Log("HERE -> " + RewardsManager.Instance.characterWindowUI.IsActive());
				if (RewardsManager.Instance.characterWindowUI.IsActive())
				{
					RewardsManager.Instance.characterWindowUI.ControllerMoveShoulder(_isRight);
				}
				else
				{
					RewardsManager.Instance.ControllerMoveShoulder(_isRight);
				}
			}
		}
	}

	private void DoTrigger(bool _isRight)
	{
		if (!KeyboardManager.Instance.IsActive() && !AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive() && !CardScreenManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && !TomeManager.Instance.IsActive() && OptionsManager.Instance.IsActive())
		{
			OptionsManager.Instance.InputMoveController(_isRight);
		}
	}

	private void NextField()
	{
		if (SettingsManager.Instance.IsActive())
		{
			return;
		}
		EventSystem current = EventSystem.current;
		Selectable selectable = current.currentSelectedGameObject?.GetComponent<Selectable>()?.FindSelectableOnDown();
		if (selectable != null)
		{
			InputField component = selectable.GetComponent<InputField>();
			if (component != null)
			{
				component.OnPointerClick(new PointerEventData(current));
			}
			current.SetSelectedGameObject(selectable.gameObject, new BaseEventData(current));
		}
	}

	private void DoButtonNorth()
	{
		if ((bool)KeyboardManager.Instance && KeyboardManager.Instance.IsActive())
		{
			KeyboardManager.Instance.DoDelete();
			return;
		}
		RaycastHit2D raycastHit2D = Physics2D.Raycast(GameManager.Instance.cameraMain.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
		if (raycastHit2D.collider != null && raycastHit2D.transform != null)
		{
			CardItem component2;
			if ((bool)HeroSelectionManager.Instance && raycastHit2D.collider.TryGetComponent<HeroSelection>(out var component))
			{
				component.RightClick();
			}
			else if (raycastHit2D.collider.TryGetComponent<CardItem>(out component2))
			{
				component2.RightClick();
			}
		}
	}

	private void DoButtonEast()
	{
		GameManager.Instance.EscapeFunction();
	}

	private void DoButtonWest()
	{
		if ((bool)KeyboardManager.Instance && GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsConnected())
		{
			if (!KeyboardManager.Instance.IsActive())
			{
				KeyboardManager.Instance.ShowKeyboard(state: true, chat: true);
			}
			else
			{
				KeyboardManager.Instance.ShowKeyboard(state: false);
			}
		}
	}

	private void DoKeyBinding(InputAction.CallbackContext _context)
	{
		if (Keyboard.current != null)
		{
			if (_context.control == Keyboard.current[Key.Tab])
			{
				NextField();
				return;
			}
			if (GameManager.Instance.ConfigKeyboardShortcuts)
			{
				if (EventSystem.current.currentSelectedGameObject != null && (EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null || EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() != null))
				{
					return;
				}
				if (_context.control == Keyboard.current[Key.Digit0])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(0);
					}
				}
				else if (_context.control == Keyboard.current[Key.Numpad0])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(0);
					}
				}
				else if (_context.control == Keyboard.current[Key.Digit1])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(1);
					}
				}
				else if (_context.control == Keyboard.current[Key.Numpad1])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(1);
					}
				}
				else if (_context.control == Keyboard.current[Key.Digit2])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(2);
					}
				}
				else if (_context.control == Keyboard.current[Key.Numpad2])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(2);
					}
				}
				else if (_context.control == Keyboard.current[Key.Digit3])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(3);
					}
				}
				else if (_context.control == Keyboard.current[Key.Numpad3])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(3);
					}
				}
				else if (_context.control == Keyboard.current[Key.Digit4])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(4);
					}
				}
				else if (_context.control == Keyboard.current[Key.Numpad4])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(4);
					}
				}
				else if (_context.control == Keyboard.current[Key.Digit5])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(5);
					}
				}
				else if (_context.control == Keyboard.current[Key.Numpad5])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(5);
					}
				}
				else if (_context.control == Keyboard.current[Key.Digit6])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(6);
					}
				}
				else if (_context.control == Keyboard.current[Key.Numpad6])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(6);
					}
				}
				else if (_context.control == Keyboard.current[Key.Digit7])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(7);
					}
				}
				else if (_context.control == Keyboard.current[Key.Numpad7])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(7);
					}
				}
				else if (_context.control == Keyboard.current[Key.Digit8])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(8);
					}
				}
				else if (_context.control == Keyboard.current[Key.Numpad8])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(8);
					}
				}
				else if (_context.control == Keyboard.current[Key.Digit9])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(9);
					}
				}
				else if (_context.control == Keyboard.current[Key.Numpad9])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardNum(9);
					}
				}
				else if (_context.control == Keyboard.current[Key.Space])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardSpace();
					}
				}
				else if (_context.control == Keyboard.current[Key.R])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardEmote(0);
					}
				}
				else if (_context.control == Keyboard.current[Key.E])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardEmote(1);
					}
				}
				else if (_context.control == Keyboard.current[Key.S])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardEmote(2);
					}
				}
				else if (_context.control == Keyboard.current[Key.A])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardEmote(3);
					}
				}
				else if (_context.control == Keyboard.current[Key.W])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardEmote(4);
					}
				}
				else if (_context.control == Keyboard.current[Key.Q])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardEmote(5);
					}
				}
				else if (_context.control == Keyboard.current[Key.NumpadEnter] || _context.control == Keyboard.current[Key.Enter])
				{
					if ((bool)MatchManager.Instance)
					{
						MatchManager.Instance.KeyboardEnter();
					}
				}
				else if (_context.control == Keyboard.current[Key.LeftCtrl] || _context.control == Keyboard.current[Key.RightCtrl])
				{
					DoFirePerformed();
				}
				else if (_context.control == Keyboard.current[Key.LeftAlt] || _context.control == Keyboard.current[Key.RightAlt])
				{
					DoButtonNorth();
				}
				else if (_context.control == Keyboard.current[Key.PageDown])
				{
					DoNextPage();
				}
				else if (_context.control == Keyboard.current[Key.PageUp])
				{
					DoPrevPage();
				}
			}
		}
		if (Gamepad.current != null)
		{
			if (_context.control == Gamepad.current.leftTrigger)
			{
				DoTrigger(_isRight: false);
			}
			else if (_context.control == Gamepad.current.rightTrigger)
			{
				DoTrigger(_isRight: true);
			}
			else if (_context.control == Gamepad.current.leftShoulder)
			{
				DoShoulder(_isRight: false);
			}
			else if (_context.control == Gamepad.current.rightShoulder)
			{
				DoShoulder(_isRight: true);
			}
			else if (_context.control == Gamepad.current.buttonNorth)
			{
				DoButtonNorth();
			}
			else if (_context.control == Gamepad.current.buttonEast)
			{
				DoButtonEast();
			}
			else if (_context.control == Gamepad.current.buttonWest)
			{
				DoButtonWest();
			}
		}
		if (!GameManager.Instance.GetDeveloperMode() || Keyboard.current == null || (EventSystem.current.currentSelectedGameObject != null && (EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null || EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() != null)))
		{
			return;
		}
		if (_context.control == Keyboard.current[Key.F1])
		{
			if ((bool)MapManager.Instance || (bool)TownManager.Instance)
			{
				AtOManager.Instance.GetHero(0).GrantExperience(150);
			}
			else if ((bool)MatchManager.Instance)
			{
				MatchManager.Instance.KeyboardEnergy();
			}
		}
		else if (_context.control == Keyboard.current[Key.F2])
		{
			if ((bool)MapManager.Instance || (bool)TownManager.Instance)
			{
				AtOManager.Instance.GetHero(1).GrantExperience(150);
			}
		}
		else if (_context.control == Keyboard.current[Key.F3])
		{
			if ((bool)MapManager.Instance || (bool)TownManager.Instance)
			{
				AtOManager.Instance.GetHero(2).GrantExperience(150);
			}
		}
		else if (_context.control == Keyboard.current[Key.F4])
		{
			if ((bool)MapManager.Instance || (bool)TownManager.Instance)
			{
				AtOManager.Instance.GetHero(3).GrantExperience(150);
			}
		}
		else if (_context.control == Keyboard.current[Key.F5])
		{
			if ((bool)MapManager.Instance || (bool)TownManager.Instance)
			{
				AtOManager.Instance.GivePlayer(0, 400);
			}
		}
		else if (_context.control == Keyboard.current[Key.F6])
		{
			if ((bool)MapManager.Instance || (bool)TownManager.Instance)
			{
				AtOManager.Instance.GivePlayer(1, 400);
			}
		}
		else if (_context.control == Keyboard.current[Key.F7])
		{
			if ((bool)MapManager.Instance || (bool)TownManager.Instance || (bool)MatchManager.Instance)
			{
				SandboxManager.Instance.ShowSandbox();
			}
		}
		else if (_context.control == Keyboard.current[Key.F8])
		{
			if ((bool)MapManager.Instance || (bool)TownManager.Instance)
			{
				GameManager.Instance.SaveGameDeveloper();
			}
			else if ((bool)MatchManager.Instance)
			{
				MatchManager.Instance.KeyboardDbg();
			}
		}
		else if (_context.control == Keyboard.current[Key.F9])
		{
			NetworkManager.Instance.ShowLagSimulatorTrigger();
		}
		else if (_context.control == Keyboard.current[Key.F10])
		{
			if (TomeManager.Instance.IsActive())
			{
				PlayerManager.Instance.UnlockHeroes();
			}
			else if ((bool)MatchManager.Instance)
			{
				MatchManager.Instance.KeyboardReloadCombat();
			}
		}
		else if (_context.control == Keyboard.current[Key.F11])
		{
			if (TomeManager.Instance.IsActive())
			{
				PlayerManager.Instance.UnlockCards();
			}
			else if (MapManager.Instance != null)
			{
				MapManager.Instance.corruption.NextCorruption();
			}
		}
		else if (_context.control == Keyboard.current[Key.F12])
		{
			if ((bool)MapManager.Instance)
			{
				MapManager.Instance.ItemCreator.GetComponent<ItemCreator>().Draw();
			}
			else if ((bool)TownManager.Instance)
			{
				TownManager.Instance.ItemCreator.GetComponent<ItemCreator>().Draw();
			}
			else if ((bool)MatchManager.Instance)
			{
				MatchManager.Instance.CardCreator.GetComponent<CardCreator>().Draw();
			}
			else if ((bool)ChallengeSelectionManager.Instance)
			{
				AtOManager.Instance.FinishObeliskDraft();
			}
			else if ((bool)HeroSelectionManager.Instance)
			{
				HeroSelectionManager.Instance.weeklySelector.GetComponent<WeeklySelector>().Draw();
			}
		}
		else if (_context.control == Keyboard.current[Key.Digit1])
		{
			if ((bool)HeroSelectionManager.Instance)
			{
				HeroSelectionManager.Instance.IncreaseHeroProgressDev(0);
			}
		}
		else if (_context.control == Keyboard.current[Key.Digit2])
		{
			if ((bool)HeroSelectionManager.Instance)
			{
				HeroSelectionManager.Instance.IncreaseHeroProgressDev(1);
			}
		}
		else if (_context.control == Keyboard.current[Key.Digit3])
		{
			if ((bool)HeroSelectionManager.Instance)
			{
				HeroSelectionManager.Instance.IncreaseHeroProgressDev(2);
			}
		}
		else if (_context.control == Keyboard.current[Key.Digit4])
		{
			if ((bool)HeroSelectionManager.Instance)
			{
				HeroSelectionManager.Instance.IncreaseHeroProgressDev(3);
			}
		}
		else if (_context.control == Keyboard.current[Key.Period])
		{
			GameManager.Instance.consoleGUI.ConsoleShow();
		}
		else if (_context.control == Keyboard.current[Key.Comma])
		{
			GameManager.Instance.DebugShow();
		}
	}

	private void DoFire(InputAction.CallbackContext _context)
	{
		if (_context.performed)
		{
			DoFirePerformed();
		}
	}

	private void DoFirePerformed()
	{
		if (SettingsManager.Instance.IsActive())
		{
			return;
		}
		pointerEventData.position = Input.mousePosition;
		raycastResultList.Clear();
		EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
		for (int i = 0; i < raycastResultList.Count; i++)
		{
			if (raycastResultList[i].gameObject.TryGetComponent<Button>(out var component))
			{
				component.onClick.Invoke();
				return;
			}
			if (raycastResultList[i].gameObject.TryGetComponent<Toggle>(out var component2))
			{
				component2.isOn = !component2.isOn;
				return;
			}
			if (raycastResultList[i].gameObject.transform.parent.gameObject.TryGetComponent<Toggle>(out var component3))
			{
				component3.isOn = !component3.isOn;
				return;
			}
			if (raycastResultList[i].gameObject.TryGetComponent<TMP_InputField>(out var component4))
			{
				if (component4.gameObject.name == "ChatField")
				{
					KeyboardManager.Instance.ShowKeyboard(state: true, chat: true);
					return;
				}
				KeyboardManager.Instance.ShowKeyboard(state: true);
				KeyboardManager.Instance.SetInputField(component4);
				return;
			}
			if (raycastResultList[i].gameObject.TryGetComponent<TMP_Dropdown>(out var component5))
			{
				component5.transform.GetChild(2).transform.localPosition = new Vector3(component5.transform.GetChild(2).transform.localPosition.x, component5.transform.GetChild(2).transform.localPosition.y, -100f);
				if (component5.value < component5.options.Count - 1)
				{
					component5.value++;
				}
				else
				{
					component5.value = 0;
				}
				component5.RefreshShownValue();
				StartCoroutine(HideDrop(component5));
				return;
			}
		}
		RaycastHit2D raycastHit2D = Physics2D.Raycast(GameManager.Instance.cameraMain.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
		if (raycastHit2D.collider != null && raycastHit2D.transform != null)
		{
			HeroSelection component7;
			if ((bool)MatchManager.Instance && raycastHit2D.collider.TryGetComponent<CardItem>(out var component6))
			{
				component6.OnMouseUpController();
			}
			else if ((bool)HeroSelectionManager.Instance && raycastHit2D.collider.TryGetComponent<HeroSelection>(out component7))
			{
				component7.OnClickedController();
			}
			else
			{
				raycastHit2D.collider.gameObject.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private IEnumerator HideDrop(TMP_Dropdown dpd)
	{
		yield return new WaitForSeconds(0.01f);
		dpd.Hide();
	}

	private void DoZoom(InputAction.CallbackContext _context)
	{
		zoomVector.x = _context.ReadValue<Vector2>().x;
		zoomVector.y = _context.ReadValue<Vector2>().y;
		if (zoomVector.x != 0f || zoomVector.y != 0f)
		{
			if (TomeManager.Instance.IsActive())
			{
				TomeManager.Instance.DoMouseScroll(zoomVector);
			}
			else if ((bool)CardCraftManager.Instance)
			{
				CardCraftManager.Instance.DoMouseScroll(zoomVector);
			}
		}
	}

	private void DoMovement(InputAction.CallbackContext _context)
	{
		movVector.x = _context.ReadValue<Vector2>().x;
		movVector.y = _context.ReadValue<Vector2>().y;
		if (_context.control.device.displayName != "Keyboard")
		{
			DoMovementVector(movVector);
		}
		else if (GameManager.Instance.ConfigKeyboardShortcuts)
		{
			DoMovementVector(movVector, fromKeyboard: true);
		}
	}

	private void DoMovementVector(Vector2 vct, bool fromKeyboard = false)
	{
		if ((TomeManager.Instance != null && TomeManager.Instance.searchInput.text.Length > 0 && TomeManager.Instance.searchInput.isFocused) || (CardCraftManager.Instance != null && CardCraftManager.Instance.searchInput.text.Length > 0 && CardCraftManager.Instance.searchInput.isFocused) || (Mathf.Abs(vct.x) != 1f && Mathf.Abs(vct.y) != 1f && (!(Mathf.Abs(vct.x) > 0.55f) || !(Mathf.Abs(vct.y) > 0.55f))))
		{
			return;
		}
		if (!fromKeyboard)
		{
			if (!(_timer >= _duration))
			{
				return;
			}
			_timer = 0f;
		}
		if (Mathf.Abs(vct.x) > Mathf.Abs(vct.y))
		{
			vct.y = 0f;
		}
		else
		{
			vct.x = 0f;
		}
		bool goingUp = false;
		bool goingRight = false;
		bool goingDown = false;
		bool goingLeft = false;
		if (vct.y > 0f)
		{
			goingUp = true;
		}
		else if (vct.y < 0f)
		{
			goingDown = true;
		}
		else if (vct.x < 0f)
		{
			goingLeft = true;
		}
		else
		{
			goingRight = true;
		}
		if (KeyboardManager.Instance.IsActive())
		{
			KeyboardManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if (AlertManager.Instance.IsActive())
		{
			AlertManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if (SettingsManager.Instance.IsActive())
		{
			SettingsManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if (CardScreenManager.Instance.IsActive())
		{
			CardScreenManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if (DamageMeterManager.Instance.IsActive())
		{
			DamageMeterManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if (TomeManager.Instance.IsActive())
		{
			TomeManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if ((bool)MainMenuManager.Instance)
		{
			MainMenuManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if (PerkTree.Instance.IsActive())
		{
			PerkTree.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if (SandboxManager.Instance.IsActive())
		{
			SandboxManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if (MadnessManager.Instance.IsActive())
		{
			MadnessManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if ((bool)LobbyManager.Instance)
		{
			LobbyManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if ((bool)HeroSelectionManager.Instance)
		{
			if (HeroSelectionManager.Instance.charPopup.IsOpened())
			{
				HeroSelectionManager.Instance.charPopup.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
			else
			{
				HeroSelectionManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
		}
		else if (GiveManager.Instance.IsActive())
		{
			GiveManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if ((bool)IntroNewGameManager.Instance)
		{
			IntroNewGameManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if ((bool)CinematicManager.Instance)
		{
			CinematicManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if ((bool)ChallengeSelectionManager.Instance)
		{
			CardCraftManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if ((bool)CardPlayerManager.Instance)
		{
			CardPlayerManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if ((bool)CardPlayerPairsManager.Instance)
		{
			CardPlayerPairsManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if ((bool)EventManager.Instance)
		{
			if (MapManager.Instance.characterWindow.IsActive())
			{
				MapManager.Instance.characterWindow.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
			else if (MapManager.Instance.Conflict != null && MapManager.Instance.Conflict.IsActive())
			{
				MapManager.Instance.Conflict.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
			else
			{
				EventManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
		}
		else if ((bool)TownManager.Instance)
		{
			if (TownManager.Instance.characterWindow.IsActive())
			{
				TownManager.Instance.characterWindow.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
			else if ((bool)CardCraftManager.Instance)
			{
				CardCraftManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
			else
			{
				TownManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
		}
		else if ((bool)MapManager.Instance)
		{
			if (MapManager.Instance.characterWindow.IsActive())
			{
				MapManager.Instance.characterWindow.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
			else if (MapManager.Instance.Conflict != null && MapManager.Instance.Conflict.IsActive())
			{
				MapManager.Instance.Conflict.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
			else if ((bool)CardCraftManager.Instance)
			{
				CardCraftManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
			else
			{
				MapManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
		}
		else if ((bool)RewardsManager.Instance)
		{
			if (RewardsManager.Instance.characterWindowUI.IsActive())
			{
				RewardsManager.Instance.characterWindowUI.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
			else
			{
				RewardsManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
		}
		else if ((bool)LootManager.Instance)
		{
			if (LootManager.Instance.characterWindowUI.IsActive())
			{
				LootManager.Instance.characterWindowUI.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
			else
			{
				LootManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
		}
		else if ((bool)FinishRunManager.Instance)
		{
			FinishRunManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
		}
		else if ((bool)MatchManager.Instance)
		{
			if (MatchManager.Instance.characterWindow.IsActive())
			{
				MatchManager.Instance.characterWindow.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
			else
			{
				MatchManager.Instance.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			}
		}
	}

	private void DoEscape(InputAction.CallbackContext _context)
	{
		GameManager.Instance.EscapeFunction();
		if ((bool)LogosManager.Instance)
		{
			StartCoroutine(CheckEscapeIsPressed());
		}
	}

	private void DoNextPage()
	{
		if (TomeManager.Instance.IsActive())
		{
			TomeManager.Instance.DoNextPage();
		}
		else if ((bool)CardCraftManager.Instance)
		{
			CardCraftManager.Instance.ControllerNextPage();
		}
	}

	private void DoPrevPage()
	{
		if (TomeManager.Instance.IsActive())
		{
			TomeManager.Instance.DoPrevPage();
		}
		else if ((bool)CardCraftManager.Instance)
		{
			CardCraftManager.Instance.ControllerNextPage(_isNext: false);
		}
	}
}
