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
        private readonly Random myRandom = new Random();

        public int Generate(int min, int max)
        {
            return myRandom.Next(min, max + 1);
        }
    }

    public class Game
    {
        private const int MIN_START_INTERVAL = -1000;
        private const int MAX_START_INTERVAL = -10;
        private const int MIN_END_INTERVAL = 10;
        private const int MAX_END_INTERVAL = 1000;
        private const int MIN_COUNT_ATTEMPTS = 1;

        private readonly IUserInput userInput;
        private readonly IRandomNumberGenerator randomNumberGenerator;

        public Game(IUserInput userInput, IRandomNumberGenerator randomNumberGenerator)
        {
            this.userInput = userInput;
            this.randomNumberGenerator = randomNumberGenerator;
        }

        public void Start()
        {
            int rangeStart = userInput.inputInterval($"Введите начало диапазона: [{MIN_START_INTERVAL}; {MAX_START_INTERVAL}]", MIN_START_INTERVAL, MAX_START_INTERVAL);
            int rangeEnd = userInput.inputInterval($"Введите конец диапазона: [{MIN_END_INTERVAL}; {MAX_END_INTERVAL}]", MIN_END_INTERVAL, MAX_END_INTERVAL);

            int maxCountAttempts = rangeEnd - rangeStart - 1;
            int attempts = userInput.inputInterval($"Введите количество попыток: [{MIN_COUNT_ATTEMPTS}; {maxCountAttempts}]", MIN_COUNT_ATTEMPTS, maxCountAttempts);

            int numberToGuess = randomNumberGenerator.Generate(rangeStart, rangeEnd);

            userInput.ShowMessageSuccess($"Угадайте число на диапазоне: [{rangeStart}; {rangeEnd}] у Вас максимум {attempts} попыток");
            bool isGuessed = false;
            int currentCountAttempts = 0;
            for (int i = 1; i <= attempts; ++i)
            {
                int playerGuess = userInput.inputInt($"\nПопытка {i}/{attempts}. Введите ваше предположение:");

                if (playerGuess == numberToGuess)
                {
                    userInput.ShowMessageSuccess($"Поздравляем! Вы угадали число с {currentCountAttempts + 1} попыток!");
                    isGuessed = true;
                    break;
                }
                else if (playerGuess < numberToGuess)
                {
                    ++currentCountAttempts;
                    userInput.ShowMessage("Загаданное число больше.");
                }
                else
                {
                    ++currentCountAttempts;
                    userInput.ShowMessage("Загаданное число меньше.");
                }
            }

            if (!isGuessed)
            {
                userInput.ShowMessageError($"Вы проиграли. Загаданное число было: {numberToGuess}");
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
