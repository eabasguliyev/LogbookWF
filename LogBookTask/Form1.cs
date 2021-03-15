using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using LogBookTask.Entities;
using LogBookTask.Helpers;

namespace LogBookTask
{
    public partial class Form1 : Form
    {
        private LogBook _logBook;

        public Form1()
        {
            InitializeComponent();

            _logBook = new LogBook();

            var newClass = new Class()
            {
                ClassName = "FSDA_3914_az",
                Students = LogBookHelper.GetStudents(),
                //Teachers = LogBookHelper.GetTeachers(),
            };

            _logBook.Classes.Add(newClass);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            DraggableForm.MouseDown(Cursor.Position, this.Location);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            var newLocation = DraggableForm.MouseMove();

            if (newLocation != Point.Empty)
                this.Location = newLocation;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            DraggableForm.MouseUp();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            //255, 31, 38
            //255, 51, 58
            Application.Exit();
        }

        private void CloseBtn_MouseEnter(object sender, EventArgs e)
        {
            CloseBtn.FillColor = Color.FromArgb(245, 0, 8);
        }

        private void CloseBtn_MouseLeave(object sender, EventArgs e)
        {
            CloseBtn.FillColor = Color.FromArgb(255, 92, 97);
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MinimizeBtn_MouseLeave(object sender, EventArgs e)
        {
            MinimizeBtn.FillColor = Color.FromArgb(169, 222, 84);
        }

        private void MinimizeBtn_MouseEnter(object sender, EventArgs e)
        {
            MinimizeBtn.FillColor = Color.FromArgb(118, 171, 33);
        }

        private void MinimizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void FillStudentsToUserPanels(List<Student> students)
        {
            if (students.Count == 0)
                return;

            for (int i = 0; i < students.Count; i++)
            {
                var student = students[i];
                var newUserPanel = CreateNewUserPanel(i);

                newUserPanel.Controls[$"NoLbl{i}"].Text = (i + 1).ToString();
                newUserPanel.Controls[$"UserFullnameLbl{i}"].Text =
                    $"{student.FirstName} {student.LastName} {student.FatherName}";
                newUserPanel.Controls[$"LastLoginDateLbl{i}"].Text = student.UserLastLogin;
            }
        }
        private Panel CreateNewUserPanel(int studentNo)
        {
            var newUserPanel = new Panel();
            newUserPanel.BackColor = Color.FromArgb(235, 249, 255);
            newUserPanel.Size = new Size(940, 45);
            newUserPanel.Location = new Point(0, 117 + studentNo * newUserPanel.Size.Height);
            newUserPanel.Name = $"UserPanel{studentNo}";

            this.Controls.Add(newUserPanel);

            var newStudentSeperatorPnl = new Panel();
            newStudentSeperatorPnl.BackColor = Color.FromArgb(16, 86, 127);
            newStudentSeperatorPnl.Size = new Size(941, 1);
            newStudentSeperatorPnl.Location = new Point(0, 0);
            newStudentSeperatorPnl.Name = $"StudentSeperatorPnl{studentNo}";

            newUserPanel.Controls.Add(newStudentSeperatorPnl);

            var newNoLbl = new Label();

            newNoLbl.Font = new Font("Microsoft Sans Serif", 9);
            newNoLbl.Location = new System.Drawing.Point(5, 15);
            newNoLbl.Name = $"NoLbl{studentNo}";
            newNoLbl.AutoSize = true;

            newUserPanel.Controls.Add(newNoLbl);

            var newUserImagePcBx = new PictureBox();

            newUserImagePcBx.Image = global::LogBookTask.Properties.Resources.userImage1;
            newUserImagePcBx.Location = new Point(20, 5);
            newUserImagePcBx.Name = $"UserImagePcBx{studentNo}";
            newUserImagePcBx.Size = new Size(35, 34);
            newUserImagePcBx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            newUserPanel.Controls.Add(newUserImagePcBx);

            var newUserFullNameLbl = new Label();

            newUserFullNameLbl.Font = new Font("Calibri", 11);
            newUserFullNameLbl.Location = new Point(59, 13);
            newUserFullNameLbl.Name = $"UserFullnameLbl{studentNo}";
            newUserFullNameLbl.AutoSize = true;

            newUserPanel.Controls.Add(newUserFullNameLbl);

            var newLastLoginDateLbl = new Label();

            newLastLoginDateLbl.Font = new Font("Calibri", 11);
            newLastLoginDateLbl.Location = new Point(260, 13);
            newLastLoginDateLbl.Name = $"LastLoginDateLbl{studentNo}";
            newLastLoginDateLbl.AutoSize = true;

            newUserPanel.Controls.Add(newLastLoginDateLbl);

            var newRecordPanel = new Panel();
            newRecordPanel.BackColor = Color.FromArgb(235, 249, 255);
            newRecordPanel.Size = new Size(76, 32);
            newRecordPanel.Location = new Point(356, 6);
            newRecordPanel.Name = $"RecordPanel{studentNo}";

            newUserPanel.Controls.Add(newRecordPanel);

            // record radio buttons

            var newAttendedRdBtn = new Guna2CustomRadioButton();
            newAttendedRdBtn.CheckedState.BorderColor = Color.FromArgb(34, 224, 0);
            newAttendedRdBtn.CheckedState.FillColor = Color.FromArgb(34, 224, 0);
            newAttendedRdBtn.CheckedState.InnerColor = Color.FromArgb(34, 224, 0);
            newAttendedRdBtn.UncheckedState.BorderColor = Color.FromArgb(34, 224, 0);
            newAttendedRdBtn.Location = new Point(13, 8);
            newAttendedRdBtn.Size = new Size(15, 16);
            newAttendedRdBtn.Name = $"AttendedRdBtn{studentNo}";

            newRecordPanel.Controls.Add(newAttendedRdBtn);

            var newNotAttendedRdBtn = new Guna2CustomRadioButton();
            newNotAttendedRdBtn.CheckedState.BorderColor = Color.FromArgb(255, 31, 31);
            newNotAttendedRdBtn.CheckedState.FillColor = Color.FromArgb(255, 31, 31);
            newNotAttendedRdBtn.CheckedState.InnerColor = Color.FromArgb(255, 31, 31);
            newNotAttendedRdBtn.UncheckedState.BorderColor = Color.FromArgb(255, 31, 31);
            newNotAttendedRdBtn.Location = new Point(52, 8);
            newNotAttendedRdBtn.Size = new Size(15, 16);
            newNotAttendedRdBtn.Name = $"NotAttendedRdBtn{studentNo}";

            newRecordPanel.Controls.Add(newNotAttendedRdBtn);

            var newPermittedRdBtn = new Guna2CustomRadioButton();
            newPermittedRdBtn.CheckedState.BorderColor = Color.FromArgb(255,246,0);
            newPermittedRdBtn.CheckedState.FillColor = Color.FromArgb(255, 246, 0);
            newPermittedRdBtn.CheckedState.InnerColor = Color.FromArgb(255, 246, 0);
            newPermittedRdBtn.UncheckedState.BorderColor = Color.FromArgb(255, 246, 0);
            newPermittedRdBtn.Location = new Point(32, 8);
            newPermittedRdBtn.Size = new Size(15, 16);
            newPermittedRdBtn.Name = $"PermittedRdBtn{studentNo}";

            newRecordPanel.Controls.Add(newPermittedRdBtn);

            var newAssignmentCmBx = new Guna2ComboBox();
            newAssignmentCmBx.BorderColor = Color.FromArgb(196, 0, 245);
            newAssignmentCmBx.ForeColor = Color.FromArgb(196, 0, 245);
            newAssignmentCmBx.Name = $"AssignmentCmBx{studentNo}";
            newAssignmentCmBx.Location = new Point(460, 5);
            newAssignmentCmBx.Size = new Size(68, 20);
            newAssignmentCmBx.Items.AddRange(new [] {
                "-",
                "12",
                "11",
                "10",
                "9",
                "8",
                "7",
                "6",
                "5",
                "4",
                "3",
                "2",
                "1"});

            newUserPanel.Controls.Add(newAssignmentCmBx);

            var newClassWorkCmBx = new Guna2ComboBox();
            newClassWorkCmBx.BorderColor = Color.FromArgb(143, 255, 31);
            newClassWorkCmBx.ForeColor = Color.FromArgb(112, 224, 0);
            newClassWorkCmBx.Name = $"ClassWorkCmBx{studentNo}";
            newClassWorkCmBx.Location = new Point(554, 5);
            newClassWorkCmBx.Size = new Size(68, 31);
            newClassWorkCmBx.Items.AddRange(new[] {
                "-",
                "12",
                "11",
                "10",
                "9",
                "8",
                "7",
                "6",
                "5",
                "4",
                "3",
                "2",
                "1"});

            newUserPanel.Controls.Add(newClassWorkCmBx);

            // crystall images

            var newCrystallsPanel = new Panel();
            newCrystallsPanel.BackColor = Color.FromArgb(235, 249, 255);
            newCrystallsPanel.Size = new Size(139, 39);
            newCrystallsPanel.Location = new Point(645, 2);
            newCrystallsPanel.Name = $"CrystallsPanel{studentNo}";

            newUserPanel.Controls.Add(newCrystallsPanel);

            var newOneCrystallBtn = new PictureBox();
            newOneCrystallBtn.Image = global::LogBookTask.Properties.Resources.crystalImage;
            newOneCrystallBtn.Location = new Point(0, 3);
            newOneCrystallBtn.Name = $"OneCrystallBtn{studentNo}";
            newOneCrystallBtn.Size = new Size(33, 32);
            newOneCrystallBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            newCrystallsPanel.Controls.Add(newOneCrystallBtn);

            var newTwoCrystallBtn = new PictureBox();
            newTwoCrystallBtn.Image = global::LogBookTask.Properties.Resources.crystalImage;
            newTwoCrystallBtn.Location = new Point(33, 3);
            newTwoCrystallBtn.Name = $"TwoCrystallBtn{studentNo}";
            newTwoCrystallBtn.Size = new Size(33, 32);
            newTwoCrystallBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            newCrystallsPanel.Controls.Add(newTwoCrystallBtn);

            var newThreeCrystallBtn = new PictureBox();
            newThreeCrystallBtn.Image = global::LogBookTask.Properties.Resources.crystalImage;
            newThreeCrystallBtn.Location = new Point(66, 3);
            newThreeCrystallBtn.Name = $"ThreeCrystallBtn{studentNo}";
            newThreeCrystallBtn.Size = new Size(33, 32);
            newThreeCrystallBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            newCrystallsPanel.Controls.Add(newThreeCrystallBtn);

            var newClearCrystallsBtn = new PictureBox();
            newClearCrystallsBtn.Image = global::LogBookTask.Properties.Resources.xImage;
            newClearCrystallsBtn.Location = new Point(99, 3);
            newClearCrystallsBtn.Name = $"ClearCrystallsBtn{studentNo}";
            newClearCrystallsBtn.Size = new Size(33, 32);
            newClearCrystallsBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            newCrystallsPanel.Controls.Add(newClearCrystallsBtn);

            var newWriteCommentBtn = new PictureBox();
            newWriteCommentBtn.Image = global::LogBookTask.Properties.Resources.feedbackImage;
            newWriteCommentBtn.Location = new Point(842, 6);
            newWriteCommentBtn.Name = $"ClearCrystallsBtn{studentNo}";
            newWriteCommentBtn.Size = new Size(33, 32);
            newWriteCommentBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            newUserPanel.Controls.Add(newWriteCommentBtn);

            return newUserPanel;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //CreateNewUserPanel(1);
            //MessageBox.Show($"{WriteCommentBtn0.Location}");
            //MessageBox.Show($"{WriteCommentBtn0.Size}");

            FillStudentsToUserPanels(_logBook.Classes[0].Students);
        }
    }
}
