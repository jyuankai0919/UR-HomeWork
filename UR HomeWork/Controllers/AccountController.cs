using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static UR_HomeWork.Models.UR_class.UserModel;
using UR_HomeWork.Controllers.Feature;
using UR_HomeWork.Models.DB_Data;
using System.Data.SqlClient;
using System.Data;

namespace UR_HomeWork.Controllers
{
    
    public class AccountController : Controller
    {
        
        MemberService memberService = new MemberService();


        public virtual ActionResult LoginPage()
        {
            UR_DB db = new UR_DB();
            if (Request.Cookies["UserKeepLogin"] != null)
            {
                if (!string.IsNullOrEmpty(Request.Cookies["UserKeepLogin"].Value))
                {
                    string ckUserKeepLoginVerify = Request.Cookies["UserKeepLogin"].Value;

                    // 取出帳號密碼
                    string UserID = ckUserKeepLoginVerify.Split('|')[0];
                    string UserToken = ckUserKeepLoginVerify.Split('|')[1];

                    Tokens tokens = db.Tokens.Where(w => w.UserId == UserID).FirstOrDefault();

                    // 檢查帳號、密碼是否正確
                    if (tokens.Value == UserToken)
                    {
                        // 有查詢到資料，表示帳號密碼正確

                        //確認 token 還在時效內
                        if (tokens.ExpiresAt > DateTime.Now)
                        {
                            // 將登入帳號記錄在 Session 內
                            Session["UserID"] = UserID;

                            //給前端的資訊
                            ViewData["UserKeepLogin"] = "Y";
                        }
                    }
                }
            }
            return View();
        }


        public ActionResult SingUpPage()
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
            UR_DB db = new UR_DB();
            DoRegisterOut outModel = new DoRegisterOut();

            if (string.IsNullOrEmpty(inModel.UserID) || string.IsNullOrEmpty(inModel.UserPwd) || string.IsNullOrEmpty(inModel.UserPwdChk) || string.IsNullOrEmpty(inModel.UserName))
            {
                outModel.ErrMsg = "請輸入資料";
            }
            else if (!memberService.IsValidEmail(inModel.UserID))
            {
                outModel.ErrMsg = "請輸入正確的Email";
            }
            else if (!memberService.IsValidPassword(inModel.UserPwd))
            {
                outModel.ErrMsg = "請確認密碼是否包含英文及數字，長度必須至少為8個字符";
            }
            else if (inModel.UserPwd != inModel.UserPwdChk)
            {
                outModel.ErrMsg = "密碼驗證錯誤";
            }
            else if (!memberService.IsValidName(inModel.UserName))
            {
                outModel.ErrMsg = "請輸入正確的姓名";
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
                        //將密碼加密
                        string NewPwd =  memberService.HashPasswordWithSHA256(inModel.UserID, inModel.UserPwd);

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


        /// <summary>
        /// 執行登入
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        public virtual ActionResult DoLogin(DoLoginIn inModel)
        {
            UR_DB db = new UR_DB();
            DoLoginOut outModel = new DoLoginOut();

            // 檢查輸入資料
            if (string.IsNullOrEmpty(inModel.UserID) || string.IsNullOrEmpty(inModel.UserPwd))
            {
                outModel.ErrMsg = "請輸入資料";
            }
            else if (!memberService.IsValidEmail(inModel.UserID)) 
            {
                outModel.ErrMsg = "請輸入Email";
            }
            else
            { 
                try
                {   
                    //SHA密碼
                    string CheckPwd = memberService.HashPasswordWithSHA256(inModel.UserID, inModel.UserPwd);

                    // 檢查帳號、密碼是否正確
                    User user = db.User.Where(w => w.Id == inModel.UserID).FirstOrDefault();

                    // 有查詢到資料，表示帳號正確
                    if (user != null && user.PassWord == CheckPwd)
                    {
                        // 將登入帳號記錄在 Session 內
                        Session["UserID"] = inModel.UserID;

                        outModel.ResultMsg = "登入成功";

                        // 檢查是否保持登入
                        if (inModel.KeepLogin == "true")
                        {

                            string token = Guid.NewGuid().ToString();// 生成一個隨機的 token
                            HttpCookie ckUserKeepLogin = new HttpCookie("UserKeepLogin"); //Cookie 名稱
                            ckUserKeepLogin.Value = inModel.UserID + "|" + token; //Cookie 值
                            ckUserKeepLogin.Expires = DateTime.Now.AddDays(7); //Cookie 有效期限
                            ckUserKeepLogin.HttpOnly = true; //防止 XSS 攻擊

                            //確認是否有token紀錄
                            Tokens tokens = db.Tokens.Where(w => w.UserId == inModel.UserID).FirstOrDefault();

                            //沒有紀錄
                            if (tokens == null)
                            {
                                tokens = new Tokens();
                                tokens.User = user;
                                tokens.UserId = inModel.UserID;
                                tokens.Value = token;
                                tokens.CreatedAt = DateTime.Now;
                                tokens.ExpiresAt = DateTime.Now.AddDays(7);

                                db.Tokens.Add(tokens);
                            }
                            else
                            {
                                //有資料代表曾經登錄過

                                //更新DB token
                                tokens.Value = token;
                                tokens.CreatedAt = DateTime.Now;
                                tokens.ExpiresAt = DateTime.Now.AddDays(7);
                            }
                            db.SaveChanges();
                            Response.Cookies.Add(ckUserKeepLogin);

                        }

                    }
                    else
                    {
                        // 查無資料，帳號或密碼錯誤
                        outModel.ErrMsg = "帳號或密碼錯誤";
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

        public ActionResult AccountPage()
        {

            return View();
        }


    }


   


}