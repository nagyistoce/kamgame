using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;



namespace KamGame
{
    public class KamMonoBehaviour : MonoBehaviour
    {

        public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation)
            where T : Object
        {
            return (T)Object.Instantiate(original, position, rotation);
        }

        public T Instantiate<T>(T original)
            where T : Object
        {
            return (T)Object.Instantiate(original, transform.position, transform.rotation);
        }

        public static T Instantiate<T>(T original, ContactPoint contact)
            where T : Object
        {
            return (T)Object.Instantiate(original, contact.point, Vector3.up.ToRotation(contact));
        }

        public static T Instantiate<T>(T original, Collision collision)
            where T : Object
        {
            return Instantiate(original, collision.contacts[0]);
        }


        public void Destroy(float time)
        {
            Destroy(gameObject, time);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }


        public static bool Fire1_IsPressed
        {
            get { return Input.GetButtonDown("Fire1"); }
        }

    }
}
