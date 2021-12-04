using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment3_n01486790.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int TeacherId { get; set; }
        public string TeacherFname { get; set; }
        public string TeacherLname { get; set; }
        public string StartDate { get; set; }
        public string FinishDate { get; set; }


    }
}