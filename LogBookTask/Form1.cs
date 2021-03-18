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
using LogBookTask.Enums;
using LogBookTask.Helpers;

namespace LogBookTask
{
    public partial class Form1 : Form
    {
        private Class _class;
        private Lesson _lesson;

        private string _fileName;

        private UserPhoto _userPhoto;
        private bool _newLesson = false;
        public Form1()
        {
            InitializeComponent();

            UserPanel0.Dispose();
            UserImagePcBx0.Dispose();

            SubjectSaveToolTip.SetToolTip(SaveLessonSubjectBtn, "Save the subject of lesson");
            ProgramSaveToolTip.SetToolTip(SaveBtn, "Save data in json file");
            var fileName = "FSDA_3914_az.json";


            if (File.Exists(fileName))
            {
                _class = FileHelper.ReadClassFromJson(fileName);

                foreach (var student in _class.Students)
                {
                    if (student.ImageBytes == null)
                        continue;

                    student.UserImage = ImageHelper.ConvertBytesToImage(student.ImageBytes);
                }
            }

            if (_class == null)
            {
                var newClass = new Class()
                {
                    ClassName = "FSDA_3914_az",
                    Students = LogBookHelper.GetStudents(),
                    Teachers = LogBookHelper.GetTeachers(),
                };
                newClass.Lessons.Add("C# programming language");

                _class = newClass;
            }

            var now = DateTime.Now;
            _fileName = $"{now.Day}-{now.Month}-{now.Year}.json";

            if (File.Exists(_fileName))
            {
                _lesson = FileHelper.ReadFromJson(_fileName);
            }

            if (_lesson == null)
            {
                _lesson = new Lesson() { Date = DateTime.Now };
                _newLesson = true;
                foreach (var student in _class.Students)
                {
                    var newStudentRecord = new StudentRecord();

                    newStudentRecord.Student = student;

                    _lesson.StudentRecords.Add(newStudentRecord);
                }
            }
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
            if(MessageBox.Show("Are you sure to exit? If you exit, the information you have not saved will be lost.",
                "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) 
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

        private void FillStudentsToUserPanels(List<StudentRecord> studentRecords)
        {
            if (studentRecords.Count == 0)
                return;

            for (int i = 0; i < studentRecords.Count; i++)
            {
                var studentRecord = studentRecords[i];
                var newUserPanel = CreateNewUserPanel(i);
                
                newUserPanel.Controls[$"NoLbl{i}"].Text = (i + 1).ToString();
                newUserPanel.Controls[$"UserFullnameLbl{i}"].Text =
                    $"{studentRecord.Student.FirstName} {studentRecord.Student.LastName} {studentRecord.Student.FatherName}";
                newUserPanel.Controls[$"LastLoginDateLbl{i}"].Text = studentRecord.Student.UserLastLogin;


                Guna2CustomRadioButton rdBtn = null;
                switch (studentRecord.RecordType)
                {
                    case RecordType.Attended:
                    {
                        rdBtn = newUserPanel.Controls[$"RecordPanel{i}"].Controls[$"AttendedRdBtn{i}"] as Guna2CustomRadioButton;
                        rdBtn.Checked = true;
                        break;
                    }
                    case RecordType.Permitted:
                    {
                        rdBtn = newUserPanel.Controls[$"RecordPanel{i}"].Controls[$"PermittedRdBtn{i}"] as Guna2CustomRadioButton;
                        rdBtn.Checked = true;

                        break;
                    }
                    case RecordType.NotAttended:
                    {
                        rdBtn = newUserPanel.Controls[$"RecordPanel{i}"].Controls[$"NotAttendedRdBtn{i}"] as Guna2CustomRadioButton;
                        rdBtn.Checked = true;
                        break;
                    }
                }
                

                if (studentRecord.AssignmentPoint != 0)
                {
                    var assignmentCmBx = newUserPanel.Controls[$"AssignmentCmBx{i}"] as Guna2ComboBox;

                    assignmentCmBx.SelectedIndex = assignmentCmBx.Items.IndexOf(studentRecord.AssignmentPoint.ToString());
                }

                if (studentRecord.ClassWorkPoint != 0)
                {
                    var classWorkCmBx = newUserPanel.Controls[$"ClassWorkCmBx{i}"] as Guna2ComboBox;

                    classWorkCmBx.SelectedIndex = classWorkCmBx.Items.IndexOf(studentRecord.ClassWorkPoint.ToString());
                }

                if (studentRecord.Diamonds > 0)
                {
                    ChangeDiamondsImage(newUserPanel.Controls[$"CrystallsPanel{i}"].Controls, studentRecord.Diamonds - 1, true);
                }
            }
        }
        private Panel CreateNewUserPanel(int studentNo)
        {
            var newUserPanel = new Panel();
            newUserPanel.BackColor = Color.FromArgb(235, 249, 255);
            newUserPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            newUserPanel.Size = new Size(960, 45);
            newUserPanel.Location = new Point(0, studentNo * newUserPanel.Size.Height);
            newUserPanel.Name = $"UserPanel{studentNo}";
            newUserPanel.Tag = studentNo;

            this.UsersPanel.Controls.Add(newUserPanel);

            var newStudentSeperatorPnl = new Panel();
            newStudentSeperatorPnl.BackColor = Color.FromArgb(16, 86, 127);
            newStudentSeperatorPnl.Size = new Size(960, 1);
            newStudentSeperatorPnl.Location = new Point(0, 0);
            newStudentSeperatorPnl.Name = $"StudentSeperatorPnl{studentNo}";
            newStudentSeperatorPnl.Tag = studentNo;

            newUserPanel.Controls.Add(newStudentSeperatorPnl);

            var newNoLbl = new Label();

            newNoLbl.Font = new Font("Microsoft Sans Serif", 9);
            newNoLbl.Location = new System.Drawing.Point(5, 15);
            newNoLbl.Name = $"NoLbl{studentNo}";
            newNoLbl.AutoSize = true;
            newNoLbl.Tag = studentNo;

            newUserPanel.Controls.Add(newNoLbl);

            var newUserImagePcBx = new PictureBox();

            newUserImagePcBx.Image = global::LogBookTask.Properties.Resources.userImage1;
            newUserImagePcBx.Location = new Point(20, 5);
            newUserImagePcBx.Name = $"UserImagePcBx{studentNo}";
            newUserImagePcBx.Size = new Size(35, 34);
            newUserImagePcBx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            newUserImagePcBx.Tag = studentNo;
            newUserImagePcBx.MouseEnter += UserImagePcBx0_MouseEnter;
            newUserImagePcBx.MouseLeave += UserImagePcBx0_MouseLeave;

            newUserPanel.Controls.Add(newUserImagePcBx);

            var newUserFullNameLbl = new Label();

            newUserFullNameLbl.Font = new Font("Calibri", 11);
            newUserFullNameLbl.Location = new Point(59, 13);
            newUserFullNameLbl.Name = $"UserFullnameLbl{studentNo}";
            newUserFullNameLbl.AutoSize = true;
            newUserFullNameLbl.Tag = studentNo;

            newUserPanel.Controls.Add(newUserFullNameLbl);

            var newLastLoginDateLbl = new Label();

            newLastLoginDateLbl.Font = new Font("Calibri", 11);
            newLastLoginDateLbl.Location = new Point(260, 13);
            newLastLoginDateLbl.Name = $"LastLoginDateLbl{studentNo}";
            newLastLoginDateLbl.AutoSize = true;
            newLastLoginDateLbl.Tag = studentNo;

            newUserPanel.Controls.Add(newLastLoginDateLbl);

            var newRecordPanel = new Panel();
            newRecordPanel.BackColor = Color.FromArgb(235, 249, 255);
            newRecordPanel.Size = new Size(76, 32);
            newRecordPanel.Location = new Point(356, 6);
            newRecordPanel.Name = $"RecordPanel{studentNo}";
            newRecordPanel.Tag = studentNo;

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
            newAttendedRdBtn.Tag = studentNo;
            newAttendedRdBtn.CheckedChanged += AttendedRdBtn0_CheckedChanged;
            newAttendedRdBtn.MouseHover += AttendedRdBtn0_MouseHover;

            newRecordPanel.Controls.Add(newAttendedRdBtn);

            var newNotAttendedRdBtn = new Guna2CustomRadioButton();
            newNotAttendedRdBtn.CheckedState.BorderColor = Color.FromArgb(255, 31, 31);
            newNotAttendedRdBtn.CheckedState.FillColor = Color.FromArgb(255, 31, 31);
            newNotAttendedRdBtn.CheckedState.InnerColor = Color.FromArgb(255, 31, 31);
            newNotAttendedRdBtn.UncheckedState.BorderColor = Color.FromArgb(255, 31, 31);
            newNotAttendedRdBtn.Location = new Point(52, 8);
            newNotAttendedRdBtn.Size = new Size(15, 16);
            newNotAttendedRdBtn.Name = $"NotAttendedRdBtn{studentNo}";
            newNotAttendedRdBtn.Tag = studentNo;
            newNotAttendedRdBtn.CheckedChanged += NotAttendedRdBtn0_CheckedChanged;
            newNotAttendedRdBtn.MouseHover += NotAttendedRdBtn0_MouseHover;

            newRecordPanel.Controls.Add(newNotAttendedRdBtn);

            var newPermittedRdBtn = new Guna2CustomRadioButton();
            newPermittedRdBtn.CheckedState.BorderColor = Color.FromArgb(255,246,0);
            newPermittedRdBtn.CheckedState.FillColor = Color.FromArgb(255, 246, 0);
            newPermittedRdBtn.CheckedState.InnerColor = Color.FromArgb(255, 246, 0);
            newPermittedRdBtn.UncheckedState.BorderColor = Color.FromArgb(255, 246, 0);
            newPermittedRdBtn.Location = new Point(32, 8);
            newPermittedRdBtn.Size = new Size(15, 16);
            newPermittedRdBtn.Name = $"PermittedRdBtn{studentNo}";
            newPermittedRdBtn.Tag = studentNo;
            newPermittedRdBtn.CheckedChanged += PermittedRdBtn0_CheckedChanged;
            newPermittedRdBtn.MouseHover += PermittedRdBtn0_MouseHover;

            newRecordPanel.Controls.Add(newPermittedRdBtn);

            var newAssignmentCmBx = new Guna2ComboBox();
            newAssignmentCmBx.BorderColor = Color.FromArgb(196, 0, 245);
            newAssignmentCmBx.ForeColor = Color.FromArgb(196, 0, 245);
            newAssignmentCmBx.Name = $"AssignmentCmBx{studentNo}";
            newAssignmentCmBx.Location = new Point(460, 5);
            newAssignmentCmBx.Size = new Size(68, 20);
            newAssignmentCmBx.Tag = studentNo;
            newAssignmentCmBx.AutoRoundedCorners = true;
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
            newAssignmentCmBx.SelectedIndexChanged += AssignmentCmBx0_SelectedIndexChanged;
            newUserPanel.Controls.Add(newAssignmentCmBx);

            var newClassWorkCmBx = new Guna2ComboBox();
            newClassWorkCmBx.BorderColor = Color.FromArgb(143, 255, 31);
            newClassWorkCmBx.ForeColor = Color.FromArgb(112, 224, 0);
            newClassWorkCmBx.Name = $"ClassWorkCmBx{studentNo}";
            newClassWorkCmBx.Location = new Point(554, 5);
            newClassWorkCmBx.Size = new Size(68, 31);
            newClassWorkCmBx.Tag = studentNo;
            newClassWorkCmBx.AutoRoundedCorners = true;
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
            newClassWorkCmBx.SelectedIndexChanged += ClassWorkCmBx0_SelectedIndexChanged;

            newUserPanel.Controls.Add(newClassWorkCmBx);

            // crystall images

            var newCrystallsPanel = new Panel();
            newCrystallsPanel.BackColor = Color.FromArgb(235, 249, 255);
            newCrystallsPanel.Size = new Size(139, 39);
            newCrystallsPanel.Location = new Point(645, 2);
            newCrystallsPanel.Name = $"CrystallsPanel{studentNo}";
            newCrystallsPanel.Tag = studentNo;
            newUserPanel.Controls.Add(newCrystallsPanel);

            var newOneCrystallBtn = new PictureBox();
            newOneCrystallBtn.Image = global::LogBookTask.Properties.Resources.diamond_64px_uncolor;
            newOneCrystallBtn.Location = new Point(0, 3);
            newOneCrystallBtn.Name = $"OneCrystallBtn{studentNo}";
            newOneCrystallBtn.Size = new Size(33, 32);
            newOneCrystallBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            newOneCrystallBtn.Tag = studentNo;
            newOneCrystallBtn.Click += OneCrystallBtn0_Click;
            newCrystallsPanel.Controls.Add(newOneCrystallBtn);

            var newTwoCrystallBtn = new PictureBox();
            newTwoCrystallBtn.Image = global::LogBookTask.Properties.Resources.diamond_64px_uncolor;
            newTwoCrystallBtn.Location = new Point(33, 3);
            newTwoCrystallBtn.Name = $"TwoCrystallBtn{studentNo}";
            newTwoCrystallBtn.Size = new Size(33, 32);
            newTwoCrystallBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            newTwoCrystallBtn.Tag = studentNo;
            newTwoCrystallBtn.Click += TwoCrystallBtn0_Click;
            newCrystallsPanel.Controls.Add(newTwoCrystallBtn);

            var newThreeCrystallBtn = new PictureBox();
            newThreeCrystallBtn.Image = global::LogBookTask.Properties.Resources.diamond_64px_uncolor;
            newThreeCrystallBtn.Location = new Point(66, 3);
            newThreeCrystallBtn.Name = $"ThreeCrystallBtn{studentNo}";
            newThreeCrystallBtn.Size = new Size(33, 32);
            newThreeCrystallBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            newThreeCrystallBtn.Tag = studentNo;
            newThreeCrystallBtn.Click += ThreeCrystallBtn0_Click;
            newCrystallsPanel.Controls.Add(newThreeCrystallBtn);

            var newClearCrystallsBtn = new PictureBox();
            newClearCrystallsBtn.Image = global::LogBookTask.Properties.Resources.delete_32px;
            newClearCrystallsBtn.Location = new Point(99, 3);
            newClearCrystallsBtn.Name = $"ClearCrystallsBtn{studentNo}";
            newClearCrystallsBtn.Size = new Size(33, 32);
            newClearCrystallsBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            newClearCrystallsBtn.Tag = studentNo;
            newClearCrystallsBtn.Click += ClearCrystallsBtn0_Click;
            newClearCrystallsBtn.MouseHover += ClearCrystallsBtn0_MouseHover;
            newCrystallsPanel.Controls.Add(newClearCrystallsBtn);

            var newWriteCommentBtn = new PictureBox();
            newWriteCommentBtn.Image = global::LogBookTask.Properties.Resources.comments_40px;
            newWriteCommentBtn.Location = new Point(842, 6);
            newWriteCommentBtn.Name = $"ClearCrystallsBtn{studentNo}";
            newWriteCommentBtn.Size = new Size(33, 32);
            newWriteCommentBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            newWriteCommentBtn.Tag = studentNo;
            newWriteCommentBtn.Click += WriteCommentBtn0_Click;
            newWriteCommentBtn.MouseHover += WriteCommentBtn0_MouseHover;

            newUserPanel.Controls.Add(newWriteCommentBtn);

            return newUserPanel;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //CreateNewUserPanel(1);
            //MessageBox.Show($"{guna2CirclePictureBox1.Location}");
            //MessageBox.Show($"{guna2CirclePictureBox1.Size}");

            if (_lesson.TeacherType == TeacherType.MainTeacher)
            {
                MainTeacherRdBtn.Checked = true;
            }
            else if(_lesson.TeacherType == TeacherType.AnotherTeacher)
            {
                AnotherTeacherRdBtn.Checked = true;
            }

            if (!string.IsNullOrWhiteSpace(_lesson.Subject))
            {
                LessonSubjectTxtBx.Text = _lesson.Subject;
            }

            GroupNameLbl.Text = $"{_class.ClassName} - {_class.Lessons[0]}";
            DiamondsCountLbl.Text = _lesson.TotalDiamondCount.ToString();
            FillStudentsToUserPanels(_lesson.StudentRecords);

        }

        private void AttendedRdBtn0_CheckedChanged(object sender, EventArgs e)
        {
            var rdBtn = sender as Guna2CustomRadioButton;

            if (rdBtn.Checked)
            {
                var studentNo = (int)rdBtn.Tag;
                _class.Students[studentNo].Records[DateTime.Now.ToShortDateString()] = RecordType.Attended;
                _lesson.StudentRecords[studentNo].RecordType = RecordType.Attended;
                ChangeUserActivity(rdBtn.Parent.Parent.Controls, studentNo, true);
            }
        }

        private void PermittedRdBtn0_CheckedChanged(object sender, EventArgs e)
        {
            var rdBtn = sender as Guna2CustomRadioButton;

            if (rdBtn.Checked)
            {
                var studentNo = (int)rdBtn.Tag;
                _class.Students[studentNo].Records[DateTime.Now.ToShortDateString()] = RecordType.Permitted;
                _lesson.StudentRecords[studentNo].RecordType = RecordType.Permitted;
                ChangeUserActivity(rdBtn.Parent.Parent.Controls, studentNo, false);
            }
        }

        private void NotAttendedRdBtn0_CheckedChanged(object sender, EventArgs e)
        {
            var rdBtn = sender as Guna2CustomRadioButton;

            if (rdBtn.Checked)
            {
                var studentNo = (int)rdBtn.Tag;
                _class.Students[studentNo].Records[DateTime.Now.ToShortDateString()] = RecordType.NotAttended;
                _lesson.StudentRecords[studentNo].RecordType = RecordType.NotAttended;

                ChangeUserActivity(rdBtn.Parent.Parent.Controls, studentNo, false);
            }
        }

        private void ChangeUserActivity(Control.ControlCollection controls, int studentNo, bool status)
        {
            controls[$"AssignmentCmBx{studentNo}"].Enabled = status;
            controls[$"ClassWorkCmBx{studentNo}"].Enabled = status;
            controls[$"CrystallsPanel{studentNo}"].Enabled = status;
        }

        private void AssignmentCmBx0_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cmBx = sender as Guna2ComboBox;

            if (cmBx.SelectedIndex != 0)
            {
                var studentNo = (int)cmBx.Tag;

                _class.Students[studentNo].AssignmentPoints[DateTime.Now.ToShortDateString()] = Convert.ToInt32(cmBx.SelectedItem);
                _lesson.StudentRecords[studentNo].AssignmentPoint = Convert.ToInt32(cmBx.SelectedItem);
            }
        }

        private void ClassWorkCmBx0_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cmBx = sender as Guna2ComboBox;

            if (cmBx.SelectedIndex != 0)
            {
                var studentNo = (int)cmBx.Tag;
                var value = Convert.ToInt32(ClassWorkCmBx0.SelectedItem);
                
                _class.Students[studentNo].ClassWorkPoints[DateTime.Now.ToShortDateString()] = Convert.ToInt32(cmBx.SelectedItem);
                _lesson.StudentRecords[studentNo].ClassWorkPoint = Convert.ToInt32(cmBx.SelectedItem);
            }
        }

        private void SaveLessonSubjectBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(LessonSubjectTxtBx.Text))
                return;

