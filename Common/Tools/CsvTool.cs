using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Tools
{
    /// <summary>
    /// CSV 관련 도구.
    /// </summary>
    public class CsvTool
    {
        #region Fields

        /// <summary>
        /// CSV 분리 캐릭터.
        /// </summary>
        public static readonly string SEPARATOR = ",";

        private static object _lockObject = new object();

        #endregion

        #region Public methods

        /// <summary>
        /// 전달된 CSV파일을 분석하여 반환한다.
        /// 없으면 생성한다.
        /// </summary>
        /// <param name="fileFullName">읽을 CSV파일 전체 경로.</param>
        /// <returns>파일 내용.</returns>
        public static IEnumerable<string[]> FromFile(string fileFullName)
        {
            List<string[]> result = new List<string[]>();

            try
            {
                string folderPath = Path.GetDirectoryName(fileFullName);
                if (Directory.Exists(folderPath) == false)
                {
                    _ = Directory.CreateDirectory(folderPath);
                }

                if (File.Exists(fileFullName) == false)
                {
                    _ = File.Create(fileFullName);
                }

                lock (_lockObject)
                {
                    using (StreamReader stream = new StreamReader(fileFullName, Encoding.Default))
                    {
                        while (stream.EndOfStream == false)
                        {
                            result.Add(stream.ReadLine().Split(new string[1] { SEPARATOR }, StringSplitOptions.None));
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                LogManager.GetCurrentClassLogger().Error("IO 예외 발생. 메시지( {0} )", ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 전달된 데이터를 CSV파일에 적는다.
        /// </summary>
        /// <param name="fileFullName">쓸 CSV파일 전체 경로.</param>
        /// <param name="parameters">쓸 데이터.</param>
        /// <param name="encoding">엔코딩 타입.</param>
        public static void ToFile(string fileFullName, IEnumerable<string[]> parameters)
        {
            try
            {
                string folderPath = Path.GetDirectoryName(fileFullName);
                if (Directory.Exists(folderPath) == false)
                {
                    _ = Directory.CreateDirectory(folderPath);
                }

                using (StreamWriter stream = new StreamWriter(fileFullName, true, Encoding.Default))
                {
                    foreach (string[] parameter in parameters)
                    {
                        stream.WriteLine(string.Join(SEPARATOR, parameter));
                    }

                    stream.Close();
                }
            }
            catch (IOException ex)
            {
                LogManager.GetCurrentClassLogger().Error("IO 예외 발생. 메시지( {0} )", ex.Message);
            }
        }

        #endregion
    }
}
