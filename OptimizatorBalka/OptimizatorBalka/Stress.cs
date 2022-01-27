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
    public partial class Stress : Form
    {
        Calc calc = new Calc();
        public Stress()
        {
            InitializeComponent();
        }

        private void Stress_Load(object sender, EventArgs e)
        {
            numericUpDown2.Maximum = Data.count - 1;
            numericUpDown3.Maximum = Data.count;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // добавить нагрузку
            int value;
            if (int.TryParse(textBox1.Text, out value))
            {
                int num = Convert.ToInt32(numericUpDown1.Value);
                int begin = Convert.ToInt32(numericUpDown2.Value);
                int end = Convert.ToInt32(numericUpDown3.Value);
                if (Data.Q.ContainsKey(num))
                {
                    Data.Q.Remove(num);
                    Data.info = "             *Для участка " + begin + " - " + end + " нагрузка";
                }

                string str = "";
                str += "LSEL,S, , ," + begin;
                for (int i = begin + 1; i < end; i++)
                {
                    str += "\nLSEL,A, , ," + i;
                }
                str += "\nESLL,S \nSFBEAM,ALL,1,PRES," + textBox1.Text + ", , , , , , ";
                Data.Q.Add(num, str);
                Data.info = "             *Для участка " + begin + " - " + end + " задана нагрузка";
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
            // ввод только числа для величины нагрузки
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
