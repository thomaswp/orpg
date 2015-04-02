using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Utils
{
    public static class UsageData
    {
        private static DateTime start = DateTime.Now;
        private static Dictionary<String, TimeSpan> usage = new Dictionary<string,TimeSpan>();
        private static Dictionary<String, DateTime> starts = new Dictionary<string,DateTime>();

        public static void StartUsage(String name)
        {
            if (starts.Keys.Contains(name))
                starts.Remove(name);    
            starts.Add(name, DateTime.Now);
        }

        public static double EndUsage(String name)
        {
            if (usage.Keys.Contains(name))
            {
                usage[name] += (DateTime.Now - starts[name]);
            }
            else
            {
                usage.Add(name, (DateTime.Now - starts[name]));
            }

            return GetUsage(name);
        }

        public static double GetUsage(String name)
        {
            double total = (DateTime.Now - start).TotalMilliseconds;
            double use = usage[name].TotalMilliseconds;
            return use / total;
        }

        public static TimeSpan GetTotalUsage(String name)
        {
            return usage[name];
        }
    }
}
