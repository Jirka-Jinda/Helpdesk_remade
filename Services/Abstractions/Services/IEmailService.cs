using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Services;
public interface IEmailService
{
    public Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true);
}
