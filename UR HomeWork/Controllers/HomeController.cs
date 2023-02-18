using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static UR_HomeWork.Models.UR_class.UserModel;
using UR_HomeWork.Models.DB_Data;

namespace UR_HomeWork.Controllers
{
    public class HomeController : Controller
    {
        UR_DB db = new UR_DB();
        public ActionResult Index()
        {


            return View();
        }

        /// <summary>
        /// 執行註冊
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        public ActionResult DoRegister(DoRegisterIn inModel)
        {
            DoRegisterOut outModel = new DoRegisterOut();

            if (string.IsNullOrEmpty(inModel.UserID) || string.IsNullOrEmpty(inModel.UserPwd) || string.IsNullOrEmpty(inModel.UserPwdChk) || string.IsNullOrEmpty(inModel.UserName))
            {
                outModel.ErrMsg = "請輸入資料";
            }
            else if (inModel.UserPwd != inModel.UserPwdChk)
            {
                outModel.ErrMsg = "密碼驗證錯誤";
            }
            else
            {
                try
                {
                    // 檢查帳號是否存在
                    var UserId = db.User.Where(w => w.Id == inModel.UserID).FirstOrDefault();

                    if (UserId != null)
                    {
                        outModel.ErrMsg = "此登入帳號已存在";
                    }
                    else
                    {
                        // 將密碼使用 SHA256 雜湊運算(不可逆)
                        string salt = inModel.UserID.Substring(0, 1).ToLower(); //使用帳號前一碼當作密碼鹽
                        SHA256 sha256 = SHA256.Create();
                        byte[] bytes = Encoding.UTF8.GetBytes(salt + inModel.UserPwd); //將密碼鹽及原密碼組合
                        byte[] hash = sha256.ComputeHash(bytes);
                        StringBuilder result = new StringBuilder();
                        for (int i = 0; i < hash.Length; i++)
                        {
                            result.Append(hash[i].ToString("X2"));
                        }
                        string NewPwd = result.ToString(); // 雜湊運算後密碼

                        // 註冊資料新增至資料庫

                        User user = new User();
                        user.Id = inModel.UserID;
                        user.PassWord = NewPwd;
                        user.Name = inModel.UserName;
                        user.Address = inModel.UserAdd;

                        db.User.Add(user);
                        db.SaveChanges();

                        outModel.ResultMsg = "註冊完成";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            // 輸出json
            return Json(outModel);
        }

        public ActionResult LoginPage()
        {


            return View();
        }

        public ActionResult SingUpPage()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SingUpPage(FormCollection post)
        {
            string account = post["Account"];
            string password = post["Password"];
            string passwordChack = post["PasswordChack"];
            string name = post["Name"];
            string address = post["Address"];

            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address))
            {
                ViewBag.Msg = "資料輸入有誤，請重新確認";
                return View();
            }

            if (string.IsNullOrWhiteSpace(password) || password != passwordChack)
            {
                ViewBag.Msg = "請重新確認密碼";
                return View();

            }
            else
            {
                //Response.Write("<script>alert('註冊成功');location.href='LoginPage'; </script>");
                Response.Redirect("LoginPage");
                return new EmptyResult();
            }
        }


    }

}