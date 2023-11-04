using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Data_Quest_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/DataSheet/Data_Quest.xlsx";
	private static readonly string exportPath = "Assets/DataSheet/Data_Quest.asset";
	private static readonly string[] sheetNames = { "Kor", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Data_Quest data = (Data_Quest)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Data_Quest));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Data_Quest> ();
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

					Data_Quest.Sheet s = new Data_Quest.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Data_Quest.Param p = new Data_Quest.Param ();
						
					cell = row.GetCell(0); p.Scene = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.Code = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.Name = (cell == null ? "" : cell.ToString());
					cell = row.GetCell(3); p.Artor = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.Text = (cell == null ? "" : cell.ToString());
					cell = row.GetCell(5); p.Return = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.Quest = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.Select_Code_1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.Select_Code_2 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.Select_Code_3 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.Select_Code_4 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.Select_Txt_1 = (cell == null ? "" : cell.ToString());
					cell = row.GetCell(12); p.Select_Txt_2 = (cell == null ? "" : cell.ToString());
					cell = row.GetCell(13); p.Select_Txt_3 = (cell == null ? "" : cell.ToString());
					cell = row.GetCell(14); p.Select_Txt_4 = (cell == null ? "" : cell.ToString());
					cell = row.GetCell(15); p.Reward = (int)(cell == null ? 0 : cell.NumericCellValue);
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
