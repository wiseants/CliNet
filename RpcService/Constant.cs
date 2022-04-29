using System;
using System.Diagnostics;
using System.Reflection;

namespace RpcService
{
    /// <summary>
    /// 상수 경로 집합 클래스.
    /// </summary>
    public class Constant
    {
        /// <summary>
        /// 제품 경로.
        /// </summary>
        public static string APPLICATION_FOLDER_PATH => string.Format(@"{0}\RpcService\", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));

        /// <summary>
        /// 로그 파일 저장 경로.
        /// </summary>
        public static string LOG_FOLDER_PATH => string.Format(@"{0}logs\", APPLICATION_FOLDER_PATH);

        /// <summary>
        /// 설정 폴더 경로.
        /// </summary>
        public static string CONFIG_FOLDER_PATH => string.Format(@"{0}config\", APPLICATION_FOLDER_PATH);

        /// <summary>
        /// 제품명.
        /// </summary>
        public static string PRODUCT_NAME => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName;
    }
}
