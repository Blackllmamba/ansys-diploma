using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptimizatorBalka
{
    public partial class Results : Form
    {
        public Results()
        {
            InitializeComponent();
        }

        private void Results_Load(object sender, EventArgs e)
        {
            double[] leftMass = new double[3] { 0, 0, 0};
            int[] rightMass = new int[3] { 0, 0, 0};
            foreach (var path in Data.Path)
            {
                switch (path.Key)
                {
                    case "0":
                        richTextBox1.Text += "Прогиб точек относительно оси 0X для Стали :\n";
                        break;
                    case "1":
                        richTextBox1.Text += "Прогиб точек относительно оси 0X для Титана:\n";
                        break;
                    case "2":
                        richTextBox1.Text += "Прогиб точек относительно оси 0X для Алюминия:\n";
                        break;

                }
                var importantLines = File.ReadLines(path.Value).
                    SkipWhile(line => !line.Contains("NODE       UY")).
                    TakeWhile(line => !line.Contains("EXIT THE ANSYS POST1 DATABASE PROCESSOR"));
                string[] res = importantLines.ToArray();
                string importantValues = res[res.Length - 2];
                string value = importantValues.Substring(7, importantValues.Length - 7);
                string[] results = value.Split('E');
                leftMass[Convert.ToInt32(path.Key)] = Convert.ToDouble(results[0].Replace(".", ","));
                rightMass[Convert.ToInt32(path.Key)] = Convert.ToInt32(results[1]);
                foreach (string str in res)
                {
                    richTextBox1.Text += str + "\n";
                }
            }
            int max = 1;
            for (int i = 0; i < 3; i++)
            {
                if (rightMass[i] < rightMass[max])
                {
                    max = i;
                    continue;
                }
                if (rightMass[i] == rightMass[max] && leftMass[i] > leftMass[max])
                {
                    max = i;
                    continue;
                }
            }
            switch (max)
            {
                case 0:
                    MessageBox.Show("Лучшее значение " + leftMass[max].ToString() + "E" + rightMass[max].ToString() + " при Стали ");

                    break;
                case 1:
                    MessageBox.Show("Лучшее значение " + leftMass[max].ToString() + "E" + rightMass[max].ToString() + " при Титане");

                    break;
                case 2:
                    MessageBox.Show("Лучшее значение " + leftMass[max].ToString() + "E" + rightMass[max].ToString() + "при Алюминии");

                    break;

            }
            // выгрузка получаемых значений
          /*  foreach (var path in Data.Path)
            {
                richTextBox1.Text += "Прогиб точек относительно оси 0X для '" + path.Key + "':\n";
                var importantLines = File.ReadLines(path.Value).SkipWhile(line => !line.Contains("NODE       UY")).TakeWhile(line => !line.Contains("MAXIMUM ABSOLUTE VALUES"));
                string[] res = importantLines.ToArray();
                foreach (string str in res)
                {
                    richTextBox1.Text += str + "\n";
                }
            }*/
            // удаление результирующих файлов
            foreach (var path in Data.Path)
            {
                File.Delete(path.Value);
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
