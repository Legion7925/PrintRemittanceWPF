using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintRemittanceWPF.Helper
{
    public enum MessageTypeEnum
    {
        /// <summary>
        /// پیام موفقیت
        /// </summary>
        Success = 0,
        /// <summary>
        /// پیام خطا
        /// </summary>
        Error = 1,
        /// <summary>
        /// پیام اخطار
        /// </summary>
        Warning = 2,
        /// <summary>
        /// پیام خبری
        /// </summary>
        Information = 3,
    }
}
