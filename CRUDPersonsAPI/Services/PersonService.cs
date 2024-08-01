using CRUDPersonsAPI.Datos;
using CRUDPersonsAPI.models;
using Microsoft.AspNetCore.Mvc;
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
            var entity = await _context.TbPersonas.FirstOrDefaultAsync(x => x.Id == personId);
            return entity;
        }

        public async Task<TbPersona> AddPerson(TbPersona person)
        {
            _context.TbPersonas.Add(person);
            await _context.SaveChangesAsync();

            return person;
        }
        //en confirmar contraseña validar si existe el uusario pero en update models no se requeire hacer eso
        
        public async Task<TbPersona> UpdatePerson(TbPersona person)
        {         

            _context.TbPersonas.Update(person);
            await _context.SaveChangesAsync();

            return person;
        }
        //valida si existe en vid
        public async Task<bool> DeletePerson(int personId)
        {
            var entity = await _context.TbPersonas.FirstOrDefaultAsync(x => x.Id == personId);
            if (entity == null) 
                return false;

            _context.TbPersonas.Remove(entity);
            return   await _context.SaveChangesAsync() > 0 ;
        }
            
    }
}
