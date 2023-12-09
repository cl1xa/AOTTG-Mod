using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class PhotonPlayer
{
	private int actorID;

	public readonly bool isLocal;

	private string nameField;

	public object TagObject;

	public Hashtable allProperties
	{
		get
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Merge(customProperties);
			hashtable[byte.MaxValue] = name;
			return hashtable;
		}
	}

	public Hashtable customProperties { get; private set; }

	public int ID => actorID;

	public bool isMasterClient => PhotonNetwork.networkingPeer.mMasterClient == this;

	public string name
	{
		get
		{
			return nameField;
		}
		set
		{
			if (!isLocal)
			{
				Debug.LogError("Error: Cannot change the name of a remote player!");
			}
			else
			{
				nameField = value;
			}
		}
	}

	public static void CleanProperties()
	{
		if (PhotonNetwork.player != null)
		{
			PhotonNetwork.player.customProperties.Clear();
			PhotonNetwork.player.SetCustomProperties(new Hashtable { 
			{
				PhotonPlayerProperty.name,
				LoginFengKAI.player.name
			} });
		}
	}

	//Modified
	protected internal PhotonPlayer(bool isLocal, int actorID, Hashtable properties)
	{
		this.actorID = -1;
		nameField = string.Empty;
		customProperties = new Hashtable();
		this.isLocal = isLocal;
		this.actorID = actorID;

		PhotonPlayer photonPlayer = Find(actorID);

		if (properties.ContainsKey("name") && (properties["name"].ToString() == string.Empty || properties["name"].ToString() == null))
            properties["name"] = "EMPTY_NAME";

        if (properties.ContainsKey("name") && properties["name"].StripHexColor().Length > 70)
        {
            properties["name"] = "NAME_TOO_LONG";
            Mod.NetworkHandler.HandleOperationResponse(photonPlayer, "Has guild name that is too long");
        }

        if (properties.ContainsKey("guildName") && properties["guildName"].StripHexColor().Length > 70)
        {
            properties["guildName"] = "GUILD_TOO_LONG";
			Mod.NetworkHandler.HandleOperationResponse(photonPlayer, "Has guild name that is too long");
        }

        properties.HashNullFix(); //Insane patch!

        InternalCacheProperties(properties);
	}

	public PhotonPlayer(bool isLocal, int actorID, string name)
	{
		this.actorID = -1;
		nameField = string.Empty;
		customProperties = new Hashtable();
		this.isLocal = isLocal;
		this.actorID = actorID;
		nameField = name;
	}

	public override bool Equals(object p)
	{
		if (p is PhotonPlayer photonPlayer)
		{
			return GetHashCode() == photonPlayer.GetHashCode();
		}
		return false;
	}

	public static PhotonPlayer Find(int ID)
	{
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
		{
			PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
			if (photonPlayer.ID == ID)
			{
				return photonPlayer;
			}
		}
		return null;
	}

	public Hashtable ChangeLocalPlayer(int NewID, string inputname)
	{
		actorID = NewID;
		string value = (nameField = $"{inputname}_ID:{NewID}");
		Hashtable result = new Hashtable
		{
			{
				PhotonPlayerProperty.name,
				value
			},
			{
				PhotonPlayerProperty.guildName,
				LoginFengKAI.player.guildname
			},
			{
				PhotonPlayerProperty.kills,
				0
			},
			{
				PhotonPlayerProperty.max_dmg,
				0
			},
			{
				PhotonPlayerProperty.total_dmg,
				0
			},
			{
				PhotonPlayerProperty.deaths,
				0
			},
			{
				PhotonPlayerProperty.dead,
				true
			},
			{
				PhotonPlayerProperty.isTitan,
				0
			},
			{
				PhotonPlayerProperty.RCteam,
				0
			},
			{
				PhotonPlayerProperty.currentLevel,
				string.Empty
			}
		};
		PhotonNetwork.AllocateViewID();
		PhotonNetwork.player.SetCustomProperties(result);
		return result;
	}

	public PhotonPlayer Get(int id)
	{
		return Find(id);
	}

	public override int GetHashCode()
	{
		return ID;
	}

	public PhotonPlayer GetNext()
	{
		return GetNextFor(ID);
	}

	public PhotonPlayer GetNextFor(PhotonPlayer currentPlayer)
	{
		if (currentPlayer == null)
		{
			return null;
		}
		return GetNextFor(currentPlayer.ID);
	}

	public PhotonPlayer GetNextFor(int currentPlayerId)
	{
		if (PhotonNetwork.networkingPeer == null || PhotonNetwork.networkingPeer.mActors == null || PhotonNetwork.networkingPeer.mActors.Count < 2)
		{
			return null;
		}
		Dictionary<int, PhotonPlayer> mActors = PhotonNetwork.networkingPeer.mActors;
		int num = int.MaxValue;
		int num2 = currentPlayerId;
		foreach (int key in mActors.Keys)
		{
			if (key < num2)
			{
				num2 = key;
			}
			else if (key > currentPlayerId && key < num)
			{
				num = key;
			}
		}
		if (num != int.MaxValue)
		{
			return mActors[num];
		}
		return mActors[num2];
	}

	internal void InternalCacheProperties(Hashtable properties)
	{
		if (properties != null && properties.Count != 0 && !customProperties.Equals(properties))
		{
			if (properties.ContainsKey(byte.MaxValue))
			{
				nameField = (string)properties[byte.MaxValue];
			}
			customProperties.MergeStringKeys(properties);
			customProperties.StripKeysWithNullValues();
		}
	}

	internal void InternalChangeLocalID(int newID)
	{
		if (!isLocal)
		{
			Debug.LogError("ERROR You should never change PhotonPlayer IDs!");
		}
		else
		{
			actorID = newID;
		}
	}

	public void SetCustomProperties(Hashtable propertiesToSet)
	{
		if (propertiesToSet != null)
		{
			customProperties.MergeStringKeys(propertiesToSet);
			customProperties.StripKeysWithNullValues();
			Hashtable actorProperties = propertiesToSet.StripToStringKeys();
			if (actorID > 0 && !PhotonNetwork.offlineMode)
			{
				PhotonNetwork.networkingPeer.OpSetCustomPropertiesOfActor(actorID, actorProperties, broadcast: true, 0);
			}
			object[] parameters = new object[2] { this, propertiesToSet };
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, parameters);
		}
	}

	public override string ToString()
	{
		if (string.IsNullOrEmpty(name))
		{
			return string.Format("#{0:00}{1}", ID, (!isMasterClient) ? string.Empty : "(master)");
		}
		return string.Format("'{0}'{1}", name, (!isMasterClient) ? string.Empty : "(master)");
	}

	public string ToStringFull()
	{
		return $"#{ID:00} '{name}' {customProperties.ToStringFull()}";
	}
}
