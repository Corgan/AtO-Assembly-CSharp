using UnityEngine;

public class Thunder : MonoBehaviour
{
	private int count;

	private int countRay = -1;

	private Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		anim.enabled = false;
	}

	private void Start()
	{
		GenerateCountRay();
	}

	private void Update()
	{
		if (Time.frameCount % 48 == 0)
		{
			count++;
			if (count == countRay)
			{
				anim.enabled = true;
				anim.Play(0);
			}
			if (count > countRay + 5)
			{
				anim.enabled = false;
				count = 0;
			}
		}
	}

	private void GenerateCountRay()
	{
		countRay = Random.Range(15, 80);
	}
}
