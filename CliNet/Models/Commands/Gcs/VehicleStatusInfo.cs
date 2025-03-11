using Newtonsoft.Json;

namespace CliNet.Models.Commands.AiModule
{
    /// <summary>
    /// 기체 스테이터스 모델.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class VehicleStatusInfo : PacketInfo
    {
        #region Constructors

        public VehicleStatusInfo()
        {
            Name = "VehicleStatus";
        }

        #endregion


        #region Properties

        /// <summary>
        /// 기체 자이로 센서 롤 값.
        /// </summary>
        public float Role
        {
            get;
            set;
        }

        /// <summary>
        /// 기체 자이로 센서 피치 값.
        /// </summary>
        public float Pitch
        {
            get;
            set;
        }

        /// <summary>
        /// 기체 위치 GEO 좌표 위도.
        /// </summary>
        public double Latitude
        {
            get;
            set;
        }

        /// <summary>
        /// 기체 위치 GEO 좌표 경도.
        /// </summary>
        public double Longitude
        {
            get;
            set;
        }

        /// <summary>
        /// 기체 상대 고도.
        /// </summary>
        public float Altitude
        {
            get;
            set;
        }

        /// <summary>
        /// 기체 헤드 방향.
        /// </summary>
        public float Head
        {
            get;
            set;
        }

        /// <summary>
        /// 기체의 지도상 이동 방향.
        /// </summary>
        public float GroundCourse
        {
            get;
            set;
        }

        /// <summary>
        /// 기체가 다음으로 이동하는 목표(경로점/원형 비행 중심) 방향.
        /// </summary>
        public float TargetBearing
        {
            get;
            set;
        }

        /// <summary>
        /// 센서로 측정되는 에어스피드.
        /// </summary>
        public float AirSpeed
        {
            get;
            set;
        }

        /// <summary>
        /// 좌표로 계산되는 그라운드스피드.
        /// </summary>
        public float GroundSpeed
        {
            get;
            set;
        }

        /// <summary>
        /// 배터리 잔량 값.
        /// </summary>
        public int BatteryRemaining
        {
            get;
            set;
        }

        /// <summary>
        /// 패킷 수신률 값.
        /// </summary>
        public int LinkQualityGcs
        {
            get;
            set;
        }

        /// <summary>
        /// X 방향(기체 앞뒤) 기체 진동 값.
        /// </summary>
        public float VibeX
        {
            get;
            set;
        }

        /// <summary>
        /// Y 방향(기체 좌우) 기체 진동 값.
        /// </summary>
        public float VibeY
        {
            get;
            set;
        }

        /// <summary>
        /// Z 방향(기체 상하) 기체 진동 값.
        /// </summary>
        public float VibeZ
        {
            get;
            set;
        }

        /// <summary>
        /// 카메라 센서에서 측정한 표적 위치 GEO 좌표 위도.
        /// </summary>
        public double TargetLatitude
        {
            get;
            set;
        }

        /// <summary>
        /// 카메라 센서에서 측정한 표적 위치 GEO 좌표 경도.
        /// </summary>
        public double TargetLongitude
        {
            get;
            set;
        }

        /// <summary>
        /// 카메라 센서에서 측정한 표적과의 경사 거리.
        /// </summary>
        public double SlantRange
        {
            get;
            set;
        }

        #endregion
    }
}
