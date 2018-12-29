using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Timers;

namespace WindowsServiceManager
{
    public class ServiceManager
    {
        static Timer timer;
        static Action _operation;
        public ServiceManager(Action operation)
        {
            _operation = operation;
            timer = new Timer();
            timer.Interval = GetInterval();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Check())
                _operation();

        }

        private double GetInterval()
        {
            double interval = 10000;
            double.TryParse(ConfigurationManager.AppSettings["ServiceWorkingInterval"], out interval);
            return interval;
        }

      
        private static bool Check()
        {
            DateTime startDate = DateTime.Now;
            DateTime.TryParseExact(ConfigurationManager.AppSettings["ServiceStartDate"],
                            "dd.MM.yyyy HH:mm:ss",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.None,
                           out startDate);

            return DateTime.Now > startDate;
        }
    }
}
