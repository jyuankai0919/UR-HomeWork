using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using UR_HomeWork.Controllers;
using UR_HomeWork.Models.DB_Data;

namespace UR_HomeWork.Tests.Stub
{
    public class LoginPageStub : AccountController
    {
        public override ActionResult LoginPage()
        {

            //測試用資料
            List<Tokens> token = new List<Tokens>();
            token.Add(new Tokens());
            token[0].UserId = "zxcv@gmail.com";
            token[0].Value = "f59c56d8 - 2c2f - 43a5 - 8f2d - e8f36613b478";
            token[0].ExpiresAt = DateTime.Now.AddDays(1);


            //邏輯開始
            if (Request.Cookies["UserKeepLogin"] != null)
            {
                if (!string.IsNullOrEmpty(Request.Cookies["UserKeepLogin"].Value))
                {
                    string ckUserKeepLoginVerify = Request.Cookies["UserKeepLogin"].Value;

                    // 取出帳號密碼
                    string UserID = ckUserKeepLoginVerify.Split('|')[0];
                    string UserToken = ckUserKeepLoginVerify.Split('|')[1];

                    Tokens tokens = token.Where(w => w.UserId == UserID).FirstOrDefault();

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
    }
}
