using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.DataAccess.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.DataAccess.Extensions
{
    public static class EmployeeExtensions
    {
        public static Employee CopyFromDto(this Employee employee, CreateEmployeeDto dto) {
            employee.FullName = dto.FullName;
            employee.Birthdate = dto.Birthdate;
            employee.TIN = dto.Tin;
            employee.EmployeeTypeId = dto.TypeId;
            employee.IsDeleted = dto.IsDeleted;
            employee.Salary = dto.Salary;

            return employee;
        }

        public static Employee CopyFromDto(this Employee employee, EditEmployeeDto dto)
        {
            employee.Id = dto.Id;
            employee.FullName = dto.FullName;
            employee.Birthdate = dto.Birthdate;
            employee.TIN = dto.Tin;
            employee.EmployeeTypeId = dto.TypeId;
            employee.IsDeleted = dto.IsDeleted;
            employee.Salary = dto.Salary;

            return employee;
        }

        public static Employee CopyFromDto(this Employee employee, EmployeeDto dto)
        {
            employee.Id = dto.Id;
            employee.FullName = dto.FullName;
            employee.Birthdate = dto.Birthdate;
            employee.TIN = dto.Tin;
            employee.EmployeeTypeId = dto.TypeId;
            employee.IsDeleted = dto.IsDeleted;
            employee.Salary = dto.Salary;

            return employee;
        }

        public static EmployeeDto CopyToEmployeeDto(this EmployeeDto employee, CreateEmployeeDto dto)
        {
            employee.FullName = dto.FullName;
            employee.Birthdate = dto.Birthdate;
            employee.Tin = dto.Tin;
            employee.TypeId = dto.TypeId;
            employee.IsDeleted = dto.IsDeleted;
            employee.Salary = dto.Salary;

            return employee;
        }
        public static EmployeeDto CopyToEmployeeDto(this EmployeeDto employee, EditEmployeeDto dto)
        {
            employee.Id = dto.Id;
            employee.FullName = dto.FullName;
            employee.Birthdate = dto.Birthdate;
            employee.Tin = dto.Tin;
            employee.TypeId = dto.TypeId;
            employee.IsDeleted = dto.IsDeleted;
            employee.Salary = dto.Salary;

            return employee;
        }
    }
}
