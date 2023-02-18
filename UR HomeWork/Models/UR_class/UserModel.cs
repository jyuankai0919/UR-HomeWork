﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UR_HomeWork.Models.UR_class
{
    public class UserModel
    {
        public class DoRegisterIn
        {
            /// <summary>
            /// 註冊參數值
            /// </summary>
            public string UserID { get; set; }
            public string UserPwd { get; set; }
            public string UserPwdChk { get; set; }
            public string UserName { get; set; }
            public string UserAdd { get; set; }
        }

        /// <summary>
        /// 註冊回傳值
        /// </summary>
        public class DoRegisterOut
        {
            public string ErrMsg { get; set; }
            public string ResultMsg { get; set; }
        }


        

    }
}