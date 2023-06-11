using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabProductLine.DataManagerModule
{
    public class DataManager:SingletonBase<DataManager>
    {
        public T GetDataById<T>(int ID) where T : BaseData
        {
            return null;
        }
    }
}
