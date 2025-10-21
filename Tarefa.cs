public class Tarefa 
{
    public int Id {get;set;} 
    public string Nome {get;set;} = string.Empty;
    public string Descricao {get;set;}= string.Empty; 
    public DateTime DataCriacao {get;set;} 
    public int Status {get;set;} 
    public DateTime? DataExecucao {get;set;} 
}