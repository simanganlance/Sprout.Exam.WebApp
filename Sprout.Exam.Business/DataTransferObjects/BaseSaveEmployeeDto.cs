using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public abstract class BaseSaveEmployeeDto
    {
        public string FullName { get; set; }
        public string Tin { get; set; }
        public DateTime Birthdate { get; set; }
        public int TypeId { get; set; }
        public decimal Salary { get; set; }
        public bool IsDeleted { get; set; }
        public string FormattedBirthDate
        {
            get
            {
                return Birthdate.ToString("yyyy-MM-dd");
            }
        }
    }
}
