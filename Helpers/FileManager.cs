using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Windows;
using Tintool.Models.Saveables;

namespace Models
{
    class FileManager
    {
        private static string _path;
        private static string _statsFolder = "SaveFiles//";
        private static string _sessionFileName = "session";
        private static string _settingsFileName = "app_settings";
        private static string _sessionFileExtension = ".bin";
        private static string _genericExtension = ".json";

        private static byte[] _encryptionKey = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

        #region Stats
        public static List<FileInfo> FindAvailableSaveFiles()
        {
            return new DirectoryInfo(_path + _statsFolder).GetFiles().Where((x)=>x.Extension.Equals(_genericExtension)).ToList();
        }

        public static StatsModel LoadStatsWithFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }
            else if (fileName.EndsWith(_genericExtension))
            {
                fileName = fileName.Replace(_genericExtension, "");
            }

            string fullFilePath = _path + @"\" + _statsFolder + fileName + _genericExtension;

            StreamReader reader = null;
            try
            {
                reader = new StreamReader(fullFilePath);
                StatsModel stats = JsonSerializer.Deserialize<StatsModel>(reader.ReadToEnd());
                stats.ResetDate();
                return stats;
            }
            catch (FileNotFoundException e)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                reader?.Close();
            }
        }

        public static List<StatsModel> LoadAllSavefiles()
        {
            List<StatsModel> result = new List<StatsModel>();

            foreach(FileInfo file in FindAvailableSaveFiles())
            {
                if (file.Extension.Equals(_genericExtension))
                {
                    StatsModel loadedStats = LoadStatsWithFileName(file.Name.Replace(_genericExtension, ""));
                    if (loadedStats != null)
                    {
                        result.Add(loadedStats);
                    }
                }
            }

            return result;
        }

        public static void SaveStats(StatsModel stats)
        {
            try
            {
                string statsAsJson = JsonSerializer.Serialize(stats);

                FileStream file = File.Create(_path + @"\" +_statsFolder+ stats.FileName + _genericExtension);
                
                StreamWriter writer = new StreamWriter(file);
                writer.Write(statsAsJson);
                writer.Close();
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
        #endregion

        public static void SaveSession(SessionModel session)
        {
            try
            {
                FileStream file = File.Create(_path + @"\" + _sessionFileName + _sessionFileExtension);

                using Aes aes = Aes.Create();
                aes.Key = _encryptionKey;
                file.Write(aes.IV, 0, aes.IV.Length);
                CryptoStream cStream = new CryptoStream(file, aes.CreateEncryptor(), CryptoStreamMode.Write);
                StreamWriter writer = new StreamWriter(cStream);
                writer.Write(JsonSerializer.Serialize(session));
                writer.Close();
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        public static SessionModel LoadSession()
        {
            string fullFilePath = _path + @"\" + _sessionFileName + _sessionFileExtension;

            StreamReader reader = null;
            try
            {
                FileStream file = new FileStream(fullFilePath, FileMode.Open);

                using Aes aes = Aes.Create();
                byte[] iv = new byte[aes.IV.Length];
                file.Read(iv, 0, iv.Length);

                CryptoStream cStream = new CryptoStream(file, aes.CreateDecryptor(_encryptionKey, iv), CryptoStreamMode.Read);
                reader = new StreamReader(cStream);

                string sessionAsJson = reader.ReadToEnd();
                SessionModel result = JsonSerializer.Deserialize<SessionModel>(sessionAsJson);

                return result;
            }
            catch (FileNotFoundException e)
            {
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            finally
            {
                reader?.Close();
            }
        }

        public static SettingsModel LoadSettings()
        {
            string fullFilePath = _path + @"\" + _settingsFileName + _genericExtension;

            StreamReader reader = null;
            try
            {
                reader = new StreamReader(fullFilePath);
                SettingsModel stats = JsonSerializer.Deserialize<SettingsModel>(reader.ReadToEnd());
                return stats;
            }
            catch (FileNotFoundException e)
            {
                return new SettingsModel();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                reader?.Close();
            }
        }

        public static void SaveSettings(SettingsModel settings)
        {
            try
            {

                string settingsAsJson = JsonSerializer.Serialize(settings);

                FileStream file = File.Create(_path + @"\" + _settingsFileName + _genericExtension);

                StreamWriter writer = new StreamWriter(file);
                writer.Write(settingsAsJson);
                writer.Close();
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        #region Preparation
        public static void Prepare()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Tintool_Debug/";
            }
            else
            {
                _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Tintool/";
            }
            Directory.CreateDirectory(_path);
            Directory.CreateDirectory(_path + _statsFolder);
        }

        public static string CreateUniqueStatsName()
        {
            string sequence = "";

            while (string.IsNullOrWhiteSpace(sequence) || FindAvailableSaveFiles().Find((x)=>x.Name.Contains(sequence))!=null)
            {
                sequence = Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + DateTime.Now.Ticks;
                sequence = System.Text.RegularExpressions.Regex.Replace(sequence, "[^0-9a-zA-Z]+", "");
            }
            return sequence;
        }
        #endregion
    }
}
