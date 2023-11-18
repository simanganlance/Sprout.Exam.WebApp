using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Helpers;
using Microsoft.Extensions.Logging;
using Sprout.Exam.DataAccess.Interfaces;
using Sprout.Exam.DataAccess.Implementation;
using Humanizer;
using Sprout.Exam.Common.Model;
using Sprout.Exam.Business.Factory;
using Sprout.Exam.WebApp.Models;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IEmployeeService _employeeService;
        public EmployeesController(ILogger<EmployeesController> logger, IEmployeeService employeeService) {
            _logger = logger;
            _employeeService = employeeService;
        }


        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _employeeService.GetAllEmployee();
                if (result == null) return NotFound();

                return Ok(result);
            }
            catch (Exception e)
            {

                _logger.LogError($"Error at GetById: {e.Message}");
                return BadRequest("Error during GetById");
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _employeeService.GetEmployeeById(id);
                if (result == null) return NotFound();

                return Ok(result);
            }
            catch(Exception e) {

                _logger.LogError($"Error at GetById: {e.Message}");
                return BadRequest("Error during GetById");
            }

               
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            try {
                var isValid = ValidateParameters.ValidateEditEmployeeParameters(input);
                if (!isValid.IsSuccess)
                    return BadRequest(isValid.Message);
                var updatedEmployee = await _employeeService.UpdateEmployee(input);

                return Ok(updatedEmployee);
            }
            catch(Exception e)
            {
                _logger.LogError($"Error at Update: {e.Message}");
                return BadRequest("Error during Employee Update");

            }
            
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {
            try {
                var isValid = ValidateParameters.ValidateCreateEmployeeParameters(input);
                if (!isValid.IsSuccess)
                    return BadRequest(isValid.Message);

                var employeeExist = await _employeeService.CheckIfEmployeeExist(input);
                if (!employeeExist.IsSuccess)
                    return BadRequest("Employee already exist");

                var tinExist = await _employeeService.CheckIfTinExist(input.Tin);
                if (!tinExist.IsSuccess)
                    return BadRequest("Tin already exist");

                var newEmployee = await _employeeService.AddEmployee(input);

                return Created($"/api/employees/{newEmployee.Id}", newEmployee.Id);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error at Create: {e.Message}");
                return BadRequest("Error during Employee Creation");
            }
            
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _employeeService.GetEmployeeById(id);

                if (result == null) return NotFound();
                await _employeeService.DeletEmployee(id);
                return Ok(id);
            }
            catch (Exception e) {
                _logger.LogError($"Error at Calculate: {e.Message}");
                return BadRequest("Error during Employee Deletion");
            }
            
        }



        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(int id,[FromBody] CalculateParameter calculateParameter)
        {
            try
            {
                    
                var result = await _employeeService.GetEmployeeById(id);

                if (result == null) return NotFound();

                var isValid =ValidateParameters.ValidateCalculateParameters(calculateParameter.AbsentDays, calculateParameter.WorkedDays);
                if (!isValid.IsSuccess)
                    return BadRequest(isValid.Message);
                var type = (EmployeeType) result.TypeId;

                var factory = new EmployeeFactory();
                var employee = factory.CreateEmployee(type);
                switch (type) {
                    case EmployeeType.Regular:
                        return Ok(employee.CalculateSalary(result.Salary, calculateParameter.AbsentDays));
                    case EmployeeType.Contractual:
                        return Ok(employee.CalculateSalary(result.Salary, calculateParameter.WorkedDays));
                    default:
                        return NotFound();
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError($"Error at Calculate: {e.Message}");
                return BadRequest("Error during Calculation");
            }
        }

    }
}
