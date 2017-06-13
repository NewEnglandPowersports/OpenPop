using System;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Web;

namespace ConsoleProject
{
    public class ProcessCycleTraderEmail
    {
        public void ParseProspect(string emailFileURI)
        {
            try
            {
                XDocument xdoc = XDocument.Load(emailFileURI);
                XElement adf = xdoc.Element("adf");
                XElement prospect = adf.Element("prospect");
                CycleTraderEmail email = new CycleTraderEmail();
                Customer cust = new Customer();
                email.requestDate = prospect.Element("requestdate").Value;
                foreach (XElement item in prospect.Element("customer").Element("contact").Elements("name"))
                {
                    foreach (var namepart in item.Attributes("part"))
                    {
                        if (namepart.Value.Equals("first"))
                            cust.FName = email.first = namepart.Parent.Value;
                        else if (namepart.Value.Equals("last"))
                            cust.LName = email.last = namepart.Parent.Value;
                    }
                }
                cust.Phone = email.phone = prospect.Element("customer").Element("contact").Element("phone").Value;
                cust.Email = email.email = prospect.Element("customer").Element("contact").Element("email").Value;
                email.year = prospect.Element("vehicle").Element("year").Value;
                email.make = prospect.Element("vehicle").Element("make").Value;
                email.model = prospect.Element("vehicle").Element("model").Value;
                email.stock = prospect.Element("vehicle").Element("stock").Value;
                email.price = prospect.Element("vehicle").Element("price").Value;  //tobedone: test
                email.requestDate = prospect.Element("requestdate").Value;
                cust.Comments = email.comments = prospect.Element("customer").Element("comments").Value;

                SelectedVehicle veh = new SelectedVehicle();
                veh.Model = email.model;
                //strip leading zero
                email.stock = email.stock.TrimStart('0');
                veh.StockNumber = email.stock;
                veh.Price = email.price;
                veh.UnitMake = email.make;
                SqlDataReader reader = DatabaseHandler.FindVehicle(email.stock, email.model);
                if (!reader.HasRows)
                    throw new Exception("Vehicle data not found for "+email.stock+"; "+email.model);
                while (reader.Read())
                {
                    veh.VIN = reader["VIN"].ToString().TrimStart('0');
                    veh.Store = reader["storeid"].ToString().ToUpper();
                    cust.VIN = veh.VIN;
                    veh.Category = Utils.FindCDKVehicleType(reader["unitclass"].ToString());
                    veh.UnitClass = reader["unitclass"].ToString();
                    veh.ModelYear = reader["modelyear"].ToString();
                }
                ProcessProspect(veh, cust);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void ProcessProspect(SelectedVehicle veh, Customer cust)
        {
            try
            {
                //send to store
                CustomerCommunications.SendCustomerInfoEmailToStore(veh.Store, cust);
                //if new customer, write to database
                cust.Customer_ID = DatabaseHandler.AddCustomer(cust);                
                //send to CEM
                CustomerCommunications.SendVSEPTXML(veh.Store, cust, veh);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
