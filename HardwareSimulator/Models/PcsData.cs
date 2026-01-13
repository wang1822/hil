using CommunityToolkit.Mvvm.ComponentModel;

namespace HardwareSimulator.Models
{
    /// <summary>
    /// PCS (Power Conversion System) 功率转换系统数据模型
    /// </summary>
    public partial class PcsData : ObservableObject
    {
        /// <summary>
        /// 直流电压 (V)
        /// </summary>
        [ObservableProperty]
        private double _dcVoltage;

        /// <summary>
        /// 直流电流 (A)
        /// </summary>
        [ObservableProperty]
        private double _dcCurrent;

        /// <summary>
        /// 直流功率 (kW)
        /// </summary>
        [ObservableProperty]
        private double _dcPower;

        /// <summary>
        /// 交流电压 (V)
        /// </summary>
        [ObservableProperty]
        private double _acVoltage;

        /// <summary>
        /// 交流电流 (A)
        /// </summary>
        [ObservableProperty]
        private double _acCurrent;

        /// <summary>
        /// 交流功率 (kW)
        /// </summary>
        [ObservableProperty]
        private double _acPower;

        /// <summary>
        /// 频率 (Hz)
        /// </summary>
        [ObservableProperty]
        private double _frequency;

        /// <summary>
        /// 功率因数
        /// </summary>
        [ObservableProperty]
        private double _powerFactor;

        /// <summary>
        /// 转换效率 (%)
        /// </summary>
        [ObservableProperty]
        private double _efficiency;

        /// <summary>
        /// 设备温度 (°C)
        /// </summary>
        [ObservableProperty]
        private double _temperature;

        /// <summary>
        /// 运行状态
        /// </summary>
        [ObservableProperty]
        private PcsStatus _status;

        /// <summary>
        /// 工作模式
        /// </summary>
        [ObservableProperty]
        private PcsWorkMode _workMode;

        /// <summary>
        /// 故障代码
        /// </summary>
        [ObservableProperty]
        private int _faultCode;

        /// <summary>
        /// 运行时间 (小时)
        /// </summary>
        [ObservableProperty]
        private double _runningHours;
    }

    /// <summary>
    /// PCS运行状态枚举
    /// </summary>
    public enum PcsStatus
    {
        Stopped,    // 停机
        Running,    // 运行
        Standby,    // 待机
        Fault,      // 故障
        Maintenance // 维护
    }

    /// <summary>
    /// PCS工作模式枚举
    /// </summary>
    public enum PcsWorkMode
    {
        Charging,       // 充电模式
        Discharging,    // 放电模式
        Idle,           // 空闲模式
        GridConnected,  // 并网模式
        OffGrid         // 离网模式
    }
}
