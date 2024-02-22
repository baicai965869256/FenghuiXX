using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using FenghuiXX.utilss;

namespace FenghuiXX
{
    class ProducePersonInfos
    {
        //默认
        static int biaotouNum = 2;//表头是第三行        
        static String sheetName = "Sheet1";//默认 Sheet1  0

        // 创建一个包含10000个Person对象的List
        static List<PersonInfoClass> personsInfoList = new List<PersonInfoClass>();


        /// <summary>
        /// 将excel中的数据导入到DataTable  personsInfoList
        /// </summary>
        /// <param name="fileNamePath">文件路径</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名，true是</param>
        /// <returns>返回的DataTable</returns>
        public static List<PersonInfoClass> ExcelToDatatable(string fileNamePath, bool isFirstRowColumn)
        {
            int cellCount = 0;//列数
            personsInfoList.Clear();

            ISheet sheet = getIWorkBookISheet(fileNamePath, sheetName);
            if (sheet == null)
            {
                MessageBox.Show(Constants.uploadExcelPlease, "警告!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return personsInfoList;
            }
            int sheetLarNums = sheet.LastRowNum;
            try
            {
                if (sheetLarNums > 3 && sheet != null)
                {                   
                    //表头 默认2（第三行）
                    IRow firstRow = sheet.GetRow(biaotouNum);
                    cellCount = firstRow.LastCellNum; //表头行 最后一个cell的编号 即总的列数
                    /*
                     手机号 姓名 性别 房间号  列的索引
                     */
                    int phoneColumnIndex = -1;
                    int nameColumnIndex = -1;
                    int genderColumnIndex = -1;
                    int roomColumnIndex = -1;

                    for (int i = 0; i < cellCount; i++)
                    {
                        if (firstRow.GetCell(i).ToString() == Constants.excelPhong)
                        {
                            phoneColumnIndex = i;
                        }
                        else if (firstRow.GetCell(i).ToString() == Constants.excelName)
                        {
                            nameColumnIndex = i;
                        }
                        else if (firstRow.GetCell(i).ToString() == Constants.excelgender)
                        {
                            genderColumnIndex = i;
                        }
                        else if (firstRow.GetCell(i).ToString() == Constants.excelROOM)
                        {
                            roomColumnIndex = i;
                        }
                    }
                    // 遍历表头开始 3到最后一行
                    for (int i = biaotouNum + 1; i <= sheetLarNums; i++)
                    {
                        PersonInfoClass personInfo = new PersonInfoClass();                 
                        // 获取当前行
                        IRow row = sheet.GetRow(i);
                        try
                        {
                            // 手机号 姓名 性别 房间号  列的值                            
                            string nameValue = row.GetCell(nameColumnIndex).ToString().Trim();
                            string phoneValue = row.GetCell(phoneColumnIndex).ToString().Trim();
                            string genderValue = row.GetCell(genderColumnIndex).ToString().Trim();
                            string roomValue = row.GetCell(roomColumnIndex).ToString();

                            personInfo.Name = string.IsNullOrEmpty(nameValue) ? Constants.excelCellNULL : nameValue; 
                            personInfo.Gender = string.IsNullOrEmpty(genderValue) ? Constants.excelCellNULL : genderValue;
                            personInfo.PhoneNumber = string.IsNullOrEmpty(phoneValue) ? Constants.excelCellNULL : phoneValue;
                            personInfo.RoomNum = string.IsNullOrEmpty(roomValue) ? Constants.excelCellNULL : roomValue;
                            personsInfoList.Add(personInfo);                                            
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("第" + i + "行数据有问题！", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }
                    }
                }
                return personsInfoList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return personsInfoList;
            }
        }        

        //获取IWorkbookSheet1
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
                sheet = workbook.GetSheetAt(0);//获取第几个sheet表（此处表示如果没有给定sheet名称，默认是第一个sheet表）  
            }
            workbook.Close();
            return sheet;

        }

        //改变签到人员 颜色
        public static void turnExcelColor(string fileName,int i,int j)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                // 创建工作簿
                IWorkbook workbook = null;
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                {
                    workbook = new HSSFWorkbook(fs);
                }

                // 获取sheet
                ISheet sheet = workbook.GetSheet(sheetName);

                // 获取单元格
                IRow row = sheet.GetRow(4);
                ICell cell = row.GetCell(4);

                // 设置单元格背景颜色为红色
                ICellStyle style = workbook.CreateCellStyle();
                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
                style.FillPattern = FillPattern.SolidForeground;
                cell.CellStyle = style;

                // 保存更改到文件
                workbook.Write(fs);
            }
        }

    }
}
