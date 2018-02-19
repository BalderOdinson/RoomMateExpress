using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using RoomMateExpressWebApi.Services;

namespace RoomMateExpressWebApi.Helpers
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link, bool isImportant)
        {
            return emailSender.SendEmailAsync(email, "Potvrdite email adresu",
                "<p>RoomMateExpress</p>" +
                $"<p>Molimo Vas potvrdite Vašu email adresu pritiskom na ovaj <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>. </p>", isImportant);
        }

        public static Task SendResetPasswordAsync(this IEmailSender emailSender, string email, string link, bool isImportant)
        {
            return emailSender.SendEmailAsync(email, "Ponovno postavljanje lozinke",
                "<p>RoomMateExpress</p>" +
                $"<p>Ponovno postavite lozinku putem sljedećeg linka: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>. </p>", isImportant);
        }

        public static Task SendBlockedUserAsync(this IEmailSender emailSender, string email, string adminEmail, string reason, bool isImportant)
        {
            return emailSender.SendEmailAsync(email, "Ponovno postavljanje lozinke",
                "<p>RoomMateExpress</p>" +
                $"<p>Poštovani, Vaš račun je blokiran od strane administratora. Razlog : </p>" +
                $"<p>{reason}</p>" +
                $"<p>Vašu žalbu možete poslati na sljedeću email adresu <a href='mailto:{adminEmail}?Subject=[ZALBA]' target='_top'>{adminEmail}</a>.</p>", isImportant);
        }
    }
}
