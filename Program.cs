using System.Globalization;
using System.Text;
using DesafioProjetoHospedagem.Models;

Console.OutputEncoding = Encoding.UTF8;
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");

// Lista de suites do hotel
List<Suite> suites = new List<Suite>{
    new SuiteStandard(),
    new SuiteStandard(),
    new SuiteStandard(),
    new SuiteSuper(),
    new SuiteSuper(),
    new SuitePremium()
};

List<Reserva> reservas = new List<Reserva>();

void ShowLogo()
{
    Console.WriteLine(@"
░██████╗░██╗░░░██╗░█████╗░██╗░░░░░██╗████████╗██╗░░░██╗  ██╗░░██╗░█████╗░████████╗███████╗██╗░░░░░
██╔═══██╗██║░░░██║██╔══██╗██║░░░░░██║╚══██╔══╝╚██╗░██╔╝  ██║░░██║██╔══██╗╚══██╔══╝██╔════╝██║░░░░░
██║██╗██║██║░░░██║███████║██║░░░░░██║░░░██║░░░░╚████╔╝░  ███████║██║░░██║░░░██║░░░█████╗░░██║░░░░░
╚██████╔╝██║░░░██║██╔══██║██║░░░░░██║░░░██║░░░░░╚██╔╝░░  ██╔══██║██║░░██║░░░██║░░░██╔══╝░░██║░░░░░
░╚═██╔═╝░╚██████╔╝██║░░██║███████╗██║░░░██║░░░░░░██║░░░  ██║░░██║╚█████╔╝░░░██║░░░███████╗███████╗
░░░╚═╝░░░░╚═════╝░╚═╝░░╚═╝╚══════╝╚═╝░░░╚═╝░░░░░░╚═╝░░░  ╚═╝░░╚═╝░╚════╝░░░░╚═╝░░░╚══════╝╚══════╝
");
}

void ExibirMenu()
{

    ShowLogo();

    Console.WriteLine("Bem-vindo!\n");
    Console.WriteLine("Selecione uma opção:\n");
    Console.WriteLine("1. Realizar uma reserva");
    Console.WriteLine("2. Verificar suites");
    Console.WriteLine("3. Realizar check-out");
    Console.WriteLine("4. Sair");
}

void RealizarReserva()
{
    Console.Clear();

    MostrarHeader("Realizar Reserva");

    HashSet<Suite> suitesIndisponiveis = new(reservas.Select(r => r.Suite));
    HashSet<Suite> suitesLivres = new(suites.Except(suitesIndisponiveis));

    if (!suitesLivres.Any())
    {
        Console.WriteLine("Não há suites disponíveis no momento.");
        return;
    }

    int quantidadeDias = 0;

    while (quantidadeDias <= 0)
    {
        Console.Write("Digite a quantidade de dias da reserva: ");
        bool sucesso = int.TryParse(Console.ReadLine(), out quantidadeDias);
        if (!sucesso || quantidadeDias <= 0)
        {
            Console.WriteLine("Quantidade de dias inválida. Tente novamente.");
        }
    }

    Reserva reserva = new Reserva(quantidadeDias);


    int quantidadeHospedes = 0;
    while (quantidadeHospedes <= 0)
    {
        Console.Write("Quantidade de hóspedes: ");
        bool sucesso = int.TryParse(Console.ReadLine(), out quantidadeHospedes);
        if (!sucesso || quantidadeHospedes <= 0)
        {
            Console.WriteLine("Quantidade de hóspedes inválida. Tente novamente.");
        }

        if (suitesLivres.Max(s => s.Capacidade) < quantidadeHospedes)
        {
            Console.WriteLine("Não há suites disponíveis para essa quantidade de hóspedes.");
            return;
        }
    }


    Console.WriteLine("\nSelecione uma suite disponível: ");

    List<Suite> suitesDisponiveis = suitesLivres
        .Where(s => s.Capacidade >= quantidadeHospedes)
        .ToList();

    foreach (var suite in suitesDisponiveis)
    {
        Console.WriteLine($"- {suite}");
    }

    int numeroSuite = 0;

    while (!suitesDisponiveis.Any(s => s.NumeroSuite == numeroSuite))
    {
        Console.Write("\nDigite o número da suite: ");
        bool sucesso = int.TryParse(Console.ReadLine(), out numeroSuite);
        if (!sucesso || !suitesDisponiveis.Any(s => s.NumeroSuite == numeroSuite) || numeroSuite <= 0)
        {
            Console.WriteLine("Número de suite inválido. Tente novamente.\n");
        }

    }

    Suite suiteSelecionada = suitesDisponiveis.First(s => s.NumeroSuite == numeroSuite);
    reserva.CadastrarSuite(suiteSelecionada);

    List<Pessoa> hospedes = new List<Pessoa>();
    for (int i = 0; i < quantidadeHospedes; i++)
    {
        Console.WriteLine($"\nDados do hóspede {i + 1}");
        string nome = string.Empty;
        while (string.IsNullOrWhiteSpace(nome))
        {
            Console.Write("\nNome: ");
            nome = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nome))
            {
                Console.WriteLine("Nome não pode ser vazio. Tente novamente.");
            }
        }
        string sobrenome = string.Empty;
        while (string.IsNullOrWhiteSpace(sobrenome))
        {
            Console.Write("\nSobrenome: ");
            sobrenome = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(sobrenome))
            {
                Console.WriteLine("Sobrenome não pode ser vazio. Tente novamente.");
            }
        }

        Pessoa hospede = new Pessoa(nome, sobrenome);
        hospedes.Add(hospede);
    }

    reserva.CadastrarHospedes(hospedes);

    reservas.Add(reserva);
    Console.WriteLine("\nReserva realizada com sucesso!\n");
    Console.WriteLine($"Suite selecionada: {reserva.Suite}");
    Console.WriteLine($"Hóspedes ({reserva.ObterQuantidadeHospedes()}) : {string.Join(", ", reserva.Hospedes.Select(h => h.NomeCompleto))}");
    Console.WriteLine($"Valor total da reserva: {reserva.CalcularValorTotal():C}");
}

