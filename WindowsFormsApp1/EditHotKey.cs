using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoClickerDD
{
    public partial class EditHotKey : Form
    {
        private KeyboardHook kh = new KeyboardHook(true);
        private static Keys key;
        private Keys newHotKey = Keys.None;
        public EditHotKey()
        {
            InitializeComponent();
            TopMost = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (newHotKey != Keys.None)
            {
                MainForm.hotKey = newHotKey;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Выберете клавишу");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private static void Kh_KeyDown(Keys Key, bool Shift, bool Ctrl, bool Alt)
        {
            key = Key;
        }

        async private Task chekKey()
        {
            while (true)
            {
                if (key != Keys.None)
                {
                    newHotKey = key;
                    key = Keys.None;
                    textBox1.Text = newHotKey.ToString();
                    kh.KeyDown -= Kh_KeyDown;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            kh.KeyDown += Kh_KeyDown;
            new Thread(async () => await chekKey()).Start();
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
        }
    }
}
