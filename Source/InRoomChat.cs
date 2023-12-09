using System;
using System.Collections.Generic;
using Settings;
using UI;
using UnityEngine;

//Modified (yes, essentially the entire class was changed)
public class InRoomChat : Photon.MonoBehaviour
{
    public static InRoomChat instance;


    public Vector2 Scroll = Vector2.zero;

    public GUIStyle style = new GUIStyle();

    public static Rect GuiRect = new Rect(0f, Screen.height - 500, 350f, 470f);

    public static Rect GuiRect2 = new Rect(28f, Screen.height - 300 + 275, 400f, 25f);


    private float deltaTime;

    private string inputLine = string.Empty;

    public static List<string> messages = new List<string>();

    public static List<string> gcMessages = new List<string>();

    //Modified
    //Made text bold
    private void ShowFPS()
    {
        Rect position = new Rect(Screen.width / 4f, 10f, 150f, 30f);
        int num = (int)Math.Round(1f / deltaTime);
        GUI.Label(position, $"<b>FPS: {num}</b>");
    }

    public void addLINE(string newLine) { messages.Add($"<color=white>{newLine}</color>"); }

    public void Update() {deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;}

    public void OnGUI()
    {
        if (PhotonNetwork.connectionStateDetailed != PeerStates.Joined)
            return;

        if (SettingsManager.GraphicsSettings.ShowFPS.Value)
            ShowFPS();

        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        {
            if (!string.IsNullOrEmpty(inputLine))
            {
                if (inputLine == "\t")
                {
                    inputLine = string.Empty;

                    GUI.FocusControl("ChatMessages");
                    return;
                }

                if (FengGameManagerMKII.RCEvents.ContainsKey("OnChatInput"))
                {
                    string text = (string)FengGameManagerMKII.RCVariableNames["OnChatInput"];

                    if (FengGameManagerMKII.stringVariables.ContainsKey(text))
                        FengGameManagerMKII.stringVariables[text] = inputLine;
                    else
                        FengGameManagerMKII.stringVariables.Add(text, inputLine);

                    ((RCEvent)FengGameManagerMKII.RCEvents["OnChatInput"]).checkEvent();
                }

                if (inputLine.StartsWith("/"))
                {
                    if (inputLine.Equals("/pause"))
                        Mod.Commands.PauseGame(true);
                    else if (inputLine.Equals("/unpause"))
                        Mod.Commands.PauseGame(false);
                    else if (inputLine.Equals("/restart"))
                        FengGameManagerMKII.instance.restartRC();
                    else
                        addLINE("idiot");
                }
                else
                {
                    object[] parameters = new object[2] { inputLine, Extensions.GetPlayerName(PhotonNetwork.player).HexColor() };
                    FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters);
                }

                inputLine = string.Empty;

                GUI.FocusControl("ChatMessages");
                return;
            }

            inputLine = "\t";
            GUI.FocusControl("ChatInput");
        }

        GUI.SetNextControlName("ChatMessages");
        GUILayout.BeginArea(GuiRect);
        {
            //Scroll.y = float.MaxValue;
            //Scroll = GUILayout.BeginScrollView(Scroll);
            //{
                GUILayout.FlexibleSpace();

                if (messages.Count < 20)
                    for (int i = 0; i < messages.Count; i++) { GUILayout.Label(messages[i], style); }
                else
                    for (int j = messages.Count - 20; j < messages.Count; j++) { GUILayout.Label(messages[j], style); }
            //}
            //GUILayout.EndScrollView();
        }
        GUILayout.EndArea();

        GUI.SetNextControlName("ChatInput");
        GUILayout.BeginArea(GuiRect2);
        {
            GUILayout.BeginHorizontal();
            inputLine = GUILayout.TextField(inputLine, GUILayout.Width(300f));
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
    }

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        style.wordWrap = true;
    }
}
