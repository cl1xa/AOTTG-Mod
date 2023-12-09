using Settings;
using System;
using System.Collections.Generic;

using UI;
using UnityEngine;

namespace Mod
{
    public class DebugConsole : Photon.MonoBehaviour
    {
        public static DebugConsole instance;


        public Vector2 Scroll;

        public GUIStyle style = new GUIStyle();

        public static Rect GuiRect = new Rect(Screen.width - 600, Screen.height - 290, 600f, 280f);

        public static List<string> display = new List<string>();

        public static string lines = string.Empty;

        private static string time;

        public enum MessageType
        {
            log,
            msg,
            warning,
            error,
            blank,

            rpc,
            obj,
            or,
        }

        public void Awake()
        {
            if (!GameMenu.Paused || IN_GAME_MAIN_CAMERA.gametype != 0)
                instance = this;
        }

        public void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F12))
                Features.debugConsole = !Features.debugConsole;

            time = DateTime.Now.ToString("(yyyy) ddd MMMM dd, h:mm:ss tt");
    }

        public void OnGUI()
        {
            if (!GameMenu.Paused && Features.debugConsole)
            {
                GUI.backgroundColor = Color.black;

                GUI.SetNextControlName("DebugConsole");

                GUILayout.BeginArea(GuiRect);
                {
                    Convert.ToInt32(lines);
                    GUILayout.FlexibleSpace();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label($" <color=red><b>[DEBUG CONSOLE]</b></color> | <color=yellow><b>{time}</b></color>");

                        if (GUILayout.Button("Clear", GUILayout.Width(75f)))
                            display.Clear();
                    }
                    GUILayout.EndHorizontal();

                    //Scroll.y = float.MaxValue;
                    Scroll = GUILayout.BeginScrollView(Scroll);
                    {
                        for (int i = 0; i < display.Count; i++) { GUILayout.Label(display[i], style); }
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndArea();
            }
        }

        public void Start()
        {
            style.wordWrap = true;
            lines = "15";
        }

        public void AddLINE(string newLine)
        {
            if (display.Count == 14)
                display.Remove(display[0]);

            display.Add(newLine);
        }

        private void UpdateLine(string line, string newline)
        {
            display.Remove(line);
            display.Add(newline);
        }

        //Lightsheir paste <3
        public static void Message(MessageType message, string mes)
        {
            string text = string.Empty;

            switch (message)
            {
                case MessageType.blank:
                    text = "<color=#ffffff>";
                    break;

                case MessageType.log:
                    text = "<color=#3498db><b>[LOG]</b></color><color=#C1C1C1> ";
                    break;

                case MessageType.msg:
                    text = "<color=lime><b>[MSG]</b></color><color=white> ";
                    break;

                case MessageType.error:
                    text = "<color=red><b>[ERROR]</b></color><color=white> ";
                    break;

                case MessageType.obj:
                    text = "<color=yellow><b>[OBJ]</b></color><color=white> ";
                    break;

                //Orange
                case MessageType.rpc:
                    text = "<color=#FFA500><b>[RPC]</b></color><color=white> ";
                    break;

                case MessageType.or:
                    text = "<color=red><b>[OR]</b></color><color=white> ";
                    break;
            }

            string text2 = text + mes + "</color>";

            if (!mes.EndsWith(" "))
                text2 += " ";

            bool flag = false;

            if (display.Count > 0)
            {
                foreach (string item in display)
                {
                    if (item.StartsWith(text2))
                    {
                        flag = true;

                        if (!item.Contains("<color=red><b>:x"))
                        {
                            instance.UpdateLine(item, item + "<color=red><b>:x2</b></color>");
                            break;
                        }

                        int num = item.IndexOf("<color=red><b>:x");
                        string text3 = item.Substring(num, item.IndexOf("</b>", num) - num);
                        string[] array = text3.Split('x');
                        string newline = item.Replace(text3, array[0] + "x" + (array[1].ToInt() + 1));

                        instance.UpdateLine(item, newline);
                        break;
                    }
                }
            }

            //<color=yellow><b>({DateTime.Now.ToString("HH:mm tt")})</b></color> 

            if (!flag)
                instance.AddLINE($"{text2}");
        }

        public static void Message(MessageType message, object mas) { Message(message, mas.ToString()); }

        public static void Message(string mes) { Message(MessageType.log, mes); }

        public static void Message(object mes) { Message(MessageType.log, mes.ToString()); }
    }
}