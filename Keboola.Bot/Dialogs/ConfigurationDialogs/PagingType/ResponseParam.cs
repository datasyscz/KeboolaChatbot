using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.PagingType
{
    [Serializable]
    public class ResponseParam
    {
        public string ResponseParamStr;
        public string QueryParam;
        public string ScrollRequestEndpoint;
        public string ScrollRequestMethod;
        public string ScrollRequestParams;

        public static IForm<ResponseParam> BuildForm()
        {
            return new FormBuilder<ResponseParam>()
                .Field(nameof(ResponseParamStr), "responseparam")
                .Field(nameof(QueryParam), "responseparam queryparam")
                .Field(nameof(ScrollRequestEndpoint), "responseparam scrollrequestendpoint")
                .Field(nameof(ScrollRequestMethod), "responseparam scrollrequestmethod")
                .Field(nameof(ScrollRequestParams), "responseparam scrollrequestparams")
                .Confirm("Is this your selection?\n{*}")
                .Build();
        }
    }
}