using System;

namespace GuessTheNumberGame
{
    public interface IUserInput
    {
        int inputInterval(string text, int minValue, int maxValue);
        int inputInt(string text);
        void DisplayMessage(string message);
    }

    public class ConsoleUserInput : IUserInput
    {
        public int inputInterval(string text, int minValue, int maxValue)
        {

            string xStr = "";
            bool isNumber = false;
            int x = 0;

            while (true)
            {
                Console.ResetColor();
                Console.WriteLine(text);

                xStr = Console.ReadLine();
                isNumber = int.TryParse(xStr, out x);

                if (!isNumber)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{xStr} - не число\n");
                }
                else if (x < minValue || x > maxValue)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Введите число в промежутке от {minValue} до {maxValue} включительно!\n");
                }
                else
                    break;
            }

            return x;
        }

        public int inputInt(string text)
        {
            string xStr;
            bool isNumber;
            int x;

            while (true)
            {
                Console.ResetColor();
                Console.WriteLine(text);

                xStr = Console.ReadLine();
                isNumber = int.TryParse(xStr, out x);

                if (!isNumber)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{xStr} - не число\n");
                }
                else
                    break;
            }

            return x;
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }

    public interface IRandomNumberGenerator
    {
        int Generate(int min, int max);
    }

    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly Random _random = new Random();

        public int Generate(int min, int max)
        {
            return _random.Next(min, max + 1);
        }
    }

    public class Game
    {
        private const int START_INTERVAL = -1000;
        private const int END_INTERVAL = 1000;
        private const int COUNT_ATTEMPS = 10;

        private readonly IUserInput _userInput;
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        public Game(IUserInput userInput, IRandomNumberGenerator randomNumberGenerator)
        {
            _userInput = userInput;
            _randomNumberGenerator = randomNumberGenerator;
        }

        public void Start()
        {
            _userInput.DisplayMessage("Ларионов гр. 410з Игра \"Угадай число\"!");

            int rangeStart = _userInput.inputInterval("Введите начало диапазона:", START_INTERVAL, END_INTERVAL);
            int rangeEnd = _userInput.inputInterval("Введите конец диапазона:", START_INTERVAL, END_INTERVAL);
            int attempts = _userInput.inputInterval("Введите количество попыток:", 1, COUNT_ATTEMPS);

            int numberToGuess = _randomNumberGenerator.Generate(rangeStart, rangeEnd);

            bool isGuessed = false;
            for (int i = 1; i <= attempts; i++)
            {
                int playerGuess = _userInput.inputInt($"Попытка {i}/{attempts}. Введите ваше предположение:");

                if (playerGuess == numberToGuess)
                {
                    _userInput.DisplayMessage("Поздравляем! Вы угадали число.");
                    isGuessed = true;
                    break;
                }
                else if (playerGuess < numberToGuess)
                {
                    _userInput.DisplayMessage("Загаданное число больше.");
                }
                else
                {
                    _userInput.DisplayMessage("Загаданное число меньше.");
                }
            }

            if (!isGuessed)
            {
                _userInput.DisplayMessage($"Вы проиграли. Загаданное число было: {numberToGuess}");
            }

            _userInput.DisplayMessage("Спасибо за игру!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IUserInput userInput = new ConsoleUserInput();
            IRandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();

            Game game = new Game(userInput, randomNumberGenerator);
            game.Start();
        }
    }
}
