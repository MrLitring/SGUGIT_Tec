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
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ТП2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 2;
        }

        private void button1_Click(object sender, EventArgs e) //кнопка подключить
        {
            string filename;
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Текстовые файлы(*.txt)|*.txt" };
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            filename = openFileDialog.FileName;
            ReadFile(filename);
        }
        private double[,] Coords; // обьявляем двумерный массив
        public void ReadFile(string file)// чтение файла тхт
        {
            Coords = new double[15, 2];
            var sr = new StreamReader(file);
            string line;
            int point = 0;
            int ser = 1;

            while ((line = sr.ReadLine()) != null)
            {
                string[] splitLine = line.Split(' ');
                Coords[point, 0] = Convert.ToDouble(splitLine[0]);
                Coords[point, 1] = Convert.ToDouble(splitLine[1]);

                if (point % 5 == 0)
                {
                    string NameSerie = "Серия" + Convert.ToString(ser);
                    checkedListBox1.Items.Add(NameSerie);
                    ser++;
                }
                point++;
            }
        }

        public void CreateSerie(int NumSerie) // создание динамической среии
        {
            double x, y;
            string NameSerie = "Серия " + Convert.ToString(NumSerie + 1);
            chart1.Series.Add(new Series(NameSerie));
            chart1.Series[NameSerie].ChartType = (System.Windows.Forms.DataVisualization.Charting.SeriesChartType)4;
            chart1.Series[NameSerie].Enabled = true;
            chart1.Series[NameSerie].BorderWidth = 2;

            for (int p = 0; p < 5; p++)
            {
                x = Coords[p + NumSerie * 5, 0];
                y = Coords[p + NumSerie * 5, 1];
                chart1.Series[NameSerie].Points.AddXY(x, y);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                    CreateSerie(i);
            }
        }
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int var = comboBox1.SelectedIndex;
            if (var == 1) var = 3;
            else
            if (var == 2) var = 4;

            foreach (Series ser in chart1.Series)
                ser.ChartType = (System.Windows.Forms.DataVisualization.Charting.SeriesChartType)var;
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            button1.Location = new Point(splitContainer1.Panel1.Width / 2 - 60,
            splitContainer1.Panel1.Height / 20 - 8);
            label1.Location = new Point(splitContainer1.Panel1.Width / 2 - 67,
            splitContainer1.Panel1.Height / 10 + 9);
            checkedListBox1.Location = new Point(splitContainer1.Panel1.Width / 2 - 57,
            splitContainer1.Panel1.Height / 5 - 8);
            checkedListBox1.Height = splitContainer1.Panel1.Height / 2;
            button2.Location = new Point(splitContainer1.Panel1.Width / 2 - 62,
            splitContainer1.Panel1.Height - splitContainer1.Panel1.Height / 4 - 6);
            label2.Location = new Point(splitContainer1.Panel1.Width / 2 - 60,
            splitContainer1.Panel1.Height - splitContainer1.Panel1.Height / 5 + 13);
            comboBox1.Location = new Point(splitContainer1.Panel1.Width / 2 - 60,
            splitContainer1.Panel1.Height - splitContainer1.Panel1.Height / 5 + 40);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void textCheck(System.Windows.Forms.TextBox textBox)
        {
            if (textBox.Text != "")
            {
                if (int.Parse(textBox.Text) > 250)
                {
                    textBox.Text = "250";
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textCheck(textBox1);
            textCheck(textBox2);
            textCheck(textBox3);
            chart1.Series[chart1.Series.Count - 1].Color = Color.FromArgb(int.Parse(textBox1.Text), int.Parse(textBox2.Text), int.Parse(textBox3.Text));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                int i = 1;
                if (chart1.Series.Count != 0)
                {
                        if (int.Parse(chart1.Series[chart1.Series .Count - 1].Name.Split(' ')[1]) > checkedListBox1.Items.Count)
                            chart1.Series.Remove(chart1.Series[chart1.Series.Count - 1]);
                    i = checkedListBox1.Items.Count + 1;
                }


                int X = int.Parse(textBox6.Text);
                double x, y;
                string NameSerie = "Серия " + Convert.ToString(i);
                chart1.Series.Add(new Series(NameSerie));
                chart1.Series[NameSerie].ChartType = (System.Windows.Forms.DataVisualization.Charting.SeriesChartType)4;
                chart1.Series[NameSerie].Enabled = true;
                chart1.Series[NameSerie].BorderWidth = 2;

                for (double p = 0; p <= X; p += 0.1)
                {
                    x = p;
                    y = Math.Sin(p);
                    chart1.Series[NameSerie].Points.AddXY(x, y);
                }

                textBox1.Text = chart1.Series[NameSerie].Color.R.ToString();
                textBox2.Text = chart1.Series[NameSerie].Color.G.ToString();
                textBox3.Text = chart1.Series[NameSerie].Color.B.ToString();
            }
        }

        

    }
}
