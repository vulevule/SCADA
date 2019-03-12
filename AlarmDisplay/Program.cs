using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlarmDisplay
{
    class Program
    {
        private static ServiceReference1.ServiceAlarmDisplayClient client = null;
        private static InstanceContext context = null;
        private delegate void RaiseAlarmDelegate(string alarm);
        private static RaiseAlarmDelegate RaiseAlarmHandler = null;
        private static string path = @"C:\Users\Vule\SNUS\SCADA Core\alarmsLog.txt";

        public class ServiceCallback : ServiceReference1.IServiceAlarmDisplayCallback
        {
            public void RaiseAlarm(string alarm)
            {
                if (RaiseAlarmHandler != null)
                {
                    RaiseAlarmHandler(alarm);
                }
            }

            public void SendToTrending(string tagId, double value)
            {
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("***************** ALARM DISPLAY *****************\n");

            context = new InstanceContext(new ServiceCallback());
            client = new ServiceReference1.ServiceAlarmDisplayClient(context);
            RaiseAlarmHandler = new RaiseAlarmDelegate(Print);

            client.AlarmDisplayInit();
            Console.WriteLine("Displaying all alarms:");

            while (true) ;
        }

        public static void Print(string alarm)
        {
            Console.WriteLine(alarm);
          
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(alarm);                
            }
        }
    }
}
