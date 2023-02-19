using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UR_HomeWork.Tests.Mock
{
    public class FakeHttpSession : HttpSessionStateBase
    {
        Dictionary<string, object> sessionStorage = new Dictionary<string, object>();

        public override object this[string name]
        {
            get { return sessionStorage.ContainsKey(name) ? sessionStorage[name] : null; }
            set { sessionStorage[name] = value; }
        }

        public override void Remove(string name)
        {
            sessionStorage.Remove(name);
        }

        public override void Clear()
        {
            sessionStorage.Clear();
        }
        
    }
}
