using Sprout.Exam.Business.Implementation;
using Sprout.Exam.Business.Interfaces;
using Sprout.Exam.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.Factory
{
    public class EmployeeFactory
    {
        public IEmployee CreateEmployee(EmployeeType type)
        {
            switch (type)
            {
                case EmployeeType.Regular:
                    return new RegularEmployee();
                case EmployeeType.Contractual:
                    return new ContractualEmployee();
                default:
                    throw new ArgumentException("Invalid employee type");
            }
        }
    }
}
