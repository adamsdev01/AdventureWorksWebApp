using AdventureWorksWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksWebApp.Services.PersonService
{
    public class PersonService
    {
        #region Property
        private readonly AdventureWorks2022Context _dbContext;
        private List<Person> _person;
        #endregion

        #region Constructor
        public PersonService(AdventureWorks2022Context dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Get List of People
        //public async Task<List<Person>> GetAllPeopleAsync()
        //{
        //    return await _dbContext.People
        //        .OrderBy(p => p.LastName)
        //        .ToListAsync();
        //}

        public async Task<List<Person>> GetAllPeopleAsync()
        {
            if (_person == null)
            {
                _person = _dbContext.People.ToList().Select(p => new Person
                {
                    FullName = p.FirstName + " " + p.MiddleName + " " + p.LastName,
                }).ToList();
            }
            //return await _dbContext.People
            //    .OrderBy(p => p.LastName)
            //    .ToListAsync();
        }
        #endregion

        #region Insert Employee
        public async Task<bool> InsertPersonAsync(Person person)
        {
            await _dbContext.People.AddAsync(person);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Get Employee by Id
        public async Task<Person> GetPersonAsync(int businessEntityId)
        {
            Person person = await _dbContext.People
                .Include(p => p.PersonCreditCards)
                .Include(p => p.BusinessEntity)
                .Include(p => p.BusinessEntityContacts)
                .Include(p => p.EmailAddresses)
                .Where(e => e.BusinessEntityId.Equals(businessEntityId))
                .FirstOrDefaultAsync();

            return person;
        }
        #endregion

        #region Update Employee
        public async Task<bool> UpdateEmployeeAsync(Person person)
        {
            _dbContext.People.Update(person);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Delete Employee
        public async Task<bool> DeleteEmployeeAsync(Person person)
        {
            person.ModifiedDate = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
