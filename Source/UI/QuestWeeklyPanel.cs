using GameProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class QuestWeeklyPanel : QuestCategoryPanel
{
	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		ElementFactory.CreateDefaultLabel(SinglePanel, new ElementStyle(24, 120f, ThemePanel), QuestHandler.GetTimeToQuestReset(daily: false), FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>().color = UIManager.GetThemeColor(ThemePanel, "QuestHeader", "ResetTextColor");
		CreateQuestItems(GameProgressManager.GameProgress.Quest.WeeklyQuestItems.Value);
	}
}
