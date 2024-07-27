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
            
    }
}
