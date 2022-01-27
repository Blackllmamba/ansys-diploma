using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptimizatorBalka
{
    public partial class MainForm : Form
    {
        Calc calc = new Calc();
        public MainForm()
        {
            InitializeComponent();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            // Задать точки опоры
            this.Hide();
            SupportPoints supportPoints = new SupportPoints();
            supportPoints.ShowDialog();
            this.Close();
            this.Dispose();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // Задать силы и моменты
            this.Hide();
            MomentsForces momentsForces = new MomentsForces();
            momentsForces.ShowDialog();
            this.Close();
            this.Dispose();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            // Задать нагрузки
            if (InfoLbl.ForeColor == Color.Red)
            {
                MessageBox.Show("Необходимо задать ключевые точки!");
            }
            else
            {
                this.Hide();
                Stress stress = new Stress();
                stress.ShowDialog();
                this.Close();
                this.Dispose();
            }
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            double L = Convert.ToDouble(textBox1.Text); // длина балки
            int c = Convert.ToInt32(NumPlotsTxt.Value); // кол-во точек на балке
            string E = "";
            Data.Path.Clear();
            // получить результат
            switch (Data.K.Count)
            {
                case 0:
                    MessageBox.Show("Необходимо ввести ключевые точки");
                    break;
                default:
                    try
                    {
                        // выгрузить данные в командный файл
                        SaveFileDialog saveFile = new SaveFileDialog();
                        if (saveFile.ShowDialog() == DialogResult.OK && saveFile.FileName.Length > 0)
                        {
                            for (int numMat = 0; numMat < 3; numMat++)
                            {
                                using (StreamWriter sw = new StreamWriter(saveFile.FileName + "_" + numMat + ".lgw", true))
                                {
                                    switch (numMat)
                                    {
                                        case 0:
                                            // для стали 
                                            E = "*SET,E,2.1e11\n*SET,nu,0.28";
                                            break;
                                        case 1:
                                            // для титан
                                            E = "*SET,E,1.2e11\n*SET,nu,0.33";
                                            break;
                                        case 2:
                                            // для меди и для длины + приращение
                                            E = "*SET,E,0.7e11\n*SET,nu,0.34";
                                            break;

                                    }
                                    sw.WriteLine("*SET,L," + Convert.ToString(L).Replace(",", "."));
                                    sw.WriteLine("*SET,b," + textBox2.Text);
                                    sw.WriteLine("*SET,h," + textBox3.Text);
                                    sw.WriteLine(E);
                                    foreach (var k in Data.K)
                                    {
                                        sw.WriteLine(k.Value);
                                    }
                                    sw.WriteLine("/PREP7");
                                    sw.WriteLine("ET,1,BEAM3");
                                    sw.WriteLine("UIMP,1,EX, , ,E,");
                                    sw.WriteLine("UIMP,1,NUXY, , ,nu,");
                                    switch (comboBox1.Text)
                                    {
                                        case "Круглая":
                                            sw.WriteLine("R,1,b*h,b*(h**3)/12,h,10/9, , ,");
                                            break;
                                        case "Прямоугольная":
                                            sw.WriteLine("R,1,b*h,b*(h**3)/12,h,6/5, , ,");
                                            break;
                                        case "Тонкостенное кольцо":
                                            sw.WriteLine("R,1,b*h,b*(h**3)/12,h,2, , ,");
                                            break;
                                        case "Тонкостенный полый квадрат":
                                            sw.WriteLine("R,1,b*h,b*(h**3)/12,h,12/5, , ,");
                                            break;
                                        default:
                                            sw.WriteLine("R,1,b*h,b*(h**3)/12,h, , , ,");
                                            break;
                                    }
                                    for (double i = 0; i < c; i++)
                                    {
                                        sw.WriteLine("K," + (i + 1) + "," + Convert.ToString(L * (i / (c - 1))).Replace(",", ".") + ",0,0,");
                                    }
                                    for (int i = 1; i < Data.K.Count+1; i++)
                                    {
                                        sw.WriteLine("LSTR," + i + "," + (i + 1) + "");
                                    }
                                    foreach (var op in Data.Op)
                                    {
                                        sw.WriteLine("FLST, 2, 1, 3, ORDE, 1");
                                        sw.WriteLine("FITEM, 2, " + op.Key);
                                        sw.WriteLine(op.Value);
                                    }
                                    foreach (var fx in Data.FX)
                                    {
                                        sw.WriteLine(fx.Value);
                                    }
                                    foreach (var fy in Data.FY)
                                    {
                                        sw.WriteLine(fy.Value);
                                    }
                                    foreach (var m in Data.M)
                                    {
                                        sw.WriteLine(m.Value);
                                    }
                                    for (int i = 1; i <= Data.K.Count; i++)
                                    {
                                        sw.WriteLine("LESIZE, " + i + ", , , ne" + i + ", , , , , 1");
                                    }
                                    sw.WriteLine("LMESH,ALL");
                                    foreach (var q in Data.Q)
                                    {
                                        sw.WriteLine(q.Value);
                                    }
                                    sw.WriteLine("SBCTRAN");
                                    sw.WriteLine("FINISH");
                                    sw.WriteLine("/SOLU");
                                    sw.WriteLine("SOLVE");
                                    sw.WriteLine("FINISH");
                                    sw.WriteLine("/POST1");
                                    sw.WriteLine("SET,1,LAST,1,");
                                    sw.WriteLine("ETABLE,Q1,SMISC,2");
                                    sw.WriteLine("ETABLE,Q2,SMISC,8");
                                    sw.WriteLine("PLLS,Q1,Q2,-3");
                                    sw.WriteLine("ETABLE,M1,SMISC,6");
                                    sw.WriteLine("ETABLE,M2,SMISC,12");
                                    sw.WriteLine("PLLS,M1,M2,3");
                                    sw.WriteLine("PRNSOL,U,Y");
                                    sw.WriteLine("FINISH");
                                    sw.Close();
                                    string cmd = "ansys201.exe -b -i " + saveFile.FileName + "_" + numMat + ".lgw" + " -o " + @"C:\Users\Nikita\Desktop\Result_" + numMat + ".txt" + " -j Try" + numMat;
                                    Data.Path.Add(Convert.ToString(numMat), @"C:\Users\Nikita\Desktop\Result_" + numMat + ".txt");
                                    ProcessStartInfo program = new ProcessStartInfo()
                                    {
                                        UseShellExecute = true,
                                        WorkingDirectory = @"C:\Windows\System32",
                                        FileName = @"C:\Windows\System32\cmd.exe",
                                        Arguments = "/C " + cmd,
                                        WindowStyle = ProcessWindowStyle.Hidden
                                    };
                                    Process.Start(program);
                                    System.Threading.Thread.Sleep(10000);
                                }
                            }
                        }
                        else
                            return;
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.ToString());
                    }
                    break;
            }
            MessageBox.Show("Готово!");
            // переходим к получившимся результатам
            this.Hide();
            Results results = new Results();
            results.ShowDialog();
            this.Close();
            this.Dispose();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // загрузка главной формы
            switch (Data.K.Count)
            {
                case 0:
                    InfoLbl.Text = "* Необходимо задать разбиение\n         на конечные элементы";
                    InfoLbl.ForeColor = Color.Red;
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button5.Enabled = false;
                    break;
                default:
                    InfoLbl.Text = Data.info;
                    InfoLbl.ForeColor = Color.Green;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button5.Enabled = true;
                    break;
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            // показать содержимое словарей
            this.Hide();
            InfoTable infoTable = new InfoTable();
            infoTable.ShowDialog();
            this.Close();
            this.Dispose();
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // ввод только чисел в длину балки
            calc.InputTextBox(e, textBox1.Text);
        }

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // ввод только чисел в ширину балки
            calc.InputTextBox(e, textBox2.Text);
        }

        private void TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // ввод только чисел в высоту балки
            calc.InputTextBox(e, textBox3.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Задать разбиение на конечные элементы на участке
            Data.count = 0;
            Data.count = Convert.ToInt32(NumPlotsTxt.Value);
            this.Hide();
            Points points = new Points();
            points.ShowDialog();
            this.Close();
            this.Dispose();
        }
    }
}
