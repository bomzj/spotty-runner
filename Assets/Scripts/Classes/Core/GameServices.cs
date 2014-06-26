using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Classes.Core
{
    /// <summary>
    /// Initialize all needed game services before game starts
    /// </summary>
    public static class GameServices
    {
        private static List<object> services = new List<object>();

        public static void AddService(object service)
        {
            services.Add(service);
        }

        public static T GetService<T>() where T : class
        {
            foreach (var item in services)
            {
                if (item is T) return item as T;
            }
            
            return null;
        }

    }
}
