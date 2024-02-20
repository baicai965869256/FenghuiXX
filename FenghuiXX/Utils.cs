using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace FenghuiXX
{
    class Utils
    {

        //默认表头是第三行
        static int biaotouNum = 2;
        static String PIN = "=";

        static String excelName = "姓名";
        static String excelPhong = "手机号";
        static String excelgender = "性别";
        // 名字/电话 查询房间号 张6
        static Dictionary<string, string> kvNameMaps = new Dictionary<string, string>();
        Dictionary<string, string> kvPhoneMaps = new Dictionary<string, string>();

        /*
        字符串编码格式 是否是UTF8 
        */
        public static bool isNotUTF8(string str)
        {
            bool bln = false;
            char[] chars = str.ToCharArray();
            foreach (char cr in chars)
            {
                int ichar = (int)cr;
                if (ichar > 127 && ichar <= 255)
                {
                    bln = true;
                    break;
                }
            }
            return bln;
        }

        // 拼接 name : gender=phone
        public static String getGenderPhong(String gender, String phong)
        {
            return gender+PIN+ phong;
        }

        // 0性别 1手机号
        public static String getGenderORPhong(String str,int i)
        {
            //char[] separators = new char[] { '=' };
            // string[] parts = str.Split(separators);
            string[] parts = str.Split('=');
            return parts[i];
        }

        //最后的路径 显示
        public static String gendPath(String path) {
            try
            {
                int i = path.LastIndexOf('\\');
                string result = path.Substring(i + 1);
                return result;
            }
            catch (Exception)
            {
                return path;
            }     
        }


        //0---默认 Sheet1
        static String sheetName = "Sheet1";

        #region 获取IWorkbookSheet1
        public static ISheet getIWorkBookISheet(string fileName, String sheetName)
        {
            FileStream fs;
            IWorkbook workbook = null;
            ISheet sheet = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                {
                    workbook = new HSSFWorkbook(fs);//根据给定的sheet名称获取数据
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
            if (workbook == null)
                return null;

            if (sheetName != null)
            {
                sheet = workbook.GetSheet(sheetName);//根据给定的sheet名称获取数据
            }
            else
            {
                //也可以根据sheet编号来获取数据
                sheet = workbook.GetSheetAt(0);//获取第几个sheet表（此处表示如果没有给定sheet名称，默认是第一个sheet表）  
            }
            workbook.Close();
            return sheet;

        }
        #endregion

        #region 1 ---读取Excel 第3列数据
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="fileNamePath">文件路径</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名，true是</param>
        /// <returns>返回的DataTable</returns>
        public static Dictionary<string, string> ExcelToDatatable(string fileNamePath, bool isFirstRowColumn)
        {
            int cellCount = 0;//列数
            kvNameMaps.Clear();

            ISheet sheet = getIWorkBookISheet(fileNamePath, sheetName);
            if (sheet == null)
            {
                MessageBox.Show("请选择Excel文件!", "警告!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kvNameMaps;
            }
            int sheetLarNums = sheet.LastRowNum;
            try
            {
                if (sheetLarNums > 3 && sheet != null)
                {
                    //获取表头 默认2（第三行）
                    IRow firstRow = sheet.GetRow(biaotouNum);
                    cellCount = firstRow.LastCellNum; //表头行 最后一个cell的编号 即总的列数

                    // 获取“手机号”和“姓名”“性别”列的索引
                    int phoneColumnIndex = -1;
                    int nameColumnIndex = -1;
                    int genderColumnIndex = -1;
                    for (int i = 0; i < cellCount; i++)
                    {
                        if (firstRow.GetCell(i).ToString() == excelPhong)
                        {
                            phoneColumnIndex = i;
                        }
                        else if (firstRow.GetCell(i).ToString() == excelName)
                        {
                            nameColumnIndex = i;
                        }
                        else if (firstRow.GetCell(i).ToString() == excelgender)
                        {
                            genderColumnIndex = i;
                        }
                    }
                    // 遍历第三到  最后一行
                    for (int i = biaotouNum + 1; i <= sheetLarNums; i++)
                    {
                        // 获取当前行
                        IRow row = sheet.GetRow(i);
                        try
                        {
                            // 获取“电话”和“姓名”"性别"  列的值
                            string phoneValue = row.GetCell(phoneColumnIndex).ToString().Trim();
                            string nameValue = row.GetCell(nameColumnIndex).ToString().Trim();
                            string genderValue = row.GetCell(genderColumnIndex).ToString().Trim();
                            if (!String.IsNullOrEmpty(nameValue) && !String.IsNullOrEmpty(phoneValue) && !String.IsNullOrEmpty(genderValue))
                            {
                                //name : gender=phone
                                kvNameMaps.Add(nameValue, getGenderPhong(genderValue, phoneValue));
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("第" + i + "行数据有问题！", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }
                    }
                }
                return kvNameMaps;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return kvNameMaps;
            }
        }
        #endregion
    }



    //gb2312文本转-----utf8编码
    /*   private static String gb2312turnToUtf8(String gb2312String)
       {

           byte[] gb2312Bytes = Encoding.GetEncoding("gb2312").GetBytes(gb2312String); // 将GB2312编码的字符串转换为字节数组
           string utf8String = Encoding.UTF8.GetString(gb2312Bytes); // 将字节数组转换为UTF-8编码的字符串

           return utf8String;
       }*/

}
