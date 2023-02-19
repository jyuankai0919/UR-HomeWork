using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace UR_HomeWork.Controllers.Feature
{
    public class MemberService
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

        /// <summary>
        /// 將密碼使用 SHA256 雜湊運算
        /// </summary>
        /// <returns>string:EncodePassWord</returns>
        public string HashPasswordWithSHA256(string UserId ,string passWord) 
        {
            string salt = UserId.Substring(0, 2).ToLower(); //使用帳號前二碼當作密碼鹽
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(salt + passWord); //將密碼鹽及原密碼組合
            byte[] hash = sha256.ComputeHash(bytes);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString(); // 雜湊運算後密碼
        }

    }
}