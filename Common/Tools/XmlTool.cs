// https://stackoverflow.com/questions/4123590/serialize-an-object-to-xml

using System.IO;
using System.Xml.Serialization;

namespace Common.Tools
{
    /// <summary>
    /// XML 파일 내보내기 가져오기 도구 클래스.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlTool<T>
    {
        /// <summary>
        /// XML 문서로 내보낸다.
        /// </summary>
        /// <param name="fileFullpath">내보낼 문서 전체경로.</param>
        /// <param name="type">내보낼 모델.</param>
        public static void Export(string fileFullpath, T type)
        {
            string folderPath = Directory.GetParent(fileFullpath).FullName;
            if (Directory.Exists(folderPath) == false)
            {
                Directory.CreateDirectory(folderPath);
            }

            var serializer = new XmlSerializer(type.GetType());
            using (var writer = new FileStream(fileFullpath, FileMode.Create))
            {
                serializer.Serialize(writer, type);
            }
        }

        /// <summary>
        /// XML 문서 파일을 가져온다.
        /// </summary>
        /// <param name="fileName">가져올 문서 전체경로.</param>
        /// <returns></returns>
        public static T Import(string fileName)
        {
            T result = default(T);

            using (var stream = System.IO.File.OpenRead(fileName))
            {
                var serializer = new XmlSerializer(typeof(T));
                result = (T)serializer.Deserialize(stream);
            }

            return result;
        }
    }
}
