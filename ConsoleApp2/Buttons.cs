using Telegram.Bot.Types.ReplyMarkups;
using static ConsoleApp1.Program;

namespace ConsoleApp2;

public class Buttons
{
    public static IReplyMarkup GetButtons()
    {
        return new InlineKeyboardMarkup(new[]
        {
        new []
        {
             InlineKeyboardButton.WithCallbackData("💰",Constants.USD )
        },
        new []
        {
             InlineKeyboardButton.WithCallbackData("💶",Constants.EUR )
        }
    });
    }

    public static IReplyMarkup GetButtonsKeyboard()
    {
        return new ReplyKeyboardMarkup(new List<List<KeyboardButton>>
    {
        new List<KeyboardButton> { new KeyboardButton(Constants.Text_Course), new KeyboardButton(Constants.Text_Question) }
    });
    }
}