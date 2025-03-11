using CliNet.Models.Commands;
using CliNet.Models.Commands.AiModule;
using Common.Templates;
using System;
using Unity;

namespace CliNet.Cores.Services
{
    public class ContainerService : Singleton<ContainerService>, IDisposable
    {
        #region Fields

        private readonly UnityContainer _container = new UnityContainer();

        #endregion

        #region Constructors

        public ContainerService()
        {
            _container.RegisterType<PacketInfo, GetServerStateInfo>("GetServerState");
            _container.RegisterType<PacketInfo, GetVehicleStatusInfo>("GetVehicleStatus");
            _container.RegisterType<PacketInfo, SetGoalInfo>("SetGoal");

            _container.RegisterType<PacketInfo, ServerStateInfo>("ServerState");
            _container.RegisterType<PacketInfo, VehicleStatusInfo>("VehicleStatus");
            _container.RegisterType<PacketInfo, ResponsePacketInfo>("Response");
        }

        #endregion

        #region Public methods

        public T Resolve<T>(string keyword)
        {
            return _container.Resolve<T>(keyword);
        }

        public bool TryResolveType<T>(string keyword, out Type type)
        {
            type = null;

            try
            {
                type = Resolve<T>(keyword).GetType();
            }
            catch { }

            return type != null;
        }

        public static void Release()
        {
            Instance.Dispose();
        }

        #endregion


        #region IDisposable implementations

        public void Dispose()
        {
            _container.Dispose();
        }

        #endregion
    }
}
