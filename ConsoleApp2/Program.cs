using ConsoleApp2;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace ConsoleApp1;

internal partial class Program
{
    private static async Task Main(string[] args)
    {
        var client = new TelegramBotClient(File.ReadAllText("D:\\desk\\testc\\network_test\\ConsoleApp1\\TG_API.txt"));

        client.StartReceiving(Update, Error);

        Console.ReadLine();
    }

    private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
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

        Console.WriteLine($"{chat.FirstName ?? "анон"} {chat.Username ?? "анон"} | {command}");

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

        if (update.Message?.Photo != null)
        {
            await botClient.SendTextMessageAsync(chat.Id, "Норм фото");
            return;
        }

        if (update.Message?.Document != null)
        {
            await botClient.SendTextMessageAsync(chat.Id, "Документ принял");
            return;
        }
    }

    private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();
    }
}