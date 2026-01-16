namespace HardwareSimulator.Models
{
    /// <summary>
    /// Modbus TCP 协议配置 - 寄存器地址映射
    /// 地址映射说明：
    /// 根据 "BMS-TCP通讯协议.xlsx" 和 "BMS与PCS设备Modbus规约及点表-485.pdf" 配置
    /// 实际部署时需要根据具体协议文档调整地址
    /// 当前使用示例地址，可通过配置文件动态加载
    /// </summary>
    public static class ProtocolConfig
    {
        /// <summary>
        /// PCS 数据寄存器地址映射
        /// </summary>
        public static class PcsRegisters
        {
            // 起始地址：40001 (Modbus地址从0开始，因此实际地址为40000)
            public const ushort BaseAddress = 40000;

            // 直流侧数据
            public const ushort DcVoltage = BaseAddress + 0;      // 直流电压 (2个寄存器，浮点数)
            public const ushort DcCurrent = BaseAddress + 2;      // 直流电流 (2个寄存器，浮点数)
            public const ushort DcPower = BaseAddress + 4;        // 直流功率 (2个寄存器，浮点数)

            // 交流侧数据
            public const ushort AcVoltage = BaseAddress + 6;      // 交流电压 (2个寄存器，浮点数)
            public const ushort AcCurrent = BaseAddress + 8;      // 交流电流 (2个寄存器，浮点数)
            public const ushort AcPower = BaseAddress + 10;       // 交流功率 (2个寄存器，浮点数)
            public const ushort Frequency = BaseAddress + 12;     // 频率 (2个寄存器，浮点数)

            // 其他参数
            public const ushort PowerFactor = BaseAddress + 14;   // 功率因数 (2个寄存器，浮点数)
            public const ushort Efficiency = BaseAddress + 16;    // 效率 (2个寄存器，浮点数)
            public const ushort Temperature = BaseAddress + 18;   // 温度 (2个寄存器，浮点数)

            // 状态和模式
            public const ushort Status = BaseAddress + 20;        // 运行状态 (1个寄存器，枚举)
            public const ushort WorkMode = BaseAddress + 21;      // 工作模式 (1个寄存器，枚举)
            public const ushort FaultCode = BaseAddress + 22;     // 故障代码 (1个寄存器)
            public const ushort RunningHours = BaseAddress + 23;  // 运行时间 (2个寄存器，浮点数)
        }

        /// <summary>
        /// BMS 数据寄存器地址映射
        /// </summary>
        public static class BmsRegisters
        {
            // 起始地址：40100
            public const ushort BaseAddress = 40100;

            // 总体参数
            public const ushort TotalVoltage = BaseAddress + 0;   // 总电压 (2个寄存器，浮点数)
            public const ushort TotalCurrent = BaseAddress + 2;   // 总电流 (2个寄存器，浮点数)
            public const ushort Soc = BaseAddress + 4;            // SOC (2个寄存器，浮点数)
            public const ushort Soh = BaseAddress + 6;            // SOH (2个寄存器，浮点数)

            // 单体电压信息
            public const ushort MaxCellVoltage = BaseAddress + 8; // 最高单体电压 (2个寄存器，浮点数)
            public const ushort MinCellVoltage = BaseAddress + 10;// 最低单体电压 (2个寄存器，浮点数)
            public const ushort AvgCellVoltage = BaseAddress + 12;// 平均单体电压 (2个寄存器，浮点数)

            // 温度信息
            public const ushort MaxTemperature = BaseAddress + 14;// 最高温度 (2个寄存器，浮点数)
            public const ushort MinTemperature = BaseAddress + 16;// 最低温度 (2个寄存器，浮点数)
            public const ushort AvgTemperature = BaseAddress + 18;// 平均温度 (2个寄存器，浮点数)

            // 其他参数
            public const ushort InsulationResistance = BaseAddress + 20; // 绝缘电阻 (2个寄存器，浮点数)
            public const ushort AvailableEnergy = BaseAddress + 22;      // 可用能量 (2个寄存器，浮点数)
            public const ushort Status = BaseAddress + 24;               // 运行状态 (1个寄存器，枚举)
            public const ushort AlarmLevel = BaseAddress + 25;           // 告警等级 (1个寄存器，枚举)
            public const ushort OnlineClusterCount = BaseAddress + 26;   // 在线簇数量 (1个寄存器)
        }

        /// <summary>
        /// 空调数据寄存器地址映射
        /// </summary>
        public static class AcRegisters
        {
            // 起始地址：40200
            public const ushort BaseAddress = 40200;

            // 温度信息
            public const ushort IndoorTemperature = BaseAddress + 0;  // 室内温度 (2个寄存器，浮点数)
            public const ushort OutdoorTemperature = BaseAddress + 2; // 室外温度 (2个寄存器，浮点数)
            public const ushort SetTemperature = BaseAddress + 4;     // 设定温度 (2个寄存器，浮点数)

            // 湿度信息
            public const ushort IndoorHumidity = BaseAddress + 6;     // 室内湿度 (2个寄存器，浮点数)
            public const ushort OutdoorHumidity = BaseAddress + 8;    // 室外湿度 (2个寄存器，浮点数)

            // 压缩机和风机
            public const ushort CompressorFrequency = BaseAddress + 10; // 压缩机频率 (2个寄存器，浮点数)
            public const ushort IndoorFanSpeed = BaseAddress + 12;      // 室内风机转速 (1个寄存器，整数)
            public const ushort OutdoorFanSpeed = BaseAddress + 13;     // 室外风机转速 (1个寄存器，整数)

            // 制冷剂参数
            public const ushort EvaporatorTemperature = BaseAddress + 14; // 蒸发器温度 (2个寄存器，浮点数)
            public const ushort CondenserTemperature = BaseAddress + 16;  // 冷凝器温度 (2个寄存器，浮点数)
            public const ushort SuctionPressure = BaseAddress + 18;       // 吸气压力 (2个寄存器，浮点数)
            public const ushort DischargePressure = BaseAddress + 20;     // 排气压力 (2个寄存器，浮点数)

            // 功率信息
            public const ushort Power = BaseAddress + 22;                 // 实时功率 (2个寄存器，浮点数)
            public const ushort CoolingCapacity = BaseAddress + 24;       // 制冷量 (2个寄存器，浮点数)

            // 状态和模式
            public const ushort Mode = BaseAddress + 26;                  // 运行模式 (1个寄存器，枚举)
            public const ushort Status = BaseAddress + 27;                // 运行状态 (1个寄存器，枚举)
            public const ushort FanSpeedLevel = BaseAddress + 28;         // 风速档位 (1个寄存器，枚举)
        }
    }
}
