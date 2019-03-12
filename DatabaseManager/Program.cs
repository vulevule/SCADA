using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseManager
{
    class Program
    {
        private static ServiceReference1.ServiceDatabaseManagerClient client;

        static void Main(string[] args)
        {
            client = new ServiceReference1.ServiceDatabaseManagerClient();

            Console.WriteLine("***************** DATABASE MANAGER *****************\n");

            while (true)
            {
                Console.WriteLine("\nChoose your option");
                Console.WriteLine("1. Add new tag");
                Console.WriteLine("2. Edit tags");
                Console.WriteLine("3. Load last state");
                Console.WriteLine("4. Clear database");
                Console.Write("Enter option: ");
                string option = Console.ReadLine();

                while (!option.Equals("1") && !option.Equals("2") && !option.Equals("3") && !option.Equals("4"))
                {
                    Console.WriteLine("Invalid input!");
                    Console.WriteLine("1. Add new tag");
                    Console.WriteLine("2. Edit tags");
                    Console.WriteLine("3. Load last state");
                    Console.WriteLine("4. Clear database");
                    Console.Write("Please try again: ");
                    option = Console.ReadLine();
                }

                if (option.Equals("1"))
                {
                    AddNewTag();
                }
                else if (option.Equals("2"))
                {
                    EditTags();
                }
                else if (option.Equals("3"))
                {
                    LoadLastState();
                }
                else if (option.Equals("4"))
                {
                    ClearDatabase();
                }
            }
        }

        private static void LoadLastState()
        {
            Console.Write("\nConfirm? (y/n): ");
            string option = Console.ReadLine();
            if (option.Equals("y"))
            {
                if (client.LoadLastState())
                {
                    Console.WriteLine("Loading successfull!");
                } else
                {
                    Console.WriteLine("Error while loading!");
                }
            } 
        }

        private static void ClearDatabase()
        {
            Console.Write("\nConfirm? (y/n): ");
            string option = Console.ReadLine();
            if (option.Equals("y"))
            {
                client.ClearDatabase();
                Console.WriteLine("Database cleared!");
            }
        }

        private static void AddNewTag()
        {
            Console.WriteLine("\nChoose tag type:");
            Console.WriteLine("1. Digital Input");
            Console.WriteLine("2. Analog Input");
            Console.WriteLine("3. Digital Output");
            Console.WriteLine("4. Analog Output");
            Console.Write("Enter option: ");

            string option = Console.ReadLine();

            while (!option.Equals("1") && !option.Equals("2") && !option.Equals("3") && !option.Equals("4"))
            {
                Console.WriteLine("\nInvalid input!");
                Console.WriteLine("1. Digital Input");
                Console.WriteLine("2. Analog Input");
                Console.WriteLine("3. Digital Output");
                Console.WriteLine("4. Analog Output");
                Console.Write("Please try again: ");
                option = Console.ReadLine();

            }

            if (option.Equals("1"))
            {
                Console.Write("\nEnter tag's ID: ");
                string id = Console.ReadLine();
                id = "DI" + id;
                Console.Write("Enter tag's description: ");
                string desc = Console.ReadLine();
                Console.Write("Enter scan time: ");
                int scanTime = int.Parse(Console.ReadLine());
                Console.WriteLine("\nChoose scanning type");
                Console.WriteLine("1. Real-Time");
                Console.WriteLine("2. Simulational");
                Console.Write("Enter option: ");
                string option2 = Console.ReadLine();

                while (!option2.Equals("1") && !option2.Equals("2"))
                {
                    Console.WriteLine("\nInvalid input!");
                    Console.WriteLine("1. Real-Time");
                    Console.WriteLine("2. Simulational");
                    Console.Write("Please try again: ");
                    option2 = Console.ReadLine();
                }

                bool realTime = option2.Equals("1") ? true : false;
        
                if (realTime)
                {
                    string[] availableRTUs = client.GetRTUs(true);
                    if (availableRTUs.Length > 0)
                    {
                        Console.WriteLine("\nAvailable RTUs:");
                        for (int i = 0; i < availableRTUs.Length; i++)
                        {
                            Console.WriteLine((i+1) + ". " + availableRTUs[i]);
                        }
                        Console.Write("Choose a RTU to connect with tag: ");
                        string option3 = Console.ReadLine();

                        string rtuId = availableRTUs[int.Parse(option3)-1];

                        if (client.AddDITag(id, desc, scanTime, realTime, rtuId))
                        {
                            Console.WriteLine("\nTag {0} successfully added on RTU {1}\n", id, rtuId);
                            //while (true)
                            //{
                            //    Thread.Sleep(scanTime / 2);
                            //    Console.WriteLine("Tag {0} has value {1}", id, client.GetTagValue(id));
                            //}
                        } else
                        {
                            Console.WriteLine("\nTag with entered ID already exists! Please try again.");
                        }

                    } else
                    {
                        Console.WriteLine("\nNo currently available RTUs");
                    }

                } else
                {
                    string address = client.GetSimAddress(true);
                    if (client.AddDITag(id, desc, scanTime, realTime, address))
                    {
                        Console.WriteLine("\nTag {0} successfully added on simulational address {1}", id, address);
                        Console.WriteLine("Simulation type: RECTANGLE");
                        Console.WriteLine("Automatic data aquisition\n");

                        //while (true)
                        //{
                        //    Thread.Sleep(scanTime / 2);
                        //    Console.WriteLine("Tag {0} has value {1}", id, client.GetTagValue(id));
                        //}
                    }
                    else
                    {
                        Console.WriteLine("\nTag with entered ID already exists! Please try again.");
                    }
                } 

            }
            else if (option.Equals("2"))
            {
                Console.Write("\nEnter tag's ID: ");
                string id = Console.ReadLine();
                id = "AI" + id;
                Console.Write("Enter tag's description: ");
                string desc = Console.ReadLine();
                Console.Write("Enter scan time: ");
                int scanTime = int.Parse(Console.ReadLine());
                Console.Write("Enter tag's low limit: ");
                double lowLimit = double.Parse(Console.ReadLine());
                Console.Write("Enter tag's high limit: ");
                double highLimit = double.Parse(Console.ReadLine());
                Console.Write("Enter tag's units: ");
                string units = Console.ReadLine();
                Console.WriteLine("\nChoose scanning type");
                Console.WriteLine("1. Real-Time");
                Console.WriteLine("2. Simulational");
                Console.Write("Enter option: ");
                string option2 = Console.ReadLine();

                while (!option2.Equals("1") && !option2.Equals("2"))
                {
                    Console.WriteLine("\nInvalid input!");
                    Console.WriteLine("1. Real-Time");
                    Console.WriteLine("2. Simulational");
                    Console.Write("Please try again: ");
                    option2 = Console.ReadLine();
                }

                bool realTime = option2.Equals("1") ? true : false;

                if (realTime)
                {
                    string[] availableRTUs = client.GetRTUs(false);
                    if (availableRTUs.Length > 0)
                    {
                        Console.WriteLine("\nAvailable RTUs:");
                        for (int i = 0; i < availableRTUs.Length; i++)
                        {
                            Console.WriteLine((i+1) + ". " + availableRTUs[i]);
                        }
                        Console.Write("Choose a RTU to connect with tag: ");
                        string option3 = Console.ReadLine();

                        string rtuId = availableRTUs[int.Parse(option3)-1];

                        if (client.AddAITag(id, desc, scanTime, realTime, rtuId, lowLimit, highLimit, units, null))
                        {
                            Console.WriteLine("\nTag {0} successfully added on RTU {1}\n", id, rtuId);
                            //while (true)
                            //{
                            //    Thread.Sleep(scanTime / 2);
                            //    Console.WriteLine("Tag {0} has value {1}", id, client.GetTagValue(id));
                            //}
                        }
                        else
                        {
                            Console.WriteLine("\nTag with entered ID already exists! Please try again.");
                        }

                    }
                    else
                    {
                        Console.WriteLine("\nNo currently available RTUs");
                    }

                }
                else
                {
                    string address = client.GetSimAddress(false);
                    Console.WriteLine("Simulation types");
                    Console.WriteLine("1. Sine");
                    Console.WriteLine("2. Cosine");
                    Console.WriteLine("3. Triangle");
                    Console.WriteLine("4. Ramp");
                    Console.Write("Choose type: ");
                    string option3 = Console.ReadLine();

                    while (!option3.Equals("1") && !option3.Equals("2") && !option3.Equals("3") && !option3.Equals("4"))
                    {
                        Console.WriteLine("\nInvalid input!");
                        Console.WriteLine("1. Sine");
                        Console.WriteLine("2. Cosine");
                        Console.WriteLine("3. Triangle");
                        Console.WriteLine("4. Ramp");
                        Console.Write("Please try again: ");
                        option3 = Console.ReadLine();
                    }

                    string simType;
                    if (option3.Equals("1"))
                    {
                        simType = "sine";
                    } else if (option3.Equals("2"))
                    {
                        simType = "cosine";
                    }
                    else if (option3.Equals("3"))
                    {
                        simType = "triangle";
                    }
                    else
                    {
                        simType = "ramp";
                    }

                    if (client.AddAITag(id, desc, scanTime, realTime, address, lowLimit, highLimit, units, simType))
                    {
                        Console.WriteLine("\nTag {0} successfully added on simulational address {1}", id, address);
                        Console.WriteLine("Simulation type: {0}", simType.ToUpper());
                        Console.WriteLine("Automatic data aquisition\n");
                        //while (true)
                        //{
                        //    Thread.Sleep(scanTime / 2);
                        //    Console.WriteLine("Tag {0} has value {1}", id, client.GetTagValue(id));
                        //}
                    }
                    else
                    {
                        Console.WriteLine("\nTag with entered ID already exists! Please try again.");
                    }
                }
            }
            else if (option.Equals("3"))
            {
                Console.Write("\nEnter tag's ID: ");
                string id = Console.ReadLine();
                id = "DO" + id;
                Console.Write("Enter tag's description: ");
                string desc = Console.ReadLine();
                Console.Write("Enter tag's initial value (1 or 0): ");
                double initVal;
                string str = Console.ReadLine();
                while (!str.Equals("1") && !str.Equals("0"))
                {
                    Console.Write("Invalid format! Please try again: ");
                    str = Console.ReadLine();
                }
                initVal = double.Parse(str);

                string address = client.GetSimAddress(true);

                if (client.AddDOTag(id, desc, initVal, address))
                {
                    Console.WriteLine("\nTag {0} successfully added on simulational address {1}", id, address);
                    Console.WriteLine("Manual data aquisition\n");
                }
                else
                {
                    Console.WriteLine("\nTag with entered ID already exists! Please try again.\n");
                }

            }
            else if (option.Equals("4"))
            {
                Console.Write("\nEnter tag's ID: ");
                string id = Console.ReadLine();
                id = "AO" + id;
                Console.Write("Enter tag's description: ");
                string desc = Console.ReadLine();
                Console.Write("Enter tag's initial value: ");
                double initVal;
                string str = Console.ReadLine();
                while (!double.TryParse(str, out initVal))
                {
                    Console.Write("Invalid format! Please try again: ");
                    str = Console.ReadLine();
                }

                Console.Write("Enter tag's low limit: ");
                int lowLimit = int.Parse(Console.ReadLine());
                Console.Write("Enter tag's high limit: ");
                int highLimit = int.Parse(Console.ReadLine());
                Console.Write("Enter tag's units: ");
                string units = Console.ReadLine();

                string address = client.GetSimAddress(false);

                if (client.AddAOTag(id, desc, initVal, address, lowLimit, highLimit, units))
                {
                    Console.WriteLine("\nTag {0} successfully added on simulational address {1}", id, address);
                    Console.WriteLine("Manual data aquisition\n");
                }
                else
                {
                    Console.WriteLine("\nTag with entered ID already exists! Please try again.\n");
                }
            }
            


        }

        private static void EditTags()
        {
            Console.WriteLine("\nAvailable tags:");
            string[] ids = client.GetTags();
            for (int i = 0; i < ids.Length; i++)
            {
                Console.WriteLine((i+1) + ". " + ids[i]);
            }

            Console.Write("Chose tag to edit: ");
            int option = int.Parse(Console.ReadLine());
            Dictionary<string, string> tagInfo = client.GetTagInfo(ids[option - 1]);

            Console.WriteLine("\nCurrent tag info:");
            DisplayTagInfo(tagInfo);

            Console.WriteLine("\n1. Change info");
            Console.WriteLine("2. Delete tag");
            Console.WriteLine("3. Switch scan ON/OFF");
            Console.WriteLine("4. Change driver");
            if (tagInfo.ContainsKey("Auto/Manual") && tagInfo["Driver"].Equals("Simulation"))
            {
                Console.WriteLine("5. Switch AUTO/MANUAL data aqusition");

                if (tagInfo["Auto/Manual"].Equals("MANUAL"))
                {
                    Console.WriteLine("6. Manually enter values");
                }
            }
            
            Console.Write("Choose option: ");
            string option2 = Console.ReadLine();

            if (option2.Equals("1"))
            {
                ChangeInfo(tagInfo);
            }
            else if (option2.Equals("2"))
            {
                DeleteTag(tagInfo["Name"]);

            }
            else if (option2.Equals("3"))
            {
                Console.Write("\nScanning is currently {0}. Switch? (y/n): ", tagInfo["Scan"]);
                string option3 = Console.ReadLine();
                if (option3.Equals("y"))
                {
                    if (client.SwitchScan(tagInfo["Name"]))
                    {
                        Console.WriteLine("\nScanning switched!");
                    }
                    else
                    {
                        Console.WriteLine("\nError while switching scanning!");
                    }
                }

            }
            else if (option2.Equals("4"))
            {
                ChangeDriver(tagInfo);
            }
            else if (option2.Equals("5") && tagInfo.ContainsKey("Auto/Manual") && tagInfo["Driver"].Equals("Simulation"))
            {
                Console.Write("\nTag is currently on {0} data aqusition. Switch? (y/n): ", tagInfo["Auto/Manual"]);
                string option3 = Console.ReadLine();
                if (option3.Equals("y"))
                {
                    if (client.SwitchAutoManual(tagInfo["Name"]))
                    {
                        Console.WriteLine("\nSwitched!");
                    }
                    else
                    {
                        Console.WriteLine("\nError while switching!");
                    }
                }
            }
            else if (option2.Equals("6") && tagInfo["Auto/Manual"].Equals("MANUAL") && tagInfo["Driver"].Equals("Simulation"))
            {
                SetManualValue(tagInfo["Name"]);
            }
        }

        private static void ChangeDriver(Dictionary<string, string> tagInfo)
        {
            Console.WriteLine("\nChoose scanning type");
            Console.WriteLine("1. Real-Time");
            Console.WriteLine("2. Simulational");
            Console.Write("Enter option: ");
            string option2 = Console.ReadLine();

            while (!option2.Equals("1") && !option2.Equals("2"))
            {
                Console.WriteLine("\nInvalid input!");
                Console.WriteLine("1. Real-Time");
                Console.WriteLine("2. Simulational");
                Console.Write("Please try again: ");
                option2 = Console.ReadLine();
            }

            bool realTime = option2.Equals("1") ? true : false;
            bool type;
            if (tagInfo["Type"].Equals("DigitalInput") || tagInfo["Type"].Equals("DigitalOutput")) type = true;
            else type = false;

            if (realTime)
            {             
                string[] availableRTUs = client.GetRTUs(type);
                
                if (availableRTUs.Length > 0)
                {
                    Console.WriteLine("\nAvailable RTUs:");
                    for (int i = 0; i < availableRTUs.Length; i++)
                    {
                        Console.WriteLine((i + 1) + ". " + availableRTUs[i]);
                    }
                    Console.Write("Choose a RTU to connect with tag: ");
                    string option3 = Console.ReadLine();

                    string rtuId = availableRTUs[int.Parse(option3) - 1];

                    if (client.SwitchDriver(tagInfo["Name"], rtuId, null))
                    {
                        Console.WriteLine("\nTag {0} successfully added on RTU {1}\n", tagInfo["Name"], rtuId);
                        
                    }
                    else
                    {
                        Console.WriteLine("\nError! Please try again.");
                    }

                }
                else
                {
                    Console.WriteLine("\nNo currently available RTUs");
                }

            }
            else
            {
                string address = client.GetSimAddress(type);
                string simType;

                if (type)
                {
                    simType = "rectangle";
                } else
                {
                    Console.WriteLine("Simulation types");
                    Console.WriteLine("1. Sine");
                    Console.WriteLine("2. Cosine");
                    Console.WriteLine("3. Triangle");
                    Console.WriteLine("4. Ramp");
                    Console.Write("Choose type: ");
                    string option3 = Console.ReadLine();

                    while (!option3.Equals("1") && !option3.Equals("2") && !option3.Equals("3") && !option3.Equals("4"))
                    {
                        Console.WriteLine("\nInvalid input!");
                        Console.WriteLine("1. Sine");
                        Console.WriteLine("2. Cosine");
                        Console.WriteLine("3. Triangle");
                        Console.WriteLine("4. Ramp");
                        Console.Write("Please try again: ");
                        option3 = Console.ReadLine();
                    }


                    if (option3.Equals("1"))
                    {
                        simType = "sine";
                    }
                    else if (option3.Equals("2"))
                    {
                        simType = "cosine";
                    }
                    else if (option3.Equals("3"))
                    {
                        simType = "triangle";
                    }
                    else
                    {
                        simType = "ramp";
                    }
                }
                

                if (client.SwitchDriver(tagInfo["Name"], address, simType))
                {
                    Console.WriteLine("\nTag {0} successfully added on simulational address {1}", tagInfo["Name"], address);
                    Console.WriteLine("Simulation type: {0}", simType.ToUpper());
                    
                    //while (true)
                    //{
                    //    Thread.Sleep(scanTime / 2);
                    //    Console.WriteLine("Tag {0} has value {1}", id, client.GetTagValue(id));
                    //}
                }
                else
                {
                    Console.WriteLine("\nTag with entered ID already exists! Please try again.");
                }
            }
        }

        private static void ChangeInfo(Dictionary<string, string> tagInfo)
        {            
            List<string> intruders = new List<string> {"Type", "Driver", "I/O Address", "Auto/Manual", "Scan", "Alarms" };

            foreach (string key in tagInfo.Keys)
            {
                if (!intruders.Contains(key)) {
                    Console.Write("New value for {0} (empty to remain old): ", key);
                    string newVal = Console.ReadLine();
                    Tuple<string, string> retVal = new Tuple<string, string>(key, newVal);
                    client.ChangeInfo(tagInfo["Name"], retVal);
                }
            }
        }

        private static void DeleteTag(string tagID)
        {
            Console.Write("\nDeleting tag {0}. Are you sure? (y/n): ", tagID);
            string option3 = Console.ReadLine();

            if (option3.Equals("y"))
            {
                if (client.RemoveTag(tagID))
                {
                    Console.WriteLine("\nTag {0} successfully deleted", tagID);

                } else
                {
                    Console.WriteLine("\nError while deleting tag {0}!", tagID);
                }
            }
            else if (option3.Equals("n"))
            {
                return;
            }
        }

        private static void SetManualValue(string tagId)
        {
            while (true) { 
                Console.Write("\nEnter new value (empty for exit): ");
                string str = Console.ReadLine();

                if (str.Equals("")) return;

                double value;

                try
                {
                    value = double.Parse(str);
                    client.SetTagValue(tagId, value);
                    Console.WriteLine("Tag {0} set on value {1}.", tagId, value);
                } catch
                {
                    Console.WriteLine("Invalid input! Please try again");
                }
            }

            
        }

        private static void DisplayTagInfo(Dictionary<string, string> tagInfo)
        {
            foreach(KeyValuePair<string, string> pair in tagInfo)
            {
                Console.WriteLine(pair.Key + " : " + pair.Value);
            }
        }
    }
}
