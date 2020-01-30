using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NerScript.RiValuer
{
    public class RiValuer
    {
        public static string version = "v0.4.0";


        public Type[] Get()
        {
            return Assembly
            .GetAssembly(typeof(RiValuer))
            .GetTypes()
            .Where(t =>
            {
                return t.IsSubclassOf(typeof(RiValuer)) && !t.IsAbstract;
            })
            .ToArray();
        }



    }
}
