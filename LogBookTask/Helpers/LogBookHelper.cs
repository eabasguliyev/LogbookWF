using System;
using System.Collections.Generic;
using System.Drawing;
using LogBookTask.Entities;

namespace LogBookTask.Helpers
{
    public static class LogBookHelper
    {
        public static List<Teacher> GetTeachers()
        {
            var teachers = new List<Teacher>()
            {
                new Teacher()
                {
                    FirstName = "Elvin",
                    LastName = "Camalzade",
                    FatherName = "",
                    Username = "elvin_ca",
                    Password = "elvin1999",
                    Email = "elvin.camalzada@gmail.com",
                    BirthDate = new DateTime(1999, 1, 1),
                },
                new Teacher()
                {
                    FirstName = "Agil",
                    LastName = "Mammadzada",
                    FatherName = "",
                    Username = "agil_ma",
                    Password = "agil1988",
                    Email = "agil.mammadzada@gmail.com",
                    BirthDate = new DateTime(1988, 1, 1),
                },
            };

            return teachers;
        }

        public static List<Student> GetStudents()
        {
            var students = new List<Student>()
            {
                new Student()
                {
                    FirstName = "Resul",
                    LastName = "Osmanli",
                    FatherName = "Ferhad",
                    Username = "resul_os",
                    Password = "resul2001",
                    Email = "resul.osmanli@gmail.com",
                    BirthDate = new DateTime(2001, 1, 1),
                    UserLastLogin = "15.03.21",
                },
                new Student()
                {
                    FirstName = "Elgun",
                    LastName = "Abasquliyev",
                    FatherName = "Fiday",
                    Username = "elgun_ab",
                    Password = "elgun2000",
                    Email = "elgun.abasquliyev@gmail.com",
                    BirthDate = new DateTime(2001, 1, 1),
                    UserLastLogin = "15.03.21",
                },
                new Student()
                {
                    FirstName = "Arifali",
                    LastName = "Bagirli",
                    FatherName = "Azad",
                    Username = "arifali_ba",
                    Password = "arifali2003",
                    Email = "arifali.bagirli@gmail.com",
                    BirthDate = new DateTime(2001, 1, 1),
                    UserLastLogin = "15.03.21",
                },
                new Student()
                {
                    FirstName = "Zaur",
                    LastName = "Ceferov",
                    FatherName = "Ceyhun",
                    Username = "zaur_ce",
                    Password = "zaur2003",
                    Email = "zaur.ceferov@gmail.com",
                    BirthDate = new DateTime(2001, 1, 1),
                    UserLastLogin = "15.03.21",
                },
            };

            foreach (var student in students)
            {
                if (student.UserImage == null)
                    continue;
                student.ImageBytes = ImageHelper.ConvertImageToBytes(student.UserImage);
            }

            return students;
        }
    }
}