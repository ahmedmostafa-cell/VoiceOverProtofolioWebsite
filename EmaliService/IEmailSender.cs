using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace EmailService
{
    public interface IEmailSender
    {
        void SendEmail(Message message, string id);
        Task SendEmailAsync(Message message , string id);

        Task SendEmailAsyncToCustomerWithBookingDetails(Message message, Guid id);
        Task SendEmailAsyncToCustomer(Message message);

        Task SendEmailAsyncToCustomerNotConfirmedBooking(Message message , string Comment, string CustomerEmail);
    }
}
