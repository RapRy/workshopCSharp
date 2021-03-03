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
using MySql.Data.MySqlClient;

namespace studentSolutions
{
    public partial class UpdateDeleteStudentForm : Form
    {
        public UpdateDeleteStudentForm()
        {
            InitializeComponent();
        }

        STUDENT student = new STUDENT();

        private void buttonUploadImage_Click(object sender, EventArgs e)
        {
            // browse image from your computer
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Image(*.jpg;*.png;*gif)|*.jpg;*.png;*.gif";

            if (opf.ShowDialog() == DialogResult.OK)
            {
                pictureBoxStudentImage.Image = Image.FromFile(opf.FileName);
            }
        }

        // create a function to verify data
        bool verif()
        {
            if ((textBoxFname.Text.Trim() == "") ||
                (textBoxLname.Text.Trim() == "") ||
                (textBoxPhone.Text.Trim() == "") ||
                (textBoxAddress.Text.Trim() == "") ||
                (pictureBoxStudentImage.Image == null))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void buttonEditStudent_Click(object sender, EventArgs e)
        {
            // update the selected student
            try
            {
                int id = Convert.ToInt32(textBoxID.Text);
                string fname = textBoxFname.Text;
                string lname = textBoxLname.Text;
                DateTime bdate = dateTimePicker1.Value;
                string phone = textBoxPhone.Text;
                string address = textBoxAddress.Text;
                string gender = "Male";

                if (radioButtonFemale.Checked)
                {
                    gender = "Female";
                }

                MemoryStream pic = new MemoryStream();

                // we need to check the age of the student
                // the student age must be between 10 - 100
                int born_year = dateTimePicker1.Value.Year;
                int this_year = DateTime.Now.Year;

                if (((this_year - born_year) < 10) || ((this_year - born_year) > 10))
                {
                    MessageBox.Show("The Student Age Must Be Between 10 and 100 Year", "Invalid Birth Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (verif())
                {
                    pictureBoxStudentImage.Image.Save(pic, pictureBoxStudentImage.Image.RawFormat);

                    if (student.updateStudent(id, fname, lname, bdate, phone, gender, address, pic))
                    {
                        MessageBox.Show("Student Information Updated", "Edit Student ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error", "Edit Student ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Empty Fields", "Edit Student ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch
            {
                MessageBox.Show("Please Enter a Valid Student Id", "Edit student", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            // remove the selected student
            try
            {
                int id = Convert.ToInt32(textBoxID.Text);
                // show a confirmation message before deleting the student
                if (MessageBox.Show("Are you sure you want to delete this Student", "Delete Student", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    if (student.deleteStudent(id))
                    {
                        MessageBox.Show("Student Deleted", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // clear fields
                        textBoxID.Text = "";
                        textBoxFname.Text = "";
                        textBoxLname.Text = "";
                        textBoxPhone.Text = "";
                        textBoxAddress.Text = "";
                        dateTimePicker1.Value = DateTime.Now;
                        pictureBoxStudentImage.Image = null;
                    }
                    else
                    {
                        MessageBox.Show("Student Not Deleted", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Please Enter a Valid Student Id", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            // search student by id
            try
            {
                int id = Convert.ToInt32(textBoxID.Text);
                MySqlCommand command = new MySqlCommand("SELECT `id`, `first_name`, `last_name`, `birthdate`, `gender`, `phone`, `address`, `picture` FROM `student` WHERE `id` =" + id);

                DataTable table = student.getStudents(command);

                if (table.Rows.Count > 0)
                {
                    textBoxFname.Text = table.Rows[0]["first_name"].ToString();
                    textBoxLname.Text = table.Rows[0]["last_name"].ToString();
                    textBoxPhone.Text = table.Rows[0]["phone"].ToString();
                    textBoxAddress.Text = table.Rows[0]["address"].ToString();

                    dateTimePicker1.Value = (DateTime)table.Rows[0]["birthdate"];

                    // gender
                    if (table.Rows[0]["gender"].ToString() == "Female")
                    {
                        radioButtonFemale.Checked = true;
                    }
                    else
                    {
                        radioButtonMale.Checked = true;
                    }

                    //image
                    byte[] pic = (byte[])table.Rows[0]["picture"];
                    MemoryStream picture = new MemoryStream(pic);
                    pictureBoxStudentImage.Image = Image.FromStream(picture);
                }
            }
            catch
            {
                MessageBox.Show("Enter a Valid Student Id", "Invalid Id", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // allow only number on key press
        private void textBoxID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
