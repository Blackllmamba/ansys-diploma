using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptimizatorBalka
{
    public partial class Points : Form
    {
        public Points()
        {
            InitializeComponent();
        }

        private void Points_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Add("ne", "№");
            dataGridView1.Columns.Add("k", "Кол-во элементов");
            for (int i = 0; i < Data.count-1; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = "ne" + (i + 1).ToString();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // добавить конечные элементы
            Data.K.Clear();
            try
            {

                for (int i = 0; i < Convert.ToUInt32(Data.count) -1; i++)
                {
                    Data.K.Add(i + 1, "*SET,ne" + (i + 1) + "," + dataGridView1[1, i].Value.ToString());
                }
                Data.info = "* Конечные элементы\n      для отрезков заданы";
                this.Hide();
                MainForm mainForm = new MainForm();
                mainForm.ShowDialog();
                this.Close();
                this.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("Необходимо ввести кол-во конечных элементов для каждой ключевой точки на балке!");
                return;
            }
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
