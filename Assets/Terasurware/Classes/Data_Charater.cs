using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data_Charater : ScriptableObject
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
		
		public string Name;
		public int HP;
		public int MHP;
		public int Gauge;
		public int MGauge;
		public int Fill_Gauge;
		public int Atk;
		public int Def;
		public int Spd;
		public int Acc;
		public int Eva;
		public int Del;
	}
}

