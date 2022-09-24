using System.Collections.Generic;

namespace src.Models;
public class Person
{

    public int Id {get; set;}
    
    public string Name { get;  set; }
    
    public string? Cpf {get;  set;}

    public int Age {get;  set;}

    public bool IsActive {get;  set;} 

    public List<Contract> Contracts {get; set;}

    public Person(string name, string cpf, int age, bool active)
    {
        this.Name      = name;
        this.Cpf       = cpf;
        this.Age       = age;
        this.IsActive  = true;
        this.Contracts = new List<Contract>();
    }

    public Person()
    {
        this.Name      = "";
        this.Age       = 0;
        this.Cpf       = "";
        this.IsActive  = true;
        this.Contracts = new List<Contract>();
    }
            
}