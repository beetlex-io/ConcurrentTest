using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.ConcurrentTest
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CTestCase : Attribute
    {
        public CTestCase()
        {
            
        }
        internal object CaseObject { get; set; }
        public string Name { get; set; }
        internal Action Action { get; set; }


    }
}
