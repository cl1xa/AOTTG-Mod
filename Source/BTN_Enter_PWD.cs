using UnityEngine;

public class BTN_Enter_PWD : MonoBehaviour
{
	private void OnClick()
	{
		string text = GameObject.Find("InputEnterPWD").GetComponent<UIInput>().label.text;
		SimpleAES simpleAES = new SimpleAES();
		if (text == simpleAES.Decrypt(PanelMultiJoinPWD.Password))
		{
			PhotonNetwork.JoinRoom(PanelMultiJoinPWD.roomName);
			return;
		}
		NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiPWD, state: false);
		NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiROOM, state: true);
		GameObject.Find("PanelMultiROOM").GetComponent<PanelMultiJoin>().refresh();
	}
}
