using System;
using System.Drawing;
using ZXing;
using ZXing.QrCode;
using System.Windows.Forms;
using System.Collections.Generic;
using ZXing.QrCode.Internal;
using FenghuiXX.utilss;
using System.Threading;

namespace FenghuiXX
{
    public partial class Form1 : Form
    {
        List<PersonInfoClass> personsInfoLists = new List<PersonInfoClass>();       
        int itemListBox1CountMin = 4;
        int itemListBox1CountMax = 7;
        //默认4个表头可选项(4-7)  姓名0 性别1 手机号2    房间号3  级别4 抵达日期5  返程日期6
        bool[] itemListBox1Booleans = new bool[] { true, true, true, true, false, false, false };

        //读取exceel 获取：第一列(图片id+row)和路径
        String excelPath;
        String inputString;

        //置二维码编码
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
            // 行列 选项默认隐藏
            checkedListBox1.Visible = false;
            listBox1.Visible = false;
            listBox1.SelectionMode = SelectionMode.One; // 设置为单选模式
        }      


            // 0--选择excel 上传
            private void uploadExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                excelPath = openDlg.FileName;
            }
            //通过路径 默认读取excel sheet1  实体字段选择itemListBox1Booleans
            personsInfoLists = ProducePersonInfos.ExcelToDatatable(@excelPath, true, itemListBox1Booleans);

            int personsInfoListsNums = personsInfoLists.Count;
            if (personsInfoListsNums == 0)
            {              
                MessageBox.Show(Constants.uploadExcelOnly, "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox1.Text = Constants.uploadExcelPlease;
                this.textBox2.Text = "";
                uploadExcel.Text = Constants.uploadExcelText;
                return;
            }
            //姓名重复 ？
            ProducePersonInfos.chongfuName(personsInfoLists);

            uploadExcel.Text = Constants.uploadExcelTextAfter;
            // 显示excel 路径            
            this.textBox1.Text = Utils.gendPath(excelPath);
            this.textBox2.Text = "已读" + personsInfoListsNums + "条数据！";

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
        //2-查询按钮：查信息&修改
        private void button1_Click(object sender, EventArgs e)
        {   //输入input 姓名电话 张6
            if (personsInfoLists.Count == 0)
            {
                MessageBox.Show(Constants.uploadExcelBeforePlease, "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            inputString = input.Text;        
            if (!String.IsNullOrEmpty(inputString))
            {
                Boolean isfand = true;    
                // 访问使用
                foreach (PersonInfoClass personinfo in personsInfoLists)
                {          
                    if (inputString.Equals(personinfo.Name) || inputString.Equals(personinfo.PhoneNumber))
                    {
                        isfand = false;
                           
                        //默认消息
                        MessageBox.Show(personinfo.ToString());
                        this.input.Text = personinfo.ToString();
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
                    ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.H //纠错级别为最高级别
                }
            };
            // 默认编码格式？？？
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

        //comboBox 选择 行列
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (personsInfoLists.Count == 0)
            {
                MessageBox.Show(Constants.uploadExcelBeforePlease, "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }*/
            string selectedOption = comboBox1.SelectedItem.ToString();
            switch (selectedOption)
            {
                case "表头(行)选择":
                    listBox1.Items.Clear();
                    checkedListBox1.Items.Clear();
                    //checkedListBox1.SelectionMode = SelectionMode.One;
                    listBox1.Visible = true;
                    checkedListBox1.Visible = false;
                    listBox1.Items.AddRange(new object[] { "第1行", "第2行", "第3行" });
                    break;
                case "表格(列)选择":
                    listBox1.Items.Clear();
                    checkedListBox1.Items.Clear();
                    listBox1.Visible = false;
                    checkedListBox1.Visible = true;

                    //checkedListBox1  默认Items
                    checkedListBox1.Items.AddRange(new object[] { "姓名", "性别", "手机号", "房间号", "级别", "抵达日期", "返程日期" });
                    for (int i = 0; i < itemListBox1CountMax; i++)
                    {
                        checkedListBox1.SetItemChecked(i, false);
                    }
                    itemListBox1CountMin = 4;
                    for (int i = 0; i < itemListBox1CountMin; i++)
                    {
                        checkedListBox1.SetItemChecked(i, true);
                    }
                    break;
                default:
                    break;
            }
            checkedListBox1.Visible = true;
        }
        // 行  选项默认隐藏
        private void listBox1_MouseLeave(object sender, EventArgs e)
        {
            Thread.Sleep(300); // 睡眠1秒（1000毫秒）
            listBox1.Visible = false;
            checkedListBox1.Visible = false;

            if (listBox1.SelectedItem != null)
            {
                // MessageBox.Show("您选择了：" + listBox1.SelectedItem.ToString());
                this.input.Text = "默认表头 第3行";
            }
            else
            {
                // MessageBox.Show("默认表头，第3行！");
                this.input.Text = "默认表头 第3行";
            }

        }
        // 列 选项默认隐藏
        private void checkedListBox1_MouseLeave(object sender, EventArgs e)
        {
            //this.Controls.Remove(checkedListBox1);
            
            listBox1.Visible = false;
            checkedListBox1.Visible = false;

            //元素数量 遍历选择/没选 (至少 4个)
            int tmpnum = checkedListBox1.Items.Count;
            itemListBox1CountMin = (tmpnum > itemListBox1CountMin) ? tmpnum : itemListBox1CountMin;
            for (int i = 0; i < itemListBox1CountMax; i++)
            {
                itemListBox1Booleans[i]=checkedListBox1.GetItemChecked(i);
            }
           Thread.Sleep(100); // 睡眠1秒（1000毫秒）
            //重读
            //通过路径 默认读取excel sheet1  实体字段选择itemListBox1Booleans
            personsInfoLists = ProducePersonInfos.ExcelToDatatable(@excelPath, true, itemListBox1Booleans);

            int personsInfoListsNums = personsInfoLists.Count;
            if (personsInfoListsNums == 0)
            {
                MessageBox.Show(Constants.uploadExcelOnly, "提示!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.textBox1.Text = Constants.uploadExcelPlease;
                this.textBox2.Text = "";
                uploadExcel.Text = Constants.uploadExcelText;
                return;
            }
            //姓名重复 ？
            ProducePersonInfos.chongfuName(personsInfoLists);

            uploadExcel.Text = Constants.uploadExcelTextAfter;
            // 显示excel 路径            
            this.textBox1.Text = Utils.gendPath(excelPath);
            this.textBox2.Text = "已读" + personsInfoListsNums + "条数据！";
        }


        //  进入获得焦点，清空
        private void input_Enter(object sender, EventArgs e)
        {            
            this.input.Text = "";
            if (input.Text == Constants.Notes)
            {
                input.ForeColor = Color.Black;
                this.input.Text = "";
            }
        }

        //  退出失去焦点，重新显示
        private void input_MouseLeave(object sender, EventArgs e)
        {            
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
