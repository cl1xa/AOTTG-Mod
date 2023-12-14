using System;
using System.Diagnostics;
using System.Linq;
using UI;
using UnityEngine;

namespace Mod
{
    //This is wip ill make it more official eventually, but for now I just need buttons and toggles for random shit
    public class Menu : Photon.MonoBehaviour
    {
        public static Menu instance = new Menu();

        public static bool visible = false;

        public Vector2 Scroll = Vector2.zero;
        public Rect GuiRect = GUIHelpers.AlignRect(Screen.width - Screen.width / 1.5f, Screen.height - Screen.height / 2.5f, GUIHelpers.Alignment.CENTER, 0f, 10f);

        private static int tab = 0, oldTab = 0;
        private static string[] tabs = { "Local", "Visuals", "Misc", "Playerlist", "Protection" };

        public void Awake()
        {
            if (!GameMenu.Paused || IN_GAME_MAIN_CAMERA.gametype != 0)
                instance = this;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
                visible = !visible;
        }

        public void OnGUI()
        {
            GUI.backgroundColor = Color.gray;
            GUI.contentColor = Color.white;

            if (visible)
                GuiRect = GUILayout.Window(byte.MaxValue, GuiRect, DrawMenu, string.Concat(new object[1] { string.Empty }));
        }

        public void DrawMenu(int ID)
        {
            GUI.SetNextControlName("Menu");

            GUI.backgroundColor = Color.black;
            tab = GUILayout.Toolbar(tab, tabs);
            GUI.backgroundColor = Color.gray;

            switch (tab)
            {
                case 0: // Local
                    GUILayout.BeginVertical("", GUI.skin.box);
                    {
                        Features.deflectBomb = GUILayout.Toggle(Features.deflectBomb, "Deflect RCMod bombs");
                        Features.ignoreRacingRed = GUILayout.Toggle(Features.ignoreRacingRed, "Ignore racing red zones");
                        Features.infiniteGas = GUILayout.Toggle(Features.infiniteGas, "Infinite gas");
                        Features.infiniteBlades = GUILayout.Toggle(Features.infiniteBlades, "Infinite blades");
                    }
                    GUILayout.EndVertical();
                    break;

                case 1: // Visuals
                    GUILayout.BeginVertical("", GUI.skin.box);
                    {
                        Features.titanESP = GUILayout.Toggle(Features.titanESP, "Titan ESP");
                    }
                    GUILayout.EndVertical();
                    break;

                case 2: // Misc
                    GUILayout.BeginVertical("", GUI.skin.box);
                    {
                        Features.bypassPassword = GUILayout.Toggle(Features.bypassPassword, "Bypass server password");
                        Features.debugConsole = GUILayout.Toggle(Features.debugConsole, "Toggle debug console");
                    }
                    GUILayout.EndVertical();
                    break;

                case 3: // Playerlist
                    if (!PhotonNetwork.connected || !PhotonNetwork.inRoom)
                    {
                        GUILayout.Label("Not in lobby");
                        break;
                    }

                    GUILayout.BeginVertical("", GUI.skin.box);
                    {
                        Scroll = GUILayout.BeginScrollView(Scroll);
                        {
                            PhotonPlayer[] array = PhotonNetwork.playerList.OrderBy((PhotonPlayer player) => player.ID).ToArray();

                            foreach (PhotonPlayer photonPlayer in array)
                            {
                                if (photonPlayer != null && !Extensions.GetPlayerName(photonPlayer).StripHex().IsNullOrEmpty() && !photonPlayer.isLocal)
                                {
                                    GUI.backgroundColor = Color.black;

                                    if (GUILayout.Button(Extensions.GetPlayerName(photonPlayer).ToString().HexColor(), GUILayout.Height(30f)))
                                        break;
                                }
                            }
                        }
                        GUILayout.EndScrollView();
                    }
                    GUILayout.EndVertical();
                    break;

                case 4: // Protection
                    GUILayout.BeginVertical("", GUI.skin.box);
                    {
                        Features.response = GUILayout.Toggle(Features.response, "Operation response");
                        Features.logInvalidProperties = GUILayout.Toggle(Features.logInvalidProperties, "Log property changes");
                        Features.logObjects = GUILayout.Toggle(Features.logObjects, "Log objects");
                        Features.logRPCs = GUILayout.Toggle(Features.logRPCs, "Log RPCs");
                        Features.blockPause = GUILayout.Toggle(Features.blockPause, "Block pauseRPC");
                        Features.blockMove = GUILayout.Toggle(Features.blockMove, "Block moveToRPC");
                    }
                    GUILayout.EndVertical();
                    break;
            }

            if (oldTab != tab)
                oldTab = tab;

            GUI.DragWindow();
        }
    }
}