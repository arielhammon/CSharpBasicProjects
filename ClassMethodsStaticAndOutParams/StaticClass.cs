using System;
using System.Collections.Generic;
using System.Text;

namespace ClassMethodsStaticAndOutParams
{
    static class StaticClass
    {
        public static string Day(int num)
        {
            switch(num)
            {
                case 1:
                    return "Sunday";
                case 2:
                    return "Monday";
                case 3:
                    return "Tuesday";
                case 4:
                    return "Wednesday";
                case 5:
                    return "Thursday";
                case 6:
                    return "Friday";
                case 7:
                    return "Saturday";
                default:
                    return "Something Else";
            }
        }
    }
}