            _class.Subjects[DateTime.Now.ToShortDateString()] = LessonSubjectTxtBx.Text;
            _lesson.Subject = LessonSubjectTxtBx.Text;

            MessageBox.Show("Subject saved", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MainTeacherRdBtn_CheckedChanged(object sender, EventArgs e)
        {
            if(MainTeacherRdBtn.Checked)
            {
                _lesson.TeacherType = TeacherType.MainTeacher;
                _lesson.Teacher = _class.Teachers[0];
                UsersPanel.Enabled = true;

                if (_newLesson)
                {
                    for (int i = 0; i < this.UsersPanel.Controls.Count; i++)
                    {
                        var userPanel = this.UsersPanel.Controls[$"UserPanel{i}"] as Panel;

                        ChangeUserActivity(userPanel.Controls, i, false);
                    }
                }
            }
        }

        private void AnotherTeacherRdBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (AnotherTeacherRdBtn.Checked)
            {
                _lesson.TeacherType = TeacherType.AnotherTeacher;
                _lesson.Teacher = _class.Teachers[1];
                UsersPanel.Enabled = true;
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            FileHelper.WriteToJson(_fileName, _lesson);
            FileHelper.WriteClassToJson($"{_class.ClassName}.json", _class);
            MessageBox.Show("Date saved to file.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OneCrystallBtn0_Click(object sender, EventArgs e)
        {
            var clickedOneCrystall = sender as PictureBox;
            var tag = (int)clickedOneCrystall.Tag;

            if (_lesson.StudentRecords[tag].Diamonds == 1)
                return;
            
            if (_lesson.StudentRecords[tag].Diamonds > 1)
            {
                ChangeDiamondsImage(clickedOneCrystall.Parent.Controls, 0, false);
                _lesson.TotalDiamondCount += _lesson.StudentRecords[tag].Diamonds;
                _class.Students[tag].Diamonds -= _lesson.StudentRecords[tag].Diamonds;
                _lesson.StudentRecords[tag].Diamonds = 1;
                _class.Students[tag].Diamonds += _lesson.StudentRecords[tag].Diamonds;
                _lesson.TotalDiamondCount -= _lesson.StudentRecords[tag].Diamonds;
            }
            else
            {
                if (_lesson.TotalDiamondCount > 0)
                {
                    ChangeDiamondsImage(clickedOneCrystall.Parent.Controls, 0, true);
                    _lesson.StudentRecords[tag].Diamonds = 1;
                    _class.Students[tag].Diamonds += _lesson.StudentRecords[tag].Diamonds;
                    _lesson.TotalDiamondCount -= _lesson.StudentRecords[tag].Diamonds;
                }
            }

            this.DiamondsCountLbl.Text = _lesson.TotalDiamondCount.ToString();
        }

        private void TwoCrystallBtn0_Click(object sender, EventArgs e)
        {
            var clickedTwoCrystall = sender as PictureBox;
            var tag = (int)clickedTwoCrystall.Tag;


            if (_lesson.StudentRecords[tag].Diamonds == 2)
                return;
                
            if (_lesson.StudentRecords[tag].Diamonds > 2)
            {
                ChangeDiamondsImage(clickedTwoCrystall.Parent.Controls, 1, false);
                _lesson.TotalDiamondCount += _lesson.StudentRecords[tag].Diamonds;
                _class.Students[tag].Diamonds -= _lesson.StudentRecords[tag].Diamonds;
                _lesson.StudentRecords[tag].Diamonds = 2;
                _class.Students[tag].Diamonds += _lesson.StudentRecords[tag].Diamonds;
                _lesson.TotalDiamondCount -= _lesson.StudentRecords[tag].Diamonds;
            }
            else
            {
                if (_lesson.StudentRecords[tag].Diamonds + _lesson.TotalDiamondCount < 2)
                {
                    return;
                }

                _lesson.TotalDiamondCount += _lesson.StudentRecords[tag].Diamonds;

                if (_lesson.TotalDiamondCount > 1)
                {
                    ChangeDiamondsImage(clickedTwoCrystall.Parent.Controls, 1, true);
                    _lesson.StudentRecords[tag].Diamonds = 2;
                    _class.Students[tag].Diamonds += _lesson.StudentRecords[tag].Diamonds;
                    _lesson.TotalDiamondCount -= _lesson.StudentRecords[tag].Diamonds;
                }
            }

            this.DiamondsCountLbl.Text = _lesson.TotalDiamondCount.ToString();
        }

        private void ChangeDiamondsImage(Control.ControlCollection controls, int index, bool isAscending)
        {
            if (isAscending)
            {
                for (int i = 0; i < index + 1; i++)
                {
                    var diamondImage = controls[i] as PictureBox;

                    diamondImage.Image = Properties.Resources.diamond_64px;
                }
            }
            else
            {
                for (int i = index + 1; i < controls.Count - 1; i++)
                {
                    var diamondImage = controls[i] as PictureBox;

                    diamondImage.Image = Properties.Resources.diamond_64px_uncolor;
                }
            }
        }

        private void ThreeCrystallBtn0_Click(object sender, EventArgs e)
        {
            var clickedThreeCrystall = sender as PictureBox;
            var tag = (int)clickedThreeCrystall.Tag;

            if (_lesson.StudentRecords[tag].Diamonds + _lesson.TotalDiamondCount < 3)
            {
                return;
            }

            _lesson.TotalDiamondCount += _lesson.StudentRecords[tag].Diamonds;

            if (_lesson.TotalDiamondCount > 2)
            {
                ChangeDiamondsImage(clickedThreeCrystall.Parent.Controls, 2, true);

                _lesson.StudentRecords[tag].Diamonds = 3;
                _class.Students[tag].Diamonds += _lesson.StudentRecords[tag].Diamonds;
                _lesson.TotalDiamondCount -= _lesson.StudentRecords[tag].Diamonds;
            }

            this.DiamondsCountLbl.Text = _lesson.TotalDiamondCount.ToString();
        }

        private void ClearCrystallsBtn0_Click(object sender, EventArgs e)
        {
            var clickedClearCrystalls = sender as PictureBox;
            var tag = (int)clickedClearCrystalls.Tag;

            if (_lesson.StudentRecords[tag].Diamonds == 0)
                return;

            ChangeDiamondsImage(clickedClearCrystalls.Parent.Controls, -1, false);

            _class.Students[tag].Diamonds -= _lesson.StudentRecords[tag].Diamonds;
            _lesson.TotalDiamondCount += _lesson.StudentRecords[tag].Diamonds;
            _lesson.StudentRecords[tag].Diamonds = 0;

            this.DiamondsCountLbl.Text = _lesson.TotalDiamondCount.ToString();
        }

        private void UserImagePcBx0_MouseEnter(object sender, EventArgs e)
        {
            var pcBx = sender as PictureBox;
            var userNo = (int)pcBx.Tag;
            _userPhoto = new UserPhoto();

            _userPhoto.BackgroundImage = _class.Students[userNo].UserImage;
            _userPhoto.BackgroundImageLayout = ImageLayout.Stretch;
            _userPhoto.StartPosition = FormStartPosition.Manual;

            _userPhoto.Location = new Point(Cursor.Position.X + 30, Cursor.Position.Y - 10);
            _userPhoto.Show();
        }

        private void UserImagePcBx0_MouseLeave(object sender, EventArgs e)
        {
            _userPhoto.Dispose();
        }

        private void WriteCommentBtn0_Click(object sender, EventArgs e)
        {
            var commentPcBx = sender as PictureBox;
            var userNo = (int)commentPcBx.Tag;
            var commentForm = new CommentForm();

            if (commentForm.ShowDialog() == DialogResult.OK)
            {
                _class.Students[userNo].Comments[DateTime.Now.ToShortDateString()] = commentForm.Comment;

                MessageBox.Show("Comment added", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void ClearCrystallsBtn0_MouseHover(object sender, EventArgs e)
        {
            var pcBx = sender as PictureBox;
            ClearDiamondsToolTip.SetToolTip(pcBx, "Clear diamonds of Student");
        }

        private void WriteCommentBtn0_MouseHover(object sender, EventArgs e)
        {
            var pcBx = sender as PictureBox;

            CommentToolTip.SetToolTip(pcBx, "Send comment to Student");
        }

        private void AttendedRdBtn0_MouseHover(object sender, EventArgs e)
        {
            var rdBtn = sender as Guna2CustomRadioButton;

            RecordToolTip.SetToolTip(rdBtn, "Student is attended");
        }

        private void PermittedRdBtn0_MouseHover(object sender, EventArgs e)
        {
            var rdBtn = sender as Guna2CustomRadioButton;

            RecordToolTip.SetToolTip(rdBtn, "Student is permitted");
        }

        private void NotAttendedRdBtn0_MouseHover(object sender, EventArgs e)
        {
            var rdBtn = sender as Guna2CustomRadioButton;

            RecordToolTip.SetToolTip(rdBtn, "Student is not attended");
        }
    }
}
