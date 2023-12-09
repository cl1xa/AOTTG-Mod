using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ExitGames.Client.Photon;
using UnityEngine;

//Modified
public static class Extensions
{
	public static bool AlmostEquals(this float target, float second, float floatDiff)
	{
		return Mathf.Abs(target - second) < floatDiff;
	}

	public static bool AlmostEquals(this Quaternion target, Quaternion second, float maxAngle)
	{
		return Quaternion.Angle(target, second) < maxAngle;
	}

	public static bool AlmostEquals(this Vector2 target, Vector2 second, float sqrMagnitudePrecision)
	{
		return (target - second).sqrMagnitude < sqrMagnitudePrecision;
	}

	public static bool AlmostEquals(this Vector3 target, Vector3 second, float sqrMagnitudePrecision)
	{
		return (target - second).sqrMagnitude < sqrMagnitudePrecision;
	}

	public static bool Contains(this int[] target, int nr)
	{
		if (target != null)
		{
			for (int i = 0; i < target.Length; i++)
			{
				if (target[i] == nr)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static PhotonView GetPhotonView(this GameObject go)
	{
		return go.GetComponent<PhotonView>();
	}

	public static PhotonView[] GetPhotonViewsInChildren(this GameObject go)
	{
		return go.GetComponentsInChildren<PhotonView>(includeInactive: true);
	}

	public static void Merge(this IDictionary target, IDictionary addHash)
	{
		if (addHash == null || target.Equals(addHash))
		{
			return;
		}
		foreach (object key in addHash.Keys)
		{
			target[key] = addHash[key];
		}
	}

	public static void MergeStringKeys(this IDictionary target, IDictionary addHash)
	{
		if (addHash == null || target.Equals(addHash))
		{
			return;
		}
		foreach (object key in addHash.Keys)
		{
			if (key is string)
			{
				target[key] = addHash[key];
			}
		}
	}

	public static void StripKeysWithNullValues(this IDictionary original)
	{
		object[] array = new object[original.Count];
		int num = 0;
		foreach (object key2 in original.Keys)
		{
			array[num++] = key2;
		}
		foreach (object key in array)
		{
			if (original[key] == null)
			{
				original.Remove(key);
			}
		}
	}

	public static ExitGames.Client.Photon.Hashtable StripToStringKeys(this IDictionary original)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		foreach (DictionaryEntry item in original)
		{
			if (item.Key is string)
			{
				hashtable[item.Key] = item.Value;
			}
		}
		return hashtable;
	}

	public static string ToStringFull(this IDictionary origin)
	{
		return SupportClass.DictionaryToString(origin, includeTypes: false);
	}

    //Modded extensions below:

    //Lightsheir paste <3
    public static string StripHexColor(this object input)
    {
        return Regex.Replace(input.ToString(), "\\[([0-9a-fA-F]{6}|-)\\]", "");
    }

    //Lightsheir paste <3
    public static string StripHexCode(string input)
    {
        return Regex.Replace(input, "[^\\u0000-\\u007F]", "", RegexOptions.IgnoreCase);
    }

    //Lightsheir paste <3
    public static string StripHTML(this string input)
    {
        return Regex.Replace(input, "((<(\\/|)(color(?(?=\\=).*?)>|b>|size.*?>|i>)))", "", RegexOptions.IgnoreCase);
    }

    //Lightsheir paste <3
    public static string StripSize(this string input)
    {
        return Regex.Replace(input, "<((\\/|)(x|y)|size)>", "", RegexOptions.IgnoreCase);
    }
    
    //Stackoverflow
    public static string StripUnicode(this string input)
    {
        return Regex.Replace(input, @"[^a-zA-Z0-9 -]", "", RegexOptions.IgnoreCase);
    }

    //Lightsheir paste <3
    public static string StripWhiteSpace(this string Value)
    {
        return new Regex("[ ]{2,}", RegexOptions.None).Replace(Value.Trim(), " ");
    }

    //Lightsheir paste <3
    internal class Pair<X, Y>
    {
        public X item;
        public Y item2;
    }
    //Lightsheir paste <3
    public static string FixHTMLFormatting(this string text)
    {
        Dictionary<string, Pair<int, string>> dictionary = new Dictionary<string, Pair<int, string>>
        {
            { "color", new Pair<int, string>{item = 0, item2 = "<color=.*?>" } },
            { "/color",new Pair<int, string> { item = 0, item2 = "<\\/color>" } },
            { "size",new Pair<int, string>{item = 0, item2 = "<size=.*?>"}},
            { "/size",new Pair<int, string>{ item = 0, item2 = "<\\/size>" } },
            { "b",new Pair<int, string>{item = 0, item2 = "<b>" }},
            { "/b",new Pair<int, string> { item = 0,item2 = "<\\/b>"}},
            { "i",new Pair<int, string>{item = 0,item2 = "<i>"} },
            { "/i", new Pair<int, string>{item = 0,item2 = "<\\/i>"}}
        };

        int count;

        foreach (string key in dictionary.Keys)
        {
            count = 0;
            string item = dictionary[key].item2;

            Regex.Replace(text, item, delegate (Match d)
            {
                if (d.Success)
                    count++;
                return d.Value;

            }, RegexOptions.IgnoreCase);

            dictionary[key].item = count;
        }
        for (int i = 0; i < dictionary.Count; i += 2)
        {
            KeyValuePair<string, Pair<int, string>> keyValuePair = dictionary.ElementAt(i);
            KeyValuePair<string, Pair<int, string>> keyValuePair2 = dictionary.ElementAt(i + 1);

            if (keyValuePair.Value.item != keyValuePair2.Value.item)
            {
                for (int j = 0; j < 2; j++)
                {
                    string pattern = ((j == 0) ? keyValuePair.Value.item2 : keyValuePair2.Value.item2);
                    text = Regex.Replace(text, pattern, "", RegexOptions.IgnoreCase);
                }
            }
        }

        return text;
    }

    //Lightsheir paste <3
    public static string ToHex(this Color c)
    {
        return $"[{(c.r * 255f).ToByte():X2}{(c.g * 255f).ToByte():X2}{(c.b * 255f).ToByte():X2}]";
    }

    public static int ToInt(this object i)
    {
        try { return Convert.ToInt32(i); }
        catch { return -1; }
    }

    public static byte ToByte(this object b)
    {
        return Convert.ToByte(b);
    }

    public static PhotonPlayer ToPlayer(this object player)
    {
        return PhotonPlayer.Find(player.ToInt());
    }

	public static bool IsTitan(PhotonPlayer player)
	{
        switch (RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.isTitan]))
        {
            case 2:
                return true;
            default:
                return false;
        }
    }

