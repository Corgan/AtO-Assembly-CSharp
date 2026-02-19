using UnityEngine;

public class CharacterGOItem : MonoBehaviour
{
	public CharacterItem _characterItem;

	private void Start()
	{
		Animator component = base.gameObject.GetComponent<Animator>();
		if (component != null && base.transform.parent != null && base.transform.parent.parent != null && base.transform.parent.parent.name == "NPCs")
		{
			AnimatorClipInfo[] currentAnimatorClipInfo = component.GetCurrentAnimatorClipInfo(0);
			float length = currentAnimatorClipInfo[0].clip.length;
			_ = currentAnimatorClipInfo[0].clip.name;
			float normalizedTime = Random.Range(0f, length);
			component.Play(component.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, normalizedTime);
		}
	}

	public void OnMouseUp()
	{
		if (_characterItem != null)
		{
			_characterItem.fOnMouseUp();
		}
	}

	private void OnMouseOver()
	{
		if (_characterItem != null)
		{
			_characterItem.fOnMouseOver();
		}
	}

	private void OnMouseEnter()
	{
		if (_characterItem != null)
		{
			_characterItem.fOnMouseEnter();
		}
	}

	private void OnMouseExit()
	{
		if (_characterItem != null)
		{
			_characterItem.fOnMouseExit();
		}
	}
}
