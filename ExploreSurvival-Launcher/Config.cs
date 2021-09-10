using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExploreSurvival_Launcher
{
    class Config
    {
        public ConfigData configData = new ConfigData("", "", "", true, 0, Environment.GetEnvironmentVariable("JAVA_HOME") + @"bin\java.exe", 1024, false, "https://github.com/ExploreSurvival-Development-Team/ExploreSurvival-Game", Cod.COMPILE);
        public void Load()
        {
            if (!File.Exists("esl.json"))
            {
                File.WriteAllText("esl.json", JsonConvert.SerializeObject(configData));
            }
            else
            {
                configData = JsonConvert.DeserializeObject<ConfigData>(File.ReadAllText("esl.json"));
            }
        }
        public void Save()
        {
            File.WriteAllText("esl.json", JsonConvert.SerializeObject(configData));
        }
    }

    class ConfigData
    {
        public string Username;
        public string Session;
        public string UUID;
        public bool OfflineLogin;
        public long Expire;
        public string JavaPath;
        public int JVMmemory;
        public bool ShowLogs;
        public string GitURL;
        public Cod Cod;

        public ConfigData(string Username, string Session, string UUID, bool OfflineLogin, long Expire, string JavaPath, int JVMmemory, bool ShowLogs, string GitURL, Cod Cod)
        {
            this.Username = Username;
            this.Session = Session;
            this.UUID = UUID;
            this.OfflineLogin = OfflineLogin;
            this.Expire = Expire;
            this.JavaPath = JavaPath;
            this.JVMmemory = JVMmemory;
            this.ShowLogs = ShowLogs;
            this.GitURL = GitURL;
            this.Cod = Cod;
        }
    }
    enum Cod
    {
        COMPILE = 0,
        DOWNLOAD = 1
    }
}
