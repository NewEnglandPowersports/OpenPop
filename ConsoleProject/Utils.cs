
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System;

namespace ConsoleProject
{
    internal class Utils
    {
        public static string FindCDKVehicleType(string key)
        {
            string vehicleType = String.Empty;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("M", "mc");
            dictionary.Add("A", "atv");
            dictionary.Add("T", "tr");
            dictionary.Add("P", "Power-Equipment");
            dictionary.Add("G", "go");
            dictionary.Add("D", "Dirt-Bikes");
            dictionary.Add("C", "sc");
            dictionary.Add("S", "sm");
            dictionary.Add("W", "wc");
            dictionary.Add("B", "wc");
            dictionary.Add("0", "otr");


            // See whether Dictionary contains this string.
            if (dictionary.ContainsKey(key.ToUpper()))
            {
                vehicleType = dictionary[key.ToUpper()];
            }
            return vehicleType;
        }

    }
}