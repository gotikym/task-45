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
    private Train _railwayCarriages = new Train();
    private List<City> _direction = new List<City>();
    private CashRegister _cashRegister = new CashRegister();
    private int _ticketCount = 0;

    public void Work()
    {
        const string CommandStart = "start";
        const string CommandExit = "exit";
        bool isExit = false;

        while (isExit == false)
        {
            ShowTable();
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
        const string CommandNo = "N";
        bool checkDirection = false;
        bool checkComposition = false;

        ShowTable();

        while (checkDirection == false)
        {
            SetDirection();
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

        ShowTable();
        Console.WriteLine("Чтобы пустить в продажу билеты, нажмите enter");
        Console.ReadKey();

        SellTickets();

        ShowTable();

        Console.WriteLine("билеты проданы, осталось сформировать поезд.");

        while (checkComposition == false)
        {
            _railwayCarriages.Assemble(_ticketCount);
            Console.WriteLine("Если поезд сформирован верно и готов к отправке, нажмите Enter, если нет - введите: " + CommandNo);
            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case CommandNo:
                    _railwayCarriages.Reset();
                    break;

                default:
                    checkComposition = true;
                    break;
            }
        }

        ShowTable();
        Console.WriteLine("Поезд сформирован, отправляем");

        _railwayCarriages.Depart();
        _sentTrains.Add(_railwayCarriages);

        ShowTable();
        Console.ReadKey();

        ResetPlan();
    }

    private void SetDirection()
    {
        Console.WriteLine("Задайте направление поезда");
        Console.WriteLine("Откуда: ");
        _direction.Add(ChooseCity());
        Console.WriteLine("Куда: ");
        _direction.Add(ChooseCity());
    }

    private City ChooseCity()
    {
        string city = Console.ReadLine();
        return new City(city);
    }

    private void SellTickets()
    {
        _ticketCount = _cashRegister.SellTickets();
    }

    private void ResetPlan()
    {
        _direction.Clear();
        _railwayCarriages.Reset();
        _ticketCount = 0;
    }

    private void ShowTable()
    {
        bool isAsseble = _railwayCarriages.IsAssemble();
        bool checkDirection = CheckDirection();
        bool isDeparted = _railwayCarriages.IsDeparted;

        Console.Clear();
        Console.Write("Направление: ");

        if (checkDirection == true)
        {
            ShowDirection();
        }
        else
        {
            Console.Write("Здесь должно быть направление");
        }


        Console.Write("\nКоличество купленых билетов: " + _ticketCount);
        Console.Write("\nФормирование поезда: ");

        if (isAsseble == true)
        {
            Console.Write("Поезд сформирован и он имеет ");
            _railwayCarriages.ShowCompositionInformation();
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

    private bool CheckDirection()
    {
        return _direction.Count > 0;
    }

    private void ShowDirection()
    {
        foreach (City city in _direction)
        {
            Console.Write(city.Name + " ");
        }
    }
}

class Train
{
    private List<RailwayCarriage> _railwayCarriages = new List<RailwayCarriage>();
    public bool IsDeparted { get; private set; }
    public int TicketCount { get; private set; }
    public int RailwayCarriageSmallCount { get; private set; }
    public int RailwayCarriageMediumCount { get; private set; }
    public int RailwayCarriageLargeCount { get; private set; }

    public Train()
    {
        List<City> Direction = new List<City>();/*direction;*/
        TicketCount = 0;
        _railwayCarriages = new List<RailwayCarriage>();
        IsDeparted = false;
        RailwayCarriageSmallCount = 0;
        RailwayCarriageMediumCount = 0;
        RailwayCarriageLargeCount = 0;
    }

    public void Assemble(int numberSeats)
    {
        while (numberSeats > 0)
        {
            const string ChoiceSmall = "S";
            const string ChoiceMedium = "M";
            const string ChoiceLarge = "L";
            const string TitleSmall = "small";
            const string TitleMedium = "medium";
            const string TitleLarge = "large";
            const int SmallCapacity = 50;
            const int MediumCapacity = 100;
            const int LargeCapacity = 150;
            Console.Clear();
            Console.WriteLine(numberSeats + " осталось купленых билетов, для добавление вагона введите соответствующий символ");
            Console.WriteLine(TitleSmall + ": " + ChoiceSmall + " вместимость: " + SmallCapacity);
            Console.WriteLine(TitleMedium + ": " + ChoiceMedium + " вместимость: " + MediumCapacity);
            Console.WriteLine(TitleLarge + ": " + ChoiceLarge + " вместимость: " + LargeCapacity);
            string dispatcherChoice = Console.ReadLine();

            if (dispatcherChoice == ChoiceSmall)
            {
                _railwayCarriages.Add(new RailwayCarriage(TitleSmall, SmallCapacity));
                numberSeats -= SmallCapacity;
                RailwayCarriageSmallCount++;
            }
            else if (dispatcherChoice == ChoiceMedium)
            {
                _railwayCarriages.Add(new RailwayCarriage(TitleMedium, MediumCapacity));
                numberSeats -= MediumCapacity;
                RailwayCarriageMediumCount++;
            }
            else if (dispatcherChoice == ChoiceLarge)
            {
                _railwayCarriages.Add(new RailwayCarriage(TitleLarge, LargeCapacity));
                numberSeats -= LargeCapacity;
                RailwayCarriageLargeCount++;
            }
        }
    }

    public void ShowCompositionInformation()
    {
        const string TitleSmall = "small";
        const string TitleMedium = "medium";
        const string TitleLarge = "large";
        const int SmallCapacity = 50;
        const int MediumCapacity = 100;
        const int LargeCapacity = 150;
        Console.Write("\n" + RailwayCarriageSmallCount + " " + TitleSmall + " вагонов, вместительностью: " + SmallCapacity + " человек");
        Console.Write("\n" + RailwayCarriageMediumCount + " " + TitleMedium + " вагонов, вместительностью: " + MediumCapacity + " человек");
        Console.Write("\n" + RailwayCarriageLargeCount + " " + TitleLarge + " вагонов, вместительностью: " + LargeCapacity + " человек");
    }

    public bool IsAssemble()
    {
        return _railwayCarriages.Count > 0;
    }

    public void Reset()
    {
        _railwayCarriages.Clear();
        IsDeparted = false;
        RailwayCarriageSmallCount = 0;
        RailwayCarriageMediumCount = 0;
        RailwayCarriageLargeCount = 0;
    }

    public void Depart()
    {
        bool isAssemble = IsAssemble();

        if (isAssemble == false)
        {
            Console.WriteLine("Перед отправкой, необходимо сформировать поезд");
            Console.ReadKey();
        }
        else
        {
            IsDeparted = true;
        }
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

class City
{
    public string Name { get; private set; }

    public City(string name)
    {
        Name = name;
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
