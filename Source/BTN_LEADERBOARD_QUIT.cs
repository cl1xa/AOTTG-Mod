using UnityEngine;

public class BTN_LEADERBOARD_QUIT : MonoBehaviour
{
	public GameObject leaderboard;

	public GameObject mainMenu;

	private void OnClick()
	{
		NGUITools.SetActive(mainMenu, state: true);
		NGUITools.SetActive(leaderboard, state: false);
	}
}
