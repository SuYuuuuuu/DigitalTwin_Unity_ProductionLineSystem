namespace LabProductLine.ControlModule
{
    public enum JogCmd
    {
        IDLE,
        AP_DOWN,
        AN_DOWN, //X-/Joint1-
        BP_DOWN, //Y+/Joint2+
        BN_DOWN, //Y-/Joint2-
        CP_DOWN, //Z+/Joint3+
        CN_DOWN, //Z-/Joint3-
        DP_DOWN, //R+/Joint4+
        DN_DOWN, //R-/Joint4-
        LP_DOWN, //L+。 仅在 isJoint=1 时， LP_DOWN 可用
        LN_DOWN //L-。 仅在 isJoint=1 时， LN_DOWN 可用
    }

    public enum PtpCmd
    {
        JUMP_XYZ, //JUMP 模式， （x,y,z,r） 为笛卡尔坐标系下的目标

        MOVJ_XYZ, //MOVJ 模式， （x,y,z,r） 为笛卡尔坐标系下的目标

        MOVL_XYZ, //MOVL 模式， （x,y,z,r） 为笛卡尔坐标系下的目标

        JUMP_ANGLE, //JUMP 模式， （x,y,z,r） 为关节坐标系下的目标点

        MOVJ_ANGLE, //MOVJ 模式， （x,y,z,r） 为关节坐标系下的目标点

        MOVL_ANGLE, //MOVL 模式， （x,y,z,r） 为关节坐标系下的目标

        MOVJ_INC, //MOVJ 模式， （x,y,z,r） 为关节坐标系下的坐标

        MOVL_INC, //MOVL 模式， （x,y,z,r） 为笛卡尔坐标系下的坐

        MOVJ_XYZ_INC, //MOVJ 模式， （x,y,z,r） 为笛卡尔坐标系下的坐

        JUMP_MOVL_XYZ, //JUMP 模式，平移时运动模式为 MOVL。


    }
}