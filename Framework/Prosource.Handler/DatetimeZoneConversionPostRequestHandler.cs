using Common.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Prosource.Handler
{
    public class DatetimeZoneConversionPostRequestHandler : IPipeLinePostRequestHandler
    {
        private static string defaultTimeZone = AppConfig.AppConfigurationService.TimeZoneConfiguration.GetDefault();
        private static Type DateTimeType { get; set; } = typeof(DateTime);
        private static Type NullableDateTimeType { get; set; } = typeof(DateTime?);

        public Task PostRequest(string serviceName, RequestBase request, ResponseBase response)
        {
            string clientTimeZone = request?.Header?.SelectedTimeZone;            

            if (AppConfig.AppConfigurationService.TimeZoneConfiguration.GetEnabled() == false)
                return Task.CompletedTask;

            if (string.IsNullOrWhiteSpace(clientTimeZone) || string.IsNullOrWhiteSpace(defaultTimeZone))
                return Task.CompletedTask;

            if (clientTimeZone == defaultTimeZone)
                return Task.CompletedTask;

            var responseType = response.GetType();
            var properties   = responseType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            this.ControlDatetime(0, response, properties, clientTimeZone);

            return Task.CompletedTask;
        }

        private void ControlDatetime(int rank, object obj, IEnumerable<PropertyInfo> properties, string clientTimeZone)
        {
            if (obj == null || rank == 5)
            {
                return;
            }
            var _type = obj.GetType().ToString();           
            if (properties == null)
            {
                return;
            }
            
            foreach (var item in properties.Where(c => c.CanWrite && c.GetIndexParameters().Length == 0))
            {
                
                if (item.PropertyType.GetTypeInfo().GetInterface("IEnumerable") != null && item.PropertyType.FullName != "System.String")
                {
                    var listItems = (dynamic)obj.GetType().GetProperty(item.Name).GetValue(obj);
                    if (listItems == null)                    
                        continue;
                    
                    foreach (var listItem in listItems)
                    {
                        var innerProperties = listItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        this.ControlDatetime(rank + 1, listItem, innerProperties, clientTimeZone);
                    }                    
                } 
                else
                {
                    var value = item.GetValue(obj, null);

                    if (item.PropertyType == DateTimeType || item.PropertyType == NullableDateTimeType)
                    {
                        if (value == null)
                            continue;

                        var datetime = (DateTime?)value;

                        if (datetime.Value.TimeOfDay.TotalMilliseconds != 0)
                        {
                            DateTime unspecifiedDateTime = new DateTime(datetime.Value.Ticks, DateTimeKind.Unspecified);                            
                                
                            item.SetValue(obj, TimeZoneInfo.ConvertTime(unspecifiedDateTime,
                                    TimeZoneInfo.FindSystemTimeZoneById(defaultTimeZone),
                                    TimeZoneInfo.FindSystemTimeZoneById(clientTimeZone)));
                        }
                    }
                    else if (!item.PropertyType.IsPrimitive)
                    {
                        var innerProperties = item.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                        this.ControlDatetime(rank + 1, value, innerProperties, clientTimeZone);
                    }
                }                                
            } 
        }
    }
}
