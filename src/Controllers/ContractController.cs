using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

using src.Models;
using src.Persistence;

namespace  src.Controllers;

[ApiController]
[Route("[controller]")]

public class ContractController : ControllerBase {

    private DataBaseContext _context { get; }

    public ContractController(DataBaseContext dbContext)
    {
         this._context = dbContext;        
    }

    [HttpGet("{personId}/{id}")]
    public ActionResult<List<Person>> Get([FromRoute] int personId,[FromRoute] int id){

         var person = _context.Persons.SingleOrDefault(p => p.Id == personId);

         if (person is null){

             return BadRequest(new {
                                msg    = $"Id Pessoa {personId} não existe.",
                                status = HttpStatusCode.BadRequest});
         }         
       
        var result = _context.Persons.Where(p => p.Id == personId).Include(c => c.Contracts.Where(c => c.Id == id)).ToList();

        if (!result.Any()){
            return NoContent();
        }

        return Ok(result);    }

    [HttpPost("{personId}")]
    public ActionResult<Contract> Post([FromRoute] int personId, [FromBody] Contract contract){

        try{

            var person = _context.Persons.SingleOrDefault(p => p.Id == personId);

            if (person is null){

               return BadRequest(new {
                                msg    = $"Id Pessoa {personId} não existe.",
                                status = HttpStatusCode.BadRequest
                            });            
            } 

            var result = _context.Contracts.SingleOrDefault(c => c.Id == contract.Id && c.PersonId == personId);

            if (result != null){
               return BadRequest(new {
                                msg    = $"Id Pessoa {personId} já possui este contrato.",
                                status = HttpStatusCode.BadRequest
                            });  
            }

            contract.PersonId = personId;
            _context.Contracts.Add(contract);
            _context.SaveChanges();

            return Created($"Contrato adicionado para o id pessoa {personId}", contract);        
        }catch{
            return BadRequest(new {
                                msg    = $"Hove um erro ao enviar a solicitação de criação.",
                                status = HttpStatusCode.BadRequest
                            });
        }        
    }

    [HttpPut("{personId}/{id}")]
    public ActionResult<object> Update([FromRoute] int personId, [FromRoute] int id, [FromBody]  Contract contractUpdate){
        try {

            var person = _context.Persons.SingleOrDefault(p => p.Id == personId);

            if (person is null){

                return BadRequest(new {
                            msg    = $"Id Pessoa {personId} não existe.",
                            status = HttpStatusCode.BadRequest
                        });            
            }   

            var contrato = _context.Contracts.SingleOrDefault(c => c.Id == id && c.PersonId == personId);

            if (contrato is null){
                return BadRequest(new {
                                msg    = $"Contrato com Id {id} não existe.",
                                status = HttpStatusCode.BadRequest
                            });            
            }

            if ((contrato.Id != id) || (contrato.PersonId != personId)) {
                return BadRequest(new {
                                msg    = $"Dados do Id {id} não corresponde ao objeto ",
                                status = HttpStatusCode.BadRequest
                            });
            }

            contrato.IsPaid    = contractUpdate.IsPaid;
            contrato.Value     = contractUpdate.Value;
            contrato.TokenId   = contractUpdate.TokenId;
            contrato.PersonId  = contractUpdate.PersonId;                                

          _context.Contracts.Update(contrato);
          _context.SaveChanges();

          return Ok(new {
                          msg    = $"Contrato do Id {id} atualizado para o id pessoa {personId}",
                          status = HttpStatusCode.OK
                        });
        }        
        catch (System.Exception){

            return BadRequest(new {
                                msg    = $"Hove um erro ao enviar a solicitação de atulização {id}.",
                                status = HttpStatusCode.BadRequest
                            });
        }       
    }

    [HttpDelete("{personId}/{id}")]

    public ActionResult<Object> Delete([FromRoute] int personId, [FromRoute] int id)
    {
        var person = _context.Persons.SingleOrDefault(p => p.Id == personId);

        if (person is null){

            return BadRequest(new {
                            msg    = $"Id Pessoa {personId} não existe.",
                            status = HttpStatusCode.BadRequest
                    });            
        }  

        var contrato = _context.Contracts.SingleOrDefault(c => c.Id == id && c.PersonId == personId);

        if (contrato is null){
            return BadRequest(new {
                            msg    = $"Contrato com Id {id} não existe.",
                            status = HttpStatusCode.BadRequest
                        });            
        }

        if ((contrato.Id != id) || (contrato.PersonId != personId)) {
            return BadRequest(new {
                            msg    = $"Dados do Id {id} não corresponde ao objeto ",
                            status = HttpStatusCode.BadRequest
                        });
        }           

         _context.Contracts.Remove(contrato);
         _context.SaveChanges();

         return Ok(new {
                            msg = $"Contrato com id {id} Deletada da pessoa {personId}.",
                            status = HttpStatusCode.OK
                        }
                    );        
    }
}