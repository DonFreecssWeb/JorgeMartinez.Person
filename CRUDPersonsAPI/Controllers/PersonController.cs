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

namespace CRUDPersonsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
       private readonly PersonService _personService;

        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;


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

        [HttpPost("")]
        public async Task<IActionResult> GetPersonsQuery()
        {
                 
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var order =  Request.Form["order[0][column]"].FirstOrDefault();
            var sortColumn = Request.Form["columns["+order+"][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[Value]"].FirstOrDefault();

            pageSize = length != null ? Convert.ToInt32(length) : 0;
            skip = start != null ? Convert.ToInt32(start) : 0;
            recordsTotal = 0;

            IQueryable<TbPersona> query =   _personService.GetAllQueryablePerson();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(temp => temp.Nombre.Contains(searchValue));
            }          

            try
            {
                              
             
                recordsTotal = query.Count();
                var data = query.Skip(skip).Take(pageSize).AsEnumerable();

                var queryModel = data.Select(temp => new PersonTableViewModel
                {
                    Id = temp.Id,
                    Name = temp.Nombre,
                    Age = Convert.ToInt32(((DateTime.Now - temp.Fnacimiento.Value).TotalDays) / 365.25)
                });

                //sortColumnDir = desc - asc
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
                {
                    if (sortColumn.Contains("nombre", StringComparison.OrdinalIgnoreCase))
                    {
                        if (sortColumnDir == "asc")
                            queryModel = queryModel.OrderBy(temp => temp.Name);
                        else if (sortColumnDir == "desc")
                        {
                            queryModel = queryModel.OrderByDescending(temp => temp.Name);
                        }
                    }
                    else if (sortColumn.Contains("edad", StringComparison.OrdinalIgnoreCase))
                    {
                        if (sortColumnDir == "asc")
                        {
                            queryModel = queryModel.OrderBy(temp => temp.Age);
                        }
                        else if (sortColumnDir == "desc")
                        {
                            queryModel = queryModel.OrderByDescending(temp => temp.Age);
                        }
                    }

                }
                return Ok(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = queryModel });

            }
            catch (Exception ex)
            {

                throw;
            }

          
        }

        // GET: api/Person/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<TbPersona>> GetTbPersona(int id)
        //{
        //    var tbPersona = await _personService.TbPersonas.FindAsync(id);

        //    if (tbPersona == null)
        //    {
        //        return NotFound();
        //    }

        //    return tbPersona;
        //}

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

        //// POST: api/Person
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<TbPersona>> PostTbPersona(TbPersona tbPersona)
        //{
        //    _personService.TbPersonas.Add(tbPersona);
        //    await _personService.SaveChangesAsync();

        //    return CreatedAtAction("GetTbPersona", new { id = tbPersona.Id }, tbPersona);
        //}

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
