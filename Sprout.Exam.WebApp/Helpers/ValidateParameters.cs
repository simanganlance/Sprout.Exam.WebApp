using Sprout.Exam.Common.Model;
using System.Reflection.Metadata;
using Sprout.Exam.Common.Constants;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.DataAccess.Repository.Models;
using Sprout.Exam.DataAccess.Extensions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System;

namespace Sprout.Exam.WebApp.Helpers
{
    public static class ValidateParameters
    {
        public static Response ValidateCalculateParameters(decimal absentDays, decimal workedDays) {
            try
            {
                if (absentDays > Constants.DaysPerMonth)
                    return new Response() { IsSuccess = false, Message = "Days of absence have surpassed the limit" };
                if (absentDays < 0)
                    return new Response() { IsSuccess = false, Message = "Days of absence should not be negative" };
                if (workedDays < 0)
                    return new Response() { IsSuccess = false, Message = "Days of worked should not be negative" };
                
            }
            catch (Exception e) {
                return new Response() { IsSuccess = false, Message = $"Error whwn validating calculate paramters {e.Message}" };
            }
            return new Response() { IsSuccess = true };
        }

        public static Response ValidateCreateEmployeeParameters(CreateEmployeeDto dto)
        {
            var employeeDto = new EmployeeDto();
            return ValidateEmployeeParameters(employeeDto.CopyToEmployeeDto(dto));
        }

        public static Response ValidateEditEmployeeParameters(EditEmployeeDto dto)
        {

           var employeeDto = new EmployeeDto();
            return ValidateEmployeeParameters(employeeDto.CopyToEmployeeDto(dto));
        }

        public static Response ValidateEmployeeParameters(EmployeeDto employee)
        {
            try
            {

                if (string.IsNullOrEmpty(employee.FullName))
                {
                    return new Response() { IsSuccess = false, Message = "FullName is required" };
                }

               

                if (!DateTime.TryParse(employee.Birthdate.ToLongDateString(), out DateTime tempDoB))
                {
                    return new Response() { IsSuccess = false, Message = "Invalid Birthdate format" };
                }

                if (!double.TryParse(employee.Salary.ToString(), NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out double money))
                {
                    return new Response() { IsSuccess = false, Message = "Invalid Salary ammount" };
                }


            }
            catch (Exception e)
            {
                return new Response() { IsSuccess = false, Message = $"Error whwn validating calculate paramters {e.Message}" };
            }

            return new Response() { IsSuccess = true };
        }
    }
}
