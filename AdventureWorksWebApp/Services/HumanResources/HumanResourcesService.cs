using AdventureWorksWebApp.Models;
using AdventureWorksWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using System;

namespace AdventureWorksWebApp.Services.HumanResources
{
    public class HumanResourcesService
    {
        #region Property
        private readonly AdventureWorks2022Context _dbContext;
        #endregion

        #region Constructor
        public HumanResourcesService(AdventureWorks2022Context dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Get List of Employees
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _dbContext.Employees.ToListAsync();
        }
        #endregion

        #region Insert Employee
        public async Task<bool> InsertEmployeeAsync(Employee employee)
        {
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Get Employee by Id
        public async Task<Employee> GetEmployeeAsync(int businessEntityId)
        {
            Employee employee = await _dbContext.Employees
                .Include(e => e.EmployeePayHistories)
                .Where(e => e.BusinessEntityId.Equals(businessEntityId))
                .FirstOrDefaultAsync();

            return employee;
        }
        #endregion

        #region Update Employee
        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Delete Employee
        public async Task<bool> DeleteEmployeeAsync(Employee employee)
        {
            employee.CurrentFlag = false;
            employee.ModifiedDate = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Get List of Employees - Persons & Employee table
        private List<EmployeeViewModel> _employeeDataAsList = null;
        public List<EmployeeViewModel> GetEmployees => _employeeDataAsList;

        public async Task<List<EmployeeViewModel>> GetAllEmployeesDataAsListAsync()
        {

            var persons = await _dbContext.People
                .Where(p => p.PersonType.Equals("EM"))
                .ToListAsync();

            var employees = await _dbContext.Employees
                .Where(e => e.CurrentFlag.Equals(true))
                .ToListAsync();
           
            var query = persons.Join(employees,
              p => p.BusinessEntityId,
              emp => emp.BusinessEntityId,
              (p, emp) => new EmployeeViewModel
              {
                 BusinessEntityId = p.BusinessEntityId,
                 FullName = p.FirstName + " " + p.MiddleName + " " + p.LastName,
                 FirstName = p.FirstName,
                 MiddleName = p.MiddleName,
                 LastName = p.LastName,
                 JobTitle =  emp.JobTitle,
                 HireDate = emp.HireDate,
                 IsSalaried = emp.SalariedFlag,
              }).ToList();

            // Store the data as a list of objects
            _employeeDataAsList = query;

            return _employeeDataAsList;
        }
        #endregion

        #region Get Employee Pay Rate
        public async Task<EmployeePayHistory> GetEmployeePayDataAsync(int businessEntityId)
        {
            EmployeePayHistory employeePayData = await _dbContext.EmployeePayHistories
                .Where(e => e.BusinessEntityId.Equals(businessEntityId))
                .FirstOrDefaultAsync();

            return employeePayData;
        }
        #endregion

        #region Get Employee Department Histories
        public async Task<List<EmployeeDepartmentHistory>> GetEmployeeDepartmentHistoryDataAsync(int businessEntityId)
        {
            var employeeDepartmentHistory = await _dbContext.EmployeeDepartmentHistories
                .Include(e => e.Department)
                .Include(e => e.Shift)
                .Where(e => e.BusinessEntityId.Equals(businessEntityId))
                .ToListAsync();

            return employeeDepartmentHistory;
        }
        #endregion

    }
}
