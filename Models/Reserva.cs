namespace DesafioProjetoHospedagem.Models;

public class Reserva
{
    public List<Pessoa> Hospedes { get; set; } = new List<Pessoa>();
    public Suite Suite { get; set; }
    public int DiasReservados { get; set; }

    public Reserva() { }

    public Reserva(int diasReservados)
    {
        DiasReservados = diasReservados;
    }

    public void CadastrarHospedes(List<Pessoa> hospedes)
    {
        if (Suite != null && Suite.Capacidade >= hospedes.Count)
        {
            Hospedes = hospedes;
        }
        else
        {
            throw new Exception("Capacidade da suite insuficiente para o número de hóspedes.");
        }
    }

    public void CadastrarSuite(Suite suite)
    {
        Suite = suite;
    }

    public int ObterQuantidadeHospedes()
    {
        return Hospedes.Count;
    }

    public decimal CalcularValorTotal()
    {
        decimal valor = DiasReservados * Suite.ValorDiaria;

        if (DiasReservados >= 10)
        {
            valor -= valor * 0.1m;
        }

        return valor;
    }
}
