
using System;
using System.Data;


namespace ConsoleProject
{
    [Serializable]
    public class Vehicle
    {
        #region "Class private members variables."
        //private int _Customer_ID;
        //Lead1x7 thumbnail
        //Lead4x5 full size
        private string _VIN;
        private string _StockNumber;
        private string _Lead4x5;
        private string _Lead1x7;
        private string _UnitType; //condition u or n
        private string _ListPrice;
        private string _UnitMake;

        private string _RetailFreightAmt;
        private string _Color;
        private string _Odometer;

        private string _DSRP;
        private string _MSRP;

        private string _ManufacturerName;
        private string _ModelYear;
        private string _Model;
        private string _Store;
        private string _Hours;
        private int _InStock;
        private int _FutureArrives;
        private string _PicturePaths;


        #endregion "Class private members variables"

        #region "Class public members variables, Gets, Sets."
        public string VIN { get { return _VIN; } set { _VIN = value; } }
        public string StockNumber { get { return _StockNumber; } set { _StockNumber = value; } }
        public string Lead4x5 { get { return _Lead4x5; } set { _Lead4x5 = value; } }
        public string Lead1x7 { get { return _Lead1x7; } set { _Lead1x7 = value; } }
        public string UnitType { get { return _UnitType; } set { _UnitType = value; } }
        public string ListPrice { get { return _ListPrice; } set { _ListPrice = value; } }
        public string UnitMake { get { return _UnitMake; } set { _UnitMake = value; } }
        public string RetailFreightAmt { get { return _RetailFreightAmt; } set { _RetailFreightAmt = value; } }
        public string Color { get { return _Color; } set { _Color = value; } }
        public string DSRP
        {
            get { return _DSRP; }
            set
            {
                //evaluation code for dsrp, msrp
                _DSRP = value;// Utils.GetItemPrice(Convert.ToDecimal(_MSRP), Convert.ToDecimal(_DSRP));
            }
        }
        public string MSRP { get { return _MSRP; } set { _MSRP = value; } }

        public string ManufacturerName { get { return _ManufacturerName; } set { _ManufacturerName = value; } }
        public string ModelYear { get { return _ModelYear; } set { _ModelYear = value; } }
        public string Model { get { return _Model; } set { _Model = value; } }
        public string Store { get { return _Store; } set { _Store = value; } }
        public string Hours { get { return _Hours; } set { _Hours = value; } }
        public string Odometer { get { return _Odometer; } set { _Odometer = value; } }
        public int InStock { get { return _InStock; } set { _InStock = value; } }
        public int FutureArrives { get { return _FutureArrives; } set { _FutureArrives = value; } }
        public string PicturePaths { get { return _PicturePaths; } set { _PicturePaths = value; } }

        #endregion "Class public members variables, Gets, Sets."

        #region constructors
        public Vehicle()
        {
            _VIN = "";
            _StockNumber = "";
            _Lead4x5 = "";
            _Lead1x7 = "";
            _UnitType = "";
            _ListPrice = "";
            _UnitMake = "";
            _RetailFreightAmt = "";

            _Color = "";
            _DSRP = "";
            _MSRP = "";


            _ManufacturerName = "";
            _ModelYear = "";
            _Model = "";
            _VIN = "";
            _Store = "";
            _Hours = "";
            _Odometer = "";
            _InStock = 0;
            _FutureArrives = 0;
            _PicturePaths = "";

        }

        public Vehicle(DataTableReader reader)
        {
            ModelYear = reader["modelyear"].ToString();
            UnitMake = reader["unitmake"].ToString();
            Model = reader["Model"].ToString();
            VIN = reader["VIN"].ToString();
            Color = reader["Color"].ToString();
            Store = reader["store"].ToString();
            Hours = reader["hours"].ToString();
            UnitType = reader["unittype"].ToString();
            StockNumber = reader["stocknumber"].ToString();
            ListPrice = reader["dealerlistprice"].ToString();
            Odometer = reader["Odometer"].ToString();
            DSRP = reader["DSRP"].ToString();
            MSRP = reader["MSRP"].ToString();
            InStock = Convert.ToInt32(reader["InStock"]);
            FutureArrives = Convert.ToInt32(reader["FutureArrives"]);
            PicturePaths = reader["PicturePaths"].ToString();
        }

        public Vehicle(DataRow reader)
        {
            ModelYear = reader["modelyear"].ToString();
            UnitMake = reader["unitmake"].ToString();
            Model = reader["Model"].ToString();
            VIN = reader["VIN"].ToString();
            Color = reader["Color"].ToString();
            Store = reader["store"].ToString();
            Hours = reader["hours"].ToString();
            UnitType = reader["unittype"].ToString();
            StockNumber = reader["stocknumber"].ToString();
            ListPrice = reader["dealerlistprice"].ToString();
            Odometer = reader["Odometer"].ToString();
            DSRP = reader["DSRP"].ToString();
            MSRP = reader["MSRP"].ToString();
            InStock = Convert.ToInt32(reader["InStock"]);
            FutureArrives = Convert.ToInt32(reader["FutureArrives"]);
            PicturePaths = reader["PicturePaths"].ToString();
        }
        #endregion constructors

    }

    public class SelectedVehicle : Vehicle
    {
        public string Price { get; set; }
        public string Category { get; set; }
        public string Condition { get; set; }
        public string UnitClass { get; set; }
    }
}