namespace ChallangeDotnet.Application.Dtos
{
    public record UnidadeDto(
        string Codigo,     
        string Nome,      
        bool Ativa,        
        string? Observacao 
    );
}
