// https://blog.kgoon.net/4
using System.Configuration;

namespace Common.Tools
{
    /// <summary>
    /// App.config 사용 도구.
    /// </summary>
    public class AppConfiguration
    {
        #region Public methods

        /// <summary>
        /// 설정 값 획득.
        /// </summary>
        /// <param name="key">설정 키워드.</param>
        /// <returns>설정 값.</returns>
        public static string GetAppConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 설정 값 대입.
        /// </summary>
        /// <param name="key">설정 키워드.</param>
        /// <param name="value">대입할 값.</param>
        public static void SetAppConfig(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection cfgCollection = config.AppSettings.Settings;

            cfgCollection.Remove(key);
            cfgCollection.Add(key, value);

            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }

        /// <summary>
        /// 설정 추가.
        /// </summary>
        /// <param name="key">설정 키워드.</param>
        /// <param name="value">최초 값.</param>
        public static void AddAppConfig(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            KeyValueConfigurationCollection cfgCollection = config.AppSettings.Settings;

            cfgCollection.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }

        /// <summary>
        /// 설정 제거.
        /// </summary>
        /// <param name="key">설정 키워드.</param>
        public static void RemoveAppConfig(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            KeyValueConfigurationCollection cfgCollection = config.AppSettings.Settings;

            try
            {
                cfgCollection.Remove(key);

                config.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
            }
            catch { }
        }

        #endregion
    }
}
