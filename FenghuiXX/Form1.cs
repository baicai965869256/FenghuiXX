using System;
using System.Drawing;
using ZXing;
using ZXing.QrCode;
using System.Windows.Forms;
using System.Text;

using System.Collections.Generic;

namespace FenghuiXX
{
    public partial class Form1 : Form
    {
        //读取exceel 获取：第一列(图片id+row)和路径
        Dictionary<string, string> kvNameMapsEnd=new Dictionary<string, string>();
        String excelPath;
        String inputString;

        public Form1()
        {
            InitializeComponent();
            //this.label2.Text = System.DateTime.Now.ToString("HH:mm:ss");
            this.label1.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            //pictureBox1.Image = Image.FromFile("dq22222.png");
        }      
        // 要生成二维码的文字内容!!!!!!!!!!!
        //string tmpxx = "12a202299 ";
  

        // 111----选择excel 上传
        private void uploadExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                excelPath = openDlg.FileName;
            }
            //通过路径 读取excel sheet1
            kvNameMapsEnd = Utils.ExcelToDatatable(@excelPath, true);
            int kvNameMapsNums = kvNameMapsEnd.Keys.Count;
            if (kvNameMapsNums == 0)
            {              
                MessageBox.Show("只读Excel文件! \r\n关闭表格后再读取数据!", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //MessageBox.Show("只读Excel文件! \r\n关闭表格后再读取数据!");
                this.textBox1.Text = "请选择Excel文件! ";
                this.textBox2.Text = "";
                uploadExcel.Text = "选择表格";
                return;
            }
            // MessageBox.Show("共读取到：" + kvNameMapsNums + " 行数据 !");

            //uploadExcel 上传后变名称
            uploadExcel.Text = "表格名称:";
            // 显示excel 路径            
            this.textBox1.Text = Utils.gendPath(excelPath);
            this.textBox2.Text = "已读" + kvNameMapsNums + "行数据";

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
        //2---按钮： 查询人员二维码信息
        private void button1_Click(object sender, EventArgs e)
        {
            //输入input 姓名电话 张6
            inputString = input.Text;
        
            String value = "";
            if (kvNameMapsEnd.Count == 0) {
                MessageBox.Show("请先选择表格 !" + value, "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!String.IsNullOrEmpty(inputString))
            {
                if (kvNameMapsEnd.TryGetValue(inputString, out value))
                {
                    
                    MessageBox.Show("查询结果: "+inputString+"\r\n性别: " + Utils.getGenderORPhong(value, 0)+"\r\n手机号: "+ Utils.getGenderORPhong(value, 1) );
                    erWeiMa_Load(inputString+value);
                }
                else
                {
                    MessageBox.Show("暂未找到数据 !", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else {
                MessageBox.Show("请输入姓名或手机号!", "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        //111信息生成二维码 
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

            byte[] utf8Bytes = Encoding.Default.GetBytes(erPic); // 获取默认编码的字节数组
            string utf8String = Encoding.UTF8.GetString(utf8Bytes); // 将字节数组转换为UTF-8字符串   

            // 生成二维码图片
            Bitmap qrCodeImage = writer.Write(utf8String);

            // 将二维码图片设置为PictureBox控件的Image属性
            pictureBox1.Image = qrCodeImage;
        }

        //清除掉 二维码！
        private void button2_Click(object sender, EventArgs e)
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
            pictureBox1.Image = flag;// 图片显示 格子条纹
            this.input.Text = "";
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


        private string Notes = "姓名或手机(后四位)";
        private void input_Enter(object sender, EventArgs e)
        {
            //  进入获得焦点，清空
            if (input.Text == Notes)
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
                this.input.Text = Notes;
            }
        }
    }

}
