using Sprout.Exam.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.Implementation
{
    public class ContractualEmployee : IEmployee
    {
        public decimal CalculateSalary(decimal monthlySalary, decimal days)
        {
            return Math.Round(monthlySalary * days, 2);
        }
    }
}
