using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data_Messages : ScriptableObject
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
		
		public int Scene;
		public int DialogIndex;
		public string Name;
		public int Actor;
		public string Text;
		public int Return;
		public int Quest;
		public int Select_Code_1;
		public int Select_Code_2;
		public int Select_Code_3;
		public int Select_Code_4;
		public string Select_Txt_1;
		public string Select_Txt_2;
		public string Select_Txt_3;
		public string Select_Txt_4;
		public int Reward;
	}
}

