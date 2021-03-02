using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace studentSolutions
{
    public partial class studentsListForm : Form
    {
        public studentsListForm()
        {
            InitializeComponent();
        }

        STUDENT student = new STUDENT();

        private void studentsListForm_Load(object sender, EventArgs e)
        {
            // populate the datagridview with students data
            MySqlCommand command = new MySqlCommand("SELECT * FROM `student`");
            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = student.getStudents(command);
            // column 7 is the image column index
            picCol = (DataGridViewImageColumn) dataGridView1.Columns[7];
            picCol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            // display the selected student in a new form to edit/remove
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            // refresh the datagridview data
        }
    }
}
