using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace WindowsServiceManager
{
    public class ServiceManager
    {
        static Timer timer;
        static Action _operation;
        static Configuration config = null;
        public ServiceManager(Action operation)
        {
            _operation = operation;
            config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetEntryAssembly().Location);
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
            double interval = 50000;
            double.TryParse(config.AppSettings.Settings["ServiceWorkingInterval"].Value, out interval);
            return interval;
        }


        private static bool Check()
        {
            DateTime startDate = DateTime.Now;
            DateTime.TryParseExact(config.AppSettings.Settings["ServiceStartDate"].Value,
                            "dd.MM.yyyy HH:mm:ss",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.None,
                           out startDate);
            return DateTime.Now > startDate;
        }
    }
}
