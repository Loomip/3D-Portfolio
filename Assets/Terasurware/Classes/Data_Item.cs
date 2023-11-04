using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data_Item : ScriptableObject
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
		public string ItemType;
		public string Name;
		public int Skill_Gauge;
		public int CoolTime;
		public int Fill_HP;
		public int MHP;
		public int Atk;
		public int Def;
		public int Spd;
		public int Acc;
		public int Eva;
		public int Del;
	}
}

