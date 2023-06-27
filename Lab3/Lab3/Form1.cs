using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {
        private int rowCount = 0;
        private int colNum = 14;
        private bool toggle = false;
        private bool CellEditable = true;
        private char[] rule = new char[8];
        private String[] patterns = new string[] { "111", "110", "101", "100", "011", "010", "001", "000" };

        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < colNum; i++)
            {
                dataGridView1.Columns.Add("", "");
            }

            rowCount = 1;
            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].HeaderCell.Value = rowCount.ToString();

            setRule(Convert.ToString((int)numRule.Value, 2));
        }

        private void Rule_ValueChanged(object sender, EventArgs e)
        {
            setRule(Convert.ToString((int)numRule.Value, 2));
        }

        private void setRule(string input)
        {
            int len = input.Length;
            while (len < 8)
            {
                input = "0" + input;
                len = input.Length;
            }
            for (int i = 0; i < input.Length; i++)
            {
                rule[i] = input[i];
            }
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            CellEditable = false;
            if (toggle)
            {
                toggle = false;
                timer1.Stop();
            }
            else
            {
                toggle = true;
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            char[] prevRow = new char[colNum];
            char[] newRow = new char[colNum];

            for (int i = 0; i < colNum; i++)
            {
                if (dataGridView1[i, rowCount - 1].Style.BackColor == Color.Black) prevRow[i] = '1';
                else prevRow[i] = '0';
            }

            for (int i = 0; i < colNum; i++)
            {
                string pattern = "";
                int prev = i - 1;
                int next = i + 1;
                var builder = new StringBuilder();

                if (i == 0) prev = colNum - 1;
                if (i == colNum - 1) next = 0;

                builder.Append(prevRow[prev]);
                builder.Append(prevRow[i]);
                builder.Append(prevRow[next]);

                pattern = builder.ToString();

                int index = Array.IndexOf(patterns, pattern);
                newRow[i] = rule[index];
            }

            rowCount = rowCount + 1;
            dataGridView1.Rows.Add();
            dataGridView1.Rows[rowCount - 1].HeaderCell.Value = rowCount.ToString();

            for (int i = 0; i < colNum; i++)
            {
                if (newRow[i] == '1') dataGridView1[i, rowCount - 1].Style.BackColor = Color.Black;
                else dataGridView1[i, rowCount - 1].Style.BackColor = Color.White;
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (CellEditable)
            {
                if (dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor == Color.White)
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Black;
                    dataGridView1.ClearSelection();
                }
                else
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
                    dataGridView1.ClearSelection();
                }
            }
        }
    }
}
