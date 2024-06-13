using Common;
using S7.Net;
using System;
using UnityEngine;

namespace LabProductLine.ControlModule
{
    /// <summary>
    ///ʵ��PLC���������ӡ��ر��Լ���ȡд��
    /// <summary>
    public class PLCManager : SingletonBase<PLCManager>
    {
        public string localIpAddress = "192.168.2.1";//PLCip地址
        private Plc plc;



        //���캯�������ڳ�ʼ��PLC
        public PLCManager()
        {
            plc = new Plc(CpuType.S7200Smart, localIpAddress, 0, 1);
        }




        public void PlcOpen()
        {
            try
            {
                plc.Open();

                if (plc.IsConnected)
                {
                    Debug.Log("PLC已连接!");
                }

                else
                {
                    Debug.Log("PLC连接失败,请重试!");
                }
            }

            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }


        public void PlcClose()
        {
            try
            {
                plc.Close();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        /// <summary>
        /// ����T����V�������ݲ��õ����ݶ�ȡ����
        /// </summary>
        /// <param name="plc">plcʵ��</param>
        /// <param name="startIndex">��ʼ��ַ</param>
        /// <param name="type">��ȡ����������</param>
        /// <param name="count">��ȡ������</param>
        /// <returns></returns>
        public object Read(int startIndex, VarType type, int count)
        {
            object data = plc.Read(DataType.DataBlock, 1, startIndex, type, count);
            return data;
        }






        /// <summary>
        /// ֱ�Ӹ������������ַ��ȡ��������I��O��M����
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="variable">���������ַ����"I0.0","Q0.1"��</param>
        /// <returns></returns>
        public object Read(string variable)
        {
            object data = plc.Read(variable);
            return data;
        }

        /// <summary>
        /// ��ȡPLC
        /// </summary>
        /// <param name="plc">������plcʵ��</param>
        /// <param name="db">���ݿ�ı��</param>
        /// <param name="db1">���ݵ�ƫ�Ƶ�ַ</param>
        /// <param name="fb">д��Ĳ���</param>
        public void Write(int db, double db1, object fb)
        {
            if (plc.IsConnected == true)
            {
                try
                {
                    plc.Write("DB" + db.ToString() + ".DBX" + db1.ToString("0.0"), fb);//ֹͣ��ť
                }
                catch (Exception)
                {
                    Debug.Log("Error");
                }
            }
        }

        /// <summary>
        /// �������������ַ����д��
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="variable">д���ַ,��"I0.1"</param>
        /// <param name="fb">д���ֵ</param>
        public void Write(string variable, object fb)
        {
            plc.Write(variable, fb);
        }
    }
}



