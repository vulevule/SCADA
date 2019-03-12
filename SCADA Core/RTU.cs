using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using static SCADACore.Vars;
using static SCADACore.Utility;
using SCADACore;

namespace SCADA_Core
{
    public class RTU
    {
        public string rtuID { get; set; }
        public bool type { get; set; }
        public double value { get; set; }
        public string address { get; set; }
        public string publicKey { get; set; }

        public void function()
        {
            //List<string> parameters = (List<string>)obj;
            //string option = parameters[0];
            //string id = parameters[1];
            //string address = parameters[2];
            Random rnd = new Random();

            while (true)
            {
                Thread.Sleep(1000);
                
                if (type)
                {
                    value = rnd.Next(0, 2) * 100;             
                }
                else
                {
                    double res = 100 * Math.Sin((double)DateTime.Now.Second / 60 * Math.PI);
                    value = Math.Round(res, 2);
                }

                string strVal = value.ToString();
                byte[] signature = CreateDigitalSignature(strVal);

                SendData(rtuID, address, strVal, signature);
                //{
                //    Console.WriteLine("Error while sending value {0} from {1} on address {2}!!!", value, id, address);
                //}
            }
        }

        private bool SendData(string RTUid, string address, string message, byte[] signature)
        {
            // If RTU is not connected on an input tag, break the operation
            if (connectedRTUs[RTUid] == null)
            {
                return false;
            }

            // Read public key
            CspParameters csp = new CspParameters();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);

            try
            {
                rsa.FromXmlString(RTUkeys[RTUid]);
            }
            catch
            {
                return false;
            }

            rsa.PersistKeyInCsp = true;

            // Compute hash value
            byte[] hashValue = ComputeHashValue(message);

            if (VerifyMessage(hashValue, signature, rsa))
            {
                RealTimeDriver.addresses[address] = double.Parse(message);
                return true;
            }

            return false;
        }

        private static byte[] CreateDigitalSignature(string message)
        {
            SHA256 sha = SHA256Managed.Create();
            byte[] hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
            RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(rsa);
            formatter.SetHashAlgorithm("SHA256");
            return formatter.CreateSignature(hashValue);
        }
    }
}