using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterAuth.Models;

public class SmsSettings
{
    public IList<string> Keywords { get; set; }
    public IList<string> Senders { get; set; }
}
