using UnityEngine;

namespace AllIn1SpriteShader;

[ExecuteInEditMode]
public class SetGlobalTime : MonoBehaviour
{
	private int globalTime;

	private void Start()
	{
		globalTime = Shader.PropertyToID("globalUnscaledTime");
	}

	private void Update()
	{
		Shader.SetGlobalFloat(globalTime, Time.unscaledTime / 20f);
	}
}
