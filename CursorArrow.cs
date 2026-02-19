using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorArrow : MonoBehaviour
{
	public Vector3 point1 = Vector3.zero;

	public Vector3 point2 = Vector3.zero;

	public Vector3 point3 = Vector3.zero;

	private bool drawLine;

	public List<Vector3> pointList;

	public LineRenderer lineRenderer;

	private int vertexCount = 30;

	private Color darkGreen = new Color(0f, 1f, 0f, 0.5f);

	private Color darkRed = new Color(1f, 0f, 0f, 0.5f);

	private Color darkGold = new Color(1f, 0.69f, 0f, 0.5f);

	private Color standart;

	private Transform auxHitTransform;

	private Vector3 desiredPosition;

	private Vector3 safePosition;

	private float safeDifferenceY;

	private Vector3 oldPosition = Vector3.zero;

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		pointList = new List<Vector3>();
		standart = lineRenderer.startColor;
	}

	public void ClearPoints()
	{
		pointList.Clear();
	}

	private void Update()
	{
		if (!drawLine)
		{
			return;
		}
		desiredPosition.x = point1.x;
		desiredPosition.y = 1f - Mathf.Pow(point3.x / 20f, 2f);
		desiredPosition.z = 1f;
		if (desiredPosition.x == oldPosition.x && desiredPosition.y == oldPosition.y)
		{
			return;
		}
		oldPosition = desiredPosition;
		safePosition = (point1 + point3) * 0.5f;
		safeDifferenceY = desiredPosition.y + 1f - point1.y;
		point2 = Vector3.Lerp(safePosition, desiredPosition, Mathf.Clamp((Mathf.Abs(point3.x - point1.x) - 1f) * 0.25f + (point3.y - point1.y) / safeDifferenceY, 0f, 1f));
		for (float num = 0f; num < 1f; num += 1f / (float)vertexCount)
		{
			Vector3 a = Vector3.Lerp(point1, point2, num);
			Vector3 b = Vector3.Lerp(point2, point3, num);
			Vector3 item = Vector3.Lerp(a, b, num);
			pointList.Add(item);
		}
		pointList.Add(point3);
		lineRenderer.positionCount = pointList.Count;
		lineRenderer.SetPositions(pointList.ToArray());
		pointList.Clear();
		if (!(MatchManager.Instance != null))
		{
			return;
		}
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = 10f;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePosition), Vector2.zero);
		if ((bool)raycastHit2D)
		{
			if (Gamepad.current != null)
			{
				auxHitTransform = raycastHit2D.collider.transform;
				MatchManager.Instance.SetTarget(auxHitTransform);
			}
			else if (raycastHit2D.collider.transform != auxHitTransform)
			{
				auxHitTransform = raycastHit2D.collider.transform;
				MatchManager.Instance.SetTarget(auxHitTransform);
			}
		}
		else if (auxHitTransform != null)
		{
			auxHitTransform = null;
			MatchManager.Instance.SetTarget(null);
		}
	}

	public void SetColor(string theColor)
	{
		switch (theColor)
		{
		case "red":
			lineRenderer.startColor = new Color(darkRed.r, darkRed.g, darkRed.b, darkRed.a + 0.3f);
			lineRenderer.endColor = darkRed;
			break;
		case "green":
			lineRenderer.startColor = new Color(darkGreen.r, darkGreen.g, darkGreen.b, darkGreen.a + 0.3f);
			lineRenderer.endColor = darkGreen;
			break;
		case "gold":
			lineRenderer.startColor = new Color(darkGold.r, darkGold.g, darkGold.b, darkGold.a + 0.3f);
			lineRenderer.endColor = darkGold;
			break;
		default:
			lineRenderer.startColor = new Color(darkGold.r, darkGold.g, darkGold.b, darkGold.a + 0.3f);
			lineRenderer.endColor = darkGold;
			break;
		}
	}

	public void Rotation(Quaternion rotation)
	{
		base.transform.localRotation = rotation;
	}

	public void StartDraw(bool canInstaCast = true)
	{
		if (drawLine)
		{
			return;
		}
		drawLine = true;
		if (MatchManager.Instance != null)
		{
			if (canInstaCast)
			{
				SetColor("green");
			}
			else
			{
				SetColor("");
			}
			MatchManager.Instance.SetGlobalOutlines(state: true);
		}
	}

	public void StopDraw()
	{
		if (drawLine)
		{
			drawLine = false;
			lineRenderer.positionCount = 0;
			if (MatchManager.Instance != null)
			{
				MatchManager.Instance.SetGlobalOutlines(state: false);
			}
		}
	}

	public void DrawArrowWithPoints(Vector3 ori, Vector3 dest)
	{
		Mathf.Clamp(Mathf.Abs(ori.x - dest.x) * 0.01f, 4f, 6f);
		point1 = new Vector3(ori.x, ori.y, 1f);
		point2 = point1;
		point3 = new Vector3(dest.x, dest.y, 1f);
		StartDraw();
	}
}
