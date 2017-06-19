using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ConsoleProject
{
    public class DatabaseHandler
    {
        public DatabaseHandler()
        {
        }

        public static SqlDataReader FindVehicle(string stockNo, string model)
        {
            try
            {
                SqlDataReader veh = null;
                using (SqlCommand command = new SqlCommand("spGetVehicleByModelStockNumber", GetConnection()))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@model", SqlDbType.VarChar, 50).Value = model;
                    command.Parameters.Add("@stocknumber", SqlDbType.VarChar, 50).Value = stockNo;
                    veh = command.ExecuteReader();
                    //CloseConnection(command.Connection);
                }
                return veh;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Inserts a row into the Customers table.
        /// </summary>
        /// <param name="description">Customer object</param>
        /// <returns>integer, identity of inserted customer record</returns>
        public static int AddCustomer(Customer cust)
        {
            try
            {
                int custNo = 0;


                using (SqlCommand command = new SqlCommand("spAddCustomer", GetConnection()))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@in_FName", SqlDbType.VarChar, 50).Value = cust.FName;
                    command.Parameters.Add("@in_LName", SqlDbType.VarChar, 50).Value = cust.LName;
                    command.Parameters.Add("@in_Address", SqlDbType.VarChar, 100).Value = cust.Address;
                    command.Parameters.Add("@in_City", SqlDbType.VarChar, 50).Value = cust.City;
                    command.Parameters.Add("@in_State", SqlDbType.VarChar, 2).Value = cust.State;
                    command.Parameters.Add("@in_Zip", SqlDbType.VarChar, 20).Value = cust.Zip;
                    command.Parameters.Add("@in_Phone", SqlDbType.VarChar, 20).Value = cust.Phone;
                    command.Parameters.Add("@in_PhoneWork", SqlDbType.VarChar, 20).Value = cust.PhoneWork;
                    command.Parameters.Add("@in_Email", SqlDbType.VarChar, 50).Value = cust.Email;
                    command.Parameters.Add("@in_PurchaseTimeframe", SqlDbType.VarChar, 20).Value = cust.PurchaseTimeframe;
                    command.Parameters.Add("@in_ExtendedWarranty", SqlDbType.Bit).Value = cust.ExtendedWarranty;
                    command.Parameters.Add("@in_TradeMfg", SqlDbType.VarChar, 50).Value = cust.TradeMfg;
                    command.Parameters.Add("@in_TradeModel", SqlDbType.VarChar, 50).Value = cust.TradeModel;
                    command.Parameters.Add("@in_TradeYear", SqlDbType.VarChar, 50).Value = cust.TradeYear;
                    command.Parameters.Add("@in_TradeMiles", SqlDbType.VarChar, 50).Value = cust.TradeMiles;
                    command.Parameters.Add("@in_VIN", SqlDbType.VarChar, 50).Value = cust.VIN;
                    command.Parameters.Add("@in_Comments", SqlDbType.Text).Value = cust.Comments;
                    command.Parameters.Add("@in_ContactReason", SqlDbType.Text).Value = cust.ContactReason;
                    command.Parameters.Add("@in_Delivery", SqlDbType.Text).Value = cust.Delivery;
                    command.Parameters.Add("@out_CustomerNo", SqlDbType.Int);
                    command.Parameters["@out_CustomerNo"].Direction = ParameterDirection.Output;
                    command.ExecuteNonQuery();

                    custNo = (int)command.Parameters["@out_CustomerNo"].Value;
                    CloseConnection(command.Connection);
                }

                return custNo;
            }
            catch (Exception ex)
            {
                SendErrorEmail("error at add customer", ex.Message);
               
            }
            return 0;
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
        /// <summary>
        /// Manages database connection
        /// </summary>
        /// <returns>connection string</returns>
        public static SqlConnection GetConnection()
        {
            try
            {
                string connectionString = string.Empty;
                // Get the connection string from the configuration file
#if DEBUG
             //   connectionString = Properties.Settings.Default.TestConnectionString;
                 connectionString = Properties.Settings.Default.PartsConnectionString;
#else
                connectionString = Properties.Settings.Default.PartsConnectionString;
#endif
                // Create a new connection object
                SqlConnection connection = new SqlConnection(connectionString);

                // Open the connection, and return it
                connection.Open();
                return connection;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void CloseConnection(SqlConnection connection)
        {
            connection.Close();
        }
    }
}