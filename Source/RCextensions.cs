using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

internal static class RCextensions
{
	public static void Add<T>(ref T[] source, T value)
	{
		T[] array = new T[source.Length + 1];
		for (int i = 0; i < source.Length; i++)
		{
			array[i] = source[i];
		}
		array[array.Length - 1] = value;
		source = array;
	}

	public static bool IsNullOrEmpty(this string value)
	{
		if (value != null)
		{
			return value.Length == 0;
		}
		return true;
	}

	public static string UpperFirstLetter(this string text)
	{
		if (text == string.Empty)
		{
			return text;
		}
		if (text.Length > 1)
		{
			return char.ToUpper(text[0]) + text.Substring(1);
		}
		return text.ToUpper();
	}

    //Modified
    //Lightsheir paste <3
    public static string StripHex(this object l)
    {
        return Regex.Replace(l.ToString(), "\\[([0-9a-fA-F]{6}|-)\\]", "");
    }

    //Modified
    //Lightsheir paste <3
    public static string HexColor(this string text)
	{
        int clrs = 0;

        text = Regex.Replace(text, "<(\\/|)(x|y)>", "");
        text = text.Replace("[-]", "</color>");

        text = Regex.Replace(text, "(\\[)[0-9a-fA-F]{6}(\\])", delegate (Match match)
        {
            string text2 = match.Value;

            if (match.Success)
            {
                text2 = text2.Replace("[", "<color=#").Replace("]", ">");
                clrs++;
            }
            return text2;
        });

        clrs -= Regex.Matches(text, "</color>").Count;

        for (int i = 0; i < clrs; i++) { text += "</color>"; }

        return text;
    }

	public static bool IsLowestID(this PhotonPlayer player)
	{
		PhotonPlayer[] playerList = PhotonNetwork.playerList;
		for (int i = 0; i < playerList.Length; i++)
		{
			if (playerList[i].ID < player.ID)
			{
				return false;
			}
		}
		return true;
	}

	public static void RemoveAt<T>(ref T[] source, int index)
	{
		if (source.Length == 1)
		{
			source = new T[0];
		}
		else
		{
			if (source.Length <= 1)
			{
				return;
			}
			T[] array = new T[source.Length - 1];
			int i = 0;
			int num = 0;
			for (; i < source.Length; i++)
			{
				if (i != index)
				{
					array[num] = source[i];
					num++;
				}
			}
			source = array;
		}
	}

	public static bool returnBoolFromObject(object obj)
	{
		if (obj != null && obj is bool)
		{
			return (bool)obj;
		}
		return false;
	}

	public static float returnFloatFromObject(object obj)
	{
		if (obj != null && obj is float)
		{
			return (float)obj;
		}
		return 0f;
	}

	public static int returnIntFromObject(object obj)
	{
		if (obj != null && obj is int)
		{
			return (int)obj;
		}
		return 0;
	}

	public static string returnStringFromObject(object obj)
	{
		if (obj != null && obj is string result)
		{
			return result;
		}
		return string.Empty;
	}

	public static T ToEnum<T>(this string value, bool ignoreCase = true)
	{
		if (Enum.IsDefined(typeof(T), value))
		{
			return (T)Enum.Parse(typeof(T), value, ignoreCase);
		}
		return default(T);
	}

	public static string[] EnumToStringArray<T>()
	{
		return Enum.GetNames(typeof(T));
	}

	public static string[] EnumToStringArrayExceptNone<T>()
	{
		List<string> list = new List<string>();
		string[] array = EnumToStringArray<T>();
		foreach (string text in array)
		{
			if (text != "None")
			{
				list.Add(text);
			}
		}
		return list.ToArray();
	}

	public static List<T> EnumToList<T>()
	{
		return Enum.GetValues(typeof(T)).Cast<T>().ToList();
	}

	public static Dictionary<string, T> EnumToDict<T>()
	{
		Dictionary<string, T> dictionary = new Dictionary<string, T>();
		foreach (T item in EnumToList<T>())
		{
			dictionary.Add(item.ToString(), item);
		}
		return dictionary;
	}

	public static float ParseFloat(string str)
	{
		return float.Parse(str, CultureInfo.InvariantCulture);
	}

	public static bool IsGray(this Color color)
	{
		if (color.r == color.g && color.r == color.b)
		{
			return color.a == 1f;
		}
		return false;
	}

	public static HERO GetMyHero()
	{
		foreach (HERO player in FengGameManagerMKII.instance.getPlayers())
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || player.photonView.isMine)
			{
				return player;
			}
		}
		return null;
	}
}
