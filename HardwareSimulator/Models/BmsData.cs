using CommunityToolkit.Mvvm.ComponentModel;

namespace HardwareSimulator.Models
{
    /// <summary>
    /// BMS (Battery Management System) 电池管理系统数据模型
    /// </summary>
    public partial class BmsData : ObservableObject
    {
        /// <summary>
        /// 电池总电压 (V)
        /// </summary>
        [ObservableProperty]
        private double _totalVoltage;

        /// <summary>
        /// 电池总电流 (A)
        /// </summary>
        [ObservableProperty]
        private double _totalCurrent;

        /// <summary>
        /// 荷电状态 SOC (%)
        /// </summary>
        [ObservableProperty]
        private double _soc;

        /// <summary>
        /// 健康状态 SOH (%)
        /// </summary>
        [ObservableProperty]
        private double _soh;

        /// <summary>
        /// 最高单体电压 (V)
        /// </summary>
        [ObservableProperty]
        private double _maxCellVoltage;

        /// <summary>
        /// 最低单体电压 (V)
        /// </summary>
        [ObservableProperty]
        private double _minCellVoltage;

        /// <summary>
        /// 平均单体电压 (V)
        /// </summary>
        [ObservableProperty]
        private double _avgCellVoltage;

        /// <summary>
        /// 单体电压差 (V)
        /// </summary>
        [ObservableProperty]
        private double _cellVoltageDiff;

        /// <summary>
        /// 最高温度 (°C)
        /// </summary>
        [ObservableProperty]
        private double _maxTemperature;

        /// <summary>
        /// 最低温度 (°C)
        /// </summary>
        [ObservableProperty]
        private double _minTemperature;

        /// <summary>
        /// 平均温度 (°C)
        /// </summary>
        [ObservableProperty]
        private double _avgTemperature;

        /// <summary>
        /// 温度差 (°C)
        /// </summary>
        [ObservableProperty]
        private double _temperatureDiff;

        /// <summary>
        /// 充电循环次数
        /// </summary>
        [ObservableProperty]
        private int _cycleCount;

        /// <summary>
        /// 总能量 (kWh)
        /// </summary>
        [ObservableProperty]
        private double _totalEnergy;

        /// <summary>
        /// 可用能量 (kWh)
        /// </summary>
        [ObservableProperty]
        private double _availableEnergy;

        /// <summary>
        /// BMS状态
        /// </summary>
        [ObservableProperty]
        private BmsStatus _status;

        /// <summary>
        /// 告警等级
        /// </summary>
        [ObservableProperty]
        private AlarmLevel _alarmLevel;

        /// <summary>
        /// 绝缘电阻 (MΩ)
        /// </summary>
        [ObservableProperty]
        private double _insulationResistance;

        /// <summary>
        /// 电池簇数量
        /// </summary>
        [ObservableProperty]
        private int _clusterCount;

        /// <summary>
        /// 在线电池簇数量
        /// </summary>
        [ObservableProperty]
        private int _onlineClusterCount;
    }

    /// <summary>
    /// BMS运行状态枚举
    /// </summary>
    public enum BmsStatus
    {
        Idle,           // 空闲
        Charging,       // 充电中
        Discharging,    // 放电中
        Balancing,      // 均衡中
        Fault,          // 故障
        Protection      // 保护状态
    }

    /// <summary>
    /// 告警等级枚举
    /// </summary>
    public enum AlarmLevel
    {
        Normal,     // 正常
        Warning,    // 警告
        Alarm,      // 告警
        Critical    // 严重
    }
}
