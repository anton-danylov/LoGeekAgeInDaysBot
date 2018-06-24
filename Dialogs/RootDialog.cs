using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;


namespace LoGeekAgeInDaysBot
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Greetings from LoGeekAgeInDaysBot");
            await context.PostAsync("If you send me your birthday, I'll calculate your age in days");

            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> args)
        {
            var message = await args;

            string birthdayStored = null;
            bool isBirthdayStored = context.UserData.TryGetValue("Birthday", out birthdayStored);
            string birthdaySent = message.Text;

            DateTime dateTemp;
            if (!String.IsNullOrEmpty(birthdaySent) && DateTime.TryParse(birthdaySent, out dateTemp)) // user have sent his b-bay
            {
                context.UserData.SetValue("Birthday", birthdaySent);
                isBirthdayStored = true;
                birthdayStored = birthdaySent;
                await context.PostAsync("Thanks, birthday stored!");
            }
            else if (!isBirthdayStored)
            {
                await context.PostAsync("What is your birthday?");
            }

            if (isBirthdayStored)
            {
                DateTime dateStored;
                // Print days since b-day
                if (DateTime.TryParse(birthdayStored, out dateStored))
                {
                    await context.PostAsync($"Your age in days: {(DateTime.Now - dateStored).TotalDays}");
                }
                else
                {
                    await context.PostAsync("Please reenter your birthday");
                }
            }

            context.Wait(MessageReceivedAsync);
        }
    }
}