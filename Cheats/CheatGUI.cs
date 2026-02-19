using UnityEngine;

namespace Cheats;

public class CheatGUI : MonoBehaviour
{
	private bool _showCheats;

	private GameManager _gameManager;

	private GUIStyle _headerStyle;

	private Vector2 _scrollPos;

	private GUIStyle _toggleStyle;

	private GUIStyle _buttonStyle;

	private GUIStyle _windowStyle;

	private Texture2D _transparentTex;

	private int _fontSize = 24;

	public float panelAlpha = 0.8f;

	private void Awake()
	{
		_gameManager = GameManager.Instance;
		if (!_gameManager.CheatMode || Object.FindObjectsOfType<CheatGUI>().Length > 1)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Object.DontDestroyOnLoad(base.gameObject);
		_transparentTex = new Texture2D(1, 1);
		_transparentTex.SetPixel(0, 0, new Color(0f, 0f, 0f, panelAlpha));
		_transparentTex.Apply();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			_showCheats = !_showCheats;
		}
	}

	private void OnGUI()
	{
		if (_showCheats && !(_gameManager == null))
		{
			if (_windowStyle == null)
			{
				_windowStyle = new GUIStyle(GUI.skin.window);
			}
			_windowStyle.normal.background = _transparentTex;
			if (_headerStyle == null)
			{
				_headerStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = _fontSize,
					fontStyle = FontStyle.Bold,
					normal = 
					{
						textColor = Color.white
					}
				};
			}
			if (_toggleStyle == null)
			{
				_toggleStyle = new GUIStyle(GUI.skin.toggle)
				{
					fontSize = _fontSize,
					fixedHeight = 25f,
					normal = 
					{
						textColor = Color.white
					}
				};
			}
			if (_buttonStyle == null)
			{
				_buttonStyle = new GUIStyle(GUI.skin.button)
				{
					fontSize = _fontSize,
					fixedHeight = 30f
				};
			}
			GUILayout.BeginArea(new Rect(500f, 10f, 650f, 950f), "Cheats", _windowStyle);
			_scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Width(550f), GUILayout.Height(850f));
			CheatDrawer.DrawCheatsRuntime(_gameManager, _headerStyle, _toggleStyle, _buttonStyle);
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
	}
}
