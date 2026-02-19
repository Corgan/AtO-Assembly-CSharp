using UnityEngine;

public class PerkNodeAmplify : MonoBehaviour
{
	public Transform bg2;

	public Transform bg3;

	public Transform bg4;

	public Transform amplifyNodes;

	private int hideCounter;

	private void Update()
	{
		if (Time.frameCount % 4 != 0 || !base.gameObject.activeSelf)
		{
			return;
		}
		if (hideCounter < 5)
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D[] array = Physics2D.RaycastAll(new Vector2(vector.x, vector.y), Vector2.zero, 0f);
			foreach (RaycastHit2D raycastHit2D in array)
			{
				if (raycastHit2D.transform.gameObject.name == base.gameObject.name)
				{
					hideCounter = 0;
					return;
				}
			}
			hideCounter++;
		}
		else
		{
			Hide();
		}
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
		hideCounter = 0;
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void SetForNodes(int _numNodes)
	{
		PolygonCollider2D component = GetComponent<PolygonCollider2D>();
		switch (_numNodes)
		{
		case 2:
			bg2.gameObject.SetActive(value: true);
			bg3.gameObject.SetActive(value: false);
			bg4.gameObject.SetActive(value: false);
			amplifyNodes.localPosition = new Vector3(-0.7f, amplifyNodes.localPosition.y, amplifyNodes.localPosition.z);
			component.points = bg2.GetComponent<PolygonCollider2D>().points;
			break;
		case 3:
			bg2.gameObject.SetActive(value: false);
			bg3.gameObject.SetActive(value: true);
			bg4.gameObject.SetActive(value: false);
			amplifyNodes.localPosition = new Vector3(-1.4f, amplifyNodes.localPosition.y, amplifyNodes.localPosition.z);
			component.points = bg3.GetComponent<PolygonCollider2D>().points;
			break;
		default:
			bg2.gameObject.SetActive(value: false);
			bg3.gameObject.SetActive(value: false);
			bg4.gameObject.SetActive(value: true);
			amplifyNodes.localPosition = new Vector3(-2.1f, amplifyNodes.localPosition.y, amplifyNodes.localPosition.z);
			component.points = bg4.GetComponent<PolygonCollider2D>().points;
			break;
		}
	}
}
