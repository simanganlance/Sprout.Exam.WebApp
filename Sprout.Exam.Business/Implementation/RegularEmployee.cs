using Sprout.Exam.Business.Interfaces;
using Sprout.Exam.Common.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.Implementation
{
    public class RegularEmployee : IEmployee
    {
        public decimal CalculateSalary(decimal monthlySalary, decimal days)
        {
            return Math.Round(monthlySalary - (days * GetDailyDeducation(monthlySalary)) - RegularTaxDeducation(monthlySalary), 2);
        }

        public static decimal GetDailyDeducation(decimal monthlySalary)
        {

            return monthlySalary != 0 ? monthlySalary / Constants.DaysPerMonth : 0;
        }

        public static decimal RegularTaxDeducation(decimal monthlySalary)
        {

            return monthlySalary != 0 ? monthlySalary * Constants.RegularTaxDeduction : 0;
        }
    }
}
