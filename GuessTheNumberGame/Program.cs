﻿using System;

namespace GuessTheNumberGame
{
    public interface IUserInput
    {
        int inputInterval(string text, int minValue, int maxValue);
        int inputInt(string text);
        void ShowMessage(string message);
        void ShowMessageError(string message);
        void ShowMessageSuccess(string message);

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

        public void ShowMessage(string message)
        {
            Console.ResetColor();
            Console.WriteLine(message);
        }
        public void ShowMessageError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
        }
        public void ShowMessageSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
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
        private const int START_SHIFT = 5;
        private const int START_INTERVAL = -1000;
        private const int END_INTERVAL = 1000;

        private readonly IUserInput _userInput;
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        public Game(IUserInput userInput, IRandomNumberGenerator randomNumberGenerator)
        {
            _userInput = userInput;
            _randomNumberGenerator = randomNumberGenerator;
        }

        public void Start()
        {
            int rangeStart;
            int rangeEnd;
            int attempts;
            int size;

            while (true)
            {
                rangeStart = _userInput.inputInterval($"Введите начало диапазона: [{START_INTERVAL}; {END_INTERVAL}]", START_INTERVAL, END_INTERVAL);
                rangeEnd = _userInput.inputInterval($"Введите конец диапазона: [{rangeStart + START_SHIFT}; {END_INTERVAL}]", rangeStart + START_SHIFT, END_INTERVAL);
                size = Math.Abs(rangeEnd - rangeStart);
                attempts = _userInput.inputInterval($"Введите количество попыток: [1; {size - 1}]", 1, size - 1);

                if (attempts > size)
                {
                    _userInput.ShowMessageError($"Количество попыток покрывает весь интервал!");
                }
                else
                {
                    break;
                }
            }

            int numberToGuess = _randomNumberGenerator.Generate(rangeStart, rangeEnd);

            _userInput.ShowMessageSuccess($"Угадайте число на диапазоне: [{rangeStart}; {rangeEnd}]");
            bool isGuessed = false;
            int currentCountAttempts = 0;
            for (int i = 1; i <= attempts; ++i)
            {
                int playerGuess = _userInput.inputInt($"Попытка {i}/{attempts}. Введите ваше предположение:");

                if (playerGuess == numberToGuess)
                {
                    _userInput.ShowMessageSuccess($"Поздравляем! Вы угадали число с {currentCountAttempts + 1} попыток!");
                    isGuessed = true;
                    break;
                }
                else if (playerGuess < numberToGuess)
                {
                    ++currentCountAttempts;
                    _userInput.ShowMessage("Загаданное число больше.");
                }
                else
                {
                    ++currentCountAttempts;
                    _userInput.ShowMessage("Загаданное число меньше.");
                }
            }

            if (!isGuessed)
            {
                _userInput.ShowMessageError($"Вы проиграли. Загаданное число было: {numberToGuess}");
            }
        }
    }

    class Program
    {
        public static bool isQuestion(string textQuestion)
        {
            Console.WriteLine("\n" + textQuestion);
            return Console.ReadLine()?.ToLower() != "n";
        }
        static void Main(string[] args)
        {
            Console.ResetColor();
            Console.WriteLine("Ларионов гр. 410з Игра \"Угадай число\"!");

            IUserInput userInput = new ConsoleUserInput();
            IRandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();

            while (true)
            {
                Game game = new Game(userInput, randomNumberGenerator);
                game.Start();

                Console.ResetColor();
                if (!isQuestion("Сыграем ещё раз [y/n]?")) {
                    Console.WriteLine("Спасибо за игру!");
                    break;
                }
            }
        }
    }
}
