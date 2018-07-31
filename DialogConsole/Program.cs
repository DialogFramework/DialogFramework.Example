//Copyright © Sergei Semenkov 2018
using System;
using System.IO;
using DialogFramework.Example;
using Newtonsoft.Json.Linq;

namespace DialogConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.Unicode;
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            Service service = new Service();
            service.OutgoingMessage += Service_OutgoingMessage;

            Console.WriteLine("DialogFramework.Example Console");
            Console.WriteLine("(c) 2018 Sergei Semenkov");
            Console.WriteLine("\ntype exit to close\n");

            JObject config = JObject.Parse(File.ReadAllText("config.json"));

            Console.WriteLine("Loading dictionaries...");
            foreach (string dictionaryfile in config["dictionaryfiles"])
            {
                service.LoadDictionary(dictionaryfile);
            }
            Console.WriteLine("Done");

            Console.WriteLine("Loading grammar...");
            foreach (string grammarfile in config["grammarfiles"])
            {
                service.LoadGrammar(grammarfile);
            }
            Console.WriteLine("Done");

            Console.WriteLine("Loading dialogs...");
            foreach (string dialogsfile in config["dialogsfiles"])
            {
                service.LoadDialog(dialogsfile);
            }
            Console.WriteLine("Done");

            Console.WriteLine("Loading persons data file ...");
            string personsDataFile = config["personsdatafile"].ToString();
            service.LoadPersonsData(personsDataFile);
            Console.WriteLine("Done");

            Console.WriteLine("Loading departments data file ...");
            string departmentsDataFile = config["departmenstdatafile"].ToString();
            service.LoadDepartmentsData(departmentsDataFile);
            Console.WriteLine("Done");

            while (true)
            {
                Console.Write("\ndc>");
                string userInput = Console.ReadLine();

                if(userInput == "exit")
                {
                    break;
                }

                service.Proccess(userInput);
            }
        }

        private static void Service_OutgoingMessage(object sender, DialogFramework.OutgoingMessageEventArgs e)
        {
            Console.Write("bot: ");
            Console.Write(e.Message);
        }
    }
}
