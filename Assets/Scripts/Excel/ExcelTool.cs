using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Excel;
using ICSharpCode;
using System.Data;
using System.IO;

public class ExcelTool 
{
    public static DataSet ReadExcel(string filePath)
    {
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();
        excelReader.Close();
        return result;
    }
}
