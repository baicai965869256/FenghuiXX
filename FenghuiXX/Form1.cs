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
        //��ȡexceel ��ȡ����һ��(ͼƬid+row)��·��
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
        // Ҫ���ɶ�ά�����������!!!!!!!!!!!
        //string tmpxx = "12a202299 ";
  

        // 111----ѡ��excel �ϴ�
        private void uploadExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                excelPath = openDlg.FileName;
            }
            //ͨ��·�� ��ȡexcel sheet1
            kvNameMapsEnd = Utils.ExcelToDatatable(@excelPath, true);
            int kvNameMapsNums = kvNameMapsEnd.Keys.Count;
            if (kvNameMapsNums == 0)
            {              
                MessageBox.Show("ֻ��Excel�ļ�! \r\n�رձ����ٶ�ȡ����!", "��ʾ!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //MessageBox.Show("ֻ��Excel�ļ�! \r\n�رձ����ٶ�ȡ����!");
                this.textBox1.Text = "��ѡ��Excel�ļ�! ";
                this.textBox2.Text = "";
                uploadExcel.Text = "ѡ����";
                return;
            }
            // MessageBox.Show("����ȡ����" + kvNameMapsNums + " ������ !");

            //uploadExcel �ϴ��������
            uploadExcel.Text = "�������:";
            // ��ʾexcel ·��            
            this.textBox1.Text = Utils.gendPath(excelPath);
            this.textBox2.Text = "�Ѷ�" + kvNameMapsNums + "������";

        }
        // ���� �����绰
        private void input_TextChanged(object sender, EventArgs e)
        {
     /*       inputString=input.Text;
            String value;
            if (!String.IsNullOrEmpty(inputString)) {
                if (kvNameMapsEnd.TryGetValue("apple", out value))
                {
                    MessageBox.Show("�ҵ���"+ value, "��ʾ!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else
                {
                    MessageBox.Show("�Ҳ������ݣ�", "��ʾ!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }*/
        }
        //2---��ť�� ��ѯ��Ա��ά����Ϣ
        private void button1_Click(object sender, EventArgs e)
        {
            //����input �����绰 ��6
            inputString = input.Text;
        
            String value = "";
            if (kvNameMapsEnd.Count == 0) {
                MessageBox.Show("����ѡ���� !" + value, "��ʾ!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!String.IsNullOrEmpty(inputString))
            {
                if (kvNameMapsEnd.TryGetValue(inputString, out value))
                {
                    
                    MessageBox.Show("��ѯ���: "+inputString+"\r\n�Ա�: " + Utils.getGenderORPhong(value, 0)+"\r\n�ֻ���: "+ Utils.getGenderORPhong(value, 1) );
                    erWeiMa_Load(inputString+value);
                }
                else
                {
                    MessageBox.Show("��δ�ҵ����� !", "��ʾ!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else {
                MessageBox.Show("�������������ֻ���!", "��ʾ!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        //111��Ϣ���ɶ�ά�� 
        private void erWeiMa_Load(String erPic)
        {
            // ������ά�����
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Width = pictureBox1.Width, // ���ö�ά������PictureBox�ؼ���ͬ
                    Height = pictureBox1.Height, // ���ö�ά��߶���PictureBox�ؼ���ͬ
                    Margin = 1, // ���ö�ά��߾�
                    ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.H // ���þ�����Ϊ��߼���
                }
            };
            // Ĭ�ϱ����ʽ������
          /*  Encoding encoding = Encoding.Default;
            String name = encoding.BodyName;*/

            // gb2312�ı�ת-----utf8����
            // String utf8String = gb2312turnUtf8(tmpxx);
            //Boolean b = isNotUTF8(utf8String);

            byte[] utf8Bytes = Encoding.Default.GetBytes(erPic); // ��ȡĬ�ϱ�����ֽ�����
            string utf8String = Encoding.UTF8.GetString(utf8Bytes); // ���ֽ�����ת��ΪUTF-8�ַ���   

            // ���ɶ�ά��ͼƬ
            Bitmap qrCodeImage = writer.Write(utf8String);

            // ����ά��ͼƬ����ΪPictureBox�ؼ���Image����
            pictureBox1.Image = qrCodeImage;
        }

        //����� ��ά�룡
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
            pictureBox1.Image = flag;// ͼƬ��ʾ ��������
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


        private string Notes = "�������ֻ�(����λ)";
        private void input_Enter(object sender, EventArgs e)
        {
            //  �����ý��㣬���
            if (input.Text == Notes)
            {
                input.ForeColor = Color.Black;
                this.input.Text = "";
            }
        }

        private void input_MouseLeave(object sender, EventArgs e)
        {
            //  �˳�ʧȥ���㣬������ʾ
            if (string.IsNullOrEmpty(input.Text))
            {
                input.ForeColor = Color.DarkGray;
                this.input.Text = Notes;
            }
        }
    }

}
