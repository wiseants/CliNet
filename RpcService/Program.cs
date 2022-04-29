using RpcService.Cores.Services;
using System.ServiceProcess;

namespace RpcService
{
    static class Program
    {
        #region Private methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var a = Constant.LOG_FOLDER_PATH;
            var b = string.Format(@"{0}${{shortdate}}.log", Constant.LOG_FOLDER_PATH);

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new RpcServerService()
            };

            ServiceBase.Run(ServicesToRun);
        }

        #endregion
    }
}
