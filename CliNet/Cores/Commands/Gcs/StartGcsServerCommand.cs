using CliNet.Cores.Implementations;
using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CliNet.Models.Commands;
using CliNet.Models.Commands.AiModule;
using CommandLine;
using System;

namespace CliNet.Cores.Commands.Gcs
{
    [Verb("start.server", HelpText = "노바코스 인터페이스 GCS 통신 서버 시작.")]
    internal class StartGcsServerCommand : IAction
    {
        #region Fields

        public static readonly string SERVER_NAME = "GCS 데이터서버";

        #endregion

        #region Constructors

        public StartGcsServerCommand() 
        {
        }

        #endregion

        #region Properties

        public bool IsValid => true;

        [Option('i', "ip", Required = false, HelpText = "로컬 서버 IP 주소.")]
        public string IpAddress
        {
            get;
            set;
        } = "127.0.0.1";

        [Option('p', "port", Required = false, HelpText = "로컬 서버 포트 번호.")]
        public int Port
        {
            get;
            set;
        } = 15300;

        #endregion

        #region Public methods

        public int Action()
        {
            if (ThreadManager.Instance.IsExist(SERVER_NAME))
            {
                Console.WriteLine($"[{SERVER_NAME}] 서버가 이미 동작중입니다.");
                return 0;
            }

            GcsServer server = new GcsServer
            {
                IpAddress = IpAddress,
                Port = Port,
            };
            server.Request += (x) =>
            {
                object result = null;

                try
                {
                    if (x is GetServerStateInfo getServerState)
                    {
                        result = BuildServerState();
                    }
                    else if (x is GetVehicleStatusInfo getVehicleStatus)
                    {
                        result = BuildVehicleStatus();
                    }
                    else if (x is SetGoalInfo setGoal)
                    {
                        result = BuildResponsePacket();
                    }
                }
                catch { }

                return result;
            };

            ThreadManager.Instance.Add(SERVER_NAME, server);

            return 0;
        }

        #endregion

        #region Private methods

        private ServerStateInfo BuildServerState()
        {
            return new ServerStateInfo()
            {
                State = Common.GcsState.Disarm,
                ResultCode = 1
            };
        }

        private VehicleStatusInfo BuildVehicleStatus()
        {
            return new VehicleStatusInfo()
            {
                Role = 1.6F,
                Pitch = 2.4F,
                Latitude = 37.293153,
                Longitude = 126.813816,
                Altitude = 0.2F,
                Head = 42.6F,
                GroundCourse = 48.0F,
                TargetBearing = 114.9F,
                AirSpeed = 0.1F,
                GroundSpeed = 0.0F,
                BatteryRemaining = 100,
                LinkQualityGcs = 99,
                VibeX = 0.0001F,
                VibeY = 0.00001F,
                VibeZ = 0.000001F,
                TargetLatitude = 37.27876,
                TargetLongitude = 126.788549,
                SlantRange = 1523,
                ResultCode = 1
            };
        }

        private ResponsePacketInfo BuildResponsePacket()
        {
            return new ResponsePacketInfo()
            {
                ResultCode = 1
            };
        }

        #endregion
    }
}
