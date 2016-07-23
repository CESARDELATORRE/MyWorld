
using System.Threading.Tasks;
using System.Web.Http;
using System.Diagnostics;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;

//(OLD-BreakingChange?) using Microsoft.Bot.Connector.Utilities;


namespace CESARDLBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {

        //public async Task<Message> Post([FromBody]Message message)
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //Call to L.U.I.S. providing the message so he can understand what's the message about
                //L.U.I.S. will call back to my PersonalDataProviderDialog callback methods/properties
                //This is how I published it to AZURE    

                //await Conversation.SendAsync(activity, () => new PersonalDataProviderDialog());

                
                //Manual dialog -----------------------------------------------------------------------
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                string retMessage = string.Empty;

                // Calculate something static for us to return
                int length = (activity.Text ?? string.Empty).Length;
                retMessage = $"Howdy! - You said '{activity.Text}' which was {length} characters";

                //Temporal Hardcoded Test querying my Service Fabric services
                //PersonalDataProviderDialog MyTempDialog = new PersonalDataProviderDialog();
                //retMessage = await MyTempDialog.TryFindVehicleVIN("camaro", "CDLTLL");

                // return our reply to the chat user
                Activity reply = activity.CreateReply(retMessage);
                await connector.Conversations.ReplyToActivityAsync(reply);
                //-----------------------------------------------------------------------
                
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);

            return response;

        }


        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        //(CDLTLL) With no custom code, just call to L.U.I.S. Dialog
        //[ResponseType(typeof(void))]
        //public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        //{
        //    if (activity != null)
        //    {
        //        // one of these will have an interface and process it
        //        switch (activity.GetActivityType())
        //        {
        //            case ActivityTypes.Message:
        //                await Conversation.SendAsync(activity, () => new PersonalDataProviderDialog());
        //                break;

        //            case ActivityTypes.ConversationUpdate:
        //            case ActivityTypes.ContactRelationUpdate:
        //            case ActivityTypes.Typing:
        //            case ActivityTypes.DeleteUserData:
        //            default:
        //                Trace.TraceError($"Unknown activity type ignored: {activity.GetActivityType()}");
        //                break;
        //        }
        //    }
        //    return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        //}
    }
}