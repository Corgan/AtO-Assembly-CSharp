using System.Collections.Generic;
using UnityEngine;

public class ChallengeBossBanners : MonoBehaviour
{
	public SpriteRenderer banner0Sprite;

	public SpriteRenderer banner1Sprite;

	public SpriteRenderer banner2Sprite;

	public Transform banner0Killed;

	public Transform banner1Killed;

	public Transform banner2Killed;

	private List<string> bossNodes;

	private List<string> bossNames;

	private void Awake()
	{
		Hide();
	}

	public bool IsActive()
	{
		if (base.gameObject.activeSelf)
		{
			return true;
		}
		return false;
	}

	public void SetBosses()
	{
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			Hide();
			return;
		}
		bossNodes = new List<string>();
		bossNames = new List<string>();
		for (int i = 0; i < 3; i++)
		{
			Sprite sprite = null;
			if (!GameManager.Instance.IsWeeklyChallenge())
			{
				string text = "";
				string text2 = "";
				NodeData nodeData = null;
				CombatData combatData = null;
				foreach (KeyValuePair<string, NodeData> item in Globals.Instance.NodeDataSource)
				{
					nodeData = item.Value;
					string text3 = "";
					Enums.CombatTier combatTier = Enums.CombatTier.T0;
					switch (i)
					{
					case 0:
						text3 = AtOManager.Instance.obeliskLow.ToLower();
						combatTier = Enums.CombatTier.T8;
						break;
					case 1:
						text3 = AtOManager.Instance.obeliskHigh.ToLower();
						combatTier = Enums.CombatTier.T9;
						break;
					case 2:
						text3 = AtOManager.Instance.obeliskFinal.ToLower();
						break;
					}
					if (!(nodeData.NodeZone.ZoneId.ToLower() == text3))
					{
						continue;
					}
					if (i < 2)
					{
						if (nodeData.NodeCombatTier == combatTier && nodeData.NodeCombat.Length != 0 && nodeData.NodeCombat[0] != null)
						{
							text = nodeData.NodeId;
							text2 = nodeData.NodeCombat[0].CombatId;
							break;
						}
						continue;
					}
					int num = 0;
					if (nodeData.NodeId == "of1_10" || nodeData.NodeId == "of2_10")
					{
						text = nodeData.NodeId;
						Random.InitState((text + AtOManager.Instance.GetGameId() + "finalBoss").GetDeterministicHashCode());
						num = Random.Range(0, nodeData.NodeCombat.Length);
						combatData = nodeData.NodeCombat[num];
						text2 = combatData.CombatId;
					}
				}
				bossNodes.Add(text);
				NPCData[] array = null;
				if (i < 2)
				{
					int deterministicHashCode = (text + AtOManager.Instance.GetGameId() + text2).GetDeterministicHashCode();
					array = Functions.GetRandomCombat(nodeData.NodeCombatTier, deterministicHashCode, text, forceIsThereRare: true);
				}
				else
				{
					array = combatData.NPCList;
				}
				if (array != null)
				{
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j] != null && (array[j].IsNamed || array[j].IsBoss))
						{
							sprite = array[j].SpriteSpeed;
							break;
						}
					}
				}
			}
			else
			{
				ChallengeData weeklyData = Globals.Instance.GetWeeklyData(AtOManager.Instance.GetWeekly());
				switch (i)
				{
				case 0:
					sprite = weeklyData.Boss1.SpriteSpeed;
					bossNames.Add(weeklyData.Boss1.NPCName);
					break;
				case 1:
					sprite = weeklyData.Boss2.SpriteSpeed;
					bossNames.Add(weeklyData.Boss2.NPCName);
					break;
				case 2:
				{
					sprite = weeklyData.Boss2.SpriteSpeed;
					for (int k = 0; k < weeklyData.BossCombat.NPCList.Length; k++)
					{
						if (weeklyData.BossCombat.NPCList[k] != null && (weeklyData.BossCombat.NPCList[k].IsNamed || weeklyData.BossCombat.NPCList[k].IsBoss))
						{
							sprite = weeklyData.BossCombat.NPCList[k].SpriteSpeed;
							bossNames.Add(weeklyData.BossCombat.NPCList[k].NPCName);
							break;
						}
					}
					break;
				}
				}
			}
			SetBossSprite(i, sprite);
		}
		for (int l = 0; l < 3; l++)
		{
			if (!GameManager.Instance.IsWeeklyChallenge())
			{
				SetBossKilled(l, AtOManager.Instance.mapVisitedNodes.Contains(bossNodes[l]));
			}
			else
			{
				SetBossKilled(l, AtOManager.Instance.IsBossKilled(bossNames[l]));
			}
		}
		Show();
	}

	public void Show()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
	}

	public void Hide()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void SetBossKilled(int _index, bool _status)
	{
		switch (_index)
		{
		case 0:
			if (_status)
			{
				if (!banner0Killed.gameObject.activeSelf)
				{
					banner0Killed.gameObject.SetActive(value: true);
				}
			}
			else if (banner0Killed.gameObject.activeSelf)
			{
				banner0Killed.gameObject.SetActive(value: false);
			}
			return;
		case 1:
			if (_status)
			{
				if (!banner1Killed.gameObject.activeSelf)
				{
					banner1Killed.gameObject.SetActive(value: true);
				}
			}
			else if (banner1Killed.gameObject.activeSelf)
			{
				banner1Killed.gameObject.SetActive(value: false);
			}
			return;
		}
		if (_status)
		{
			if (!banner2Killed.gameObject.activeSelf)
			{
				banner2Killed.gameObject.SetActive(value: true);
			}
		}
		else if (banner2Killed.gameObject.activeSelf)
		{
			banner2Killed.gameObject.SetActive(value: false);
		}
	}

	public void SetBossSprite(int _index, Sprite _sprite)
	{
		switch (_index)
		{
		case 0:
			banner0Sprite.sprite = _sprite;
			break;
		case 1:
			banner1Sprite.sprite = _sprite;
			break;
		default:
			banner2Sprite.sprite = _sprite;
			break;
		}
	}
}
