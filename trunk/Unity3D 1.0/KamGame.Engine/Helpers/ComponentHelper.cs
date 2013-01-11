using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KamGame
{
    public static class ComponentHelper
    {

        public static Collider IgnoreCollision(this Collider cmp, Collider cmp2)
        {
            Physics.IgnoreCollision(cmp, cmp2);
            return cmp;
        }

        public static Collider IgnoreCollision(this Collider cmp, Component cmp2)
        {
            Physics.IgnoreCollision(cmp, cmp2.collider);
            return cmp;
        }

        public static T IgnoreCollision<T>(this T cmp, Collider cmp2)
            where T: Component
        {
            Physics.IgnoreCollision(cmp.collider, cmp2);
            return cmp;
        }

        public static T IgnoreCollision<T>(this T cmp, Component cmp2)
            where T : Component
        {
            Physics.IgnoreCollision(cmp.collider, cmp2.collider);
            return cmp;
        }

    }
}
