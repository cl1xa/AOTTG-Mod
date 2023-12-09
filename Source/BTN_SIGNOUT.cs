using UnityEngine;

public class BTN_SIGNOUT : MonoBehaviour
{
	public GameObject logincomponent;

	public GameObject loginPanel;

	private void OnClick()
	{
		NGUITools.SetActive(base.transform.parent.gameObject, state: false);
		NGUITools.SetActive(loginPanel, state: true);
		logincomponent.GetComponent<LoginFengKAI>().logout();
	}
}
