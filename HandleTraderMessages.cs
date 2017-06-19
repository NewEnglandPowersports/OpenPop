using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPop.Common;
using OpenPop.Mime;
using OpenPop.Pop3;
using System.Net.Mail;

namespace ConsoleProject
{
    public static class HandleTraderMessages
    {
        //Credentials for downloading Trader emails

        static string hostname = Properties.Settings.Default.TraderHostName;
        static int port = 110;               
        static bool useSsl = false;
                    
        static string username = Properties.Settings.Default.TraderUserName;
        static string password = Properties.Settings.Default.TraderPassword;

        public static void GetTraderMessages()
        {
            try
            {
                List<OpenPop.Mime.Message> messages = FetchAllTraderMessages(hostname, port, useSsl, username, password);
                for (int i = messages.Count - 1; i >= 0; i--)
                {
                    OpenPop.Mime.MessagePart xml = messages[i].FindFirstPlainTextVersion();
                    if (xml != null)
                    {
                        string xmlString = xml.GetBodyAsText();
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.LoadXml(xmlString);
                        doc.Save("c:\\openpop\\test.xml");
                        // }  removed by jim
                        // as a non xml email ending up in mailstop would trigger
                        //a duplicate send of the last message on disk.
                        ProcessCycleTraderEmail readXML = new ProcessCycleTraderEmail();
                        try
                        {
                            readXML.ParseProspect("c:\\openpop\\test.xml");
                        }
                        catch (Exception)
                        {                             
                        }
                       
                        DeleteMessage(i+1);  
                        }//added by jim
                   
                }
                Console.Write("Processing complete.");
                System.Threading.Thread.Sleep(10000);
                // Console.Read();
                 Environment.Exit(0);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static void DeleteMessage(int messageNumber)
        {
            try
            {
                using (Pop3Client client = new Pop3Client())
                {
                    // Connect to the server
                    client.Connect(hostname, port, useSsl);

                    // Authenticate ourselves towards the server
                    client.Authenticate(username, password);

                    // Mark the message as deleted
                    // Notice that it is only MARKED as deleted
                    // POP3 requires you to "commit" the changes
                    // which is done by sending a QUIT command to the server
                    // You can also reset all marked messages, by sending a RSET command.
                    client.DeleteMessage(messageNumber);

                    // When a QUIT command is sent to the server, the connection between them are closed.
                    // When the client is disposed, the QUIT command will be sent to the server
                    // just as if you had called the Disconnect method yourself.
                    client.Dispose();
                }
                return;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                throw;
            }
        }
        /// <summary>
        ///  Fetch all messages from the POP3 server
        /// </summary>
        /// <param name="hostname">Hostname of the server. For example: pop3.live.com</param>
        /// <param name="port">Host port to connect to. Normally: 110 for plain POP3, 995 for SSL POP3</param>
        /// <param name="useSsl">Whether or not to use SSL to connect to server</param>
        /// <param name="username">Username of the user on the server</param>
        /// <param name="password">Password of the user on the server</param>
        /// <returns>All Messages on the POP3 server</returns>
        public static List<Message> FetchAllTraderMessages(string hostname, int port, bool useSsl, string username, string password)
        {
            try
            {
                // The client disconnects from the server when being disposed
                using (Pop3Client client = new Pop3Client())
                {
                    // Connect to the server
                    client.Connect(hostname, port, useSsl);

                    // Authenticate ourselves towards the server
                    client.Authenticate(username, password);

                    // Get the number of messages in the inbox
                    int messageCount = client.GetMessageCount();

                    // We want to download all messages
                    List<Message> allMessages = new List<Message>(messageCount);

                    //Notice that the for loop starts at the messageCount number, and goes up to and includes 1.
                    //This is because POP3 is 1-based.This is the case for all methods taking a message number as an argument.              
                    // Messages are numbered in the interval: [1, messageCount]
                    // Most servers give the latest message the highest number
                    for (int i = messageCount; i > 0; i--)
                    {
                        allMessages.Add(client.GetMessage(i));
                    }
                    // Now return the fetched messages
                    return allMessages;
                }
            }
            catch (Exception ex)
            {
                SendErrorEmail("error at fetch trader messages", ex.Message);
            }
            return null;
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
