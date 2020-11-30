using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Tinder.DataStructures;
using Tinder.DataStructures.Responses.Matches;
using Tintool.Models.DataStructures;

namespace Models
{
    class FileManager
    {
        private static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"/Tintool/";
        private static string statsFileName = "tst";
        private static string statsFileExtension = ".stat";
        private static string tokenFileName = "tkn";
        private static string tokenFileExtension = ".ttool";
        private static string settingsFileName = "app_settings";
        private static string settingsFileExtension = ".set";

        public static Stats LoadStats()
        {
            Directory.CreateDirectory(path);
            string fullFilePath = path + @"\" + statsFileName + statsFileExtension;

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
                Console.WriteLine(e.Message);
                return null;
            }
            finally
            {
                reader?.Close();
            }
        }

        public static void SaveStats(Stats stats)
        {
            try
            {
                Directory.CreateDirectory(path);

                string statsAsJson = JsonSerializer.Serialize(stats);

                FileStream file = File.Create(path + @"\" + statsFileName + statsFileExtension);
                
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

        public static void SaveToken(string token)
        {
            try
            {
                Directory.CreateDirectory(path);

                FileStream file = File.Create(path + @"\" + tokenFileName + tokenFileExtension);

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
            Directory.CreateDirectory(path);
            string fullFilePath = path + @"\" + tokenFileName + tokenFileExtension;

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
            Directory.CreateDirectory(path);
            string fullFilePath = path + @"\" + settingsFileName + settingsFileExtension;

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
                Console.WriteLine(e.Message);
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
                Directory.CreateDirectory(path);

                string settingsAsJson = JsonSerializer.Serialize(settings);

                FileStream file = File.Create(path + @"\" + settingsFileName + settingsFileExtension);

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
    }
}
