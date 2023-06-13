using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabProductLine.DataManagerModule
{
    public class DataManager : SingletonBase<DataManager>
    {
        Dictionary<int, BaseData> dataStorageDic;

        public DataManager()
        {
            dataStorageDic = new Dictionary<int, BaseData>();
        }


        ///根据ID获取Data
        public T GetDataById<T>(int ID) where T : BaseData
        {
            if (!dataStorageDic.ContainsKey(ID))
                return null;
            return dataStorageDic[ID] as T;
        }

        ///根据ID添加data,因为添加的可能为new T()类型没有名字，所以返回一个新添加的数据方便引用
        public T AddData<T>(int ID, T data) where T : BaseData
        {
            if (dataStorageDic.ContainsKey(ID)) return null;
            data.ID = ID;//这里直接进行外部赋值，保证ID一致
            dataStorageDic.Add(ID, data);
            return data;
        }

        ///重载
        public T AddData<T>(T data) where T : BaseData
        {
            if (dataStorageDic.ContainsKey(data.ID)) return null;
            dataStorageDic.Add(data.ID, data);
            return data;
        }


        ///根据ID更新data
        public void UpdateData<T>(T data) where T : BaseData
        {
            if (!dataStorageDic.ContainsKey(data.ID)) //如果字典中不存在数据，则自动进行添加
                AddData<T>(data.ID, data);
            dataStorageDic[data.ID] = data;
        }


        ///删除数据
        public void RemoveData<T>(T data) where T : BaseData
        {
            if (!dataStorageDic.ContainsKey(data.ID)) return;
            dataStorageDic.Remove(data.ID);
        }

    }
}
