using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "DobotAnimation", menuName = "ProductionLineSystem/DobotAnimation", order = 0)]
public class DobotAnimation : ScriptableObject
{
        public string animName;

        public int animId;
        public string animDescription;

        public List<AnimationClip> animClips;
}
