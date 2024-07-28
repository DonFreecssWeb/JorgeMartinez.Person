using CRUDPersonsAPI.Datos;
using CRUDPersonsAPI.models;
using Microsoft.EntityFrameworkCore;

namespace CRUDPersonsAPI.Services
{
    public class PersonService
    {
        private readonly ApplicationDbContext _context;
        public PersonService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TbPersona>> GetAllPersons()
        {
            return await _context.TbPersonas.ToListAsync();
        }
        public IQueryable<TbPersona> GetAllQueryablePerson()
        {
            return  _context.TbPersonas.AsQueryable();
        }
        public async Task<TbPersona?> GetPersonByPersonId(int personId)
        {
            if (personId <= 0) {
                return null;
            }

            return await _context.TbPersonas.FirstOrDefaultAsync(x => x.Id == personId);
        }

        public async Task<TbPersona> AddPerson(TbPersona person)
        {
            _context.TbPersonas.Add(person);
            await _context.SaveChangesAsync();

            return person;
        }
            
    }
}
