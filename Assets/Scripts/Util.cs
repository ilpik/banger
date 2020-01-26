using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    static class Util
    {
        public static IList<GameObject> FindChildGameObjectsByTag(this Transform transform, string tag) // this позволяет вызывать FindChildGameObjectsByTag как будто это метод класса Transform (метод расширения)
        {
            //transform.childCount //колво детей
            //transform.GetChild() //
            List<GameObject> results = new List<GameObject>();

            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                //Debug.Log(child);
                if (child.tag == tag)
                {
                    results.Add(child.gameObject);
                }

            }

            return results;
        }
    }
}
