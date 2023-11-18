using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Data_Item_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/DataSheet/Data_Item.xlsx";
	private static readonly string exportPath = "Assets/DataSheet/Data_Item.asset";
	private static readonly string[] sheetNames = { "Sheet1", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Data_Item data = (Data_Item)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Data_Item));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Data_Item> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					Data_Item.Sheet s = new Data_Item.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Data_Item.Param p = new Data_Item.Param ();
						
					cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.ItemType = (cell == null ? "" : cell.ToString());
					cell = row.GetCell(2); p.Name = (cell == null ? "" : cell.ToString());
					cell = row.GetCell(3); p.SpritName = (cell == null ? "" : cell.ToString());
					cell = row.GetCell(4); p.Skill_Gauge = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.CoolTime = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.Fill_HP = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.MHP = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.Atk = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.Def = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.Spd = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.Acc = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.Eva = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.Del = (int)(cell == null ? 0 : cell.NumericCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
