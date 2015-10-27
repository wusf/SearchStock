using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string _server = "192.168.193.139";
        public string server
        { 
            get
            { 
               return _server; 
            }
            set
            {  
                _server = value;
            }
        }
        private string _username = "geniusfi";
        public string username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }
        private string _database = "Pgenius";
        public string database
        {
            get
            {
                return _database;
            }
            set
            {
                _database = value;
            }
        }
        private string _pwd = "Qt2013";
        public string pwd
        {
            get
            {
                return _pwd;
            }
            set
            {
                _pwd = value;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public List<string> positions = new List<string>();

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
                StreamReader sr = new StreamReader(this.openFileDialog1.FileName);

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("Ticker");
                dt1.Columns.Add("Qty");
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string[] data = sr.ReadLine().Split('\t');
                    DataRow dr = dt1.NewRow();
                    dr[0] = data[0];
                    dr[1] = data[1];
                    dt1.Rows.Add(dr);
                    positions.Add(data[0]);
                }
                this.dataGridView1.DataSource = dt1;

            }
            this.propertyGrid1.SelectedObject = this;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] date = this.textBox2.Text.Split(',');
            string _date = '\''+date[0]+'\'';
            foreach (string s in date)
            {
                _date+=(','+"'"+s+"'");
            }

            string connStr = "server=192.168.193.139;database=Pgenius;uid=geniusfi;pwd=Qt2013";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            string sqlStr = string.Format(@"SELECT STK_COM_PROFILE.ISSUE_SECS,REG_DATE FROM STK_DIV_INFO
                                            LEFT JOIN STK_COM_PROFILE 
                                            ON STK_DIV_INFO.COMCODE=STK_COM_PROFILE.COMCODE
                                            WHERE REG_DATE IN ({0})", _date);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sqlStr;
            SqlDataReader sqlRd = cmd.ExecuteReader();

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Ticker");
            dt2.Columns.Add("RegistDay");
            while (sqlRd.Read())
            {
                string stockInfo = (string)sqlRd[0];
                string stockID = stockInfo.Split(new Char[] { '：', '(' })[1];
                if (positions.Contains(stockID))
                {
                    DataRow dr = dt2.NewRow();
                    dr[0] = stockID;
                    dr[1] = Convert.ToDateTime(sqlRd[1]).ToString("yyyyMMdd");
                    dt2.Rows.Add(dr);
                }
            }
            this.dataGridView2.DataSource = dt2;
            

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
