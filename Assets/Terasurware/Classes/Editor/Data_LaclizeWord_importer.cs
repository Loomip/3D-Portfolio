using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Data_LaclizeWord_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/DataSheet/Data_LaclizeWord.xlsx";
	private static readonly string exportPath = "Assets/DataSheet/Data_LaclizeWord.asset";
	private static readonly string[] sheetNames = { "Sheet1", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Data_LaclizeWord data = (Data_LaclizeWord)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Data_LaclizeWord));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Data_LaclizeWord> ();
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

					Data_LaclizeWord.Sheet s = new Data_LaclizeWord.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Data_LaclizeWord.Param p = new Data_LaclizeWord.Param ();
						
					cell = row.GetCell(0); p.Type = (cell == null ? "" : cell.ToString());
					cell = row.GetCell(1); p.Kor = (cell == null ? "" : cell.ToString());
					cell = row.GetCell(2); p.Eng = (cell == null ? "" : cell.ToString());
					cell = row.GetCell(3); p.Jpn = (cell == null ? "" : cell.ToString());
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
