using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Internals.Fibers;

namespace Keboola.Bot.Dialogs
{
    [Serializable]
    public class EndpointCollection
    {
        public List<Endpoint> Endpoints = new List<Endpoint>();

        public int UncompletedEndpoints
        {
            get { return Endpoints.Count(a => !a.IsComplete()); }
        }

        public int EnpointsCount;
    }

    [Serializable]
    public class Endpoint
    {
        public List<Endpoint> SubEnpoints = new List<Endpoint>();
        public int? SubEnpointsCount;
        public string DataType;
        public string EndpointName;
        public string parentName;
        public bool RootEndpoint;
        //public Endpoint Clone()
        //{
        //    var res = (Endpoint)this.MemberwiseClone();

        //    if (SubEnpoints != null)
        //    {
        //        res.SubEnpoints = new List<Endpoint>();
        //        foreach (var subEnpoint in SubEnpoints)
        //        {
        //            res.SubEnpoints.Add(subEnpoint.Clone());
        //        }
        //    }
        //    return res;
        //}

        public int UncompletedSubEndpoints
        {
            get { return SubEnpoints.Count(a => !a.IsComplete()); }
        }

        public bool IsComplete()
        {
            return SubEnpointsCount != null &&
                   !String.IsNullOrEmpty(DataType) &&
                   !String.IsNullOrEmpty(EndpointName) &&
                   (SubEnpointsCount == SubEnpoints.Count);
        }
    }

    [Serializable]
    public class EndpointDialog : IDialog<object>
    {
        EndpointCollection EndpointCollection;
        public List<Endpoint> EndpointsQueue = new List<Endpoint>();

        public async Task StartAsync(IDialogContext context)
        {
            EndpointCollection = new EndpointCollection();
        //    EndpointsQueue[0] = new Endpoint();

            await context.PostAsync("Add endpoints");
            await context.PostAsync("How many endpoints?");
            context.Wait(AddRootEndpoints);
        }

        public virtual async Task AddRootEndpoints(IDialogContext context,
            IAwaitable<IMessageActivity> argument)
        {
            var msg = await argument;
            if (!string.IsNullOrEmpty(msg.Text))
            {
                int endpoints = 0;
                if (int.TryParse(msg.Text, out endpoints))
                {
                    if (endpoints != 0)
                    {
                        EndpointCollection.EnpointsCount = endpoints;
                        for (int i = 0; i < endpoints; i++)
                        {
                            var newRootEndpoint = new Endpoint() {RootEndpoint = true};
                            EndpointCollection.Endpoints.Add(newRootEndpoint);
                            EndpointsQueue.Add(newRootEndpoint);
                        }

                        var ordinal = string.Empty;
                        //TODO add ordinal Properties.Settings.Default.Ordinals.Split(';')[0];
                        await context.PostAsync($"Add {ordinal}endpoint");
                        context.Wait(AddEnpoint);
                    }
                    else
                    {
                        await context.PostAsync("Minimum endpoints is 1. How many endpoints?");
                        context.Wait(AddRootEndpoints);
                    }
                }
                else
                {
                    await context.PostAsync("Don't understand. How many endpoints?");
                    context.Wait(AddRootEndpoints);
                }
            }
        }


        public virtual async Task AddEnpoint(IDialogContext context,
           IAwaitable<IMessageActivity> argument)
        {

            var msg = await argument;
        
            if (string.IsNullOrEmpty(EndpointsQueue[0].EndpointName))
            {
                EndpointsQueue[0].EndpointName = msg.Text;
                await context.PostAsync("DataType?");
                context.Wait(AddEndpointToCollection);
            }
            else if (string.IsNullOrEmpty(EndpointsQueue[0].DataType))
            {
                EndpointsQueue[0].DataType = msg.Text;
                await context.PostAsync("How many subendpoints?");
                context.Wait(AddEndpointToCollection);
            }
            else if (EndpointsQueue[0].SubEnpointsCount == null)
            {
                int subEndpoints;
                if (!int.TryParse(msg.Text, out subEndpoints))
                {
                    await context.PostAsync("Don't understand. How many subendpoints?");
                    context.Wait(AddEndpointToCollection);
                }
                else
                {
                    EndpointsQueue[0].SubEnpointsCount = subEndpoints;
                    if (subEndpoints != 0)
                    {
                        for(int i = 0; i < subEndpoints ; i++)
                        {
                            var newSubEndpoint = new Endpoint();
                            newSubEndpoint.parentName = EndpointsQueue[0].EndpointName;
                            EndpointsQueue[0].SubEnpoints.Add(newSubEndpoint);
                            EndpointsQueue.Insert(1,newSubEndpoint);

                        }
                    }
                    PromptDialog.Confirm(
                        context,
                        ConfirmEndpoint,
                        $"Is this your selection?\n\n* Endpoint: {EndpointsQueue[0].EndpointName} \n\n* DataType: {EndpointsQueue[0].DataType}",
                        "Didn't get that!",
                        promptStyle: PromptStyle.None);
                }
            }
        }

        public async Task ConfirmEndpoint(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                EndpointsQueue.RemoveAt(0);
                if (EndpointsQueue.Count != 0)
                {
                    var nextEndpoint = EndpointsQueue[0];
                    await GenerateAddtext(context, nextEndpoint);
                    context.Wait(AddEnpoint);
                }
                else
                {
                    await context.PostAsync($"Generate Config!!");
                    context.Done(EndpointCollection);
                }
            }
            else
            {
                EndpointsQueue[0].EndpointName = null;
                EndpointsQueue[0].SubEnpointsCount = null;
                EndpointsQueue[0].DataType = null;
                EndpointsQueue[0].SubEnpoints = new List<Endpoint>();
                await GenerateAddtext(context, EndpointsQueue[0]);
                context.Wait(AddEnpoint);
            }
        }

        private static async Task GenerateAddtext(IDialogContext context, Endpoint endpoint)
        {
            //TODO add ordinal text
            var ordinal = string.Empty;  // Properties.Settings.Default.Ordinals.Split(';')[(endpoint.SubEnpoints.Count)];
            string sub = endpoint.RootEndpoint ? string.Empty : "sub";
            string parent = endpoint.RootEndpoint ? string.Empty : $"for \"{endpoint.parentName}\"";
            await context.PostAsync($"Add {ordinal}{sub}endpoint {parent}");
        }

        public virtual async Task AddEndpointToCollection(IDialogContext context,
            IAwaitable<IMessageActivity> argument)
        {
            if (EndpointCollection.UncompletedEndpoints > 0)
            {
                await AddEnpoint(context, argument);
            }
            else
            {
                context.Done(EndpointCollection);
            }
        }

        private void AddEnpoint()
        {
            throw new NotImplementedException();
        }
    }
}