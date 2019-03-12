using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;

namespace SCADACore
{
    public static class Utility
    {
        public static byte[] ComputeHashValue(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            SHA256 sha = SHA256Managed.Create();
            byte[] hashValue = sha.ComputeHash(messageBytes);
            return hashValue;
        }

        public static bool VerifyMessage(byte[] hashValue, byte[] signature, RSACryptoServiceProvider rsa)
        {
            RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(rsa);
            deformatter.SetHashAlgorithm("SHA256");
            return deformatter.VerifySignature(hashValue, signature);
        }

    }

    public class SimulationItem
    {
        public string type { get; set; }
        public double value { get; set; }
        public string address { get; set; }
    }
}