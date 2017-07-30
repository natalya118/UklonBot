
namespace UklonBot.Factories
{
    public class DialogFactoryType
    {
        public enum Root
        {
            Order,
            Register,
            ChangeCity,
            Help, 
            Phone
        }

        public enum Order
        {
            Address,
            Street,
            Number,
            CorrectStreet,
            Destination,
            Reporting,
            Modify,
            ConfirmPhone
        }

        public enum Register
        {
            Phone
        }
   
    }
}