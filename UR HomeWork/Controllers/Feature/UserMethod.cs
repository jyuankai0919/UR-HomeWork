using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace UR_HomeWork.Controllers.Feature
{
    public class UserMethod
    {
        /// <summary>
        /// 確認Email格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns>bool</returns>
        public bool IsValidEmail(string input)
        {
            try
            {
                //直接調用 Net.Mail 確認input是否符合Email格式
                var email = new MailAddress(input);
                return email.Address == input;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 密碼必須包含至少一個英文字母和一個數字，長度必須至少為8個字符
        /// </summary>
        /// <param name="password"></param>
        /// <returns>bool</returns>
        public bool IsValidPassword(string password)
        {
            // 密碼必須包含至少一個英文字母和一個數字，長度必須至少為8個字符
            Regex regex = new Regex(@"^(?=.*[a-zA-Z])(?=.*\d).{8,}$");
            return regex.IsMatch(password);
        }
        /// <summary>
        /// 驗證輸入的資料是否為中文或英文
        /// </summary>
        /// <param name="name"></param>
        /// <returns>bool</returns>
        public bool IsValidName(string name)
        {
            // 驗證輸入的資料是否為中文或英文 \u4E00-\u9FA5A 為中文範圍
            Regex regex = new Regex(@"^[\u4E00-\u9FA5A-Za-z]+$");
            return regex.IsMatch(name);
        }
    }
}