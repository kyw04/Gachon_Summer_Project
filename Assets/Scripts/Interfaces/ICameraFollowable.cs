using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

interface ICameraFollowable
{
    public Vector3 lookFoward { get; set; }
    public Vector3 lookRight { get; set; }
}
