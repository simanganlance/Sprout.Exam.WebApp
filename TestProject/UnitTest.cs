using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.Factory;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.DataAccess.Repository.Models;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.DataAccess.Implementation;
using Sprout.Exam.WebApp.Data;
using Sprout.Exam.WebApp.DataAccessWrapper;
using Sprout.Exam.DataAccess.Repository.Interfaces;

namespace TestProject
{
    public class UnitTest
    {


        [Fact]
        public void TestCalculateRegularEmployee()
        {
            //Arrange
            decimal salary = 20000, absentDays = 1;


            //Action
            var factory = new EmployeeFactory();
            var employee = factory.CreateEmployee(EmployeeType.Regular);
            var result = employee.CalculateSalary(salary, absentDays);

            //Assert
            Assert.Equal(16690.91m, result);
        }

        [Fact]
        public void TestCalculateContractualEmployee()
        {
            //Arrange
            decimal salary = 500, workedDays = 15.5m;


            //Action
            var factory = new EmployeeFactory();
            var employee = factory.CreateEmployee(EmployeeType.Contractual);
            var result = employee.CalculateSalary(salary, workedDays);

            //Assert
            Assert.Equal(7750m, result);
        }


        [Fact]
        public async Task TestEmployeeParameter()
        {
            int nextId = 1;
            List<Employee> employeeList = new List<Employee> {
                new Employee { Id = 1, FullName= "Regular Employee", Birthdate= new DateTime(2000,1,1), Salary = 20000, EmployeeTypeId = 1, TIN= "11111111111111111",IsDeleted= false},
                new Employee { Id = 2, FullName= "Contactual Employee", Birthdate= new DateTime(2000,1,1), Salary = 500, EmployeeTypeId = 2, TIN= "11111111111111112",IsDeleted= false},
            };
            CreateEmployeeDto existingEmployee = new CreateEmployeeDto() { FullName = "Regular Employee", Birthdate = new DateTime(2000, 1, 1), Salary = 20000, TypeId = 1, Tin = "11111111111111111", IsDeleted = false };
            string existingTin = "11111111111111111";
            // Mocking the DbSet<Employee>
            var mockSet = new Mock<DbSet<Employee>>();
            mockSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employeeList.AsQueryable().Provider);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employeeList.AsQueryable().Expression);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employeeList.AsQueryable().ElementType);
            mockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(() => employeeList.AsQueryable().GetEnumerator());
            mockSet.As<IAsyncEnumerable<Employee>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
           .Returns(new TestAsyncEnumerator<Employee>(employeeList.GetEnumerator()));


            // Initialize the next ID
            mockSet.Setup(m => m.Add(It.IsAny<Employee>())).Callback((Employee employee) =>
            {
                // Assign an incremented ID to the added employee
                employee.Id = nextId++;
                employeeList.Add(employee);
            });
          
            mockSet.Setup(m => m.Remove(It.IsAny<Employee>())).Callback((Employee employee) => employeeList.Remove(employee));
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync((object[] ids) => employeeList.FirstOrDefault(p => p.Id == (int)ids[0]));


            var mockContext = new Mock<IDbContextWrapper>();
            mockContext.Setup(c => c.Employee).Returns(mockSet.Object);
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>();
            });
            var mapper = new Mapper(configuration);

            var loggerMock = new Mock<ILogger<EmployeeService>>();
            var employeeService = new EmployeeService(loggerMock.Object, mapper, mockContext.Object);

            await employeeService.AddEmployee(new CreateEmployeeDto { FullName = "Regular Employee", Birthdate = new DateTime(2000, 1, 1), Salary = 20000, TypeId = 1, Tin = "11111111111111111", IsDeleted = false });
            await employeeService.AddEmployee( new CreateEmployeeDto {  FullName = "Contactual Employee", Birthdate = new DateTime(2000, 1, 1), Salary = 500, TypeId = 2, Tin = "11111111111111112", IsDeleted = false });
            Assert.Equal(2,employeeList.Count);
            Assert.Equal(1, employeeList[0].Id);
            Assert.Equal(2, employeeList[1].Id);
        }
    }
}