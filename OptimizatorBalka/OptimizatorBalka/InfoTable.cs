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
    public partial class InfoTable : Form
    {
        public InfoTable()
        {
            InitializeComponent();
        }
        private void ComboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "Конечные элементы":
                    dataGridView1.DataSource = Data.K.ToArray();
                    break;
                case "Точки опоры":
                    dataGridView1.DataSource = Data.Op.ToArray();
                    break;
                case "Силы по OX":
                    dataGridView1.DataSource = Data.FX.ToArray();
                    break;
                case "Силы по OY":
                    dataGridView1.DataSource = Data.FY.ToArray();
                    break;
                case "Моменты":
                    dataGridView1.DataSource = Data.M.ToArray();
                    break;
                case "Нагрузки":
                    dataGridView1.DataSource = Data.Q.ToArray();
                    break;
                default:
                    break;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
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
