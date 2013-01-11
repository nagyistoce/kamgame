using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



namespace KamGame
{
    
    
    public static class GraphicsHelper
    {

        public static Quaternion ToRotation(this Vector3 fromDirection, Vector3 toDirection)
        {
            return Quaternion.FromToRotation(fromDirection, toDirection);
        }

        public static Quaternion ToRotation(this Vector3 fromDirection, ContactPoint toDirection)
        {
            return Quaternion.FromToRotation(fromDirection, toDirection.normal);
        }

    }


}
