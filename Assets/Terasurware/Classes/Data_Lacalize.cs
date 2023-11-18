using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data_Lacalize : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public int ID;
		public string Name;
		public string SpritName;
		public string TooltipName;
		public string Explan;
	}
}

