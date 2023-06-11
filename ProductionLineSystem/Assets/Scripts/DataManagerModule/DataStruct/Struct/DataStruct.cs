// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace LabProductLine.DataManagerModule
// {

//     public struct JogJointParams
//     {
//         public float[] velocity;
//         public float[] acceleration;

//         public override bool Equals(object obj)
//         {
//             if (!(obj is JogJointParams))
//                 return false;
//             JogJointParams other = (JogJointParams)obj;
//             return this.velocity == other.velocity && this.acceleration == other.acceleration;
//         }

//         public override int GetHashCode()
//         {
//             return velocity.GetHashCode() ^ acceleration.GetHashCode();
//         }

//         public static bool operator ==(JogJointParams person1, JogJointParams person2)
//         {
//             return person1.Equals(person2);
//         }

//         public static bool operator !=(JogJointParams person1, JogJointParams person2)
//         {
//             return !person1.Equals(person2);
//         }
//     }

//     public struct JogCoordinateParams
//     {
//         public float[] velocity;
//         public float[] acceleration;

//     }

//     public struct JOGCommonParams
//     {
//         public float[] velocityRatio;
//         public float[] accelerationRatio;

//     }

//     public struct PTPJointParams
//     {
//         public float[] velocity;
//         public float[] acceleration;

//     }

//     public struct PTPCoordinateParams
//     {
//         public float[] velocity;
//         public float[] acceleration;

//     }

//     public struct PTPCommonParams
//     {
//         public float[] velocityRatio;
//         public float[] accelerationRatio;

//     }


// }