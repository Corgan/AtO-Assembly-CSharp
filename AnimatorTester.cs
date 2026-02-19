using UnityEngine;

public class AnimatorTester : MonoBehaviour
{
	private Animator _animator;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (_animator != null)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				_animator.SetTrigger("hit");
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				_animator.SetTrigger("attack");
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				_animator.SetTrigger("cast");
			}
		}
	}
}
