using System;
using System.IO;
using System.Text;
using System.Drawing;
using FenghuiXX.utilss;

namespace FenghuiXX
{
    class Utils
    {

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
        public static bool is8Encoded(string input)
        {
            try
            {
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(input);
                string utf8String = Encoding.UTF8.GetString(utf8Bytes);
                return input == utf8String;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 0性别 1手机号
        public static String getGenderORPhong(String str,int i)
        {
            //char[] separators = new char[] { '=' };
            // string[] parts = str.Split(separators);
            string[] parts = str.Split('=');
            return parts[i];
        }
        // 查询信息最后值
        public  static String geterStrEnd(String inputName, String roonum,String gender)
        {
            String end = "";
            if (Constants.genderMan.Equals(gender)) {
                end = Constants.titleleft + inputName + Constants.genderManJiYu+Constants.titleright + roonum;
            }
            else
            end =  Constants.titleleft + inputName + Constants.genderWoManJiYu + Constants.titleright + roonum;

            return end;
        }

        //清楚2w码
        public static Bitmap getQingChuErPic() {           
            String imagePath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "001.png");
            try
            {                
                Bitmap bitmap = new Bitmap(imagePath1);//imagePath  "..\\..\\images\\001.png"
                return bitmap;
            }
            catch (Exception)
            {
                Bitmap flag = new Bitmap(245, 245);
                Graphics flagGraphics = Graphics.FromImage(flag);
                int blue = 0;
                int white = 22;
                while (white <= 245)
                {
                    flagGraphics.FillRectangle(Brushes.Gray, 0, blue, 245, 245);
                    flagGraphics.FillRectangle(Brushes.White, 0, white, 245, 245);
                    blue += 40;
                    white += 40;
                }
                return flag;
            }
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
    }

    //gb2312文本转-----utf8编码
    /*   private static String gb2312turnToUtf8(String gb2312String)
       {

           byte[] gb2312Bytes = Encoding.GetEncoding("gb2312").GetBytes(gb2312String); // 将GB2312编码的字符串转换为字节数组
           string utf8String = Encoding.UTF8.GetString(gb2312Bytes); // 将字节数组转换为UTF-8编码的字符串

           return utf8String;
       }*/

}
