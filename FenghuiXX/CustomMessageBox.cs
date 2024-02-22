using System;
using System.Drawing;
using System.Windows.Forms;

namespace FenghuiXX
{
    class CustomMessageBox : Form
    {
        private Label label;

        public CustomMessageBox(string message)
        {
            label = new Label();
            label.Text = message;
            label.Font = new Font("Arial", 16); // 设置字体大小为16
            label.AutoSize = true;
            label.Location = new Point((ClientSize.Width - label.Width) / 2, (ClientSize.Height - label.Height) / 2);
            this.Controls.Add(label);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.Width = label.Width + 20; // 设置对话框宽度
            this.Height = label.Height + 40; // 设置对话框高度
        }

        public static void Show(string message)
        {
            using (CustomMessageBox customMessageBox = new CustomMessageBox(message))
            {
                customMessageBox.ShowDialog();
            }
        }

    }
}
