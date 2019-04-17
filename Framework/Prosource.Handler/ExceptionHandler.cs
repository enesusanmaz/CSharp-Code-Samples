using Common.Contract.Base;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Prosource.Handler
{
    public class ExceptionHandler : IPipeLinePostRequestHandler
    {
        public Task PostRequest(string serviceName, RequestBase request, ResponseBase response)
        {
            Task.Run(() => {
                if (response.HasException)
                {
                    TelemetryClient _telemetryClient = new TelemetryClient();
                    _telemetryClient.InstrumentationKey = AppConfig.AppConfigurationService.ApplicationInsightsConfiguration.GetInstrumentationKey();

                    var telemetry = new ExceptionTelemetry();
                    string userID = "";
                    if (request.Header != null)
                    {
                        userID = request.Header.UserId.ToString();
                    }

                    telemetry.Properties.Add("User_ID", userID);
                    telemetry.Properties.Add("REQUEST", Newtonsoft.Json.JsonConvert.SerializeObject(request));
                    telemetry.Properties.Add("RESPONSE", Newtonsoft.Json.JsonConvert.SerializeObject(response));                    

                    _telemetryClient.Context.Properties.Add("User_ID", userID);
                    _telemetryClient.Context.User.AccountId = userID;
                    _telemetryClient.Context.User.AuthenticatedUserId = userID;
                    _telemetryClient.Context.User.Id = userID;

                    foreach (var ex in response.Exceptions)
                    {
                        telemetry.Exception = ex;
                        _telemetryClient.TrackException(telemetry);
                    }
                }
            }).ContinueWith(t => {
                if (t.IsFaulted)
                {
                    //farklı bir yere yazılabilir.
                }
            });

            return Task.CompletedTask;
        }
    }
}
