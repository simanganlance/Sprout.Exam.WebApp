using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.Interfaces
{
    public interface IEmployee
    {
        decimal CalculateSalary(decimal monthlySalary, decimal days);

    }
}
