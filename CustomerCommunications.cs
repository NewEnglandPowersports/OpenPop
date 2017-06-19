using System.Configuration;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace ConsoleProject
{
    public class CustomerCommunications
    {

        public static string emailServer = string.Empty;

        public CustomerCommunications()
        {
        }

        public static void SendCustomerInfoEmailToStore(string hostStore, Customer cust)
        {
            try
            {
                EmailHelper mailHelper = new EmailHelper();
                if (hostStore == null)
                    mailHelper.sTo = Properties.Settings.Default.DefaultToEmailAddress;
                else
                {
//#if DEBUG
//                    mailHelper.sTo = "mmclean@capecoder.com; jim@ne-ps.net;";
//                    mailHelper.sFrom = "mmclean@capecoder.com";                     
//#else
                mailHelper.sTo = Utils.EmailToText(hostStore);
                mailHelper.sFrom = Properties.Settings.Default.DefaultFromEmailAddress; 
//#endif
                }
                mailHelper.sStore = hostStore;
                mailHelper.sSubject = "Cycle Trader Lead";

                mailHelper.sBody = BuildSB(cust);
                mailHelper.SendEmailMessage(mailHelper);
            }
            catch (Exception ex)
            {
                SendErrorEmail("SendCustomerInfoEmail Exception", "SendCustomerEmail Exception: " + ex.Message.ToString());
            }
        }

        public static string BuildSB(Customer cust)
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("<br/>");
            sBuilder.Append("First Name: ");
            sBuilder.Append(cust.FName);
            sBuilder.Append("<br/>");
            sBuilder.Append("Last Name: ");
            sBuilder.Append(cust.LName);
            sBuilder.Append("<br/>");
            sBuilder.Append("<br/>");
            sBuilder.Append("Phone: ");
            sBuilder.Append(cust.Phone);
            sBuilder.Append("<br/>");
            //sBuilder.Append("Work Phone: ");
            //sBuilder.Append(cust.PhoneWork);
            //sBuilder.Append("<br/>");
            sBuilder.Append("Email: ");
            sBuilder.Append(cust.Email);
            sBuilder.Append("<br/>");
            //sBuilder.Append("Trade: ");
            //sBuilder.Append(cust.Trade);
            //sBuilder.Append("<br/>");
            sBuilder.Append("Comments: ");
            sBuilder.Append(cust.Comments);
            sBuilder.Append("<br/>");
            sBuilder.Append("<br/>");
            sBuilder.Append("<br/>");
            sBuilder.Append("VIN: ");
            sBuilder.Append(cust.VIN);
            sBuilder.Append("<br/>");
            sBuilder.Append("<a href='http://www.newenglandpowersports.com/details.aspx?VIN=");
            sBuilder.Append(cust.VIN.Trim());
            sBuilder.Append("'>See selection</a>");
            return sBuilder.ToString();
        }
        //enum DealerID { c128 = 76015103, gbm = 76013403, pps = 76022447, pkwy = 76013991, cmps = 76158942 };

        public static string GetDealerID(string storeid)
        {
            string dealerID = String.Empty;
            switch (storeid.ToUpper())
            {
                case "C128":
                    dealerID = "76015103";
                    emailServer = "cycles128.com";
                    break;
                case "GBM":
                    dealerID = "76013403";
                    emailServer = "greaterbostonmotorsports.com";
                    break;
                case "GBMS":
                    dealerID = "76013403";
                    emailServer = "greaterbostonmotorsports.com";
                    break;
                case "PKWY":
                    dealerID = "76013991";
                    emailServer = "parkwaycycle.com";
                    break;
                case "PLAI":
                    dealerID = "76022447";
                    emailServer = "plaistowpowersports.com";
                    break;
                case "CMPS":
                    dealerID = "76158942";
                    emailServer = "centralmasspowersports.com";
                    break;
                default:
                    dealerID = "admin";
                    emailServer = "newenglandpowersports.com";
                    break;
            }
            return dealerID;
        }


        public static void SendVSEPTXML(string hostStore, Customer cust, SelectedVehicle veh)
        {

            if (hostStore == null || hostStore.Trim() == "") { return; }//if unit is not found then store is unkno

            string postData = "";
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                //here's the data we'll be sending
                StringBuilder postDataBuilder = new StringBuilder();
                postDataBuilder.Append("<?xml version=\"1.0\" ?><ProspectImport><Item><SourceProspectId>" + cust.Customer_ID + "</SourceProspectId><DealershipId>" + GetDealerID(hostStore) + "</DealershipId>");
                postDataBuilder.Append("<Email>" + cust.Email + "</Email>");
                postDataBuilder.Append("<Name>" + cust.FName + " " + cust.LName + "</Name>");
                postDataBuilder.Append("<Phone>" + cust.Phone + "</Phone>");
                postDataBuilder.Append("<SourceDate>" + System.DateTime.Now.ToString());
                postDataBuilder.Append("</SourceDate>");
                postDataBuilder.Append("<VehicleType>" + Utils.FindCDKVehicleType(veh.UnitClass) + "</VehicleType>");
                postDataBuilder.Append("<VehicleMake>" + veh.UnitMake + "</VehicleMake>");
                postDataBuilder.Append("<VehicleModel>" + veh.Model + "</VehicleModel>");
                postDataBuilder.Append("<VehicleYear>" + veh.ModelYear + "</VehicleYear>");
                postDataBuilder.Append("<Notes><![CDATA[" + BuildSB(cust) + "< ]]></Notes>");
                postDataBuilder.Append("</Item></ProspectImport>");
                postData = postDataBuilder.ToString();
                XmlDocument importDoc = new XmlDocument();
                importDoc.LoadXml(postData);
                ConsoleProject.VSEPTPCHService.Service1 sc = new ConsoleProject.VSEPTPCHService.Service1();
                string response = sc.AddProspect(importDoc.OuterXml, "Cycles128");
                sc.Dispose();
                //check for error
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

                doc.LoadXml(@"<?xml version=""1.0""?>" + response);
                XmlNode element = doc.SelectSingleNode("/AddProspectResults");

                if (element["Prospect"]["SourceProspectId"].InnerXml.Contains("error") || element["Prospect"].InnerXml == null)
                {
                    SendErrorEmail("XMLtoCEM-Error", element["Prospect"]["SourceProspectId"].InnerXml + " " + hostStore);
                }
                SendErrorEmail("XMLtoCEM-Good Response", element["Prospect"]["SourceProspectId"].InnerXml + " " + hostStore);
            }
            catch (Exception exc)
            {
                SendErrorEmail("CEM Send Error", "Error in sending xml file to CEM: " + "Exception: " + exc.Message);
            }
        }

        public void ReadResponse(System.IO.Stream responseStream)
        {
            try
            {
                //  string localFolder = "C:\\inetpub\\wwwroot\\NEPS\\errors\\";//System.IO.Directory.GetCurrentDirectory();
                String localFolder = @"c:\openpop\";
                int totalBytesRead = 0;
                byte[] buffer = new byte[4096];
                using (responseStream)
                {
                    using (FileStream localFileStream =
                      new FileStream(Path.Combine(localFolder, "responseStream.txt"), FileMode.Create))
                    {
                        int bytesRead;
                        while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            totalBytesRead += bytesRead;
                            localFileStream.Write(buffer, 0, bytesRead);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                
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

                


            }
            catch (Exception e)
            {
                
                Console.Write(e.Message);
                throw;
            }
        }
    }
}
