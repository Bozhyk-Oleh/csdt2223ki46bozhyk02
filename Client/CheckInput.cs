using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client
{
    public class CheckInput
    {
        public string checkmessage(String input)
        {
            if (input.Length <= 30)
            {
                if (Regex.IsMatch(input, @"^[a-zA-Z0-9]+$"))
                {
                    return input;
                }
                else
                {
                    return "Message must contain only latin characters";
                }
            }
            else
            {
                return "Message must be less than 31 characters";
            }
        }
    }
}
