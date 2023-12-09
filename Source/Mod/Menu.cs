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

        public static int tab = 0;

        public Vector2 Scroll = Vector2.zero;

        public Rect GuiRect = GUIHelpers.AlignRect(Screen.width - Screen.width / 1.5f, Screen.height - Screen.height / 2.5f, GUIHelpers.Alignment.CENTER, 0f, 10f);

        public void Awake()
        {
            if (!GameMenu.Paused || IN_GAME_MAIN_CAMERA.gametype != 0)
                instance = this;
        }

        public void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
                visible = !visible;
        }

        public void OnGUI()
        {
            if (visible)
                GuiRect = GUI.Window(12, GuiRect, DrawMenu, string.Concat(new object[1] { string.Empty }));
        }

        public void DrawMenu(int ID)
        {
            GUI.SetNextControlName("Menu");

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Local", GUILayout.Height(25f)))
                    tab = 0;
                if (GUILayout.Button("Visuals", GUILayout.Height(25f)))
                    tab = 1;
                if (GUILayout.Button("Misc", GUILayout.Height(25f)))
                    tab = 2;
                if (GUILayout.Button("Players", GUILayout.Height(25f)))
                    tab = 3;
                if (GUILayout.Button("Protection", GUILayout.Height(25f)))
                    tab = 4;
            }
            GUILayout.EndHorizontal();

            switch (tab)
            {
                case 0: // Local
                    Features.deflectBomb = GUILayout.Toggle(Features.deflectBomb, "Deflect RCMod bombs" + CheckBoxToggle(Features.deflectBomb));
                    Features.ignoreRacingRed = GUILayout.Toggle(Features.ignoreRacingRed, "Ignore racing red zones" + CheckBoxToggle(Features.ignoreRacingRed));
                    Features.infiniteGas = GUILayout.Toggle(Features.infiniteGas, "Infinite gas" + CheckBoxToggle(Features.infiniteGas));
                    Features.infiniteBlades = GUILayout.Toggle(Features.infiniteBlades, "Infinite blades" + CheckBoxToggle(Features.infiniteBlades));
                    //Features.instantRespawn = GUILayout.Toggle(Features.instantRespawn, "Instant respawn" + CheckBoxToggle(Features.instantRespawn));
                    break;

                case 1: // Visuals
                    Features.titanESP = GUILayout.Toggle(Features.titanESP, "Titan ESP" + CheckBoxToggle(Features.titanESP));
                    break;

                case 2: // Misc
                    //Features.horseSpeedhack = GUILayout.Toggle(Features.horseSpeedhack, "Horse speedhack" + CheckBoxToggle(Features.horseSpeedhack));
                    Features.bypassPassword = GUILayout.Toggle(Features.bypassPassword, "Bypass server password" + CheckBoxToggle(Features.bypassPassword));
                    Features.debugConsole = GUILayout.Toggle(Features.debugConsole, "Toggle debug console" + CheckBoxToggle(Features.debugConsole));
                    break;

                case 3: // Playerlist
                    if (!PhotonNetwork.connected || !PhotonNetwork.inRoom)
                        break;

                    Scroll = GUILayout.BeginScrollView(Scroll);
                    {
                        PhotonPlayer[] array = PhotonNetwork.playerList.OrderBy((PhotonPlayer player) => player.ID).ToArray();

                        foreach (PhotonPlayer photonPlayer in array)
                        {
                            if (photonPlayer != null && !Extensions.GetPlayerName(photonPlayer).StripHex().IsNullOrEmpty() && !photonPlayer.isLocal)
                            {
                                if (GUILayout.Button(Extensions.GetPlayerName(photonPlayer).ToString().HexColor(), GUILayout.Height(30f)))
                                    break;
                            }
                        }
                    }
                    GUILayout.EndScrollView();
                    break;

                case 4: // Protection
                    Features.response = GUILayout.Toggle(Features.response, "Operation response" + CheckBoxToggle(Features.response));
                    Features.logInvalidProperties = GUILayout.Toggle(Features.logInvalidProperties, "Log property changes" + CheckBoxToggle(Features.logInvalidProperties));
                    Features.logObjects = GUILayout.Toggle(Features.logObjects, "Log objects" + CheckBoxToggle(Features.logObjects));
                    Features.logRPCs = GUILayout.Toggle(Features.logRPCs, "Log RPCs" + CheckBoxToggle(Features.logRPCs));
                    Features.blockPause = GUILayout.Toggle(Features.blockPause, "Block pauseRPC" + CheckBoxToggle(Features.blockPause));
                    Features.blockMove = GUILayout.Toggle(Features.blockMove, "Block moveToRPC" + CheckBoxToggle(Features.blockMove));
                    break;
            }
        }

        public string CheckBoxToggle(bool toggle)
        {
            toggle = !toggle;

            if (toggle)
                return " <color=red>Disabled</color>";

            return " <color=lime>Enabled</color>";
        }
    }
}