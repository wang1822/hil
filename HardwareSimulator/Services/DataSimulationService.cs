using HardwareSimulator.Models;

namespace HardwareSimulator.Services
{
    /// <summary>
    /// 数据模拟服务 - 用于生成模拟的硬件数据
    /// </summary>
    public class DataSimulationService
    {
        private readonly Random _random = new();

        /// <summary>
        /// 生成随机的PCS数据
        /// </summary>
        public void SimulatePcsData(PcsData pcsData)
        {
            // 模拟直流侧数据
            pcsData.DcVoltage = 700 + _random.NextDouble() * 100; // 700-800V
            pcsData.DcCurrent = 50 + _random.NextDouble() * 100;  // 50-150A
            pcsData.DcPower = pcsData.DcVoltage * pcsData.DcCurrent / 1000; // kW

            // 模拟交流侧数据
            pcsData.AcVoltage = 380 + _random.NextDouble() * 20 - 10; // 370-390V
            pcsData.AcCurrent = 40 + _random.NextDouble() * 80;       // 40-120A
            pcsData.AcPower = pcsData.AcVoltage * pcsData.AcCurrent * Math.Sqrt(3) * pcsData.PowerFactor / 1000; // kW

            // 模拟其他参数
            pcsData.Frequency = 49.9 + _random.NextDouble() * 0.2;  // 49.9-50.1Hz
            pcsData.PowerFactor = 0.95 + _random.NextDouble() * 0.04; // 0.95-0.99
            pcsData.Efficiency = 95 + _random.NextDouble() * 3;     // 95-98%
            pcsData.Temperature = 35 + _random.NextDouble() * 20;   // 35-55°C
            pcsData.RunningHours += 0.001; // 累计运行时间

            // 随机设置状态（大概率运行）
            if (_random.NextDouble() > 0.95)
            {
                pcsData.Status = (PcsStatus)_random.Next(0, 5);
            }
            else
            {
                pcsData.Status = PcsStatus.Running;
            }

            // 随机设置工作模式
            if (_random.NextDouble() > 0.9)
            {
                pcsData.WorkMode = (PcsWorkMode)_random.Next(0, 5);
            }
        }

        /// <summary>
        /// 生成随机的BMS数据
        /// </summary>
        public void SimulateBmsData(BmsData bmsData)
        {
            // 模拟电压电流
            bmsData.TotalVoltage = 700 + _random.NextDouble() * 100; // 700-800V
            bmsData.TotalCurrent = -100 + _random.NextDouble() * 200; // -100 to 100A

            // 模拟SOC和SOH
            double socChange = (_random.NextDouble() - 0.5) * 0.1;
            bmsData.Soc = Math.Clamp(bmsData.Soc + socChange, 10, 95); // 10-95%
            bmsData.Soh = 95 + _random.NextDouble() * 5; // 95-100%

            // 模拟单体电压
            bmsData.AvgCellVoltage = 3.2 + _random.NextDouble() * 0.5; // 3.2-3.7V
            bmsData.MaxCellVoltage = bmsData.AvgCellVoltage + _random.NextDouble() * 0.05;
            bmsData.MinCellVoltage = bmsData.AvgCellVoltage - _random.NextDouble() * 0.05;
            bmsData.CellVoltageDiff = bmsData.MaxCellVoltage - bmsData.MinCellVoltage;

            // 模拟温度
            bmsData.AvgTemperature = 25 + _random.NextDouble() * 15; // 25-40°C
            bmsData.MaxTemperature = bmsData.AvgTemperature + _random.NextDouble() * 3;
            bmsData.MinTemperature = bmsData.AvgTemperature - _random.NextDouble() * 3;
            bmsData.TemperatureDiff = bmsData.MaxTemperature - bmsData.MinTemperature;

            // 模拟能量
            bmsData.TotalEnergy = 100; // 100kWh
            bmsData.AvailableEnergy = bmsData.TotalEnergy * bmsData.Soc / 100;

            // 模拟绝缘电阻
            bmsData.InsulationResistance = 500 + _random.NextDouble() * 500; // 500-1000MΩ

            // 电池簇
            bmsData.ClusterCount = 8;
            bmsData.OnlineClusterCount = 8;

            // 随机设置状态
            if (bmsData.TotalCurrent > 0)
                bmsData.Status = BmsStatus.Discharging;
            else if (bmsData.TotalCurrent < 0)
                bmsData.Status = BmsStatus.Charging;
            else
                bmsData.Status = BmsStatus.Idle;

            // 告警等级
            if (bmsData.MaxTemperature > 50 || bmsData.CellVoltageDiff > 0.1)
                bmsData.AlarmLevel = AlarmLevel.Warning;
            else
                bmsData.AlarmLevel = AlarmLevel.Normal;
        }

