using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CRUDPersonsAPI.models;
using CRUDPersonsAPI.Services;
using CRUDPersonsAPI.DTO;

using CRUDPersonsAPI.Features;
using CRUDPersonsAPI.Exception;
using CRUDPersonsAPI.Filters;

namespace CRUDPersonsAPI.Controllers
{
    [Route("api/v1/person")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    [TypeFilter(typeof(VerifySession2))]
    public class PersonController : ControllerBase
    {
       private readonly PersonService _personService;

       public PersonController(PersonService service)
        {
            _personService = service;
        }

       //GET: api/Person
       [HttpGet]
        public async Task<IActionResult> GetPersons()
        {
            var data = await _personService.GetAllPersons();

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK,data));
        }

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



        // GET: api/v1/Person/5
        [HttpGet("{userId}")]
        public async Task<IActionResult> ShowPerson(int userId)
        {
            
            if (userId == 0)
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest));


            TbPersona? data = await _personService.GetPersonByPersonId(userId);

            if (data == null)
            {
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK,data));
        }

        // PUT: api/v1/Person/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> EditPerson(TbPersona person)
        {        
            var data = await _personService.UpdatePerson(person);

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data));
        }

        // POST: api/Person
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<IActionResult> AddPerson(TbPersona tbPersona)
        {
          
            TbPersona person = await _personService.AddPerson(tbPersona);         
             
            
            return  StatusCode(StatusCodes.Status201Created ,ResponseApiService.Response(StatusCodes.Status201Created, person)) ;
        }

        // DELETE: api/Person/5
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeletePerson(int userId)
        {
            if (userId == 0)
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest));
            
            var data = await _personService.DeletePerson(userId);

            if (data == false)
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound,data));

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK,data));
        }

        
    }
}
