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
        Train _railwayCarriages = new Train();

        _railwayCarriages.ShowTable();
        _railwayCarriages.SetDirection();

        _railwayCarriages.ShowTable();

        Console.WriteLine("Чтобы пустить в продажу билеты, нажмите enter");
        Console.ReadKey();

        _railwayCarriages.GetPassengers();
        _railwayCarriages.ShowTable();

        Console.WriteLine("билеты проданы, осталось сформировать поезд.");

        _railwayCarriages.ShowTable();
        _railwayCarriages.CreateWagons();

        Console.WriteLine("Поезд сформирован, отправляем");
        Console.ReadKey();

        _railwayCarriages.Depart();
        _sentTrains.Add(_railwayCarriages);

        _railwayCarriages.ShowTable();
        _railwayCarriages.Reset();
    }
}

class Train
{
    private List<RailwayCarriage> _railwayCarriages = new List<RailwayCarriage>();
    private List<string> _direction = new List<string>();
    public bool IsDeparted { get; private set; }
    public int Passengers { get; private set; }

    public Train()
    {
        _railwayCarriages = new List<RailwayCarriage>();
        _direction = new List<string>();
        Passengers = 0;
        IsDeparted = false;
    }

    public void Assemble(int numberSeats)
    {
        while (numberSeats > 0)
        {
            const string TitleSmall = "S";
            const string TitleMedium = "M";
            const string TitleLarge = "L";
            int smallCapacity = 50;
            int mediumCapacity = 100;
            int largeCapacity = 150;
            Console.Clear();
            Console.WriteLine(numberSeats + " осталось купленых билетов, для добавление вагона введите соответствующий символ");
            Console.WriteLine(TitleSmall + ": вместимость " + smallCapacity + " человек");
            Console.WriteLine(TitleMedium + ": вместимость " + mediumCapacity + " человек");
            Console.WriteLine(TitleLarge + ": вместимость " + largeCapacity + " человек");
            string dispatcherChoice = Console.ReadLine();

            switch (dispatcherChoice)
            {
                case TitleSmall:
                    numberSeats -= AddRailwatCarriage(TitleSmall, smallCapacity);
                    break;

                case TitleMedium:
                    numberSeats -= AddRailwatCarriage(TitleMedium, mediumCapacity);
                    break;

                case TitleLarge:
                    numberSeats -= AddRailwatCarriage(TitleLarge, largeCapacity);
                    break;
            }
        }
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
        bool checkDirection = false;

        while (checkDirection == false)
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
                    checkDirection = true;
                    break;
            }
        }
    }

    public void GetPassengers()
    {
        CashRegister cashRegister = new CashRegister();
        Passengers = cashRegister.SellTickets();
    }

    public void Reset()
    {
        _railwayCarriages.Clear();
        _direction.Clear();
        IsDeparted = false;
        Passengers = 0;
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

    private int AddRailwatCarriage(string title, int capacity)
    {
        _railwayCarriages.Add(new RailwayCarriage(title, capacity));
        return capacity;
    }

    private string ChooseCity()
    {
        string city = Console.ReadLine();
        return city;
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
        string titleSmall = "S";
        string titleMedium = "M";
        string titleLarge = "L";
        int numberSmallRailwayCarriage = 0;
        int numberMediumRailwayCarriage = 0;
        int numberLargeRailwayCarriage = 0;

        foreach (RailwayCarriage railwayCarriage in _railwayCarriages)
        {
            if (railwayCarriage.Name == titleSmall)
            {
                numberSmallRailwayCarriage++;
            }
            else if (railwayCarriage.Name == titleMedium)
            {
                numberMediumRailwayCarriage++;
            }
            else if (railwayCarriage.Name == titleLarge)
            {
                numberLargeRailwayCarriage++;
            }
        }

        Console.Write("\n" + numberSmallRailwayCarriage + " " + titleSmall + " вагонов");
        Console.Write("\n" + numberMediumRailwayCarriage + " " + titleMedium + " вагонов");
        Console.Write("\n" + numberLargeRailwayCarriage + " " + titleLarge + " вагонов");
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
