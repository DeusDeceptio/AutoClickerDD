using System;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace AutoClickerDD
{
    internal partial class MainForm : Form
    {
        //делегат для эмитации кликов 
        private delegate void MousClick();
        private MousClick mousClick;

        private delegate void MouseButtonDown();
        private MouseButtonDown mousDown;
        private delegate void MouseButtonUp();
        private MouseButtonUp mousUp;

        //Горячая клавиша вкл\выкл 
        internal static Keys hotKey = Keys.F6;
        private KeyboardHook kh = new KeyboardHook(true);

        //Активность кликера (true - работает, false - не работает)
        private bool _ = false;

        //использование координат клика
        private bool coordinates = false;

        public MainForm()
        {
            TopMost = true;
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            kh.KeyDown += Kh_KeyDown;
            mousClick = MouseInterface.LeftClick;
            mousDown += MouseInterface.LeftDown;
            mousUp += MouseInterface.LeftUp;
        }

        //получение нажатой клавиши
        private void Kh_KeyDown(Keys Key, bool Shift, bool Ctrl, bool Alt)
        {
            if (Key == hotKey)
            {
                chengeSost();
            }
        }

        //вкл\выкл кликера кнопками формы
        private void button13_Click(object sender, EventArgs e)
        {
            chengeSost();
        }

        //кликер
        async private Task click()
        {
            int i = textBox1.Text != "" ? Convert.ToInt32(textBox1.Text) : 100;
            textBox1.Text = i.ToString();
            switch (tabControl1.SelectedIndex)
            { 
                case 0:
                    if (coordinates)
                    {
                        Point coordClick = new Point(Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text));
                        while (_)
                        {
                            Point oldCoord = new Point(MouseInterface.Position.X, MouseInterface.Position.Y);
                            MouseInterface.MoveTo(coordClick.X, coordClick.Y);
                            mousClick();
                            MouseInterface.MoveTo(oldCoord.X, oldCoord.Y);
                            Thread.Sleep(i);
                        }
                    }
                    else
                    {
                        while (_)
                        {
                            mousClick();
                            Thread.Sleep(i);
                        }
                    }
                    break;
                case 1:
                    if (mousDown != null & mousUp != null)
                        checkBox3.Checked = true;
                    int j = textBox11.Text != "" ? Convert.ToInt32(textBox11.Text) : 100;
                    textBox11.Text = j.ToString();
                    if (coordinates)
                    {
                        Point coordClick = new Point(Convert.ToInt32(textBox6.Text), Convert.ToInt32(textBox5.Text));
                        while (_)
                        {
                            MouseInterface.MoveTo(coordClick.X, coordClick.Y);
                            mousDown();
                            if (j != 0)
                            {
                                Thread.Sleep(j);
                                mousUp();
                            }
                            Thread.Sleep(i);
                        }
                    }
                    else
                    {
                        while (_)
                        {                                    
                            mousDown();
                            if (j != 0)
                            {
                                Thread.Sleep(j);
                                mousUp();
                            }
                            Thread.Sleep(i);
                        }
                    }
                    break; 
                case 2:
                    if (mousDown != null & mousUp != null)
                        checkBox8.Checked = true;
                    int k = textBox12.Text != "" ? Convert.ToInt32(textBox12.Text) : 100;
                    textBox12.Text = k.ToString();
                    Point coordStart = new Point(Convert.ToInt32(textBox8.Text), Convert.ToInt32(textBox7.Text));
                    Point coordEnd = new Point(Convert.ToInt32(textBox10.Text), Convert.ToInt32(textBox9.Text));
                    while (_)
                    {
                        MouseInterface.MoveTo(coordStart.X, coordStart.Y);
                        mousDown();
                        MouseInterface.SmoothMoveTo(coordEnd.X, coordEnd.Y, k);
                        mousUp();
                        Thread.Sleep(i);
                    }
                    break;
            };
        }

        //изменение активности кликера
        private void chengeSost()
        {
            if (!_)
            {
                _ = true;
                new Thread(async () => await click()).Start();
                label4.Text = "Состояние: активен";
            }
            else
            {
                _ = false;
                label4.Text = "Состояние: не активен";
            }
            groupBox4.Enabled = !_;
            button1.Enabled = !_;
            textBox1.Enabled = !_;
            button3.Enabled = _;
            tabControl1.Enabled = !_;
        }
        
        //Изменение горячей клавиши
        private void button2_Click(object sender, EventArgs e)
        {
            EditHotKey editHotKey = new EditHotKey();
            this.Hide();
            _ = false;
            kh.KeyDown -= Kh_KeyDown;
            if (editHotKey.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = hotKey.ToString();
            }
            kh.KeyDown += Kh_KeyDown;
            this.Show();
        }
        
        //изменение закрепления окна поверх остальных
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = checkBox1.Checked;
        }

        //закрытие потоков при закрытии программы
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        //изменение клавиши мыши          
        private void radioButton123_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                mousClick = MouseInterface.LeftClick;
                if (checkBox2.Checked)
                {
                    mousClick += MouseInterface.LeftClick;
                }
                return;
            }
            if (radioButton2.Checked)
            {
                mousClick = MouseInterface.RightClick;
                if (checkBox2.Checked)
                {
                    mousClick += MouseInterface.RightClick;
                }
                return;
            }
            if (radioButton3.Checked)
            {
                mousClick = MouseInterface.MiddleClick;
                if (checkBox2.Checked)
                {
                    mousClick += MouseInterface.MiddleClick;
                }
                return;
            }
        }
        
        //начало записи координат при клике мышью
        private void button456_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            MouseHook.MouseAction += new EventHandler(EventMous1);
            MouseHook.Start();
        }

        //запись координат курсора
        private void EventMous1(object sender, EventArgs e)
        {
            MouseHook.stop();
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    textBox3.Text = Cursor.Position.X.ToString();
                    textBox4.Text = Cursor.Position.Y.ToString();
                    break;
                case 1:
                    textBox6.Text = Cursor.Position.X.ToString();
                    textBox5.Text = Cursor.Position.Y.ToString();
                    break; 
                case 2:
                    textBox8.Text = Cursor.Position.X.ToString();
                    textBox7.Text = Cursor.Position.Y.ToString();
                    break;
            };
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            MouseHook.MouseAction -= new EventHandler(EventMous1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Enabled = false;
            MouseHook.MouseAction += new EventHandler(EventMous2);
            MouseHook.Start();
        }

        private void EventMous2(object sender, EventArgs e)
        {
            MouseHook.stop();
            textBox10.Text = Cursor.Position.X.ToString();
            textBox9.Text = Cursor.Position.Y.ToString();
            button7.Enabled = true;
            MouseHook.MouseAction -= new EventHandler(EventMous2);
        }

        //включить использование координат
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            coordinates = true;
        }

        //выключить использование координат
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            coordinates = false;
        }

        //использование даблклика
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton123_Click(null, new EventArgs());
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            mousDown -= MouseInterface.LeftDown;
            mousUp -= MouseInterface.LeftUp;
            mousDown -= MouseInterface.RightDown;
            mousUp -= MouseInterface.RightUp;
            mousDown -= MouseInterface.MiddleDown;
            mousUp -= MouseInterface.MiddleUp;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    break; 
                case 1:
                    checkBox8.Checked = true;
                    checkBox678_CheckedChanged(null, new EventArgs());
                    break; 
                case 2:
                    checkBox3.Checked = true;
                    checkBox345_CheckedChanged(null, new EventArgs());
                    break;
            };
        }
        private void checkBox345_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                mousDown += MouseInterface.LeftDown;
                mousUp += MouseInterface.LeftUp;
            }
            else
            {
                mousDown -= MouseInterface.LeftDown;
                mousUp -= MouseInterface.LeftUp;
            }
            if (checkBox4.Checked)
            {
                mousDown += MouseInterface.RightDown;
                mousUp += MouseInterface.RightUp;
            }
            else
            {
                mousDown -= MouseInterface.RightDown;
                mousUp -= MouseInterface.RightUp;
            }
            if (checkBox5.Checked)
            {
                mousDown += MouseInterface.MiddleDown;
                mousUp += MouseInterface.MiddleUp;
            }
            else
            {
                mousDown -= MouseInterface.MiddleDown;
                mousUp -= MouseInterface.MiddleUp;
            }
        }

        private void checkBox678_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                mousDown += MouseInterface.LeftDown;
                mousUp += MouseInterface.LeftUp;
            }
            else
            {
                mousDown -= MouseInterface.LeftDown;
                mousUp -= MouseInterface.LeftUp;
            }
            if (checkBox7.Checked)
            {
                mousDown += MouseInterface.RightDown;
                mousUp += MouseInterface.RightUp;
            }
            else
            {
                mousDown -= MouseInterface.RightDown;
                mousUp -= MouseInterface.RightUp;
            }
            if (checkBox6.Checked)
            {
                mousDown += MouseInterface.MiddleDown;
                mousUp += MouseInterface.MiddleUp;
            }
            else
            {
                mousDown -= MouseInterface.MiddleDown;
                mousUp -= MouseInterface.MiddleUp;
            }
        }

        //проверка написания цифр и backspace в поле задержки
        private void KeyNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8) //цифры, клавиша BackSpace а ASCII
            {
                e.Handled = true;
            }
        }
    }
}
