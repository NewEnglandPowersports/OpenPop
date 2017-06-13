using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using MimeKit;

namespace ConsoleProject
{
    class ReadWithMimeKit
    {
        MimeParser parser = new MimeParser(stream, MimeFormat.Entity);
        var message = parser.ParseMessage();
    }
}
