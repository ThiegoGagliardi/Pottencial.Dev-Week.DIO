using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

using src.Models;
using src.Persistence;

namespace  src.Controllers;

[ApiController]
[Route("[controller]")]

public class PersonController : ControllerBase {

    private DataBaseContext _context { get; }

    public PersonController(DataBaseContext dbContext)
    {
         this._context = dbContext;        
    }

    [HttpGet]
    public ActionResult<List<Person>> Get(){
       
        var result = _context.Persons.Include(p => p.Contracts).ToList();

        if (!result.Any()){
            return NoContent();
        }

        return Ok(result);    }

    [HttpPost]
    public ActionResult<Person> Post([FromBody] Person person){

        try{
            
            _context.Persons.Add(person);
            _context.SaveChanges();

            return Created("Pessoa criada.", person);        
        }catch{
            return BadRequest(new {
                                msg    = $"Hove um erro ao enviar a solicitação de criação.",
                                status = HttpStatusCode.BadRequest
                            });
        }        
    }

    [HttpPut("{id}")]
    public ActionResult<object> Update([FromRoute] int id, [FromBody]Person person){

        var result = _context.Persons.SingleOrDefault(p => p.Id == id);

        if (result is null){
            return BadRequest(new {
                                msg    = $"Id {id} não existe.",
                                status = HttpStatusCode.BadRequest
                            });            
        }

        if (person.Id != id) {
            return BadRequest(new {
                                msg    = $"Dados do Id {id} não corresponde ao objeto ",
                                status = HttpStatusCode.BadRequest
                            });
        }

        try { 

            result.Name      = person.Name;
            result.Age       = person.Age;
            result.Cpf       = person.Cpf;
            result.IsActive  = person.IsActive;   
            
           _context.Persons.Update(result);
           _context.SaveChanges();

          return Ok(new {
                          msg    = $"Dados do Id {id} atualizados",
                          status = HttpStatusCode.OK
                        });
        }        
        catch (System.Exception e){

            return BadRequest(new {
                                msg    = $"Hove um erro ao enviar a solicitação de atualização {id}." +e.Message,
                                status = HttpStatusCode.BadRequest
                            });
        }       
    }

    [HttpDelete("{id}")]

    public ActionResult<Object> Delete([FromRoute] int id)
    {
        var person = _context.Persons.SingleOrDefault(p => p.Id == id);

        if (person is null) {
            
            return BadRequest(new { 
                            msg = "Conteúdo inexistente, solicitação inválida",
                            status = HttpStatusCode.BadRequest
                            });
        }

         _context.Persons.Remove(person);
         _context.SaveChanges();

         return Ok(new {
                            msg = $"Pessoa com id {id} Deletada.",
                            status = HttpStatusCode.OK
                        }
                    );        
    }
}