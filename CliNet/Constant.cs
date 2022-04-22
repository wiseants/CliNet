using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CliNet
{
    /// <summary>
    /// 상수 경로 집합 클래스.
    /// </summary>
    public class Constant
    {
        /// <summary>
        /// 제품 경로.
        /// </summary>
        public static string APPLICATION_FOLDER_PATH => string.Format(@"{0}\Cli\", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

        /// <summary>
        /// 로그 파일 저장 경로.
        /// </summary>
        public static string LOG_FOLDER_PATH => string.Format(@"{0}logs\", APPLICATION_FOLDER_PATH);

        /// <summary>
        /// 설정 폴더 경로.
        /// </summary>
        public static string CONFIG_FOLDER_PATH => string.Format(@"{0}config\", APPLICATION_FOLDER_PATH);

        /// <summary>
        /// 캐쉬 폴더 경로.
        /// </summary>
        public static string CACHE_FOLDER_PATH => string.Format(@"{0}cash\", APPLICATION_FOLDER_PATH);

        /// <summary>
        /// 제품명.
        /// </summary>
        public static string PRODUCT_NAME => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName;

        /// <summary>
        /// 리소스 폴더.
        /// </summary>
        public static string RESOURCE_FOLDER_PATH => string.Format(@"{0}resource\", APPLICATION_FOLDER_PATH);

        /// <summary>
        /// 컨텐츠 리소스 폴더.
        /// </summary>
        public static string CONTENT_RESOURCE_FOLDER_PATH => string.Format(@"{0}\Assets\Resources\", Directory.GetCurrentDirectory());
    }
}
