using Ionic.Zip;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.Tools
{
    /// <summary>
    /// 폴더와 관련된 내용을 가져오는 도구.
    /// </summary>
    public class FolderTool
    {
        #region Fields

        /// <summary>
        /// 현재 작업 폴더의 전체경로를 가져온다.
        /// </summary>
        public static string WorkingFolderFullPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        /// <summary>
        /// 사용자 폴더 전체경로를 가져온다.
        /// </summary>
        // https://stackoverflow.com/questions/1140383/how-can-i-get-the-current-user-directory
        public static string UserFolderFullPath
        {
            get
            {
                string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    path = Directory.GetParent(path).ToString();
                }

                return path;
            }
        }

        /// <summary>
        /// 다수의 폴더 및 파일을 압축 한다.
        /// </summary>
        /// <param name="fullPathList">압축 할 폴더와 파일 전체 경로 리스트.</param>
        /// <param name="zipFilePath">압축 파일 전체 경로.</param>
        /// <returns>성공 여부.</returns>
        // https://johnlnelson.com/tag/c-zip-file/
        // https://docs.microsoft.com/en-us/dotnet/api/system.io.compression.zipfileextensions.createentryfromfile?redirectedfrom=MSDN&view=net-5.0#System_IO_Compression_ZipFileExtensions_CreateEntryFromFile_System_IO_Compression_ZipArchive_System_String_System_String_System_IO_Compression_CompressionLevel_
        public static void Compression(
            string zipFilePath,
            string basePath,
            List<string> fullPathList,
            List<string> validExtensionList,
            List<string> ignoreFolderList = null)
        {
            using (ZipFile zip = new ZipFile(zipFilePath, Encoding.UTF8))
            {
                foreach (string path in fullPathList)
                {
                    Compression(zip, path, basePath, validExtensionList, ignoreFolderList);
                }

                zip.Save();
            }
        }

        /// <summary>
        /// 타겟경로가 부모경로안에 포함되는 경로인지를 확인한다.
        /// </summary>
        /// <param name="targetPath">타겟 경로.</param>
        /// <param name="parentPath">부모 경로.</param>
        /// <returns></returns>
        // https://stackoverflow.com/questions/5617320/given-full-path-check-if-path-is-subdirectory-of-some-other-path-or-otherwise
        public static bool ContainsPath(string targetPath, string parentPath)
        {
            if (string.IsNullOrEmpty(targetPath) || string.IsNullOrEmpty(parentPath))
            {
                return false;
            }

            DirectoryInfo targetDirectory = new DirectoryInfo(targetPath);
            DirectoryInfo parentDirectory = new DirectoryInfo(parentPath);
            if (DirectoryEqual(targetDirectory, parentDirectory))
            {
                return true;
            }

            bool isParent = false;
            while (targetDirectory.Parent != null)
            {
                if (DirectoryEqual(targetDirectory.Parent, parentDirectory))
                {
                    isParent = true;
                    break;
                }
                else
                {
                    targetDirectory = targetDirectory.Parent;
                }
            }

            return isParent;
        }

        /// <summary>
        /// DirectoryInfo의 비교 메소드.
        /// </summary>
        /// <param name="target1">첫번째 비교 오브젝트.</param>
        /// <param name="target2">두번째 비교 오브젝트.</param>
        /// <returns>true : 두 오브젝트가 같다. false : 두 오브젝트가 같지 않다.</returns>
        // https://stackoverflow.com/questions/3155034/why-isnt-this-directoryinfo-comparison-working
        public static bool DirectoryEqual(DirectoryInfo target1, DirectoryInfo target2)
        {
            if (target1.Parent == null || target2.Parent == null)
            {
                return target1.Name == target2.Name;
            }

            var strA = Path.Combine(target1.Parent.FullName, target1.Name);
            var strB = Path.Combine(target2.Parent.FullName, target2.Name);

            return strA.Equals(strB, StringComparison.CurrentCultureIgnoreCase);
        }

        #endregion

        #region Private methods

        private static void Compression(
            ZipFile zip, 
            string targetPath,
            string basePath,
            List<string> validExtensionList,
            List<string> ignoreFolderList)
        {
            FileInfo fileInfo = new FileInfo(targetPath);
            // 받은 경로가 폴더인 경우.
            if ((fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                // 무시 경로인 경우 리턴.
                if (ignoreFolderList != null &&
                    ignoreFolderList.Count > 0 &&
                    ignoreFolderList.Any(x => x.Equals(Path.GetFileName(fileInfo.Name), StringComparison.OrdinalIgnoreCase)) == true)
                {
                    LogManager.GetCurrentClassLogger().Info(string.Format("Ignored folder for diagonosis. Name({0})", fileInfo.Name));
                    return;
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(fileInfo.FullName);
                if (directoryInfo == null)
                {
                    LogManager.GetCurrentClassLogger().Error(string.Format("Invalid file. Name({0})", fileInfo.FullName));
                    return;
                }

                foreach (FileInfo innerFileInfo in directoryInfo.GetFiles())
                {
                    Compression(zip, innerFileInfo.FullName, basePath, validExtensionList, ignoreFolderList);
                }

                foreach (DirectoryInfo innerDirectoryInfo in directoryInfo.GetDirectories())
                {
                    Compression(zip, innerDirectoryInfo.FullName, basePath, validExtensionList, ignoreFolderList);
                }
            }
            // 받은 경로가 파일인 경우.
            else
            {
                if (string.IsNullOrEmpty(fileInfo.Extension) == true)
                {
                    return;
                }

                if (validExtensionList != null && 
                    validExtensionList.Count > 0 && 
                    validExtensionList.Exists(x => x.Equals(fileInfo.Extension.Replace(".", ""), StringComparison.OrdinalIgnoreCase)) == false)
                {
                    return;
                }

                string targetParentPath = Directory.GetParent(fileInfo.FullName).FullName;
                string directoryPathInArchive = targetParentPath;

                if (string.IsNullOrEmpty(basePath) == false)
                {
                    directoryPathInArchive = new Uri(basePath).MakeRelativeUri(new Uri(targetParentPath)).ToString();
                }

                zip.AddFile(fileInfo.FullName, directoryPathInArchive);
            }
        }

        #endregion
    }
}
