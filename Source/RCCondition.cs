internal class RCCondition
{
	public enum castTypes
	{
		typeInt,
		typeBool,
		typeString,
		typeFloat,
		typePlayer,
		typeTitan
	}

	public enum operands
	{
		lt,
		lte,
		e,
		gte,
		gt,
		ne
	}

	public enum stringOperands
	{
		equals,
		notEquals,
		contains,
		notContains,
		startsWith,
		notStartsWith,
		endsWith,
		notEndsWith
	}

	private int operand;

	private RCActionHelper parameter1;

	private RCActionHelper parameter2;

	private int type;

	public RCCondition(int sentOperand, int sentType, RCActionHelper sentParam1, RCActionHelper sentParam2)
	{
		operand = sentOperand;
		type = sentType;
		parameter1 = sentParam1;
		parameter2 = sentParam2;
	}

	private bool boolCompare(bool baseBool, bool compareBool)
	{
		return operand switch
		{
			2 => baseBool == compareBool, 
			5 => baseBool != compareBool, 
			_ => false, 
		};
	}

	public bool checkCondition()
	{
		return type switch
		{
			0 => intCompare(parameter1.returnInt(null), parameter2.returnInt(null)), 
			1 => boolCompare(parameter1.returnBool(null), parameter2.returnBool(null)), 
			2 => stringCompare(parameter1.returnString(null), parameter2.returnString(null)), 
			3 => floatCompare(parameter1.returnFloat(null), parameter2.returnFloat(null)), 
			4 => playerCompare(parameter1.returnPlayer(null), parameter2.returnPlayer(null)), 
			5 => titanCompare(parameter1.returnTitan(null), parameter2.returnTitan(null)), 
			_ => false, 
		};
	}

	private bool floatCompare(float baseFloat, float compareFloat)
	{
		switch (operand)
		{
		case 0:
			if (baseFloat >= compareFloat)
			{
				return false;
			}
			return true;
		case 1:
			if (baseFloat > compareFloat)
			{
				return false;
			}
			return true;
		case 2:
			if (baseFloat != compareFloat)
			{
				return false;
			}
			return true;
		case 3:
			if (baseFloat < compareFloat)
			{
				return false;
			}
			return true;
		case 4:
			if (baseFloat <= compareFloat)
			{
				return false;
			}
			return true;
		case 5:
			if (baseFloat == compareFloat)
			{
				return false;
			}
			return true;
		default:
			return false;
		}
	}

	private bool intCompare(int baseInt, int compareInt)
	{
		switch (operand)
		{
		case 0:
			if (baseInt >= compareInt)
			{
				return false;
			}
			return true;
		case 1:
			if (baseInt > compareInt)
			{
				return false;
			}
			return true;
		case 2:
			if (baseInt != compareInt)
			{
				return false;
			}
			return true;
		case 3:
			if (baseInt < compareInt)
			{
				return false;
			}
			return true;
		case 4:
			if (baseInt <= compareInt)
			{
				return false;
			}
			return true;
		case 5:
			if (baseInt == compareInt)
			{
				return false;
			}
			return true;
		default:
			return false;
		}
	}

	private bool playerCompare(PhotonPlayer basePlayer, PhotonPlayer comparePlayer)
	{
		return operand switch
		{
			2 => basePlayer == comparePlayer, 
			5 => basePlayer != comparePlayer, 
			_ => false, 
		};
	}

	private bool stringCompare(string baseString, string compareString)
	{
		switch (operand)
		{
		case 0:
			if (!(baseString == compareString))
			{
				return false;
			}
			return true;
		case 1:
			if (!(baseString != compareString))
			{
				return false;
			}
			return true;
		case 2:
			if (!baseString.Contains(compareString))
			{
				return false;
			}
			return true;
		case 3:
			if (baseString.Contains(compareString))
			{
				return false;
			}
			return true;
		case 4:
			if (!baseString.StartsWith(compareString))
			{
				return false;
			}
			return true;
		case 5:
			if (baseString.StartsWith(compareString))
			{
				return false;
			}
			return true;
		case 6:
			if (!baseString.EndsWith(compareString))
			{
				return false;
			}
			return true;
		case 7:
			if (baseString.EndsWith(compareString))
			{
				return false;
			}
			return true;
		default:
			return false;
		}
	}

	private bool titanCompare(TITAN baseTitan, TITAN compareTitan)
	{
		return operand switch
		{
			2 => baseTitan == compareTitan, 
			5 => baseTitan != compareTitan, 
			_ => false, 
		};
	}
}
