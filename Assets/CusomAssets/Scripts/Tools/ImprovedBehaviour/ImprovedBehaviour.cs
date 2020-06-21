using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTools.Helpers
{
    public abstract class ImprovedBehaviour : MonoBehaviour
    {
        GameObject go = null;
        Transform tr = null;

        public GameObject GO => this.go ?? (this.go = gameObject);
        public Transform TR => this.tr ?? (this.tr = transform);
    }
}
