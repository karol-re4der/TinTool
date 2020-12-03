using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Tinder.DataStructures;
using Tinder.DataStructures.Responses.Matches;
using Tintool.Models.DataStructures;

namespace Models
{
    class FileManager
    {
        private static string _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"/Tintool/";
        private static string _statsFolder = "SaveFiles//";
        private static string _tokenFileName = "tkn";
        private static string _settingsFileName = "app_settings";
        private static string _bindingsFileName = "bindings";
        private static string _tokenFileExtension = ".ttool";
        private static string _extension = ".json";

        private static BindTable _bindTable;

        #region Stats
        public static List<FileInfo> FindAvailableSaveFiles()
        {
            return new DirectoryInfo(_path + _statsFolder).GetFiles().Where((x)=>x.Extension.Equals(_extension)).ToList();
        }

        public static Stats LoadStatsWithNumber(string number)
        {
            string statsFileName = _bindTable.bindings.Find((x) => x.PhoneNumber.Equals(number))?.SaveFileName;
            if (string.IsNullOrWhiteSpace(statsFileName))
            {
                return null;
            }

            return LoadStatsWithFileName(statsFileName);
        }

        public static Stats LoadStatsWithFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }

            string fullFilePath = _path + @"\" + _statsFolder + fileName + _extension;

            StreamReader reader = null;
            try
            {
                reader = new StreamReader(fullFilePath);
                Stats stats = JsonSerializer.Deserialize<Stats>(reader.ReadToEnd());
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


        public static List<Stats> LoadAllSavefiles()
        {
            List<Stats> result = new List<Stats>();

            foreach(FileInfo file in FindAvailableSaveFiles())
            {
                if (file.Extension.Equals(_extension))
                {
                    Stats loadedStats = LoadStatsWithFileName(file.Name.Replace(_extension, ""));
                    if (loadedStats != null)
                    {
                        result.Add(loadedStats);
                    }
                }
            }

            return result;
        }

        public static void SaveStats(Stats stats)
        {
            try
            {
                string statsAsJson = JsonSerializer.Serialize(stats);

                FileStream file = File.Create(_path + @"\" +_statsFolder+ stats.FileName + _extension);
                
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

        public static void SaveToken(string token)
        {
            try
            {
                FileStream file = File.Create(_path + @"\" + _tokenFileName + _tokenFileExtension);

                StreamWriter writer = new StreamWriter(file);
                writer.Write(token);
                writer.Close();
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        public static string LoadToken()
        {
            string fullFilePath = _path + @"\" + _tokenFileName + _tokenFileExtension;

            StreamReader reader = null;
            try
            {
                reader = new StreamReader(fullFilePath);
                string token = reader.ReadToEnd();
                return token;
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

        public static AppSettings LoadSettings()
        {
            string fullFilePath = _path + @"\" + _settingsFileName + _extension;

            StreamReader reader = null;
            try
            {
                reader = new StreamReader(fullFilePath);
                AppSettings stats = JsonSerializer.Deserialize<AppSettings>(reader.ReadToEnd());
                return stats;
            }
            catch (FileNotFoundException e)
            {
                return new AppSettings();
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

        public static void SaveSettings(AppSettings settings)
        {
            try
            {

                string settingsAsJson = JsonSerializer.Serialize(settings);

                FileStream file = File.Create(_path + @"\" + _settingsFileName + _extension);

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
            Directory.CreateDirectory(_path);
            Directory.CreateDirectory(_path + _statsFolder);
            LoadBindings();
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

        public static void AddBinding(string number, string fileName)
        {
            var foo = _bindTable.bindings.Find((x) => x.PhoneNumber.Equals(number));

            if (foo == null)
            {
                _bindTable.bindings.Add(new BindTableItemModel
                {
                    PhoneNumber = number,
                    SaveFileName = fileName
                });
            }
            else
            {
                foo.SaveFileName = fileName;
            }

            SaveBindings();
        }

        private static void SaveBindings()
        {
            try
            {
                string bindingsAsJson = JsonSerializer.Serialize(_bindTable);

                FileStream file = File.Create(_path + @"\" + _bindingsFileName + _extension);

                StreamWriter writer = new StreamWriter(file);
                writer.Write(bindingsAsJson);
                writer.Close();
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        private static void LoadBindings()
        {
            string fullFilePath = _path + @"\" + _bindingsFileName + _extension;

            StreamReader reader = null;
            try
            { 
                reader = new StreamReader(fullFilePath);
                _bindTable = JsonSerializer.Deserialize<BindTable>(reader.ReadToEnd());
            }
            catch (FileNotFoundException e)
            {
                _bindTable = new BindTable();
                SaveBindings();
            }
            finally
            {
                reader?.Close();
            }
        }
        #endregion
    }
}
