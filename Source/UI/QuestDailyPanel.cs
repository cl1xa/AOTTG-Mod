using GameProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class QuestDailyPanel : QuestCategoryPanel
{
	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		ElementFactory.CreateDefaultLabel(SinglePanel, new ElementStyle(24, 120f, ThemePanel), QuestHandler.GetTimeToQuestReset(daily: true), FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>().color = UIManager.GetThemeColor(ThemePanel, "QuestHeader", "ResetTextColor");
		CreateQuestItems(GameProgressManager.GameProgress.Quest.DailyQuestItems.Value);
	}
}
