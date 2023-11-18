using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sprout.Exam.DataAccess.Repository.Models
{
    public class Employee
    {
        [Required]
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string TIN { get; set; }
        public int EmployeeTypeId { get; set; }
        public decimal Salary { get; set; }
        public bool IsDeleted { get; set; }
    }
}
