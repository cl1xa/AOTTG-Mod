using Microsoft.SqlServer.Server;
using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod
{
    //Each command is completely unmodified from the original, aside from code organization
    //I will eventually add my own custom command handler, but this is quicker for now
    //Converting these commands to my system will take longer than I want it to and I'm lazy

    public static class Commands
    {
        // /aso
        public static void EndlessRacingEnabled(string inputLine)
        {
            if (PhotonNetwork.isMasterClient)
            {
                LegacyGameSettings legacyGameSettings = SettingsManager.LegacyGameSettings;
                LegacyGameSettings legacyGameSettingsUI = SettingsManager.LegacyGameSettingsUI;

                string text = inputLine.Substring(5);

                if (!(text == "kdr"))
                {
                    if (text == "racing")
                    {
                        if (!legacyGameSettings.RacingEndless.Value)
                        {
                            BoolSetting racingEndless = legacyGameSettings.RacingEndless;
                            bool value = (legacyGameSettingsUI.RacingEndless.Value = true);
                            racingEndless.Value = value;

                            FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Endless racing enabled.</color>");
                        }
                        else
                        {
                            BoolSetting racingEndless2 = legacyGameSettings.RacingEndless;
                            bool value = (legacyGameSettingsUI.RacingEndless.Value = false);
                            racingEndless2.Value = value;

                            FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Endless racing disabled.</color>");
                        }
                    }
                }
                else if (!legacyGameSettings.PreserveKDR.Value)
                {
                    legacyGameSettings.PreserveKDR.Value = true;
                    legacyGameSettingsUI.PreserveKDR.Value = true;

                    FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>KDRs will be preserved from disconnects.</color>");
                }
                else
                {
                    legacyGameSettings.PreserveKDR.Value = false;
                    legacyGameSettingsUI.PreserveKDR.Value = false;

                    FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>KDRs will not be preserved from disconnects.</color>");
                }
            }
        }

        public static void PauseGame(bool paused)
        {
            if (PhotonNetwork.isMasterClient)
            {
                object[] parameters = new object[2] { string.Format("<color=#FFCC00>MasterClient has{0}paused the game.</color>", paused ? " " : " un"), string.Empty };

                FengGameManagerMKII.instance.photonView.RPC("pauseRPC", PhotonTargets.All, paused);
                FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters);
            }
            else
                FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>error: not master client</color>");
        }
    }
}