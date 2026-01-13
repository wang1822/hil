using CommunityToolkit.Mvvm.ComponentModel;

namespace HardwareSimulator.Models
{
    /// <summary>
    /// 空调系统数据模型
    /// </summary>
    public partial class AirConditionerData : ObservableObject
    {
        /// <summary>
        /// 室内温度 (°C)
        /// </summary>
        [ObservableProperty]
        private double _indoorTemperature;

        /// <summary>
        /// 室外温度 (°C)
        /// </summary>
        [ObservableProperty]
        private double _outdoorTemperature;

        /// <summary>
        /// 设定温度 (°C)
        /// </summary>
        [ObservableProperty]
        private double _setTemperature;

        /// <summary>
        /// 室内湿度 (%)
        /// </summary>
        [ObservableProperty]
        private double _indoorHumidity;

        /// <summary>
        /// 室外湿度 (%)
        /// </summary>
        [ObservableProperty]
        private double _outdoorHumidity;

        /// <summary>
        /// 压缩机频率 (Hz)
        /// </summary>
        [ObservableProperty]
        private double _compressorFrequency;

        /// <summary>
        /// 室内风机转速 (RPM)
        /// </summary>
        [ObservableProperty]
        private int _indoorFanSpeed;

        /// <summary>
        /// 室外风机转速 (RPM)
        /// </summary>
        [ObservableProperty]
        private int _outdoorFanSpeed;

        /// <summary>
        /// 蒸发器温度 (°C)
        /// </summary>
        [ObservableProperty]
        private double _evaporatorTemperature;

        /// <summary>
        /// 冷凝器温度 (°C)
        /// </summary>
        [ObservableProperty]
        private double _condenserTemperature;

        /// <summary>
        /// 吸气压力 (MPa)
        /// </summary>
        [ObservableProperty]
        private double _suctionPressure;

        /// <summary>
        /// 排气压力 (MPa)
        /// </summary>
        [ObservableProperty]
        private double _dischargePressure;

        /// <summary>
        /// 实时功率 (kW)
        /// </summary>
        [ObservableProperty]
        private double _power;

        /// <summary>
        /// 累计能耗 (kWh)
        /// </summary>
        [ObservableProperty]
        private double _totalEnergyConsumption;

        /// <summary>
        /// 运行模式
        /// </summary>
        [ObservableProperty]
        private AcMode _mode;

        /// <summary>
        /// 运行状态
        /// </summary>
        [ObservableProperty]
        private AcStatus _status;

        /// <summary>
        /// 风速档位
        /// </summary>
        [ObservableProperty]
        private FanSpeed _fanSpeedLevel;

        /// <summary>
        /// 运行时间 (小时)
        /// </summary>
        [ObservableProperty]
        private double _runningHours;

        /// <summary>
        /// 制冷量 (kW)
        /// </summary>
        [ObservableProperty]
        private double _coolingCapacity;

        /// <summary>
        /// 能效比 EER
        /// </summary>
        [ObservableProperty]
        private double _eer;
    }

    /// <summary>
    /// 空调运行模式枚举
    /// </summary>
    public enum AcMode
    {
        Cooling,        // 制冷
        Heating,        // 制热
        Dehumidifying,  // 除湿
        Ventilation,    // 通风
        Auto            // 自动
    }

    /// <summary>
    /// 空调运行状态枚举
    /// </summary>
    public enum AcStatus
    {
        Off,        // 关机
        Running,    // 运行
        Standby,    // 待机
        Defrosting, // 除霜
        Fault       // 故障
    }

    /// <summary>
    /// 风速档位枚举
    /// </summary>
    public enum FanSpeed
    {
        Low,    // 低速
        Medium, // 中速
        High,   // 高速
        Auto    // 自动
    }
}
