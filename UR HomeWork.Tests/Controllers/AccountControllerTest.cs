using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using UR_HomeWork;
using UR_HomeWork.Controllers;
using UR_HomeWork.Tests.Mock;
using UR_HomeWork.Tests.Stub;
using static UR_HomeWork.Models.UR_class.UserModel;

namespace UR_HomeWork.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest : Controller
    {

        /// <summary>
        /// 測試沒有Cookie的狀態
        /// </summary>
        [TestMethod]
        public void nonCookieLoginPage()
        {

            // Arrange
            LoginPageStub controller = new LoginPageStub();

            // 假的 cookie
            var cookie = new HttpCookie("UserKeepLogin", null);

            // 假的 request
            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.Cookies).Returns(new HttpCookieCollection() { cookie });

            // 假的 controller context
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(x => x.HttpContext.Request).Returns(request.Object);

            // 傳入假的 controller context
            controller.ControllerContext = controllerContext.Object;

            // Act
            ViewResult result = controller.LoginPage() as ViewResult;
            
            // Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// 測試有Cookie狀態
        /// </summary>
        [TestMethod]
        public void CookieLoginPage()
        {
            // Arrange
            LoginPageStub controller = new LoginPageStub();

            // 假的 cookie
            var cookie = new HttpCookie("UserKeepLogin", "zxcv@gmail.com|f59c56d8 - 2c2f - 43a5 - 8f2d - e8f36613b478");

            // 假的 session
            var fakeSession = new FakeHttpSession();

            // 假的 request
            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.Cookies).Returns(new HttpCookieCollection() { cookie });

            // 假的 controller context
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(x => x.HttpContext.Request).Returns(request.Object);
            controllerContext.SetupGet(x => x.HttpContext.Session).Returns(fakeSession);


            // 傳入假的 controller context
            controller.ControllerContext = controllerContext.Object;

            // Act
            ViewResult result = controller.LoginPage() as ViewResult;

            // Assert
            Assert.AreEqual("Y", result.ViewData["UserKeepLogin"]);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SingUpPage()
        {
            // Arrange
            AccountController controller = new AccountController();

            // Act
            ViewResult result = controller.SingUpPage() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// 測試CheckBox未勾選
        /// </summary>
        [TestMethod]
        public void nonCheckedDoLogin()
        {
            // Arrange
            DoLoginStub controller = new DoLoginStub();

            //假的 帳號登入
            DoLoginIn inMode = new DoLoginIn();
            inMode.UserID = "test@gmail.com";
            inMode.UserPwd = "a1234567";
            inMode.KeepLogin = "false";

            // 假的 session
            var fakeSession = new FakeHttpSession();

            // 假的 controller context
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(x => x.HttpContext.Session).Returns(fakeSession);

            // 傳入假的 controller context
            controller.ControllerContext = controllerContext.Object;

            // Act
            ActionResult result = controller.DoLogin(inMode) ;
            var jsonResult = result as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult, "Result is not a JSON result");
            var serializer = new JsonSerializer();
            var reader = new JsonTextReader(new StringReader(JsonConvert.SerializeObject(jsonResult.Data)));
            dynamic responseObject = serializer.Deserialize(reader);
           
            Assert.AreEqual("", responseObject.ErrMsg.ToString());
            Assert.AreEqual("登入成功", responseObject.ResultMsg.ToString());
        }

        /// <summary>
        /// 測試CheckBox勾選
        /// </summary>
        [TestMethod]
        public void CheckedDoLogin()
        {
            // Arrange
            DoLoginStub controller = new DoLoginStub();

            //假的 帳號登入
            DoLoginIn inModel = new DoLoginIn();
            inModel.UserID = "test@gmail.com";
            inModel.UserPwd = "a1234567";
            inModel.KeepLogin = "true";

            // 假的 cookie
            var cookie = new HttpCookie("UserKeepLogin", "zxcv@gmail.com|f59c56d8 - 2c2f - 43a5 - 8f2d - e8f36613b478");

            // 假的 session
            var fakeSession = new FakeHttpSession();

            // 假的 request
            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.Cookies).Returns(new HttpCookieCollection() { cookie });

            // 假的 controller context
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(x => x.HttpContext.Request).Returns(request.Object);
            controllerContext.SetupGet(x => x.HttpContext.Session).Returns(fakeSession);

            // 傳入假的 controller context
            controller.ControllerContext = controllerContext.Object;

            // Act
            ActionResult result = controller.DoLogin(inModel);
            var jsonResult = result as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult, "Result is not a JSON result");
            var serializer = new JsonSerializer();
            var reader = new JsonTextReader(new StringReader(JsonConvert.SerializeObject(jsonResult.Data)));
            dynamic responseObject = serializer.Deserialize(reader);


            Assert.AreEqual("", responseObject.ErrMsg.ToString());
            Assert.AreEqual("登入成功", responseObject.ResultMsg.ToString());
            // 確認Cookie值是否符合預期
            var a = cookie.Value;
            Assert.AreEqual($"{inModel.UserID}|{null}", cookie.Value);
            Assert.AreEqual(DateTime.Now.AddDays(7), cookie.Expires);
            Assert.IsTrue(cookie.HttpOnly);
        }

        [TestMethod]
        public void AccountPage()
        {
            // Arrange
            AccountController controller = new AccountController();

            // Act
            ViewResult result = controller.AccountPage() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }



    }
}
