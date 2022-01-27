using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptimizatorBalka
{
    public partial class MomentsForces : Form
    {
        Calc calc = new Calc();
        public MomentsForces()
        {
            InitializeComponent();
        }

        private void MomentsForces_Load(object sender, EventArgs e)
        {
            numericUpDown1.Maximum = Data.count;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBox1.Text, out value))
            { // добавление силы или момента
                int num = Convert.ToInt32(numericUpDown1.Value);
                // добавление силы или момента
                if (radioButton1.Checked)
                {
                    if (Data.FX.ContainsKey(num))
                    {
                        Data.FX.Remove(num);
                        Data.info = "             *Для точки " + num + " сила по OX";
                    }
                    Data.FX.Add(num, "FK," + num + ",FX ," + value);
                    Data.info = "             *Для точки " + num + " задана сила по 0X";
                }
                else if (radioButton2.Checked)
                {
                    if (Data.FY.ContainsKey(num))
                    {
                        Data.FY.Remove(num);
                        Data.info = "             *Для точки " + num + " сила по OY";
                    }
                    Data.FY.Add(num, "FK," + num + ",FY ," + value);
                    Data.info = "             *Для точки " + num + " задана сила по FY";
                }
                else if (radioButton3.Checked)
                {
                    if (Data.M.ContainsKey(num))
                    {
                        Data.M.Remove(num);
                        Data.info = "             *Для точки " + num + " момент";
                    }
                    Data.M.Add(num, "FK," + num + ",MZ ," + value);
                    Data.info = "             *Для точки " + num + " момент ";
                }
                else if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked)
                {
                    MessageBox.Show("Не указано действие");
                }
                this.Hide();
                MainForm mainForm = new MainForm();
                mainForm.ShowDialog();
                this.Close();
                this.Dispose();
            }
            else
                MessageBox.Show("Введено не число!");


        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // ввод только чисел в величину Силы или Момента
            calc.InputTextBox(e, textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // вернуться на главную форму
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
            this.Close();
            this.Dispose();
        }
    }
}
