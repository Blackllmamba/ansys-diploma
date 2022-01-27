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
    public partial class SupportPoints : Form
    {
        public SupportPoints()
        {
            InitializeComponent();
        }

        private void SupportPoints_Load(object sender, EventArgs e)
        {
            numericUpDown1.Maximum = Data.count;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (Data.Op.ContainsKey(Convert.ToInt32(numericUpDown1.Value)))
            {
                Data.Op.Remove(Convert.ToInt32(numericUpDown1.Value));
                Data.info = "             *Для точки " + Convert.ToInt32(numericUpDown1.Value) + " \n опорная точка была удалена";
            }
            // добавить опору в точке
            if (checkBox1.Checked && !checkBox2.Checked /*проверка на x без y*/)
            {
                Data.Op.Add(Convert.ToInt32(numericUpDown1.Value), "DK,P51X , ,0 , , 0, UX, , , , , , ");
                Data.info = "             *Для точки " + Convert.ToInt32(numericUpDown1.Value) + " \n задана опорная точка по 0X";
            }
            else if (!checkBox1.Checked && checkBox2.Checked/*проверка на y без x*/)
            {
                Data.Op.Add(Convert.ToInt32(numericUpDown1.Value), "DK,P51X , ,0 , , 0, UY, , , , , , ");
                Data.info = "             *Для точки " + Convert.ToInt32(numericUpDown1.Value) + " \n задана опорная точка по 0Y";
            }
            else if (checkBox1.Checked && checkBox2.Checked/*проверка на x c y*/)
            {
                Data.Op.Add(Convert.ToInt32(numericUpDown1.Value), "DK,P51X , ,0 , , 0, ALL, , , , , , ");
                Data.info = "             *Для точки " + Convert.ToInt32(numericUpDown1.Value) + " \n задана опорная точка по 0X и 0Y";
            }
            else
            {
                MessageBox.Show("Точка опоры удалена или не существует!");
            }
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
            this.Close();
            this.Dispose();
        }
        private void Button2_Click(object sender, EventArgs e)
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
