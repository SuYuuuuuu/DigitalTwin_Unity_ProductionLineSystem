namespace LabProductLine.DataManagerModule
{
    public interface IProductionDataAccess
    {
        EarPhoneData GetEarPhoneDataByID(int ID);
        PlugData GetPlugDataByID(int ID);
        TrackData GetTrackDataByID(int ID);
        PhoneData GetPhoneDataByID(int ID);
        PhoneBoxData GetPhoneBoxDataByID(int ID);
        GrapData GetGrapDataByID(int ID);
    }
}