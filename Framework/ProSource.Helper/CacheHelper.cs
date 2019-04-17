using System;
using System.Collections.Generic;
using System.Text;

namespace ProSource.Helper
{
    public static class CacheHelper
    {
        /// <summary>
        /// destek kullanıcısı login olduğunda onun kullanıcı id'sini alıyoruz.
        /// </summary>
        public static int? SupportUserId { get; set; }
        /// <summary>
        /// Destek kullanıcısı login olduğunda bu kullanıcı yetkilerini kullanarak işlem yapacaktır. Bu kullanıcınında userid bilgisini tutalım laızm olur
        /// </summary>
        public static int? CustomerUserId { get; set; }
    }
}
