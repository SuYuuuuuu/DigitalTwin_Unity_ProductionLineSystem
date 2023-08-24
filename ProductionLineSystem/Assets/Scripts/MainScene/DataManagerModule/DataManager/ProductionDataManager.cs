using System.Collections.Generic;
using UnityEngine;

namespace LabProductLine.DataManagerModule
{
    public class ProductionDataManager : IProductionDataAccess
    {
        private static ProductionDataManager instance;
        public static ProductionDataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductionDataManager();
                    return instance;
                }
                return instance;
            }
        }

        Dictionary<int, EarPhoneData> earPhoneDic;
        Dictionary<int, PlugData> plugDic;
        Dictionary<int, TrackData> trackDic;
        Dictionary<int, PhoneData> phoneDic;
        Dictionary<int, PhoneBoxData> phoneBoxDic;
        Dictionary<int, GrapData> grapDic;

        public ProductionDataManager()
        {
            earPhoneDic = new Dictionary<int, EarPhoneData>();
            plugDic = new Dictionary<int, PlugData>();
            trackDic = new Dictionary<int, TrackData>();
            phoneDic = new Dictionary<int, PhoneData>();
            phoneBoxDic = new Dictionary<int, PhoneBoxData>();
            grapDic = new Dictionary<int, GrapData>();
        }

        public EarPhoneData GetEarPhoneDataByID(int ID)
        {
            if (!earPhoneDic.ContainsKey(ID))
                return null;
            return earPhoneDic[ID];
        }
        public PlugData GetPlugDataByID(int ID)
        {
            if (!plugDic.ContainsKey(ID))
                return null;
            return plugDic[ID];
        }
        public TrackData GetTrackDataByID(int ID)
        {
            if (!trackDic.ContainsKey(ID))
                return null;
            return trackDic[ID];
        }
        public PhoneData GetPhoneDataByID(int ID)
        {
            if (!phoneDic.ContainsKey(ID))
                return null;
            return phoneDic[ID];
        }
        public PhoneBoxData GetPhoneBoxDataByID(int ID)
        {
            if (!phoneBoxDic.ContainsKey(ID))
                return null;
            return phoneBoxDic[ID];
        }
        public GrapData GetGrapDataByID(int ID)
        {
            if (!grapDic.ContainsKey(ID))
                return null;
            return grapDic[ID];
        }

        public void AddEarPhoneData(EarPhoneData data)
        {
            if (!earPhoneDic.ContainsKey(data.ID))
                earPhoneDic.Add(data.ID, data);
            else
                Debug.LogWarning("EarPhone data with ID " + data.ID + " already exists.");
        }

        public void AddPlugData(PlugData data)
        {
            if (!plugDic.ContainsKey(data.ID))
                plugDic.Add(data.ID, data);
            else
                Debug.LogWarning("Plug data with ID " + data.ID + " already exists.");
        }

        public void AddTrackData(TrackData data)
        {
            if (!trackDic.ContainsKey(data.ID))
                trackDic.Add(data.ID, data);
            else
                Debug.LogWarning("Track data with ID " + data.ID + " already exists.");
        }

        public void AddPhoneData(PhoneData data)
        {
            if (!phoneDic.ContainsKey(data.ID))
                phoneDic.Add(data.ID, data);
            else
                Debug.LogWarning("Phone data with ID " + data.ID + " already exists.");
        }

        public void AddPhoneBoxData(PhoneBoxData data)
        {
            if (!phoneBoxDic.ContainsKey(data.ID))
                phoneBoxDic.Add(data.ID, data);
            else
                Debug.LogWarning("PhoneBox data with ID " + data.ID + " already exists.");
        }

        public void AddGrapData(GrapData data)
        {
            if (!grapDic.ContainsKey(data.ID))
                grapDic.Add(data.ID, data);
            else
                Debug.LogWarning("Grap data with ID " + data.ID + " already exists.");
        }

    }
}
