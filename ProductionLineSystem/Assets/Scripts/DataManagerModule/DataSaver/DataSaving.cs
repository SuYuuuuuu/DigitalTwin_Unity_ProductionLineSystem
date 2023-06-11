using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace LabProductLine.DataManagerModule
{

    public class DataSaving
    {
        // MySQL数据库连接字符串
        private static string connectionString = "Server=127.0.0.1;Database=robot_arm_db;UserId=root;Password=abc6311268;";

        // 缓存机械臂数据的队列
        private Queue<RobotData> robotDataQueue = new Queue<RobotData>();

        // 数据库写入间隔时间
        private float writeInterval = 5.0f;

        // 上次数据库写入时间
        private float lastWriteTime = 0.0f;

        // 数据库连接
        private static IDbConnection dbConnection;


        public static void InsertRobotArmData(int robotId, float jointAngle1, float jointAngle2, float jointAngle3, float jointAngle4,
                                  float vibrationFrequency, float endEffectorPosX, float endEffectorPosY, float endEffectorPosZ,
                                  float temperature, int operationStatus)
        {
            ConnectDataBase();

            IDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = "INSERT INTO robot_arm_data (robotId,timestamp, joint_angle1, joint_angle2, joint_angle3, joint_angle4, " +
                                    "vibration_frequency, end_effector_position_x, end_effector_position_y, end_effector_position_z, temperature,operation) " +
                                    "VALUES (@robotId,NOW(), @jointAngle1, @jointAngle2, @jointAngle3, @jointAngle4, " +
                                    "@vibrationFrequency, @endEffectorPosX, @endEffectorPosY, @endEffectorPosZ, @temperature,@operationStatus)";
            dbCommand.Parameters.Add(new MySqlParameter("@robotId", robotId));
            dbCommand.Parameters.Add(new MySqlParameter("@jointAngle1", jointAngle1));
            dbCommand.Parameters.Add(new MySqlParameter("@jointAngle2", jointAngle2));
            dbCommand.Parameters.Add(new MySqlParameter("@jointAngle3", jointAngle3));
            dbCommand.Parameters.Add(new MySqlParameter("@jointAngle4", jointAngle4));
            dbCommand.Parameters.Add(new MySqlParameter("@vibrationFrequency", vibrationFrequency));
            dbCommand.Parameters.Add(new MySqlParameter("@endEffectorPosX", endEffectorPosX));
            dbCommand.Parameters.Add(new MySqlParameter("@endEffectorPosY", endEffectorPosY));
            dbCommand.Parameters.Add(new MySqlParameter("@endEffectorPosZ", endEffectorPosZ));
            dbCommand.Parameters.Add(new MySqlParameter("@temperature", temperature));
            dbCommand.Parameters.Add(new MySqlParameter("@operationStatus", operationStatus));

            dbCommand.ExecuteNonQuery();

            CloseDataBase();
        }
        private static void ConnectDataBase()
        {
            dbConnection = new MySqlConnection(connectionString);
            dbConnection.Open();
        }
        private static void CloseDataBase()
        {
            dbConnection.Close();
        }

        // 创建传感器数据表
        private void CreateSensorDataTable(string command)
        {
            MySqlConnection dbConnection = new MySqlConnection(connectionString);
            MySqlCommand cmd = dbConnection.CreateCommand();
            cmd.CommandText = command;
            cmd.ExecuteNonQuery();
        }

        public void SaveData()
        {

        }

        public void LoadData()
        {

        }
    }
}
