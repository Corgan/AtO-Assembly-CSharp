using UnityEngine;

public class anim : MonoBehaviour
{
	public ParticleSystem vfx;

	public void PrintEvent(string s)
	{
		vfx.Play();
	}

	public void TriggerVFX()
	{
		if (vfx != null)
		{
			vfx.Play();
		}
	}
}
