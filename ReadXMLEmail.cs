using System;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Web;
using System.Threading;

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

                //stock number must be 6 digits. seems to be an issue with stk #s like 000233, kills all the zeros. pad it back
                for (int i = email.stock.Length; i < 6; i++)
                {
                    email.stock = '0' + email.stock;
                    
                }

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
                SendErrorEmail("error at parse prospect", ex.Message);            
                
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

                SendErrorEmail("error at process prospect", ex.Message);
            }
        }
        public static void SendErrorEmail(string subject, string msg)
        {
            try
            {
                //send email to me
                EmailHelper mailHelper = new EmailHelper();
                mailHelper.sStore = "testing";
#if DEBUG
                mailHelper.sTo = "mmclean@capecoder.com";
                mailHelper.sFrom = "mmcclean@capecoder.com";
#else
                mailHelper.sTo = Properties.Settings.Default.DefaultToEmailAddress;
                mailHelper.sFrom = Properties.Settings.Default.DefaultFromEmailAddress;
#endif
                mailHelper.sSubject = subject;
                mailHelper.sBody = msg + " " + System.DateTime.Now.ToString();
                mailHelper.SendEmailMessage(mailHelper);
                Environment.Exit(0);
            }
            catch (Exception e)
            {

                Console.Write(e.Message);
                throw;
            }
        }
    }
}
