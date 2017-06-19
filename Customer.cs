namespace ConsoleProject
{
    public class Customer
    {
        #region "Class private members variables."
        private int _Customer_ID;

        private string _FName;
        private string _LName;
        private string _Address;
        private string _City;
        private string _State;
        private string _Zip;
        private string _Phone;
        private string _PhoneWork;
        private string _Email;

        private string _PurchaseTimeframe;
        private bool _ExtendedWarranty;
        private string _Trade;
        private string _TradeMfg;
        private string _TradeModel;
        private string _TradeYear;
        private string _TradeMiles;

        private string _Comments;
        private string _ContactReason;
        private string _Delivery;

        private string _ManufacturerName;
        private string _ModelYear;
        private string _VehicleModelName;
        private string _VehicleModelPrice;
        private string _VIN;

        #endregion "Class private members variables"

        #region "Class public members variables, Gets, Sets."
        public int Customer_ID { get { return _Customer_ID; } set { _Customer_ID = value; } }
        public string FName { get { return _FName; } set { _FName = value; } }
        public string LName { get { return _LName; } set { _LName = value; } }
        public string Address { get { return _Address; } set { _Address = value; } }
        public string City { get { return _City; } set { _City = value; } }
        public string State { get { return _State; } set { _State = value; } }
        public string Zip { get { return _Zip; } set { _Zip = value; } }
        public string Phone { get { return _Phone; } set { _Phone = value; } }
        public string PhoneWork { get { return _PhoneWork; } set { _PhoneWork = value; } }
        public string Email { get { return _Email; } set { _Email = value; } }

        public string PurchaseTimeframe { get { return _PurchaseTimeframe; } set { _PurchaseTimeframe = value; } }
        public bool ExtendedWarranty { get { return _ExtendedWarranty; } set { _ExtendedWarranty = value; } }
        public string TradeMfg { get { return _TradeMfg; } set { _TradeMfg = value; } }
        public string Trade { get { return _Trade; } set { _Trade = value; } }
        public string TradeModel { get { return _TradeModel; } set { _TradeModel = value; } }
        public string TradeYear { get { return _TradeYear; } set { _TradeYear = value; } }
        public string TradeMiles { get { return _TradeMiles; } set { _TradeMiles = value; } }

        public string Comments { get { return _Comments; } set { _Comments = value; } }
        public string ContactReason { get { return _ContactReason; } set { _ContactReason = value; } }
        public string Delivery { get { return _Delivery; } set { _Delivery = value; } }

        public string ManufacturerName { get { return _ManufacturerName; } set { _ManufacturerName = value; } }
        public string ModelYear { get { return _ModelYear; } set { _ModelYear = value; } }
        public string VehicleModelName { get { return _VehicleModelName; } set { _VehicleModelName = value; } }
        public string VehicleModelPrice { get { return _VehicleModelPrice; } set { _VehicleModelPrice = value; } }
        public string VIN { get { return _VIN; } set { _VIN = value; } }


        #endregion "Class public members variables, Gets, Sets."

        #region constructors
        public Customer()
        {
            _Customer_ID = 0;

            _FName = "";
            _LName = "";
            _Address = "";
            _City = "";
            _State = "";
            _Zip = "";
            _Phone = "";
            _PhoneWork = "";
            _Email = "";

            _PurchaseTimeframe = "";
            _ExtendedWarranty = false;
            _Trade = "No";
            _TradeMfg = "";
            _TradeModel = "";
            _TradeYear = "";
            _TradeMiles = "";
            _Comments = "";
            _ContactReason = "";
            _Delivery = "";

            _ManufacturerName = "";
            _ModelYear = "";
            _VehicleModelName = "";
            _VehicleModelPrice = "";
            _VIN = "";

        }
        #endregion constructors

    }
}