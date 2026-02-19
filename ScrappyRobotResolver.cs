using System.Collections.Generic;
using UnityEngine;

public static class ScrappyRobotResolver
{
	private static Dictionary<string, Dictionary<string, string>> robotRequirementMap = new Dictionary<string, Dictionary<string, string>>
	{
		{
			"robotbalanced",
			new Dictionary<string, string>
			{
				{ "armdisclauncher", "scrappy1" },
				{ "armblowtorch", "scrappy2" },
				{ "armsmallcannon", "scrappy3" },
				{ "armchargedrod", "scrappy4" },
				{ "armcoolingengine", "scrappy5" }
			}
		},
		{
			"robotheavy",
			new Dictionary<string, string>
			{
				{ "armdisclauncher", "scrappy6" },
				{ "armblowtorch", "scrappy7" },
				{ "armsmallcannon", "scrappy8" },
				{ "armchargedrod", "scrappy9" },
				{ "armcoolingengine", "scrappy10" }
			}
		},
		{
			"robotspiked",
			new Dictionary<string, string>
			{
				{ "armdisclauncher", "scrappy11" },
				{ "armblowtorch", "scrappy12" },
				{ "armsmallcannon", "scrappy13" },
				{ "armchargedrod", "scrappy14" },
				{ "armcoolingengine", "scrappy15" }
			}
		},
		{
			"robotlightweight",
			new Dictionary<string, string>
			{
				{ "armdisclauncher", "scrappy16" },
				{ "armblowtorch", "scrappy17" },
				{ "armsmallcannon", "scrappy18" },
				{ "armchargedrod", "scrappy19" },
				{ "armcoolingengine", "scrappy20" }
			}
		},
		{
			"robotengraved",
			new Dictionary<string, string>
			{
				{ "armdisclauncher", "scrappy21" },
				{ "armblowtorch", "scrappy22" },
				{ "armsmallcannon", "scrappy23" },
				{ "armchargedrod", "scrappy24" },
				{ "armcoolingengine", "scrappy25" }
			}
		}
	};

	private static Dictionary<string, List<string>> robotLayerList = new Dictionary<string, List<string>>
	{
		{
			"scrappy1",
			new List<string> { "robotbalanced", "armdisclauncher" }
		},
		{
			"scrappy2",
			new List<string> { "robotbalanced", "armblowtorch" }
		},
		{
			"scrappy3",
			new List<string> { "robotbalanced", "armsmallcannon" }
		},
		{
			"scrappy4",
			new List<string> { "robotbalanced", "armchargedrod" }
		},
		{
			"scrappy5",
			new List<string> { "robotbalanced", "armcoolingengine" }
		},
		{
			"scrappy6",
			new List<string> { "robotheavy", "armdisclauncher" }
		},
		{
			"scrappy7",
			new List<string> { "robotheavy", "armblowtorch" }
		},
		{
			"scrappy8",
			new List<string> { "robotheavy", "armsmallcannon" }
		},
		{
			"scrappy9",
			new List<string> { "robotheavy", "armchargedrod" }
		},
		{
			"scrappy10",
			new List<string> { "robotheavy", "armcoolingengine" }
		},
		{
			"scrappy11",
			new List<string> { "robotspiked", "armdisclauncher" }
		},
		{
			"scrappy12",
			new List<string> { "robotspiked", "armblowtorch" }
		},
		{
			"scrappy13",
			new List<string> { "robotspiked", "armsmallcannon" }
		},
		{
			"scrappy14",
			new List<string> { "robotspiked", "armchargedrod" }
		},
		{
			"scrappy15",
			new List<string> { "robotspiked", "armcoolingengine" }
		},
		{
			"scrappy16",
			new List<string> { "robotlightweight", "armdisclauncher" }
		},
		{
			"scrappy17",
			new List<string> { "robotlightweight", "armblowtorch" }
		},
		{
			"scrappy18",
			new List<string> { "robotlightweight", "armsmallcannon" }
		},
		{
			"scrappy19",
			new List<string> { "robotlightweight", "armchargedrod" }
		},
		{
			"scrappy20",
			new List<string> { "robotlightweight", "armcoolingengine" }
		},
		{
			"scrappy21",
			new List<string> { "robotengraved", "armdisclauncher" }
		},
		{
			"scrappy22",
			new List<string> { "robotengraved", "armblowtorch" }
		},
		{
			"scrappy23",
			new List<string> { "robotengraved", "armsmallcannon" }
		},
		{
			"scrappy24",
			new List<string> { "robotengraved", "armchargedrod" }
		},
		{
			"scrappy25",
			new List<string> { "robotengraved", "armcoolingengine" }
		}
	};

	public static string GetScrappyRobot(List<string> requirements)
	{
		foreach (string requirement in requirements)
		{
			if (!robotRequirementMap.ContainsKey(requirement))
			{
				continue;
			}
			foreach (string requirement2 in requirements)
			{
				if (robotRequirementMap[requirement].ContainsKey(requirement2))
				{
					AtOManager.Instance.RemovePlayerRequirement(null, requirement);
					AtOManager.Instance.RemovePlayerRequirement(null, requirement2);
					return robotRequirementMap[requirement][requirement2];
				}
			}
		}
		return string.Empty;
	}

	public static void ShowHideRobotLayers(Transform objT, string robotName)
	{
		List<string> list = new List<string> { "robotbalanced", "robotheavy", "robotspiked", "robotlightweight", "robotengraved", "armdisclauncher", "armblowtorch", "armsmallcannon", "armchargedrod", "armcoolingengine" };
		if (robotLayerList.ContainsKey(robotName))
		{
			for (int i = 0; i < robotLayerList[robotName].Count; i++)
			{
				list.Remove(robotLayerList[robotName][i]);
			}
		}
		foreach (Transform item in objT)
		{
			if (list.Contains(item.gameObject.name.ToLower()))
			{
				item.gameObject.SetActive(value: false);
			}
		}
	}
}
