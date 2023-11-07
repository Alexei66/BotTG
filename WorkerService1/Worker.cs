using ConsoleApp1;
using ConsoleApp2;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WorkerService1;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private ITelegramBotClient _botClient;
    private string _botToken;

    public Worker(IConfiguration configuration, ILogger<Worker> logger)
    {
        _logger = logger;
        _configuration = configuration;
        //_botToken = _configuration["BotSettings:TelegramBotToken"];
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service is starting.");
        //string botToken = _configuration["BotSettings:TelegramBotToken"];
        _botToken = _configuration["Logging:BotSettings:TelegramBotToken"];
        _botClient = new TelegramBotClient(_botToken);
        //_logger.LogInformation($"Bot Token: {_botToken}");

        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service is stopping.");
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // _logger.LogInformation("Worker is running.");
        //while (!stoppingToken.IsCancellationRequested)
        //{
        //    try
        //    {
        //        var updates = await _botClient.GetUpdatesAsync();
        //        _logger.LogInformation($"Received {updates.Length} updates.");
        //        //foreach (var update in updates)
        //        //{
        //        //    Task.Run(() => Update(_botClient, update, stoppingToken)).Wait();
        //        //    //await Update(_botClient, update, stoppingToken); // Обработка обновления.
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error processing updates.");
        //    }
        //    await Task.Delay(500, stoppingToken);
        //}

        //var client = new TelegramBotClient(botToken);
        _botClient.StartReceiving(Update, Error);

        while (!stoppingToken.IsCancellationRequested)
        {
            //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(10_000, stoppingToken);
        }
    }

    private async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        var command = update.Type switch
        {
            UpdateType.Message => update.Message.Text,
            UpdateType.CallbackQuery => update.CallbackQuery.Data,
        };

        var chat = update.Type switch
        {
            UpdateType.Message => update.Message.Chat,
            UpdateType.CallbackQuery => update.CallbackQuery.Message.Chat,
        };

        if (command != null)
        {
            _logger.LogInformation($"{chat.FirstName ?? "анон"} {chat.Username ?? "анон"} пишет | {command} ");

            switch (command.ToLower())
            {
                case Constants.Text_Course:

                    botClient.SendTextMessageAsync(chat.Id, "Выберите валюту:", replyMarkup: Buttons.GetButtons());
                    break;

                case Constants.Text_Question:

                    botClient.SendTextMessageAsync(chat.Id, "Вы нажали кнопку которая еще в разработке");
                    break;

                case Constants.USD:

                    var usd = await new GetCoursePrivatBank().UsdCoursePrivBank();
                    botClient.SendTextMessageAsync(chat.Id, $"{usd.ccy} : {usd.buy}/{usd.sale}", replyMarkup: Buttons.GetButtons());
                    break;

                case Constants.EUR:

                    var eur = await new GetCoursePrivatBank().EurCoursePrivBank();
                    botClient.SendTextMessageAsync(chat.Id, $"{eur.ccy} : {eur.buy}/{eur.sale}", replyMarkup: Buttons.GetButtons());
                    break;

                default:
                    await botClient.SendTextMessageAsync(chat.Id, "Знаю команду доллар и евро", replyMarkup: Buttons.GetButtonsKeyboard());
                    break;
            }
        }
        if (update.Message?.Photo != null)
        {
            await botClient.SendTextMessageAsync(chat.Id, "Норм фото");

            _logger.LogInformation($"{chat.FirstName ?? "анон"} {chat.Username ?? "анон"} Отправил фото ");

            return;
        }

        if (update.Message?.Document != null)
        {
            await botClient.SendTextMessageAsync(chat.Id, "Документ принял");

            _logger.LogInformation($"{chat.FirstName ?? "анон"} {chat.Username ?? "анон"} Отправил документ ");

            return;
        }
    }

    private Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();
    }
}