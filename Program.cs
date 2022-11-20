using System;
using System.Collections.Generic;

internal class Program
{
    static void Main(string[] args)
    {
        User user = new User();

        user.Work();
    }
}

class User
{
    private List<City> _direction = new List<City>();
    private Passengers _passengers = new Passengers();
    private Train _train = new Train();
    private RailwayCarriage _railwayCarriage = new RailwayCarriage();
    private RailwayCarriageSmall _railwayCarriageSmall = new RailwayCarriageSmall();
    private int _ticketCount = 0;

    private void ShowTable()
    {
        bool isAsseble = _train.IsAssemble();
        bool isDirection = IsDirection();
        bool isDeparted = _train.IsDeparted;
        int railwayCarriageCount = _train.RailwayCarriageCount;
        int railwayCarriageSpaciousness = _railwayCarriage.NumberSeats;
        int railwayCarriageSmallCount = _train.RailwayCarriageSmallCount;
        int railwayCarriageSmallSpaciousness = _railwayCarriageSmall.NumberSeats;
        Console.Clear();
        Console.Write("Направление: ");

        if (isDirection == true)
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
            ShowRailwayCarriage(railwayCarriageCount, railwayCarriageSpaciousness);
            ShowRailwayCarriage(railwayCarriageSmallCount, railwayCarriageSmallSpaciousness);
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
    }

    public void Work()
    {
        const string Direction = "Direction";
        const string TicketsSell = "Sell";
        const string Form = "Form";
        const string Depart = "Depart";
        const string Exit = "Exit";
        const string NewDirection = "New";
        bool isExit = false;

        while (isExit == false)
        {
            ShowTable();

            Console.SetCursorPosition(15, 15);
            Console.WriteLine("\nЗадать направление: " + Direction + "\nПродать билеты: " + TicketsSell +
                "\nСформировать поезд: " + Form + "\nОтправить поезд: " + Depart + "\nЗадать новое направление: " +
                NewDirection + "\nВыйти из программы: " + Exit);
            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case Direction:
                    SetDirection();
                    break;

                case TicketsSell:
                    SellTickets();
                    break;

                case Form:
                    FormTrain();
                    break;

                case Depart:
                    _train.Depart();
                    break;

                case NewDirection:
                    ResetDirection();
                    break;

                case Exit:
                    isExit = true;
                    break;
            }
        }
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

    private void ShowDirection()
    {
        foreach (City city in _direction)
        {
            Console.Write(city.Name + " ");
        }
    }

    private bool IsDirection()
    {
        return _direction.Count > 0;
    }

    private void SellTickets()
    {
        bool isDirection = IsDirection();

        if (isDirection == false)
        {
            Console.WriteLine("Прежде чем продавать билеты, нужно указать направление");
            Console.ReadKey();
        }
        else
        {
            _ticketCount = _passengers.Amount;
        }
    }

    private void FormTrain()
    {
        if (_ticketCount == 0)
        {
            Console.WriteLine("Сперва продайте билеты, чтобы знать кол-во нужных мест");
            Console.ReadKey();
        }
        else
        {
            _train.Assemble(_ticketCount);
        }
    }

    private void ShowRailwayCarriage(int railwayCarriageCount, int railwayCarriageSpaciousness)
    {
        Console.Write("\n" + railwayCarriageCount + " вагонов, вместительностью " + railwayCarriageSpaciousness + " человек");
    }

    private void ResetDirection()
    {
        bool isDeparted = _train.IsDeparted;

        if (isDeparted == true)
        {
            _direction.Clear();
            _train.Reset();
            _ticketCount = 0;
        }
        else
        {
            Console.WriteLine("Вы не отправили предыдущий поезд");
            Console.ReadKey();
        }
    }
}

class Train
{
    private List<RailwayCarriage> _train = new List<RailwayCarriage>();
    private RailwayCarriage _railwayCarriage = new RailwayCarriage();
    private RailwayCarriageSmall _railwayCarriageSmall = new RailwayCarriageSmall();
    public bool IsDeparted { get; private set; }
    public int RailwayCarriageCount { get; private set; }
    public int RailwayCarriageSmallCount { get; private set; }

    public Train()
    {
        _train = new List<RailwayCarriage>();
        IsDeparted = false;
        RailwayCarriageCount = 0;
        RailwayCarriageSmallCount = 0;
    }

    public void Assemble(int numberSeats)
    {
        int railwayCarriageSpaciousness = _railwayCarriage.NumberSeats;
        int railwayCarriageSmallSpaciousness = _railwayCarriageSmall.NumberSeats;

        while (numberSeats > 0)
        {
            if (numberSeats > railwayCarriageSpaciousness)
            {
                numberSeats -= railwayCarriageSpaciousness;
                _train.Add(new RailwayCarriage());
                RailwayCarriageCount++;
            }
            else
            {
                if (numberSeats > railwayCarriageSmallSpaciousness)
                {
                    numberSeats -= railwayCarriageSpaciousness;
                    _train.Add(new RailwayCarriage());
                    RailwayCarriageCount++;
                }
                else
                {
                    numberSeats -= railwayCarriageSmallSpaciousness;
                    _train.Add(new RailwayCarriageSmall());
                    RailwayCarriageSmallCount++;
                }
            }
        }
    }

    public bool IsAssemble()
    {
        return _train.Count > 0;
    }

    public void Reset()
    {
        _train.Clear();
        IsDeparted = false;
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
    public int NumberSeats { get; protected set; }

    public RailwayCarriage()
    {
        NumberSeats = 80;
    }
}

class RailwayCarriageSmall : RailwayCarriage
{
    public RailwayCarriageSmall()
    {
        NumberSeats = 50;
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

class Passengers
{
    private Random _random = new Random();
    public int Amount { get; private set; }

    public Passengers()
    {
        int minPassengers = 570;
        int maxPassengers = 1500;
        Amount = _random.Next(minPassengers, maxPassengers);
    }
}
