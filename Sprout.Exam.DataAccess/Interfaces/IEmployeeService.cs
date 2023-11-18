using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.DataAccess.Interfaces
{
    public interface IEmployeeService
    {
        Task<Response> AddEmployee(CreateEmployeeDto dto);
        Task<EmployeeDto> UpdateEmployee(EditEmployeeDto dto);
        Task<Response> DeletEmployee(int id);
        Task<EmployeeDto> GetEmployeeById(int id);
        Task<Response> CheckIfEmployeeExist(CreateEmployeeDto dto);
        Task<Response> CheckIfTinExist(string tin);
        Task<List<EmployeeDto>> GetAllEmployee();
    }
}
