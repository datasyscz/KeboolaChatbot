using System;
using System.Diagnostics;
using System.Threading.Tasks;
using API;
using Microsoft.Bot.Builder.FormFlow;

namespace Keboola.Bot
{
    public class Validation
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly WitAI witAI;

        public Validation(WitAI witAI)
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
                    if ((result?.entities?.number != null) && (result?.entities?.number.Length > 0))
                        return new ValidateResult
                        {
                            IsValid = true,
                            Value = result?.entities?.number[0].value.ToString()
                        };
                }
            }
            catch (Exception ex)
            {
                logger.Error("WitAI",ex);
                Debug.Fail(ex.Message);
            }

            //Second try without WitAI
            var outInt = 0;
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