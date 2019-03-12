using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using static SCADACore.Vars;

namespace SCADACore
{
    [DataContract]
    public static class RealTimeDriver
    {
        public static Dictionary<string, double> addresses = new Dictionary<string, double>()
            {
                {"1", -1}, {"2", -1}, {"3", -1}, {"4", -1}, {"5", -1 }, {"6", -1}, {"7", -1}, {"8", -1}, {"9", -1}, {"10", -1}
            };

       
    }

    [DataContract]
    public static class SimulationDriver
    {
        public static Dictionary<string, double> addresses = new Dictionary<string, double>()
            {
                {"1", -1}, {"2", -1}, {"3", -1}, {"4", -1}, {"5", -1}, {"6", -1}, {"7", -1}, {"8", -1}, {"9", -1}, {"10", -1}
            };

        public static double amplitude = 100;

        public static void Sine(object obj)
        {
            string address = (string)obj;

            while (true)
            {
                Thread.Sleep(1000);
                double res = amplitude * Math.Sin((double)DateTime.Now.Second / 60 * Math.PI);
                addresses[address] = Math.Round(res, 2);

                UpdateSimulationItems(address, Math.Round(res, 2));
            }
        }

        public static void Cosine(object obj)
        {
            string address = (string)obj;

            while (true)
            {
                Thread.Sleep(1000);
                double res = amplitude * Math.Cos((double)DateTime.Now.Second / 60 * Math.PI);
                addresses[address] = Math.Round(res, 2);

                UpdateSimulationItems(address, Math.Round(res, 2));
            }
        }

        public static void Triangle(object obj)
        {
            string address = (string)obj;
            int value = 0;
            int adder = 10;
            while (true)
            {
                Thread.Sleep(1000);
                value += adder;
                addresses[address] = value;
                UpdateSimulationItems(address, value);

                if (value == 0 || value == 100)
                {
                    adder = -adder;
                }
            }
        }

        public static void Rectangle(object obj)
        {
            string address = (string)obj;
            int value = 0;
            int iter = 0;
            int period = 3;
            while (true)
            {
                Thread.Sleep(1000);
                addresses[address] = value * 100.00;
                UpdateSimulationItems(address, value * 100.00);
                iter += 1;
                if (iter == period)
                {
                    iter = 0;
                    value = (value == 0) ? 1 : 0;
                }
            }
        }

        public static void Ramp(object obj)
        {
            string address = (string)obj;

            while (true)
            {
                Thread.Sleep(1000);
                double res = amplitude * DateTime.Now.Second / 60;
                addresses[address] = Math.Round(res, 2);

                UpdateSimulationItems(address, Math.Round(res, 2));
            }
        }

        private static void UpdateSimulationItems(string address, double value)
        {
            int i;
            for (i = 0; i < simulations.Count; i++)
            {
                if (simulations[i].address.Equals(address))
                {
                    simulations[i].value = value;
                }
            }
        }
    }
}