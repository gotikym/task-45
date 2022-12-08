using System;
using System.Collections.Generic;

internal class Program
{
    static void Main(string[] args)
    {
        Dispatcher dispatcher = new Dispatcher();

        dispatcher.Work();
    }
}

class Dispatcher
{
    private List<Train> _sentTrains = new List<Train>();
    private CashRegister _cashRegister = new CashRegister();

    public void Work()
    {
        const string CommandStart = "start";
        const string CommandExit = "exit";
        bool isExit = false;

        while (isExit == false)
        {
            Console.SetCursorPosition(0, 15);
            Console.WriteLine("Для создания плана поезда введите: " + CommandStart + "\nДля завершения работы введите: " + CommandExit);
            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case CommandStart:
                    MakePlan();
                    break;

                case CommandExit:
                    isExit = true;
                    break;
            }
        }
    }

    private void MakePlan()
    {
        Train train = new Train();

        train.ShowTable();
        train.SetDirection();

        train.ShowTable();

        Console.WriteLine("Чтобы пустить в продажу билеты, нажмите enter");
        Console.ReadKey();

        train.SetPassengers(_cashRegister.SellTickets());
        train.ShowTable();

        Console.WriteLine("билеты проданы, осталось сформировать поезд.");

        train.ShowTable();
        train.CreateWagons();

        Console.WriteLine("Поезд сформирован, отправляем");
        Console.ReadKey();

        train.Depart();
        _sentTrains.Add(train);

        train.ShowTable();
    }
}

class Train
{
    private List<RailwayCarriage> _railwayCarriages = new List<RailwayCarriage>();
    private List<string> _direction = new List<string>();
    private Dictionary<string, int> _types = new Dictionary<string, int>();
    public bool IsDeparted { get; private set; }
    public int Passengers { get; private set; }

    public Train()
    {
        _railwayCarriages = new List<RailwayCarriage>();
        _direction = new List<string>();
        Passengers = 0;
        IsDeparted = false;
        AddType();
    }

    public void SetPassengers(int ticketsSold)
    {
        Passengers = ticketsSold;
    }

    public void CreateWagons()
    {
        const string CommandNo = "N";
        bool isSetComposition = false;

        while (isSetComposition == false)
        {
            Assemble(Passengers);
            Console.WriteLine("Если поезд сформирован верно и готов к отправке, нажмите Enter, если нет - введите: " + CommandNo);
            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case CommandNo:
                    _railwayCarriages.Clear();
                    break;

                default:
                    isSetComposition = true;
                    break;
            }
        }
    }

    public void SetDirection()
    {
        const string CommandNo = "N";
        bool isSetDirection = false;

        while (isSetDirection == false)
        {
            Console.WriteLine("Задайте направление поезда");
            Console.WriteLine("Откуда: ");
            _direction.Add(ChooseCity());
            Console.WriteLine("Куда: ");
            _direction.Add(ChooseCity());
            Console.WriteLine("Вы указали верное направление? Если да, нажмите enter, если нет введите: " + CommandNo);
            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case CommandNo:
                    _direction.Clear();
                    break;

                default:
                    isSetDirection = true;
                    break;
            }
        }
    }

    public void Depart()
    {
        IsDeparted = true;
    }

    public void ShowTable()
    {
        bool isAsseble = IsAssemble();
        bool isSetDirection = IsSetDirection();
        bool isDeparted = IsDeparted;

        Console.Clear();
        Console.Write("Направление: ");

        if (isSetDirection == true)
        {
            ShowDirection();
        }
        else
        {
            Console.Write("Здесь должно быть направление");
        }

        Console.Write("\nКоличество купленых билетов: " + Passengers);
        Console.Write("\nФормирование поезда: ");

        if (isAsseble == true)
        {
            Console.Write("Поезд сформирован и он имеет ");
            ShowCompositionInformation();
        }
        else
        {
            Console.Write("Поезд не сформирован");
        }

        Console.Write("\nСостояние поезда: ");

        if (isDeparted == true)
        {
            Console.Write("Поезд отправлен");
        }
        else
        {
            Console.Write("Поезд ждет сигнала для отправки");
        }

        Console.SetCursorPosition(0, 15);
    }

    private string ChooseCity()
    {
        string city = Console.ReadLine();
        return city;
    }

    private void Assemble(int numberSeats)
    {
        while (numberSeats > 0)
        {
            bool isFound = false;
            Console.Clear();
            Console.WriteLine(numberSeats + " осталось купленых билетов, для добавление вагона введите его название");

            while (isFound == false)
            {
                ShowType();
                string title = Console.ReadLine();
                foreach (var wagon in _types)
                {
                    if (wagon.Key == title)
                    {
                        _railwayCarriages.Add(new RailwayCarriage(wagon.Key, wagon.Value));
                        numberSeats -= wagon.Value;
                        isFound = true;
                    }
                }
            }
        }
    }

    private void ShowType()
    {
        foreach (var wagon in _types)
        {
            Console.WriteLine("Вагон " + wagon.Key + " вместимостью " + wagon.Value);
        }
    }

    private void AddType()
    {
        _types.Add("small", 50);
        _types.Add("medium", 100);
        _types.Add("large", 150);
    }

    private void ShowDirection()
    {
        for (int i = 0; i < _direction.Count; i++)
        {
            Console.Write(_direction[i] + " ");
        }
    }

    private void ShowCompositionInformation()
    {
        string title = "";
        int numberRailwayCarriage = 0;

        foreach (var type in _types)
        {
            foreach (RailwayCarriage railwayCarriage in _railwayCarriages)
            {
                if (railwayCarriage.Name == type.Key)
                {
                    title = type.Key;
                    numberRailwayCarriage++;
                }
            }

            Console.Write("\n" + numberRailwayCarriage + " " + title + " вагонов");
        }
    }

    private bool IsAssemble()
    {
        return _railwayCarriages.Count > 0;
    }

    private bool IsSetDirection()
    {
        return _direction.Count > 0;
    }
}

class RailwayCarriage
{
    public string Name { get; private set; }
    public int NumberSeats { get; private set; }

    public RailwayCarriage(string name, int numberSeats)
    {
        Name = name;
        NumberSeats = numberSeats;
    }
}

class CashRegister
{
    private Random _random = new Random();

    public int SellTickets()
    {
        int minPassengers = 570;
        int maxPassengers = 1500;
        return _random.Next(minPassengers, maxPassengers);
    }
}
