using System;
using System.IO;
using UnityEngine;
using MySql.Data.MySqlClient;
using LitJson;

public class User_info
{
    public string UID { get; private set; }
    public string Password { get; private set; }

    public User_info(string uid, string password)
    {
        UID = uid;
        Password = password;
    }
}

public class SQLManager : MonoBehaviour
{
    public User_info info = new User_info("", "");

    public MySqlConnection con;
    public MySqlDataReader reader;

    private string DB_Path = string.Empty;
    public static SQLManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DB_Path = Application.dataPath + "/Database";

        string serverinfo = DBserverSet(DB_Path);
        try
        {
            if (string.IsNullOrEmpty(serverinfo))
            {
                Debug.LogWarning("SQL server Json Error!");
                return;
            }
            con = new MySqlConnection(serverinfo);
            con.Open();
            Debug.Log("SQL server Open Complete");
        }
        catch (Exception e)
        {
            Debug.LogError("DB 연결 실패: " + e.Message);
        }
    }

    private string DBserverSet(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string jsonstring = File.ReadAllText(path + "/config.json");
        JsonData data = JsonMapper.ToObject(jsonstring);
        string serverinfo =
            $"Server={data[0]["IP"]};" +
            $"Database={data[0]["TableName"]};" +
            $"Uid={data[0]["ID"]};" +
            $"Pwd={data[0]["PW"]};" +
            $"Port={data[0]["PORT"]};" +
            $"CharSet=utf8;";

        return serverinfo;
    }

    private bool connect_check(MySqlConnection c)
    {
        if (c.State != System.Data.ConnectionState.Open)
        {
            c.Open();
            if (c.State != System.Data.ConnectionState.Open)
            {
                Debug.LogError("MySqlConnection is not open...");
                return false;
            }
        }
        return true;
        
    }
    

    // 중복 체크
    public bool CheckDuplicate(string id)
    {
        try
        {
            if (!connect_check(con))
                return false;

            string checksql = $"SELECT UID FROM user_info WHERE UID = '{id}';";
            MySqlCommand cmd = new MySqlCommand(checksql, con);

            reader = cmd.ExecuteReader();

            bool exists = reader.HasRows;

            if (!reader.IsClosed) reader.Close();

            return exists;
        }
        catch (Exception e)
        {
            Debug.LogError("CheckDuplicate Error: " + e.Message);
            if (reader != null && !reader.IsClosed) reader.Close();
            return false;
        }
    }

    // 회원가입
    public bool Register(string id, string password)
{
    Debug.Log($"[Register] ID: {id}, PW: {password}");

    if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(password))
    {
        Debug.LogWarning("아이디 또는 비밀번호가 비어 있습니다.");
        return false;
    }

    try
    {
        if (!connect_check(con)) return false;

        string checkUserSql = $"SELECT COUNT(*) FROM user_info WHERE UID = '{id}';";
        MySqlCommand checkCmd = new MySqlCommand(checkUserSql, con);
        int userCount = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (userCount > 0)
        {
            Debug.Log("이미 존재하는 아이디입니다.");
            return false;
        }

        // ✅ Birthday 제거된 쿼리
        string sqlcommend = $"INSERT INTO user_info (UID, Password) VALUES ('{id}', '{password}');";

        MySqlCommand cmd = new MySqlCommand(sqlcommend, con);
        int result = cmd.ExecuteNonQuery();

        if (result > 0)
        {
            Debug.Log("회원가입 성공!");
            return true;
        }
        else
        {
            Debug.Log("회원가입 실패!");
            return false;
        }
    }
    catch (Exception e)
    {
        Debug.LogError("[Register] 예외 발생: " + e.Message);
        return false;
    }
}




    // 로그인
         public bool Login(string id, string password)
        {
            try
            {
                if (!connect_check(con))
                    return false;

                string sqlCommend = $"SELECT UID, Password FROM user_info WHERE UID = '{id}' AND Password = '{password}';";
                MySqlCommand cmd = new MySqlCommand(sqlCommend, con);
                
                using (reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string uid = reader.IsDBNull(0) ? string.Empty : reader["UID"].ToString();
                        string pwd = reader.IsDBNull(1) ? string.Empty : reader["Password"].ToString();

                        if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(pwd))
                        {
                            info = new User_info(uid, pwd);
                            return true;
                        }
                    }
                }

                // 로그인 실패 시 info는 null로 유지
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError("Login Error: " + e.Message);
                return false;
            }
        }

}
