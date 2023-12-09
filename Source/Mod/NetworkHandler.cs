using Settings;
using System;
using System.Collections.Generic;

namespace Mod
{
    public class NetworkHandler
    {
        public static List<int> ignoreList;

        public static ExitGames.Client.Photon.Hashtable banHash;

        public static bool IsPlayerIgnored(PhotonPlayer player)
        {
            if (ignoreList.Contains(player.ID))
                return true;

            return false;
        }

        public static bool IsPlayerBanned(PhotonPlayer player)
        {
            if (banHash.ContainsValue(Extensions.GetPlayerName(player)))
                return true;

            return false;
        }

        //Testing
        public static void ApplyRandomProperty(bool self = true, PhotonPlayer player = null)
        {
            string text = string.Empty;
            string text2 = Extensions.RandomString(6).StripHex();

            ExitGames.Client.Photon.Hashtable undetectedHash = new ExitGames.Client.Photon.Hashtable();

            for (int i = 0; i < text2.Length; i++) { text += text2[i]; }

            undetectedHash[text] = Extensions.RandomString(10);
            PhotonNetwork.networkingPeer.OpSetCustomPropertiesOfActor(self ? PhotonNetwork.player.ID : player.ID, undetectedHash.StripToStringKeys(), true, 1);
            undetectedHash[text] = null;
            PhotonNetwork.networkingPeer.OpSetCustomPropertiesOfActor(self ? PhotonNetwork.player.ID : player.ID, undetectedHash.StripToStringKeys(), true, 1);

            RevalidateLocalProperties();
        }

        //Testing
        public static void RevalidateLocalProperties()
        {
            PhotonNetwork.networkingPeer.OpRaiseEvent(254, null, true, null);
            PhotonNetwork.networkingPeer.OpRaiseEvent(byte.MaxValue, PhotonNetwork.player.customProperties, true, null);
        }

        public static void MasterKick(PhotonPlayer player)
        {
            if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
            {
                FengGameManagerMKII.ServerCloseConnection(player, true, Extensions.GetPlayerName(player));
                return;
            }

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
            raiseEventOptions.TargetActors = new int[1] { player.ID };

            FengGameManagerMKII.instance.photonView.RPC("ignorePlayer", PhotonTargets.Others, player.ID);

            PhotonNetwork.RaiseEvent(254, null, true, raiseEventOptions);
            PhotonNetwork.DestroyPlayerObjects(player);
            PhotonNetwork.CloseConnection(player);

            FengGameManagerMKII.instance.RecompilePlayerList(0.1f);
        }

        public static void RelayKick(PhotonPlayer player)
        {
            //NONE
            //GO BEG BAHAA
        }

        public static void Response(PhotonPlayer player)
        {
            if (PhotonNetwork.isMasterClient)
                MasterKick(player);

            RelayKick(player);
        }

        public static void HandleOperationResponse(PhotonPlayer player, string msg, bool ban = true)
        {
            //I want to log this first, because if for whatever reason I trigger something and call this on myself
            //due to patching something improperly, I want it to be logged
            DebugConsole.Message(DebugConsole.MessageType.or, $"[{player.ID}] {Extensions.GetPlayerName(player).HexColor()} » {msg}");

            if (!ban)
                return;

            if (player == PhotonNetwork.player)
            {
                DebugConsole.Message(DebugConsole.MessageType.or, $"You somehow banned yourself idiot nice iq");
                return;
            }

            if (player.isMasterClient)
            {
                DebugConsole.Message(DebugConsole.MessageType.or, $"[{player.ID}] {Extensions.GetPlayerName(player).HexColor()} » Operation response haulted as sender is MasterClient");
                return;
            }

            if (!IsPlayerIgnored(player))
                ignoreList.Add(player.ID);

            if (!IsPlayerBanned(player))
                banHash.Add(player.ID, Extensions.GetPlayerName(player).StripHex());

            if (Features.response)
            {
                Response(player);
                NetworkingPeer.instance.HandleEventLeave(player.ID, player);
            }
        }
    }
}