void VerificarSuites()
{
    Console.Clear();

    MostrarHeader("Situação das Suites");

    foreach (var suite in suites)
    {
        Reserva reservaEncontrada = reservas.FirstOrDefault(r => r.Suite.NumeroSuite == suite.NumeroSuite);
        if (reservaEncontrada != null)
        {
            Console.WriteLine($"- Suite {suite.NumeroSuite} - Reservada");
            Console.WriteLine($"  Tipo: {suite.TipoSuite}, Capacidade: {suite.Capacidade}, Valor Diária: {suite.ValorDiaria:C}");
            Console.WriteLine($"  Hóspedes: {string.Join(", ", reservaEncontrada.Hospedes.Select(h => h.NomeCompleto))}");
            Console.WriteLine($"  Dias Reservados: {reservaEncontrada.DiasReservados}");
            Console.WriteLine($"  Valor Total da Reserva: {reservaEncontrada.CalcularValorTotal():C}");
        }
        else
        {
            Console.WriteLine($"- Suite {suite.NumeroSuite} - Disponível");
            Console.WriteLine($"  Tipo: {suite.TipoSuite}, Capacidade: {suite.Capacidade}, Valor Diária: {suite.ValorDiaria:C}");
        }

        Console.WriteLine();
    }

}

void RealizarCheckout()
{
    Console.Clear();

    MostrarHeader("Realizar Check-out");

    HashSet<Suite> suitesReservadas = new(reservas.Select(r => r.Suite));

    if (!suitesReservadas.Any())
    {
        Console.WriteLine("Não há reservas ativas no momento.");
        return;
    }

    foreach (var suite in suitesReservadas.OrderBy(s => s.NumeroSuite))
    {
        Reserva r = reservas.First(r => r.Suite.NumeroSuite == suite.NumeroSuite);
        Console.WriteLine($"- Suite {suite.NumeroSuite} {suite.TipoSuite},{string.Join(",", r.Hospedes.Select(h => h.NomeCompleto))} ");
    }

    int numeroSuite = 0;

    do
    {
        Console.Write("\nDigite o número da suite para check-out: ");
        bool sucesso = int.TryParse(Console.ReadLine(), out numeroSuite);
        if (!sucesso || numeroSuite <= 0 || !suitesReservadas.Any(s => s.NumeroSuite == numeroSuite))
        {
            Console.WriteLine("Número de suite inválido. Tente novamente.");
        }
    } while (numeroSuite <= 0 || !suitesReservadas.Any(s => s.NumeroSuite == numeroSuite));

    Reserva reserva = reservas.First(r => r.Suite.NumeroSuite == numeroSuite);

    reservas.Remove(reserva);
    Console.WriteLine($"\nCheck-out realizado com sucesso para a suite {numeroSuite}.");
    Console.WriteLine($"Valor total da reserva: {reserva.CalcularValorTotal():C}");

}

void MostrarHeader(string title)
{
    string asterisks = String.Empty.PadLeft(title.Length, '*');

    Console.WriteLine(asterisks);
    Console.WriteLine(title);
    Console.WriteLine($"{asterisks}\n");
}

int opcaoMenu = 0;
while (opcaoMenu != 4)
{
    ExibirMenu();
    Console.Write("\nDigite a opção desejada: ");
    bool sucesso = int.TryParse(Console.ReadLine(), out opcaoMenu);
    if (!sucesso || opcaoMenu < 1 || opcaoMenu > 4)
    {
        Console.WriteLine("Opção inválida. Tente novamente.");
        continue;
    }

    switch (opcaoMenu)
    {
        case 1:
            RealizarReserva();
            break;
        case 2:
            VerificarSuites();
            break;
        case 3:
            RealizarCheckout();
            break;
        case 4:
            Console.WriteLine("Saindo do sistema...");
            break;
        default:
            break;
    }

    if (opcaoMenu != 4)
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
        Console.Clear();
    }
}

