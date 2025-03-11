// Создаем компоненты с использованием Dependency Injection
IGameSettings settings = new GameSettings(1, 100, 7);
INumberGenerator generator = new RandomNumberGenerator();
IUserInterface ui = new ConsoleUserInterface();
    
// Собираем игру из компонентов
IGame game = new NumberGuessingGame(settings, generator, ui);
    
// Запускаем игру
game.Play();