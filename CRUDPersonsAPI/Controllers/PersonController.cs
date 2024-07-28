using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDPersonsAPI.Datos;
using CRUDPersonsAPI.models;
using CRUDPersonsAPI.Services;
using CRUDPersonsAPI.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using CRUDPersonsAPI.Features;

namespace CRUDPersonsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
       private readonly PersonService _personService;

       public PersonController(PersonService service)
        {
            _personService = service;
        }

        // GET: api/Person
        //[HttpGet]
        //public async Task<IActionResult> GetPersons()
        //{

        //    return Ok(new { data = await _personService.GetAllPersons() });
        //}

        [HttpPost("datatable")]
        public async Task<IActionResult> GetPersonsDataTable()
        {                 
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var order =  Request.Form["order[0][column]"].FirstOrDefault();
            var sortColumn = Request.Form["columns["+order+"][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[Value]"].FirstOrDefault();

            var pageSize = length != null ? Convert.ToInt32(length) : 0;
            var skip = start != null ? Convert.ToInt32(start) : 0;
            var recordsTotal = 0;

            // Filtrar en la base de datos
            IQueryable<TbPersona> query =   _personService.GetAllQueryablePerson();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(temp => temp.Nombre.Contains(searchValue));
            }
            // Ordenar en la base de datos

            //sortColumnDir = desc - asc
            query = query.OrderByDescending(temp  => temp.Id);

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                if (sortColumn.Contains("name", StringComparison.OrdinalIgnoreCase))
                {
                    if (sortColumnDir == "asc")
                        query = query.OrderBy(temp => temp.Nombre);
                    else if (sortColumnDir == "desc")
                    {
                        query = query.OrderByDescending(temp => temp.Nombre);
                    }
                }
                else if (sortColumn.Contains("age", StringComparison.OrdinalIgnoreCase))
                {
                    if (sortColumnDir == "asc")
                    {
                        query = query.OrderBy(temp => temp.Fnacimiento);
                    }
                    else if (sortColumnDir == "desc")
                    {
                        query = query.OrderByDescending(temp => temp.Fnacimiento);
                    }
                }
            }


            try
            {
                // Contar el total de registros
                recordsTotal = await query.CountAsync();

                // Paginación en la base de datos
                query = query.Skip(skip).Take(pageSize);

                // Traer los datos paginados a la memoria
                var personList = await query.ToListAsync();

                var queryModel = personList.Select(temp => new PersonTableViewModel
                {
                    Id = temp.Id,
                    Name = temp.Nombre,
                    Age = Convert.ToInt32(((DateTime.Now - temp.Fnacimiento.Value).TotalDays) / 365.25)
                });

                

                return Ok(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = queryModel });
            }
            catch (Exception ex)
            {
                throw;
            }          
        }



        // GET: api/Person/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TbPersona>> GetPerson(int id)
        {
            TbPersona? tbPersona = await _personService.GetPersonByPersonId(id);

            if (tbPersona == null)
            {
                return NotFound();
            }

            return tbPersona;
        }

        //// PUT: api/Person/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTbPersona(int id, TbPersona tbPersona)
        //{
        //    if (id != tbPersona.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _personService.Entry(tbPersona).State = EntityState.Modified;

        //    try
        //    {
        //        await _personService.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TbPersonaExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Person
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       
        [HttpPost]
        public async Task<IActionResult> AddPerson(TbPersona? tbPersona)
        {
            TbPersona person = await _personService.AddPerson(tbPersona);          
             
            
            return  Ok(ResponseApiService.Response(StatusCodes.Status200OK, person, "Ejecución exitosa" )) ;
        }

        //// DELETE: api/Person/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTbPersona(int id)
        //{
        //    var tbPersona = await _personService.TbPersonas.FindAsync(id);
        //    if (tbPersona == null)
        //    {
        //        return NotFound();
        //    }

        //    _personService.TbPersonas.Remove(tbPersona);
        //    await _personService.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool TbPersonaExists(int id)
        //{
        //    return _personService.TbPersonas.Any(e => e.Id == id);
        //}
    }
}
