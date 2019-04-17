using Common.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProSource.Helper
{
    public static class TimeZoneHelper
    {
        private static Dictionary<int, string> timeZoneList = null;
        public static string GetTimeZoneNameByID(int id)
        {
            if (timeZoneList == null)
            {
                FillTimeZone();
            }

            string timeZone = "";

            if (timeZoneList.TryGetValue(id, out timeZone))
            {
                return timeZone;
            }

            return AppConfig.AppConfigurationService.TimeZoneConfiguration.GetDefault();
        }
        public static void FillTimeZone()
        {
            if (timeZoneList == null)
            {
                timeZoneList = new Dictionary<int, string>();

                var req = new GetTimezoneListRequest();
                var resp = Dispatcher.DispatcherEngine.Current.Execute<GetTimezoneListRequest, GetTimezoneListResponse>(req);

                if (!resp.HasErrorMessage)
                {
                    if (resp.Data != null)
                    {
                        foreach (var item in resp.Data)
                        {
                            if (string.IsNullOrWhiteSpace(item.WindowsIdentifiers))                            
                                continue;
                            
                            timeZoneList.Add(item.ID, item.WindowsIdentifiers);
                        }
                    }
                }
            }
        }
    }
}
