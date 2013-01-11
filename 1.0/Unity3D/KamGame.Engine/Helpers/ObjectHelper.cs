using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;



namespace KamGame
{

    public static class ObjectHelper
    {

        public static void Destroy(this Object obj)
        {
            Object.Destroy(obj);
        }

        public static T Destroy<T>(this T obj, float time)
            where T: Object
        {
            Object.Destroy(obj, time);
            return obj;
        }

    }


}
