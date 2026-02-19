using UnityEngine;

public class AnimatedCurveObject : MonoBehaviour
{
	[SerializeField]
	private AnimationCurve animationCurve;

	[SerializeField]
	private float speed = 100f;

	private float curveDeltaTime;

	private void Start()
	{
		Object.Destroy(base.gameObject, 15f);
	}

	private void Update()
	{
		Vector3 position = base.transform.position;
		position.z += speed * Time.deltaTime;
		curveDeltaTime += Time.deltaTime;
		position.y = animationCurve.Evaluate(curveDeltaTime);
		base.transform.position = position;
	}
}
