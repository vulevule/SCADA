using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealTimeUnit
{
    class Program
    {
        private static ServiceReference1.IServiceRealTimeUnit client = null;
        //private static CspParameters csp = new CspParameters();
        //private static RSACryptoServiceProvider rsa = null;

        static void Main(string[] args)
        {
            Console.WriteLine("*************** REAL TIME UNIT ***************\n");

            while (true)
            {
                Console.WriteLine("\n1. Enter new RTU");
                Console.WriteLine("2. Delete RTU");
                Console.Write("Enter option: ");
                string option1 = Console.ReadLine();
                while (!option1.Equals("1") && !option1.Equals("2"))
                {
                    Console.WriteLine("\nInvalid input!");
                    Console.WriteLine("1. Enter new RTU");
                    Console.WriteLine("2. Delete RTU");
                    Console.Write("Please try again: ");
                    option1 = Console.ReadLine();
                }

                if (option1.Equals("1"))
                {
                    EnterNewRTU();
                } else
                {
                    DeleteRTU();
                }


                

                //Thread newThread = new Thread(new ParameterizedThreadStart(function));
                //List<string> parameters = new List<string> {option, id, address };
                //newThread.Start(parameters);
            }           
        }

        private static void EnterNewRTU()
        {
            Console.Write("\nEnter new RTU's ID: ");
            string id = Console.ReadLine();
            Console.WriteLine("Choose data type");
            Console.WriteLine("1. Analog");
            Console.WriteLine("2. Digital");
            Console.Write("Enter option: ");
            string option = Console.ReadLine();

            while (!option.Equals("1") && !option.Equals("2"))
            {
                Console.WriteLine("\nInvalid input!");
                Console.WriteLine("1. Analog");
                Console.WriteLine("2. Digital");
                Console.Write("Please try again: ");
                option = Console.ReadLine();
            }

            if (option.Equals("1"))
            {
                id = "AN" + id;
            }
            else if (option.Equals("2"))
            {
                id = "DI" + id;
            }

            client = new ServiceReference1.ServiceRealTimeUnitClient();

            // Creating keys
            //csp.KeyContainerName = "KeyContainerName";
            //csp.Flags = CspProviderFlags.UseMachineKeyStore;
            //rsa = new RSACryptoServiceProvider(csp);
            //rsa.PersistKeyInCsp = true;

            //string publicKey = rsa.ToXmlString(false);

            string address = client.ApplyOnSystem(id);
            if (address.Equals("-1"))
            {
                Console.WriteLine("\nAll addresses are currently busy.");
                return;
            }
            else if (address.Equals("0"))
            {
                Console.WriteLine("\nRTU with entered ID already exists. Please try again.");
                return;
            }
            else
            {
                Console.WriteLine("\nRTU with ID {0} created on address {1}.\nSending data...", id, address);
            }

            client.InitRTU(id);
        }

        private static void DeleteRTU()
        {
            Console.WriteLine("\nFeature currently unavailable! Server problems...");

            //List<string> availableRTUs = null; //client.GetRTUs();

            //if (availableRTUs.Count > 0)
            //{
            //    Console.WriteLine("\nAvailable RTUs:");
            //    for (int i = 0; i < availableRTUs.Count; i++)
            //    {
            //        Console.WriteLine((i + 1) + ". " + availableRTUs[i]);
            //    }
            //    Console.Write("Choose a RTU to delete: ");
            //    string option3 = Console.ReadLine();

            //    string rtuId = availableRTUs[int.Parse(option3) - 1];

            //    Console.Write("Are you sure? (y/n): ");
            //    string option = Console.ReadLine();

            //    if (option.Equals("y"))
            //    {
            //        //if (client.DeleteRTU(rtuId)) Console.WriteLine("\nRTU {0} deleted!", rtuId);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("\nNo installed RTUs!");
            //}
        }

        //public static void function(object obj)
        //{
        //    List<string> parameters = (List<string>)obj;
        //    string option = parameters[0];
        //    string id = parameters[1];
        //    string address = parameters[2];
        //    Random rnd = new Random();

        //    while (true)
        //    {
        //        Thread.Sleep(period);
        //        string value;
        //        if (option.Equals("1"))
        //        {
        //            double res = 100 * Math.Sin((double)DateTime.Now.Second / 60 * Math.PI);
        //            value = (Math.Round(res, 2)).ToString();
        //        }
        //        else
        //        {
        //            value = (rnd.Next(0, 2)*100).ToString();                    
        //        }

        //        byte[] signature = CreateDigitalSignature(value);

        //        client.SendData(id, address, value, signature);
        //        //{
        //        //    Console.WriteLine("Error while sending value {0} from {1} on address {2}!!!", value, id, address);
        //        //}
        //    }
        //}

        //private static byte[] CreateDigitalSignature(string message)
        //{
        //    SHA256 sha = SHA256Managed.Create();
        //    byte[] hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
        //    RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(rsa);
        //    formatter.SetHashAlgorithm("SHA256");
        //    return formatter.CreateSignature(hashValue);
        //}
    }
}
