using System;
using System.Drawing;
using ZXing;
using ZXing.QrCode;
using System.Windows.Forms;
using System.Collections.Generic;
using ZXing.QrCode.Internal;
using FenghuiXX.utilss;

namespace FenghuiXX
{
    public partial class Form1 : Form
    {

        List<PersonInfoClass> personsInfoLists = new List<PersonInfoClass>();
        //读取exceel 获取：第一列(图片id+row)和路径
        String excelPath;
        String inputString;

        //置二维码编码
        //QrCodeEncodingOptions qr = new QrCodeEncodingOptions();

        QrCodeEncodingOptions qr = new QrCodeEncodingOptions
        {//配置二维码规格 张7
            ErrorCorrection = ErrorCorrectionLevel.H,
            CharacterSet = Constants.bianma,
            DisableECI = false,   
        };

        public Form1()
        {
            InitializeComponent();
            //this.label2.Text = System.DateTime.Now.ToString("HH:mm:ss");
            this.label1.Text = System.DateTime.Now.ToString(Constants.timeYMD);
        }      
  
        // 0--选择excel 上传
        private void uploadExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                excelPath = openDlg.FileName;
            }
            //通过路径 默认读取excel sheet1
            personsInfoLists = ProducePersonInfos.ExcelToDatatable(@excelPath, true);

            int personsInfoListsNums = personsInfoLists.Count;
            if (personsInfoListsNums == 0)
            {              
                MessageBox.Show(Constants.uploadExcelOnly, "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox1.Text = Constants.uploadExcelPlease;
                this.textBox2.Text = "";
                uploadExcel.Text = Constants.uploadExcelText;
                return;
            }            
            uploadExcel.Text = Constants.uploadExcelTextAfter;
            // 显示excel 路径            
            this.textBox1.Text = Utils.gendPath(excelPath);
            this.textBox2.Text = "已读" + personsInfoListsNums + "行数据";

        }
        // 输入 姓名电话
        private void input_TextChanged(object sender, EventArgs e)
        {
     /*       inputString=input.Text;
            String value;
            if (!String.IsNullOrEmpty(inputString)) {
                if (kvNameMapsEnd.TryGetValue("apple", out value))
                {
                    MessageBox.Show("找到了"+ value, "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else
                {
                    MessageBox.Show("找不到数据！", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }*/
        }
        //2---按钮：查询信息&修改
        private void button1_Click(object sender, EventArgs e)
        {   //输入input 姓名电话 张6
            if (personsInfoLists.Count == 0)
            {
                MessageBox.Show(Constants.uploadExcelBeforePlease, "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            inputString = input.Text;        
            String value = "";
            if (!String.IsNullOrEmpty(inputString))
            {
                Boolean isfand = true;
                // 访问使用
                foreach (PersonInfoClass personinfo in personsInfoLists)
                {
                    if (inputString.Equals(personinfo.Name) || inputString.Equals(personinfo.PhoneNumber))
                    {
                        isfand = false;
                        String message = (personinfo.Name + "   " + personinfo.Gender+ Environment.NewLine
                            + "电话：" + personinfo.PhoneNumber+ Environment.NewLine
                            + "房间：" + personinfo.RoomNum);
                        
                        MessageBox.Show(message);
                        this.input.Text = message;

                        btn_ZXing_Click(Utils.geterStrEnd(personinfo.Name, personinfo.RoomNum, personinfo.Gender));//2为嘛显示内容
                    }                  
                }
                if (isfand) {
                    MessageBox.Show(Constants.notFoundExcel, "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else {
                MessageBox.Show(Constants.namePhongPlease, "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //1-信息生成二维码 
        private void btn_ZXing_Click(String erPic)
        {
            qr.Width = pictureBox1.Width;// 设置二维码宽度与PictureBox控件相同
            qr.Height = pictureBox1.Height;

            string rqcodeData = erPic;//二维码string
            if (string.IsNullOrWhiteSpace(rqcodeData))
            {
                MessageBox.Show(Constants.notFoundExcel); return;
            }
     
            //生成二维码位图
            BarcodeWriter wr = new BarcodeWriter();
            wr.Format = BarcodeFormat.QR_CODE;//选择二维码，还可以选择条码，类型有很多
            wr.Options = qr;
            Bitmap bitmap = wr.Write(rqcodeData);

            //pictureBox1绑定图
            pictureBox1.Image = Image.FromHbitmap(bitmap.GetHbitmap());      
            bitmap.Dispose();
        }

        //xx信息生成二维码 
        private void erWeiMa_Load(String erPic)
        {
            // 创建二维码对象
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Width = pictureBox1.Width, // 设置二维码宽度与PictureBox控件相同
                    Height = pictureBox1.Height, // 设置二维码高度与PictureBox控件相同
                    Margin = 1, // 设置二维码边距
                    ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.H // 设置纠错级别为最高级别
                }
            };
            // 默认编码格式？？？
          /*  Encoding encoding = Encoding.Default;
            String name = encoding.BodyName;*/
            // gb2312文本转-----utf8编码
            // String utf8String = gb2312turnUtf8(tmpxx);
            //Boolean b = isNotUTF8(utf8String);

          /*  byte[] utf8Bytes = Encoding.Default.GetBytes(erPic); // 获取默认编码的字节数组
            string utf8String = Encoding.UTF8.GetString(utf8Bytes); // 将字节数组转换为UTF-8字符串   
            MessageBox.Show("222是么？" + Utils.is8Encoded(utf8String)); */

            // 生成二维码图片 张6
            Bitmap qrCodeImage = writer.Write(erPic);
           // Bitmap qrCodeImage = exportErPic(utf8String);
            // 将二维码图片设置为PictureBox控件的Image属性
            pictureBox1.Image = qrCodeImage;
        }

        //清除掉 二维码！
        private void button2_Click(object sender, EventArgs e)
        {
      /*      Bitmap flag = new Bitmap(245, 245);
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
            pictureBox1.Image = flag;// 图片显示 格子条纹*/

            pictureBox1.Image= Utils.getQingChuErPic();
            this.input.Text = "";
        }

        private void input_Enter(object sender, EventArgs e)
        {
            //  进入获得焦点，清空
            this.input.Text = "";
            if (input.Text == Constants.Notes)
            {
                input.ForeColor = Color.Black;
                this.input.Text = "";
            }
        }

        private void input_MouseLeave(object sender, EventArgs e)
        {
            //  退出失去焦点，重新显示
            if (string.IsNullOrEmpty(input.Text))
            {
                input.ForeColor = Color.DarkGray;
                this.input.Text = Constants.Notes;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //改变签到人员 颜色？workbook.Write  {"无法访问已关闭的文件。"} 
        private void button3_Click(object sender, EventArgs e)
        {
            ProducePersonInfos.turnExcelColor(@excelPath,4,4);
        }
    }

}
