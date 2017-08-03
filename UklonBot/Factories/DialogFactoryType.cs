
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
            Phone,
            Loss,
            LossDetails,
            Complaint,
            ComplaintReason

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
            ConfirmPhone,
            ModifyAfterCreation,
            Rank
        }

      
   
    }
}