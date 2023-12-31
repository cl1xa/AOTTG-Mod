using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class MultiplayerRoomListPopup : BasePopup
{
	protected MultiplayerPasswordPopup _multiplayerPasswordPopup;

	protected BasePopup _multiplayerCreatePopup;

	protected MultiplayerFilterPopup _multiplayerFilterPopup;

	protected Text _pageLabel;

	protected Text _playersOnlineLabel;

	protected GameObject _roomList;

	protected GameObject _noRoomsLabel;

	protected List<GameObject> _roomButtons = new List<GameObject>();

	public StringSetting _filterQuery = new StringSetting(string.Empty);

	public BoolSetting _filterShowFull = new BoolSetting(defaultValue: true);

	public BoolSetting _filterShowPassword = new BoolSetting(defaultValue: true);

	protected IntSetting _currentPage = new IntSetting(0, 0);

	private float _maxUpdateDelay = 5f;

	private float _currentUpdateDelay = 5f;

	private int _roomsPerPage = 10;

	private RoomInfo[] _rooms;

	private char[] _roomSeperator = new char[1] { "`"[0] };

	private int _lastPageCount;

	protected override string ThemePanel => "MultiplayerRoomListPopup";

	protected override bool HasPremadeContent => true;

	protected override int HorizontalPadding => 0;

	protected override int VerticalPadding => 0;

	protected override float Width => 1000f;

	protected override float Height => 660f;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		string category = "MainMenu";
		string subCategory = "MultiplayerRoomListPopup";
		ElementStyle elementStyle = new ElementStyle(ButtonFontSize, 120f, ThemePanel);
		ElementFactory.CreateDefaultButton(BottomBar, elementStyle, UIManager.GetLocaleCommon("Create"), 0f, 0f, delegate
		{
			OnButtonClick("Create");
		});
		ElementFactory.CreateDefaultButton(BottomBar, elementStyle, UIManager.GetLocaleCommon("Back"), 0f, 0f, delegate
		{
			OnButtonClick("Back");
		});
		TopBar.Find("SearchInputSetting").gameObject.AddComponent<InputSettingElement>().Setup(_filterQuery, new ElementStyle(24, 0f), UIManager.GetLocaleCommon("Search"), string.Empty, 160f, 40f, multiLine: false, null, delegate
		{
			RefreshList();
		});
		TopBar.Find("FilterButton").GetComponent<Button>().onClick.AddListener(delegate
		{
			OnButtonClick("Filter");
		});
		TopBar.Find("RefreshButton").GetComponent<Button>().onClick.AddListener(delegate
		{
			OnButtonClick("Refresh");
		});
		TopBar.Find("Page/LeftButton").GetComponent<Button>().onClick.AddListener(delegate
		{
			OnButtonClick("LeftPage");
		});
		TopBar.Find("Page/RightButton").GetComponent<Button>().onClick.AddListener(delegate
		{
			OnButtonClick("RightPage");
		});
		_pageLabel = TopBar.Find("Page/PageLabel").GetComponent<Text>();
		_roomList = SinglePanel.Find("RoomList").gameObject;
		_noRoomsLabel = _roomList.transform.Find("NoRoomsLabel").gameObject;
		_noRoomsLabel.GetComponent<Text>().text = UIManager.GetLocale(category, subCategory, "NoRooms");
		_playersOnlineLabel = TopBar.Find("PlayersOnlineLabel").GetComponent<Text>();
		_playersOnlineLabel.text = "0 " + UIManager.GetLocale(category, subCategory, "PlayersOnline");
		TopBar.Find("FilterButton").Find("Text").GetComponent<Text>()
			.text = UIManager.GetLocaleCommon("Filters");
		Button[] componentsInChildren = TopBar.GetComponentsInChildren<Button>();
		foreach (Button button in componentsInChildren)
		{
			button.colors = UIManager.GetThemeColorBlock(elementStyle.ThemePanel, "DefaultButton", "");
			if (button.transform.Find("Text") != null)
			{
				button.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor(elementStyle.ThemePanel, "DefaultButton", "TextColor");
			}
		}
		TopBar.Find("Page/PageLabel").GetComponent<Text>().color = UIManager.GetThemeColor(elementStyle.ThemePanel, "DefaultLabel", "TextColor");
		TopBar.Find("PlayersOnlineLabel").GetComponent<Text>().color = UIManager.GetThemeColor(elementStyle.ThemePanel, "DefaultLabel", "TextColor");
		_noRoomsLabel.GetComponent<Text>().color = UIManager.GetThemeColor(elementStyle.ThemePanel, "RoomButton", "TextColor");
		_roomList.GetComponent<Image>().color = UIManager.GetThemeColor(elementStyle.ThemePanel, "MainBody", "BackgroundColor");
	}

	public override void Show()
	{
		base.Show();
		_currentPage.Value = 0;
		RefreshList();
		_currentUpdateDelay = 0.5f;
	}

	public override void Hide()
	{
		if (base.gameObject.activeSelf)
		{
			PhotonNetwork.Disconnect();
		}
		base.Hide();
	}

	protected void Update()
	{
		_currentUpdateDelay -= Time.deltaTime;
		if (_currentUpdateDelay <= 0f)
		{
			RefreshList();
			_currentUpdateDelay = _maxUpdateDelay;
		}
	}

	protected override void SetupPopups()
	{
		base.SetupPopups();
		_multiplayerPasswordPopup = ElementFactory.CreateHeadedPanel<MultiplayerPasswordPopup>(base.transform).GetComponent<MultiplayerPasswordPopup>();
		_multiplayerFilterPopup = ElementFactory.CreateHeadedPanel<MultiplayerFilterPopup>(base.transform).GetComponent<MultiplayerFilterPopup>();
		_multiplayerCreatePopup = ElementFactory.CreateHeadedPanel<MultiplayerCreatePopup>(base.transform).GetComponent<MultiplayerCreatePopup>();
		_popups.Add(_multiplayerPasswordPopup);
		_popups.Add(_multiplayerFilterPopup);
		_popups.Add(_multiplayerCreatePopup);
	}

	public void RefreshList(bool refetch = true)
	{
		_currentUpdateDelay = _maxUpdateDelay;
		if (refetch)
		{
			_rooms = PhotonNetwork.GetRoomList();
			_playersOnlineLabel.text = PhotonNetwork.countOfPlayers + " " + UIManager.GetLocale("MainMenu", "MultiplayerRoomListPopup", "PlayersOnline");
		}
		ClearRoomButtons();
		List<RoomInfo> filteredRooms = GetFilteredRooms();
		if (filteredRooms.Count == 0)
		{
			_noRoomsLabel.SetActive(value: true);
			_pageLabel.text = "0/0";
			return;
		}
		_noRoomsLabel.SetActive(value: false);
		_lastPageCount = GetPageCount(filteredRooms);
		_currentPage.Value = Math.Min(_currentPage.Value, _lastPageCount - 1);
		_pageLabel.text = _currentPage.Value + 1 + "/" + _lastPageCount;
		foreach (RoomInfo room in GetCurrentPageRooms(filteredRooms))
		{
			GameObject gameObject = ElementFactory.InstantiateAndBind(_roomList.transform, "MultiplayerRoomButton");
			_roomButtons.Add(gameObject);
			gameObject.GetComponent<Button>().onClick.AddListener(delegate
			{
				OnRoomClick(room.name);
			});
			gameObject.transform.Find("Text").GetComponent<Text>().text = GetRoomFormattedName(room);
			if (GetRoomPassword(room.name) == string.Empty)
			{
				gameObject.transform.Find("PasswordIcon").gameObject.SetActive(value: false);
			}
			gameObject.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(ThemePanel, "RoomButton", "");
			gameObject.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor(ThemePanel, "RoomButton", "TextColor");
		}
	}

	protected List<RoomInfo> GetCurrentPageRooms(List<RoomInfo> rooms)
	{
		if (rooms.Count <= _roomsPerPage)
		{
			return rooms;
		}
		List<RoomInfo> list = new List<RoomInfo>();
		int num = _currentPage.Value * _roomsPerPage;
		int num2 = Math.Min(num + _roomsPerPage, rooms.Count);
		for (int i = num; i < num2; i++)
		{
			list.Add(rooms[i]);
		}
		return list;
	}

	protected List<RoomInfo> GetFilteredRooms()
	{
		List<RoomInfo> list = new List<RoomInfo>();
		RoomInfo[] rooms = _rooms;
		foreach (RoomInfo roomInfo in rooms)
		{
			if (IsValidRoom(roomInfo) && (!(_filterQuery.Value != string.Empty) || roomInfo.name.ToLower().Contains(_filterQuery.Value.ToLower())) && (_filterShowFull.Value || roomInfo.playerCount < roomInfo.maxPlayers) && (_filterShowPassword.Value || !(GetRoomPassword(roomInfo.name) != string.Empty)))
			{
				list.Add(roomInfo);
			}
		}
		return list;
	}

	protected int GetPageCount(List<RoomInfo> rooms)
	{
		if (rooms.Count == 0)
		{
			return 0;
		}
		return (rooms.Count - 1) / _roomsPerPage + 1;
	}

	protected void ClearRoomButtons()
	{
		foreach (GameObject roomButton in _roomButtons)
		{
			UnityEngine.Object.Destroy(roomButton);
		}
		_roomButtons.Clear();
	}

	protected bool IsValidRoom(RoomInfo info)
	{
		return info.name.Split(_roomSeperator).Length > 5;
	}

	protected string GetRoomPassword(string name)
	{
		string[] array = name.Split(_roomSeperator);
		if (array.Length > 5)
		{
			return array[5];
		}
		return string.Empty;
	}

	protected string GetRoomFormattedName(RoomInfo room)
	{
		char[] separator = new char[1] { "`"[0] };
		string[] array = room.name.Split(separator);
		return (array[0] + " / " + array[1] + " / " + array[2].UpperFirstLetter() + " / " + array[4].UpperFirstLetter() + "   " + room.playerCount + "/" + room.maxPlayers).HexColor();
	}

	private void OnRoomClick(string name)
	{
		string roomPassword = GetRoomPassword(name);
		if (roomPassword != string.Empty)
		{
			HideAllPopups();
			_multiplayerPasswordPopup.Show(roomPassword, name);
		}
		else
		{
			PhotonNetwork.JoinRoom(name);
		}
	}

	private void OnButtonClick(string name)
	{
		HideAllPopups();
		switch (name)
		{
		case "Back":
			((MainMenu)UIManager.CurrentMenu).ShowMultiplayerMapPopup();
			break;
		case "Create":
			_multiplayerCreatePopup.Show();
			break;
		case "Filter":
			_multiplayerFilterPopup.Show();
			break;
		case "Refresh":
			RefreshList();
			break;
		case "LeftPage":
			if (_currentPage.Value <= 0)
			{
				_currentPage.Value = _lastPageCount - 1;
			}
			else
			{
				_currentPage.Value--;
			}
			RefreshList(refetch: false);
			break;
		case "RightPage":
			if (_currentPage.Value >= _lastPageCount - 1)
			{
				_currentPage.Value = 0;
			}
			else
			{
				_currentPage.Value++;
			}
			RefreshList(refetch: false);
			break;
		}
	}
}
