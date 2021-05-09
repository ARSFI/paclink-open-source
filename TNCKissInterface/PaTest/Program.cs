using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TNCKissInterface;

namespace PaTest
{
    class Program
    {
        static void Main(string[] args)
        {
            UInt32 result = WinlinkAuth.ChallengedPassword("12345678", "PASSWORD");
        }
    }
}
