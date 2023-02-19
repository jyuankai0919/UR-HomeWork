using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UR_HomeWork.Controllers;
using UR_HomeWork.Controllers.Feature;
using UR_HomeWork.Models.DB_Data;
using UR_HomeWork.Models.UR_class;
using static System.Net.Mime.MediaTypeNames;
using static UR_HomeWork.Models.UR_class.UserModel;

namespace UR_HomeWork.Tests.Stub
{
    public class DoLoginStub : AccountController
    {
        public override ActionResult DoLogin(UserModel.DoLoginIn inModel)
        {
            //調用工具包
            MemberService memberService = new MemberService();
            //設定測試資料
            List<User> users= new List<User>();
            users.Add(new User());
            users[0].Id = "test@gmail.com";
            users[0].PassWord = "0611C4A148395BB2295E77C8E95368BCE63034E41EBD1DA35509D1039C25D8C5";//預設密碼a1234567

            List<Tokens> tokenList= new List<Tokens>();
            tokenList.Add(new Tokens());
            tokenList[0].UserId = "test@gmail.com";
            tokenList[0].Value = "testToken";

            //邏輯開始
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
                    User user = users.Where(w => w.Id == inModel.UserID).FirstOrDefault();

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
                            Tokens tokens = tokenList.Where(w => w.UserId == inModel.UserID).FirstOrDefault();

                            //沒有紀錄
                            if (tokens == null)
                            {
                                tokens = new Tokens();
                                tokens.User = user;
                                tokens.UserId = inModel.UserID;
                                tokens.Value = token;
                                tokens.CreatedAt = DateTime.Now;
                                tokens.ExpiresAt = DateTime.Now.AddDays(7);
                                
                                 /***
                                  * 測試，不與資料庫連線
                                 ***/
                                //db.Tokens.Add(tokens);
                            }
                            else
                            {
                                //有資料代表曾經登錄過

                                //更新DB token
                                tokens.Value = token;
                                tokens.CreatedAt = DateTime.Now;
                                tokens.ExpiresAt = DateTime.Now.AddDays(7);
                            }
                            /***
                              *測試，不與資料庫連線
                            ***/
                            //db.SaveChanges();

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
    }
}
