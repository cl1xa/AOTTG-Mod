using ApplicationManagers;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class MainMenu : BaseMenu
{
	public BasePopup _singleplayerPopup;

	public BasePopup _multiplayerMapPopup;

	public BasePopup _settingsPopup;

	public BasePopup _toolsPopup;

	public BasePopup _multiplayerRoomListPopup;

	public BasePopup _editProfilePopup;

	public BasePopup _questsPopup;

	protected Text _multiplayerStatusLabel;

	//Modified
	//Removed stupid background picture when no animated intro, hated it
	public override void Setup()
	{
		base.Setup();

		SetupIntroPanel();
		SetupLabels();
	}

	public void ShowMultiplayerRoomListPopup()
	{
		HideAllPopups();
		_multiplayerRoomListPopup.Show();
	}

	public void ShowMultiplayerMapPopup()
	{
		HideAllPopups();
		_multiplayerMapPopup.Show();
	}

	protected override void SetupPopups()
	{
		base.SetupPopups();
		_singleplayerPopup = ElementFactory.CreateHeadedPanel<SingleplayerPopup>(base.transform).GetComponent<BasePopup>();
		_multiplayerMapPopup = ElementFactory.InstantiateAndSetupPanel<MultiplayerMapPopup>(base.transform, "MultiplayerMapPopup").GetComponent<BasePopup>();
		_editProfilePopup = ElementFactory.CreateHeadedPanel<EditProfilePopup>(base.transform).GetComponent<BasePopup>();
		_settingsPopup = ElementFactory.CreateHeadedPanel<SettingsPopup>(base.transform).GetComponent<BasePopup>();
		_toolsPopup = ElementFactory.CreateHeadedPanel<ToolsPopup>(base.transform).GetComponent<BasePopup>();
		_multiplayerRoomListPopup = ElementFactory.InstantiateAndSetupPanel<MultiplayerRoomListPopup>(base.transform, "MultiplayerRoomListPopup").GetComponent<BasePopup>();
		_questsPopup = ElementFactory.CreateHeadedPanel<QuestPopup>(base.transform).GetComponent<BasePopup>();
		_popups.Add(_singleplayerPopup);
		_popups.Add(_multiplayerMapPopup);
		_popups.Add(_editProfilePopup);
		_popups.Add(_settingsPopup);
		_popups.Add(_toolsPopup);
		_popups.Add(_multiplayerRoomListPopup);
		_popups.Add(_questsPopup);
	}

	private void SetupIntroPanel()
	{
		GameObject obj = ElementFactory.InstantiateAndBind(base.transform, "IntroPanel");
		ElementFactory.SetAnchor(obj, TextAnchor.LowerRight, TextAnchor.LowerRight, new Vector2(-10f, 30f));
		foreach (Transform item in obj.transform.Find("Buttons"))
		{
			IntroButton introButton = item.gameObject.AddComponent<IntroButton>();
			introButton.onClick.AddListener(delegate
			{
				OnIntroButtonClick(introButton.name);
			});
		}
	}

    //Modified
    //Fuck your stupid donate button
    private void SetupLabels()
	{
        _multiplayerStatusLabel = ElementFactory.CreateDefaultLabel(base.transform, ElementStyle.Default, string.Empty).GetComponent<Text>();
		ElementFactory.SetAnchor(_multiplayerStatusLabel.gameObject, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(20f, -20f));
		_multiplayerStatusLabel.color = Color.white;
		Text component = ElementFactory.CreateDefaultLabel(base.transform, ElementStyle.Default, string.Empty).GetComponent<Text>();
		ElementFactory.SetAnchor(component.gameObject, TextAnchor.LowerCenter, TextAnchor.LowerCenter, new Vector2(0f, 20f));
		component.color = Color.white;
        component.text = "";

		if (ApplicationConfig.DevelopmentMode)
			component.text = "RC MOD DEVELOPMENT VERSION";
		else
			component.text = string.Empty;
    }

	private void Update()
	{
		if (_multiplayerStatusLabel != null)
		{
			_multiplayerStatusLabel.text = PhotonNetwork.connectionStateDetailed.ToString();
			if (PhotonNetwork.connected)
			{
				Text multiplayerStatusLabel = _multiplayerStatusLabel;
				multiplayerStatusLabel.text = multiplayerStatusLabel.text + " ping:" + PhotonNetwork.GetPing();
			}
		}
	}

	private void OnIntroButtonClick(string name)
	{
		HideAllPopups();
		switch (name)
		{
		case "SingleplayerButton":
			_singleplayerPopup.Show();
			break;
		case "MultiplayerButton":
			_multiplayerMapPopup.Show();
			break;
		case "ProfileButton":
			_editProfilePopup.Show();
			break;
		case "QuestsButton":
			_questsPopup.Show();
			break;
		case "SettingsButton":
			_settingsPopup.Show();
			break;
		case "ToolsButton":
			_toolsPopup.Show();
			break;
		case "QuitButton":
			Application.Quit();
			break;
		case "Donate":
			Application.OpenURL("https://www.patreon.com/aottg2");
			break;
		}
	}
}
