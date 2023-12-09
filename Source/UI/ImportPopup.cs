using Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Weather;

namespace UI;

internal class ImportPopup : PromptPopup
{
	private UnityAction _onSave;

	private InputSettingElement _element;

	private Text _text;

	public StringSetting ImportSetting = new StringSetting(string.Empty);

	protected override string Title => UIManager.GetLocaleCommon("Import");

	protected override float Width => 500f;

	protected override float Height => 600f;

	protected override int VerticalPadding => 20;

	protected override int HorizontalPadding => 20;

	protected override float VerticalSpacing => 10f;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		ElementStyle style = new ElementStyle(ButtonFontSize, 120f, ThemePanel);
		ElementFactory.CreateDefaultButton(BottomBar, style, UIManager.GetLocaleCommon("Save"), 0f, 0f, delegate
		{
			OnButtonClick("Save");
		});
		ElementFactory.CreateDefaultButton(BottomBar, style, UIManager.GetLocaleCommon("Cancel"), 0f, 0f, delegate
		{
			OnButtonClick("Cancel");
		});
		_element = ElementFactory.CreateInputSetting(SinglePanel, style, ImportSetting, string.Empty, "", 460f, 390f, multiLine: true).GetComponent<InputSettingElement>();
		_text = ElementFactory.CreateDefaultLabel(SinglePanel, style, "").GetComponent<Text>();
		_text.color = Color.red;
	}

	public void Show(UnityAction onSave)
	{
		if (!base.gameObject.activeSelf)
		{
			Show();
			_onSave = onSave;
			ImportSetting.Value = string.Empty;
			_text.text = string.Empty;
			_element.SyncElement();
		}
	}

	private void OnButtonClick(string name)
	{
		if (name == "Cancel")
		{
			Hide();
		}
		else if (name == "Save")
		{
			string text = new WeatherSchedule().DeserializeFromCSV(ImportSetting.Value);
			if (text != string.Empty)
			{
				_text.text = text;
				return;
			}
			_onSave();
			Hide();
		}
	}
}
