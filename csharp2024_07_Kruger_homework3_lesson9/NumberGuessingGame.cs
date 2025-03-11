/// <summary>
/// Single Responsibility Principle: отвечает только за логику Игры
/// Dependency Inversion Principle: зависит от абстракций, а не конкретных классов
/// Open/Closed Principle: открыт для расширения, закрыт для модификации
/// </summary>
public class NumberGuessingGame : IGame
{
    private readonly IGameSettings _settings;
    private readonly INumberGenerator _numberGenerator;
    private readonly IUserInterface _ui;

    public NumberGuessingGame(
        IGameSettings settings,
        INumberGenerator numberGenerator,
        IUserInterface ui)
    {
        _settings = settings;
        _numberGenerator = numberGenerator;
        _ui = ui;
    }

    public void Play()
    {
        int targetNumber = _numberGenerator.GenerateNumber(_settings.MinNumber, _settings.MaxNumber);
        int attemptsLeft = _settings.MaxAttempts;
        int currentPlayer = 1;

        _ui.DisplayMessage(
            $"Игра началась! Я загадал число от {_settings.MinNumber} до {_settings.MaxNumber}.");
        _ui.DisplayMessage($"У вас {_settings.MaxAttempts} попыток.");

        while (attemptsLeft > 0)
        {
            _ui.DisplayMessage($"\nХодит игрок {currentPlayer}. Осталось попыток: {attemptsLeft}");
            int guess = _ui.GetUserInput("Введите ваше предположение: ");

            if (guess == targetNumber)
            {
                _ui.DisplayMessage($"Поздравляем, игрок {currentPlayer}! Вы угадали число {targetNumber}!");
                return;
            }

            string hint = guess < targetNumber ? "меньше" : "больше";
            _ui.DisplayMessage($"Ваше число {hint} загаданного.");

            attemptsLeft--;
            currentPlayer = currentPlayer == 1 ? 2 : 1; // Чередование игроков
        }

        _ui.DisplayMessage($"Игра окончена. Никто не угадал число {targetNumber}.");
    }
}