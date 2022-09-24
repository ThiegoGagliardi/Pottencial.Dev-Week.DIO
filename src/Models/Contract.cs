namespace src.Models;

public class Contract{

    public int Id {get; set;}

    public DateTime CreateDate {get; set;}

    public double Value {get; set;}

    public string TokenId {get; set;}

    public bool IsPaid {get; set;}

    public int PersonId {get; set;}

    public Contract()
    {
        this.Value      = 0;
        this.CreateDate = DateTime.Now; 
        this.TokenId    = "0000";
        this.IsPaid     = false;
    }

    public Contract(string tokenId, double value)    
    {
        this.Value      = value;
        this.CreateDate = DateTime.Now; 
        this.TokenId    = tokenId;
        this.IsPaid      = false;
    }  
}