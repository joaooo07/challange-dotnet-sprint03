namespace ChallangeDotnet.Application.Dtos
{
    public record VagaDto(
        string Codigo,       
        bool Coberta,       
        bool Ocupada,        
        string? Observacao    
    );
}
