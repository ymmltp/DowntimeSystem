using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DowntimeSystem.Models
{
    public static class Common
    {
        public static bool ArrayIsTrueNull(this string[] paras) {
            bool flag = false;
            if (paras.Length == 0) flag = true;
            else if (string.IsNullOrEmpty(paras[0])) flag = true;
            return flag;
        }
    }
}