    //Lightsheir paste <3
    public static int GetRCID(this string name)
    {
        string text = string.Empty;
        int num = 1;

        for (int i = 0; i < name.Length; i++)
        {
            if (name[i] == '[')
                num = i + 1;

            else if (name[i] == ']')
            {
                for (int j = num; j < i; j++) { text += name[j]; }

                if (text.ToInt() > 0)
                    break;

                text = string.Empty;
            }
        }
        return text.ToInt();
    }

    //Updated? Might be broken idk
    //Lightsheir paste <3
    public static bool InRoom(this string name, int ID, bool ignore = false)
    {
        PhotonPlayer photonPlayer = name.StripHex().GetRCID().ToPlayer();

        if (photonPlayer != null)
            name = GetPlayerName(photonPlayer);

        //Does this need to be null? Idk
        PhotonPlayer photonPlayer2;

        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            photonPlayer2 = PhotonNetwork.playerList[i];

            if (photonPlayer2.customProperties.ContainsKey("name") && GetPlayerName(photonPlayer2) == name)
                return true;
        }

        return false;
    }

	public static string GetPlayerName(PhotonPlayer player)
    {
        object properties = player.customProperties[PhotonPlayerProperty.name];

        if (properties != null && properties != string.Empty)
            return RCextensions.returnStringFromObject(properties).ToString();

        return "EMPTY_NAME";
    }

    public static string GetPlayerGuild(PhotonPlayer player)
    {
        return RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.guildName]).ToString();
    }

    public static string RandomString(int length)
    {
        System.Random random = new System.Random();

        return new string(Enumerable.Repeat(@"ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    //Lightsheir paste <3
    public static void HashNullFix(this IDictionary original)
    {
        int num = 0;
        object[] array = new object[original.Count];

        foreach (object key in original.Keys){ array[num++] = key; }

        for (int i = 0; i < array.Length; i++)
        {
            object obj;

            if (original[obj = array[i]] != null)
                continue;

            if (obj is string)
            {
                switch ((string)obj)
                {
                    case "kills":
                    case "deaths":
                    case "max_dmg":
                    case "total_dmg":
                    case "isTitan":
                    case "team":
                    case "sex":
                    case "heroCostumeId":
                    case "cape":
                    case "hairInfo":
                    case "eye_texture_id":
                    case "beard_texture_id":
                    case "glass_texture_id":
                    case "skin_color":
                    case "division":
                        original[obj] = 0;
                        continue;
                    case "statACL":
                    case "statBLA":
                    case "statGAS":
                    case "statSPD":
                        original[obj] = 100;
                        continue;
                    case "hair_color1":
                    case "hair_color2":
                    case "hair_color3":
                        original[obj] = 0f;
                        continue;
                    case "guildName":
                        original[obj] = string.Empty;
                        continue;
                    case "dead":
                        original[obj] = true;
                        continue;
                }
            }

            original.Remove(obj);
        }
    }
}
