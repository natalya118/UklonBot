
namespace UklonBot.Factories
{
    public class DialogFactoryType
    {
        public enum Root
        {
            Order,
            Register,
            ChangeCity,
            Help
        }

        public enum Order
        {
            Address,
            Street,
            Number,
            CorrectStreet,
            Destination,
            Reporting
        }

   
    }
}