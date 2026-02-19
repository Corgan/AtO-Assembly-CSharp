using System;
using UnityEngine;

namespace PolygonArsenal;

public class PolygonBeamStatic : MonoBehaviour
{
	[Header("Prefabs")]
	public GameObject beamLineRendererPrefab;

	public GameObject beamStartPrefab;

	public GameObject beamEndPrefab;

	private GameObject beamStart;

	private GameObject beamEnd;

	private GameObject beam;

	private LineRenderer line;

	[Header("Beam Options")]
	public bool beamCollides = true;

	public float beamLength = 100f;

	public float beamEndOffset;

	public float textureScrollSpeed;

	public float textureLengthScale = 1f;

	[Header("Width Pulse Options")]
	public float widthMultiplier = 1.5f;

	private float customWidth;

	private float originalWidth;

	private float lerpValue;

	public float pulseSpeed = 1f;

	private bool pulseExpanding = true;

	private void Start()
	{
		SpawnBeam();
		originalWidth = line.startWidth;
		customWidth = originalWidth * widthMultiplier;
	}

	private void FixedUpdate()
	{
		if ((bool)beam)
		{
			line.SetPosition(0, base.transform.position);
			Vector3 vector = base.transform.position + base.transform.forward * beamLength;
			if (beamCollides && Physics.Raycast(base.transform.position, base.transform.forward, out var hitInfo))
			{
				vector = hitInfo.point - base.transform.forward * beamEndOffset;
				vector = ((Vector3.Distance(base.transform.position, vector) > beamLength) ? (base.transform.position + base.transform.forward * beamLength) : vector);
			}
			else
			{
				vector = base.transform.position + base.transform.forward * beamLength;
			}
			line.SetPosition(1, vector);
			beamStart.transform.position = base.transform.position;
			beamStart.transform.LookAt(vector);
			beamEnd.transform.position = vector;
			beamEnd.transform.LookAt(beamStart.transform.position);
			float num = Vector3.Distance(base.transform.position, vector);
			line.material.mainTextureScale = new Vector2(num / textureLengthScale, 1f);
			line.material.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0f);
		}
		if (pulseExpanding)
		{
			lerpValue += Time.deltaTime * pulseSpeed;
		}
		else
		{
			lerpValue -= Time.deltaTime * pulseSpeed;
		}
		if (lerpValue >= 1f)
		{
			pulseExpanding = false;
			lerpValue = 1f;
		}
		else if (lerpValue <= 0f)
		{
			pulseExpanding = true;
			lerpValue = 0f;
		}
		float num2 = Mathf.Lerp(originalWidth, customWidth, Mathf.Sin(lerpValue * MathF.PI));
		line.startWidth = num2;
		line.endWidth = num2;
	}

	public void SpawnBeam()
	{
		if ((bool)beamLineRendererPrefab)
		{
			beam = UnityEngine.Object.Instantiate(beamLineRendererPrefab);
			beam.transform.position = base.transform.position;
			beam.transform.parent = base.transform;
			beam.transform.rotation = base.transform.rotation;
			line = beam.GetComponent<LineRenderer>();
			line.useWorldSpace = true;
			line.positionCount = 2;
			beamStart = (beamStartPrefab ? UnityEngine.Object.Instantiate(beamStartPrefab, beam.transform) : null);
			beamEnd = (beamEndPrefab ? UnityEngine.Object.Instantiate(beamEndPrefab, beam.transform) : null);
		}
		else
		{
			Debug.LogError("A prefab with a line renderer must be assigned to the `beamLineRendererPrefab` field in the PolygonArsenalBeamStatic script on " + base.gameObject.name);
		}
	}
}
