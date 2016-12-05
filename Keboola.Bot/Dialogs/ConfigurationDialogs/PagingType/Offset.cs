using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot.Dialogs.ConfigurationDialogs.PagingType
{
    [Serializable]
    public class Offset
    {
        public string Limit;
        public string LimitParam;
        public string OffsetParam;

        public static IForm<Offset> BuildForm()
        {
            return new FormBuilder<Offset>()
                .Field(nameof(Limit), "offset limit", validate: new Validation(RootDialog.WitAI).Int)
                .Field(nameof(LimitParam), "offset limitParam", validate: new Validation(RootDialog.WitAI).Int)
                .Field(nameof(OffsetParam), "offset offsetparam", validate: new Validation(RootDialog.WitAI).Int)
                .Confirm("Is this your selection?\n{*}")
                .Build();
        }

        private static Task<ValidateResult> Validate(object state, object value)
        {
            throw new NotImplementedException();
        }
    }
}