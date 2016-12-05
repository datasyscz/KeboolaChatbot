using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Keboola.Bot.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Framework.Logging;

namespace Keboola.Bot
{
    public class Validation
    {
        private API.WitAI witAI;

        public Validation(API.WitAI witAI)
        {
            this.witAI = witAI;
        }

        public async Task<ValidateResult> Int(object state, object value)
        {
            //Try use witAI
            try
            {
                if (witAI != null)
                {
                    var result = await witAI.Analyze((string) value);
                    if (result?.entities?.number != null && result?.entities?.number.Length > 0)
                        return new ValidateResult
                        {
                            IsValid = true,
                            Value = result?.entities?.number[0].value.ToString()
                        };
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }

            //Second try without WitAI
            int outInt = 0;
            if (int.TryParse((string) value, out outInt))
                return new ValidateResult
                {
                    IsValid = true,
                    Value = outInt.ToString()
                };

            return new ValidateResult {IsValid = false, Value = (string) value + " is not number."};
        }
    }
}