        /// <summary>
        /// 生成随机的空调数据
        /// </summary>
        public void SimulateAcData(AirConditionerData acData)
        {
            // 室内外温度
            double tempChange = (_random.NextDouble() - 0.5) * 0.5;
            acData.IndoorTemperature = Math.Clamp(acData.IndoorTemperature + tempChange, 18, 35);
            acData.OutdoorTemperature = 25 + _random.NextDouble() * 15; // 25-40°C
            
            // 如果室内温度接近设定温度，变化减小
            if (Math.Abs(acData.IndoorTemperature - acData.SetTemperature) < 1)
            {
                acData.IndoorTemperature = acData.SetTemperature + (_random.NextDouble() - 0.5) * 0.5;
            }
            else if (acData.IndoorTemperature > acData.SetTemperature && acData.Mode == AcMode.Cooling)
            {
                acData.IndoorTemperature -= _random.NextDouble() * 0.3;
            }
            else if (acData.IndoorTemperature < acData.SetTemperature && acData.Mode == AcMode.Heating)
            {
                acData.IndoorTemperature += _random.NextDouble() * 0.3;
            }

            // 湿度
            acData.IndoorHumidity = 40 + _random.NextDouble() * 30; // 40-70%
            acData.OutdoorHumidity = 50 + _random.NextDouble() * 40; // 50-90%

            // 压缩机和风机
            if (acData.Status == AcStatus.Running)
            {
                acData.CompressorFrequency = 30 + _random.NextDouble() * 90; // 30-120Hz
                acData.IndoorFanSpeed = 800 + _random.Next(0, 400); // 800-1200 RPM
                acData.OutdoorFanSpeed = 600 + _random.Next(0, 300); // 600-900 RPM
            }
            else
            {
                acData.CompressorFrequency = 0;
                acData.IndoorFanSpeed = 0;
                acData.OutdoorFanSpeed = 0;
            }

            // 蒸发器和冷凝器温度
            acData.EvaporatorTemperature = 5 + _random.NextDouble() * 10; // 5-15°C
            acData.CondenserTemperature = 40 + _random.NextDouble() * 15; // 40-55°C

            // 压力
            acData.SuctionPressure = 0.4 + _random.NextDouble() * 0.2; // 0.4-0.6 MPa
            acData.DischargePressure = 1.5 + _random.NextDouble() * 0.5; // 1.5-2.0 MPa

            // 功率和能耗
            if (acData.Status == AcStatus.Running)
            {
                acData.Power = 2 + _random.NextDouble() * 3; // 2-5 kW
                acData.TotalEnergyConsumption += acData.Power * 0.001; // 累计能耗
                acData.RunningHours += 0.001;
            }
            else
            {
                acData.Power = 0;
            }

            // 制冷量和能效比
            acData.CoolingCapacity = acData.Power * (3 + _random.NextDouble()); // COP 3-4
            if (acData.Power > 0)
                acData.Eer = acData.CoolingCapacity / acData.Power;
            else
                acData.Eer = 0;
        }

        /// <summary>
        /// 初始化PCS数据
        /// </summary>
        public void InitializePcsData(PcsData pcsData)
        {
            pcsData.DcVoltage = 750;
            pcsData.DcCurrent = 100;
            pcsData.DcPower = 75;
            pcsData.AcVoltage = 380;
            pcsData.AcCurrent = 80;
            pcsData.AcPower = 50;
            pcsData.Frequency = 50;
            pcsData.PowerFactor = 0.98;
            pcsData.Efficiency = 96.5;
            pcsData.Temperature = 45;
            pcsData.Status = PcsStatus.Running;
            pcsData.WorkMode = PcsWorkMode.Discharging;
            pcsData.FaultCode = 0;
            pcsData.RunningHours = 1000;
        }

        /// <summary>
        /// 初始化BMS数据
        /// </summary>
        public void InitializeBmsData(BmsData bmsData)
        {
            bmsData.TotalVoltage = 750;
            bmsData.TotalCurrent = 50;
            bmsData.Soc = 75;
            bmsData.Soh = 98;
            bmsData.MaxCellVoltage = 3.55;
            bmsData.MinCellVoltage = 3.45;
            bmsData.AvgCellVoltage = 3.50;
            bmsData.CellVoltageDiff = 0.10;
            bmsData.MaxTemperature = 32;
            bmsData.MinTemperature = 28;
            bmsData.AvgTemperature = 30;
            bmsData.TemperatureDiff = 4;
            bmsData.CycleCount = 500;
            bmsData.TotalEnergy = 100;
            bmsData.AvailableEnergy = 75;
            bmsData.Status = BmsStatus.Discharging;
            bmsData.AlarmLevel = AlarmLevel.Normal;
            bmsData.InsulationResistance = 800;
            bmsData.ClusterCount = 8;
            bmsData.OnlineClusterCount = 8;
        }

        /// <summary>
        /// 初始化空调数据
        /// </summary>
        public void InitializeAcData(AirConditionerData acData)
        {
            acData.IndoorTemperature = 26;
            acData.OutdoorTemperature = 35;
            acData.SetTemperature = 25;
            acData.IndoorHumidity = 55;
            acData.OutdoorHumidity = 70;
            acData.CompressorFrequency = 60;
            acData.IndoorFanSpeed = 1000;
            acData.OutdoorFanSpeed = 750;
            acData.EvaporatorTemperature = 10;
            acData.CondenserTemperature = 45;
            acData.SuctionPressure = 0.5;
            acData.DischargePressure = 1.8;
            acData.Power = 3.5;
            acData.TotalEnergyConsumption = 100;
            acData.Mode = AcMode.Cooling;
            acData.Status = AcStatus.Running;
            acData.FanSpeedLevel = FanSpeed.Auto;
            acData.RunningHours = 500;
            acData.CoolingCapacity = 12;
            acData.Eer = 3.4;
        }
    }
}
