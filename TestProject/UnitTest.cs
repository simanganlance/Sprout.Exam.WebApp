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
using System.Linq.Expressions;

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
        public async Task AddEmployee_Success()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);

            var createEmployeeDto = new CreateEmployeeDto(); // Assuming you have this defined for test data

            dbContextWrapperMock.Setup(mock => mock.AddAsync(It.IsAny<Employee>())).ReturnsAsync(new Employee { Id = 1 }); // Mocking the AddAsync method to return a dummy Employee with ID

            // Act
            var result = await employeeService.AddEmployee(createEmployeeDto);

            // Assert
            Assert.True(result.IsSuccess); // Check if the operation was successful
            Assert.Equal(1, result.Id); // Check if the ID returned matches the expected ID
        }

        [Fact]
        public async Task AddEmployee_Failure()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);

            var createEmployeeDto = new CreateEmployeeDto();

            dbContextWrapperMock.Setup(mock => mock.AddAsync(It.IsAny<Employee>())).ThrowsAsync(new Exception("Error at AddEmployee")); // Mocking AddAsync to throw an exception

            // Act
            var result = await employeeService.AddEmployee(createEmployeeDto);

            // Assert
            Assert.False(result.IsSuccess); // Check if the operation failed
            Assert.Contains("Error at AddEmployee", result.Message); // Check if the error message is as expected
        }

        [Fact]
        public async Task CheckIfEmployeeExist_EmployeeExists()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);

            var createEmployeeDto = new CreateEmployeeDto
            {
                Tin = "123456", // Replace with valid values for testing
                FullName = "John Doe",
                Birthdate = DateTime.Parse("2003-01-01")
            };

            var employees = new List<Employee>
            {
                new Employee { TIN = "123456", FullName = "John Doe", Birthdate = DateTime.Parse("2003-01-01") }
                // Add more employees as needed for your test cases
            };

            dbContextWrapperMock.Setup(mock => mock.Find(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns<Expression<Func<Employee, bool>>>(predicate => employees.AsQueryable().Where(predicate));

            // Act
            var result = await employeeService.CheckIfEmployeeExist(createEmployeeDto);

            // Assert
            Assert.False(result.IsSuccess); // Check if the employee exists
            Assert.Equal("Employee already exist", result.Message); // Check if the message is as expected
        }

        [Fact]
        public async Task CheckIfEmployeeExist_EmployeeDoesNotExist()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);

            var createEmployeeDto = new CreateEmployeeDto
            {
                Tin = "123456", // Replace with valid values for testing
                FullName = "John Doe",
                Birthdate = DateTime.Parse("2003-01-01") 
            };

            var employees = new List<Employee>(); // Empty list to simulate no existing employees

            dbContextWrapperMock.Setup(mock => mock.Find(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns<Expression<Func<Employee, bool>>>(predicate => employees.AsQueryable().Where(predicate));

            // Act
            var result = await employeeService.CheckIfEmployeeExist(createEmployeeDto);

            // Assert
            Assert.True(result.IsSuccess); // Check if the employee does not exist
        }


        [Fact]
        public async Task CheckIfTinExist_TinExists()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);

            var tinToCheck = "123456"; // Replace with a valid TIN for testing

            var employees = new List<Employee>
            {
                new Employee { TIN = "123456" },
                new Employee { TIN = "654321" } 
            };

            dbContextWrapperMock.Setup(mock => mock.Find(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns<Expression<Func<Employee, bool>>>(predicate => employees.AsQueryable().Where(predicate));

            // Act
            var result = await employeeService.CheckIfTinExist(tinToCheck);

            // Assert
            Assert.False(result.IsSuccess); // Check if the TIN exists
            Assert.Equal("TIN already exist", result.Message); // Check if the message is as expected
        }

        [Fact]
        public async Task CheckIfTinExist_TinDoesNotExist()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);

            var tinToCheck = "123456"; // Replace with a valid TIN for testing

            var employees = new List<Employee>(); // Empty list to simulate no existing employees

            dbContextWrapperMock.Setup(mock => mock.Find(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns<Expression<Func<Employee, bool>>>(predicate => employees.AsQueryable().Where(predicate));

            // Act
            var result = await employeeService.CheckIfTinExist(tinToCheck);

            // Assert
            Assert.True(result.IsSuccess); // Check if the TIN does not exist
        }

        [Fact]
        public async Task DeleteEmployee_Success()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);

            var existingEmployeeId = 1; // Replace with an existing employee ID for testing

            var existingEmployee = new Employee { Id = existingEmployeeId }; // Creating a sample existing employee

            dbContextWrapperMock.Setup(mock => mock.GetByIdAsync<Employee>(existingEmployeeId))
                .ReturnsAsync(existingEmployee);

            // Act
            var result = await employeeService.DeleteEmployee(existingEmployeeId);

            // Assert
            Assert.True(result.IsSuccess); // Check if deletion was successful
            Assert.Equal(existingEmployeeId, result.Id); // Check if the returned ID matches the expected ID
            Assert.Equal("Employee deleted successfully", result.Message); // Check if the message is as expected
        }

        [Fact]
        public async Task DeleteEmployee_NotFound()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);

            var nonExistingEmployeeId = 2; // Replace with a non-existing employee ID for testing

            dbContextWrapperMock.Setup(mock => mock.GetByIdAsync<Employee>(nonExistingEmployeeId))
                .ReturnsAsync((Employee)null); // Simulating non-existing employee

            // Act
            var result = await employeeService.DeleteEmployee(nonExistingEmployeeId);

            // Assert
            Assert.False(result.IsSuccess); // Check if deletion failed due to non-existing employee
            Assert.Equal(nonExistingEmployeeId, result.Id); // Check if the returned ID matches the expected ID
            Assert.Equal("Employee not found", result.Message); // Check if the message is as expected
        }

       

        [Fact]
        public async Task GetAllEmployee_Success()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);

            var employees = new List<Employee>
             {
                 new Employee { Id = 1, FullName = "John Doe" ,Birthdate = DateTime.Parse("2003-01-01") ,TIN ="242432", EmployeeTypeId= 1, Salary=20000, IsDeleted=false },
                 new Employee { Id = 2, FullName = "Jane Smith" ,Birthdate = DateTime.Parse("2003-01-01") ,TIN ="78868", EmployeeTypeId= 2, Salary=500, IsDeleted=false}
             };

            var employeeDtos = employees.Select(e => new EmployeeDto { Id = e.Id, FullName = e.FullName, Birthdate = (DateTime)e.Birthdate, Tin =e.TIN, TypeId = e.EmployeeTypeId, Salary = e.Salary, IsDeleted = e.IsDeleted  }).ToList();

            // Mocking DbSet<Employee>
            var mockDbSet = new Mock<DbSet<Employee>>();
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employees.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employees.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employees.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employees.AsQueryable().GetEnumerator());

            dbContextWrapperMock.Setup(mock => mock.Set<Employee>()).Returns(mockDbSet.Object);

            mapperMock.Setup(mapper => mapper.Map<List<EmployeeDto>>(It.IsAny<List<Employee>>())).Returns(employeeDtos);

            // Act
            var result = await employeeService.GetAllEmployee();

            // Assert
            Assert.NotNull(result); // Check if the result is not null
            Assert.Equal(employees.Count, result.Count); // Check if the number of returned items matches the expected number
            Assert.Equal(employees[0].FullName, result[0].FullName);
            Assert.Equal(employees[0].Birthdate, result[0].Birthdate);
            Assert.Equal(employees[0].TIN, result[0].Tin);
            Assert.Equal(employees[0].EmployeeTypeId, result[0].TypeId);
            Assert.Equal(employees[0].Salary, result[0].Salary);
        }

        [Fact]
        public async Task GetEmployeeById_Success()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);

            var employeeId = 1; // Replace with an existing employee ID for testing

            var employee = new Employee { Id = 1, FullName = "John Doe", Birthdate = DateTime.Parse("2003-01-01"), TIN = "242432", EmployeeTypeId = 1, Salary = 20000, IsDeleted = false }; 

            var employeeDto = new EmployeeDto { Id = employee.Id, FullName = employee.FullName, Birthdate = (DateTime)employee.Birthdate, Tin = employee.TIN, TypeId = employee.EmployeeTypeId, Salary = employee.Salary}; // Corresponding DTO

            // Mocking GetByIdAsync<TEntity> method
            dbContextWrapperMock.Setup(mock => mock.GetByIdAsync<Employee>(employeeId))
                                .ReturnsAsync(employee);

            // Mocking Mapper
            mapperMock.Setup(mapper => mapper.Map<EmployeeDto>(It.IsAny<Employee>()))
                      .Returns(employeeDto);

            // Act
            var result = await employeeService.GetEmployeeById(employeeId);

            // Assert
            Assert.NotNull(result); // Check if the result is not null
            Assert.Equal(employeeId, result.Id); // Check if the returned ID matches the expected ID
            Assert.Equal("John Doe", result.FullName);
            Assert.Equal("242432", result.Tin);
            Assert.Equal(DateTime.Parse("2003-01-01"), result.Birthdate);
            Assert.Equal(1, result.TypeId);
            Assert.Equal(20000, result.Salary);
        }

        [Fact]
        public async Task GetEmployeeById_Failure()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);

            var employeeId = 1; // Replace with an existing employee ID for testing

            // Simulating null return from GetByIdAsync<TEntity>
            dbContextWrapperMock.Setup(mock => mock.GetByIdAsync<Employee>(employeeId))
                                .ReturnsAsync((Employee)null);

            // Act
            var result = await employeeService.GetEmployeeById(employeeId);

            // Assert
            Assert.Null(result); // Check if the result is null, indicating a failure to retrieve the employee
        }

        [Fact]
        public async Task UpdateEmployee_Success()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);



            var editEmployeeDto = new EditEmployeeDto
            {
                Id = 1,
                FullName = "John Doe",
                Birthdate = DateTime.Parse("2003-01-01"),
                Tin = "242432",
                TypeId= 1,
                Salary = 20000,
                IsDeleted = false
            };

            var existingEmployee = new Employee { Id = 1, FullName = "John Doe", Birthdate = DateTime.Parse("2003-01-01"), TIN = "242432", EmployeeTypeId = 1, Salary = 20000, IsDeleted = false };
            var updatedEmployee = new Employee { Id = 1, FullName = "John Doe", Birthdate = DateTime.Parse("2003-03-03"), TIN = "99897", EmployeeTypeId = 1, Salary = 50000, IsDeleted = false };

            var updatedEmployeeDto = new EmployeeDto
            {
                Id = updatedEmployee.Id,
                FullName = updatedEmployee.FullName,
                Birthdate = (DateTime)updatedEmployee.Birthdate,
                Tin = updatedEmployee.TIN,
                TypeId = updatedEmployee.EmployeeTypeId,
                Salary = updatedEmployee.Salary,
                IsDeleted = updatedEmployee.IsDeleted
            };

            dbContextWrapperMock.Setup(mock => mock.GetByIdAsync<Employee>(editEmployeeDto.Id))
                                .ReturnsAsync(existingEmployee);

            dbContextWrapperMock.Setup(mock => mock.UpdateAsync<Employee>(It.IsAny<Employee>()))
                                .ReturnsAsync(updatedEmployee);

            mapperMock.Setup(mapper => mapper.Map<EmployeeDto>(It.IsAny<Employee>()))
            .Returns((Employee source) => new EmployeeDto
            {
                Id = source.Id,
                FullName = source.FullName,
                Birthdate = (DateTime)source.Birthdate,
                Tin = source.TIN,
                TypeId = source.EmployeeTypeId,
                Salary = source.Salary,
                IsDeleted = source.IsDeleted
            });

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);


            // Act
            var result = await employeeService.UpdateEmployee(editEmployeeDto);

            // Assert
            Assert.NotNull(result); // Check if the result is not null
            Assert.Equal(DateTime.Parse("2003-03-03"), result.Birthdate);
            Assert.Equal("99897", result.Tin);
            Assert.Equal(50000, result.Salary);
        }

        [Fact]
        public async Task UpdateEmployee_FailureEmployeeNotFound()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);

            var editEmployeeDto = new EditEmployeeDto { Id = 1, FullName = "John Doe", Birthdate = DateTime.Parse("2003-03-03"), Tin = "99897", TypeId = 1, Salary = 50000, IsDeleted = false };

            // Simulating null return from GetByIdAsync<TEntity>
            dbContextWrapperMock.Setup(mock => mock.GetByIdAsync<Employee>(editEmployeeDto.Id))
                                .ReturnsAsync((Employee)null);

            // Act
            var result = await employeeService.UpdateEmployee(editEmployeeDto);

            // Assert
            Assert.NotNull(result); // Check if the result is null, indicating a failure to update due to the employee not being found
            Assert.Null(result.FullName);
        }
    }
}