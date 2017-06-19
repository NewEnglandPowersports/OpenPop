using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleProject;


namespace ConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                HandleTraderMessages.GetTraderMessages();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

    }
}
