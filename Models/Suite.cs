namespace DesafioProjetoHospedagem.Models;

public enum SuiteType
{
    Standard,
    Super,
    Premium
}

public abstract class Suite
{
    private static int contador = 0;
    public Suite(int capacidade, SuiteType tipoSuite, decimal valorDiaria)
    {
        Capacidade = capacidade;
        TipoSuite = tipoSuite;
        ValorDiaria = valorDiaria;
        NumeroSuite = ++contador;
    }
    public int Capacidade { get; set; }
    public SuiteType TipoSuite { get; set; }
    public decimal ValorDiaria { get; set; }
    public int NumeroSuite { get; private set; }

    public override string ToString()
    {
        return $"Suite {NumeroSuite}, Tipo: {TipoSuite}, Capacidade: {Capacidade}, Valor Diária: {ValorDiaria:C}";
    }


}


