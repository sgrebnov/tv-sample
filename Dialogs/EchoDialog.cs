using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using AdaptiveCards;
using System.Collections.Generic;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {


            var message = await argument;

            if ("Switch to HBO".Equals(message.Text)) {
                await context.PostAsync("Got it, switching TV channel to HBO..");
                return;
            }

            var welcomeMessage = context.MakeMessage();
            welcomeMessage.Text = "Here are recommended for you programs:";

            await context.PostAsync(welcomeMessage);

            await this.DisplayOptionsAsync(context);
            //context.Wait(MessageReceivedAsync);

        }

        
        public async Task DisplayOptionsAsync(IDialogContext context)
        {

            var reply = context.MakeMessage();

            reply.Attachments = new List<Attachment>();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            reply.Attachments.Add(
                new HeroCard
                {
                    Title = "HBO | Game of Thrones | 67: The Dragon and the Wolf",
                    Subtitle = "Started 60 mins ago",
                    Text = "The seventh and final episode of the seventh season of Game of Thrones. It is the sixty-seventh episode of the series overall.",
                    Images = new List<CardImage> { new CardImage("http://i.lv3.hbo.com/assets/images/series/game-of-thrones/episodes/7/67/episode-67-300.jpg") },
                    Buttons = new List<CardAction> {
                       new CardAction
                        {
                            Title = "Watch",
                            Value = $"Switch to HBO",
                            Type = ActionTypes.ImBack
                        },
                        new CardAction(ActionTypes.OpenUrl, "Get Info", value: "http://www.hbo.com/game-of-thrones/episodes/7/67-67-episode/index.html")
                    }
                }.ToAttachment());


            reply.Attachments.Add(
               new HeroCard
               {
                   Title = "NBC Sports | NHL | Wild vs. Red Wings",
                   Subtitle = "Will start in 10 mins",
                   Text = "It’s the beginning of a new era for the Red Wings, as they’ll play their first regular season game at Little Caesars Arena tonight.",
                   Images = new List<CardImage> { new CardImage("https://nbcprohockeytalk.files.wordpress.com/2017/10/659417388-e1507212969936.jpg?w=300&crop=1") },
                   Buttons = new List<CardAction> {
                        new CardAction(ActionTypes.ImBack, "Set Reminder"),
                        new CardAction(ActionTypes.OpenUrl, "Get Info", value: "http://nhl.nbcsports.com/2017/10/05/nhl-on-nbcsn-doubleheader-wild-vs-red-wings-flyers-vs-kings/")
                   }
               }.ToAttachment());

            await context.PostAsync(reply);

        }
    }
}