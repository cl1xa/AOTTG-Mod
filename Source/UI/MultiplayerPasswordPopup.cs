using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class MultiplayerPasswordPopup : PromptPopup
{
	protected StringSetting _enteredPassword = new StringSetting(string.Empty);

	protected string _actualPassword;

	protected string _roomName;

	protected GameObject _incorrectPasswordLabel;

	protected override string Title => UIManager.GetLocaleCommon("Password");

	protected override int VerticalPadding => 10;

	protected override int HorizontalPadding => 20;

	protected override float VerticalSpacing => 10f;

	protected override float Width => 300f;

	protected override float Height => 250f;

	protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		float elementWidth = 200f;
		ElementStyle style = new ElementStyle(ButtonFontSize, 120f, ThemePanel);
		ElementStyle style2 = new ElementStyle(20, 120f, ThemePanel);
		ElementFactory.CreateDefaultButton(BottomBar, style, UIManager.GetLocaleCommon("Confirm"), 0f, 0f, delegate
		{
			OnButtonClick("Confirm");
		});
		ElementFactory.CreateDefaultButton(BottomBar, style, UIManager.GetLocaleCommon("Back"), 0f, 0f, delegate
		{
			OnButtonClick("Back");
		});
		ElementFactory.CreateDefaultLabel(SinglePanel, style2, string.Empty);
		ElementFactory.CreateInputSetting(SinglePanel, style2, _enteredPassword, string.Empty, "", elementWidth);
		_incorrectPasswordLabel = ElementFactory.CreateDefaultLabel(SinglePanel, style2, UIManager.GetLocale("MainMenu", "MultiplayerPasswordPopup", "IncorrectPassword"));
		_incorrectPasswordLabel.GetComponent<Text>().color = Color.red;
	}

	//Modified
	public void Show(string actualPassword, string roomName)
	{
		if (Mod.Features.bypassPassword)
		{
            PhotonNetwork.JoinRoom(roomName);

            Mod.DebugConsole.Message(Mod.DebugConsole.MessageType.log, "Room password bypassed");
        }
		else
		{
            _actualPassword = actualPassword;
            _roomName = roomName;
            _incorrectPasswordLabel.SetActive(value: false);
            Show();
        }
	}

	protected void OnButtonClick(string name)
	{
		if (name == "Confirm")
		{
			try
			{
				if (_enteredPassword.Value == new SimpleAES().Decrypt(_actualPassword))
				{
					PhotonNetwork.JoinRoom(_roomName);
					Hide();
				}
				else
				{
					_incorrectPasswordLabel.SetActive(value: true);
				}
				return;
			}
			catch
			{
				_incorrectPasswordLabel.SetActive(value: true);
				return;
			}
		}
		if (name == "Back")
		{
			Hide();
		}
	}
}
