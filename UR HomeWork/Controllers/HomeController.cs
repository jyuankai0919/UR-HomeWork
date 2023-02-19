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
using UR_HomeWork.Controllers.Feature;

namespace UR_HomeWork.Controllers
{
    public class HomeController : Controller
    {
        UR_DB db = new UR_DB();
        MemberService userMethod = new MemberService();
        public ActionResult Index()
        {


            return View();
        }
    }

}