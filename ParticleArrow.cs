using System.Collections.Generic;
using UnityEngine;

public class ParticleArrow : MonoBehaviour
{
	public Vector3 point1 = Vector3.zero;

	public Vector3 point2 = Vector3.zero;

	public Vector3 point3 = Vector3.zero;

	public bool drawLine = true;

	private List<Vector3> pointList;

	private int vertexCount = 24;

	public int movementIndex;

	public TrailRenderer trailRenderer;

	public ParticleSystem particleS;

	public CursorArrow cursorArrow;

	private void Start()
	{
		trailRenderer = GetComponent<TrailRenderer>();
		particleS = GetComponent<ParticleSystem>();
		pointList = new List<Vector3>();
	}

	private void Update()
	{
		if (Time.frameCount % 1 != 0)
		{
			return;
		}
		movementIndex++;
		if (movementIndex >= vertexCount)
		{
			movementIndex = 0;
			base.transform.position = cursorArrow.lineRenderer.GetPosition(0);
			return;
		}
		base.transform.position = cursorArrow.lineRenderer.GetPosition(movementIndex);
		if (movementIndex > vertexCount - 2)
		{
			particleS.Stop();
		}
		else if (movementIndex == 4)
		{
			particleS.Play();
		}
	}
}
