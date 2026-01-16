using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using HardwareSimulator.Models;
using NModbus;

namespace HardwareSimulator.Services
{
    /// <summary>
    /// Modbus TCP 通信服务
    /// 负责通过 Modbus TCP 协议将模拟数据发送到工控机
    /// </summary>
    public class ModbusTcpService : IDisposable
    {
        private TcpClient? _tcpClient;
        private IModbusMaster? _modbusMaster;
        private bool _isConnected;
        private readonly object _lockObject = new();

        /// <summary>
        /// 获取连接状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                lock (_lockObject)
                {
                    return _isConnected && _tcpClient?.Connected == true;
                }
            }
        }

        /// <summary>
        /// 异步连接到 Modbus TCP 服务器
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">端口号</param>
        /// <returns>连接成功返回 true，否则返回 false</returns>
        public async Task<bool> ConnectAsync(string ipAddress, int port)
        {
            try
            {
                lock (_lockObject)
                {
                    // 如果已连接，先断开
                    if (_isConnected)
                    {
                        Disconnect();
                    }

                    _tcpClient = new TcpClient();
                }

                // 异步连接
                await _tcpClient.ConnectAsync(ipAddress, port);

                lock (_lockObject)
                {
                    if (_tcpClient.Connected)
                    {
                        var factory = new ModbusFactory();
                        _modbusMaster = factory.CreateMaster(_tcpClient);
                        _isConnected = true;
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                lock (_lockObject)
                {
                    _isConnected = false;
                }
                Dispose();
                return false;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            lock (_lockObject)
            {
                _isConnected = false;
            }
            Dispose();
        }

        /// <summary>
        /// 发送 PCS 数据到工控机
        /// </summary>
        /// <param name="pcsData">PCS 数据</param>
        /// <param name="slaveId">从站 ID</param>
        /// <returns>发送成功返回 true，否则返回 false</returns>
        public async Task<bool> SendPcsDataAsync(PcsData pcsData, byte slaveId)
        {
            if (!IsConnected || _modbusMaster == null)
                return false;

            try
            {
                // 直流侧数据
                await WriteFloatAsync(slaveId, ProtocolConfig.PcsRegisters.DcVoltage, (float)pcsData.DcVoltage);
                await WriteFloatAsync(slaveId, ProtocolConfig.PcsRegisters.DcCurrent, (float)pcsData.DcCurrent);
                await WriteFloatAsync(slaveId, ProtocolConfig.PcsRegisters.DcPower, (float)pcsData.DcPower);

                // 交流侧数据
                await WriteFloatAsync(slaveId, ProtocolConfig.PcsRegisters.AcVoltage, (float)pcsData.AcVoltage);
                await WriteFloatAsync(slaveId, ProtocolConfig.PcsRegisters.AcCurrent, (float)pcsData.AcCurrent);
                await WriteFloatAsync(slaveId, ProtocolConfig.PcsRegisters.AcPower, (float)pcsData.AcPower);
                await WriteFloatAsync(slaveId, ProtocolConfig.PcsRegisters.Frequency, (float)pcsData.Frequency);

                // 其他参数
                await WriteFloatAsync(slaveId, ProtocolConfig.PcsRegisters.PowerFactor, (float)pcsData.PowerFactor);
                await WriteFloatAsync(slaveId, ProtocolConfig.PcsRegisters.Efficiency, (float)pcsData.Efficiency);
                await WriteFloatAsync(slaveId, ProtocolConfig.PcsRegisters.Temperature, (float)pcsData.Temperature);

                // 状态和模式
                await WriteSingleRegisterAsync(slaveId, ProtocolConfig.PcsRegisters.Status, (ushort)pcsData.Status);
                await WriteSingleRegisterAsync(slaveId, ProtocolConfig.PcsRegisters.WorkMode, (ushort)pcsData.WorkMode);
                await WriteSingleRegisterAsync(slaveId, ProtocolConfig.PcsRegisters.FaultCode, (ushort)pcsData.FaultCode);
                await WriteFloatAsync(slaveId, ProtocolConfig.PcsRegisters.RunningHours, (float)pcsData.RunningHours);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 发送 BMS 数据到工控机
        /// </summary>
        /// <param name="bmsData">BMS 数据</param>
        /// <param name="slaveId">从站 ID</param>
        /// <returns>发送成功返回 true，否则返回 false</returns>
        public async Task<bool> SendBmsDataAsync(BmsData bmsData, byte slaveId)
        {
            if (!IsConnected || _modbusMaster == null)
                return false;

            try
            {
                // 总体参数
                await WriteFloatAsync(slaveId, ProtocolConfig.BmsRegisters.TotalVoltage, (float)bmsData.TotalVoltage);
                await WriteFloatAsync(slaveId, ProtocolConfig.BmsRegisters.TotalCurrent, (float)bmsData.TotalCurrent);
                await WriteFloatAsync(slaveId, ProtocolConfig.BmsRegisters.Soc, (float)bmsData.Soc);
                await WriteFloatAsync(slaveId, ProtocolConfig.BmsRegisters.Soh, (float)bmsData.Soh);

                // 单体电压信息
                await WriteFloatAsync(slaveId, ProtocolConfig.BmsRegisters.MaxCellVoltage, (float)bmsData.MaxCellVoltage);
                await WriteFloatAsync(slaveId, ProtocolConfig.BmsRegisters.MinCellVoltage, (float)bmsData.MinCellVoltage);
                await WriteFloatAsync(slaveId, ProtocolConfig.BmsRegisters.AvgCellVoltage, (float)bmsData.AvgCellVoltage);

                // 温度信息
                await WriteFloatAsync(slaveId, ProtocolConfig.BmsRegisters.MaxTemperature, (float)bmsData.MaxTemperature);
                await WriteFloatAsync(slaveId, ProtocolConfig.BmsRegisters.MinTemperature, (float)bmsData.MinTemperature);
                await WriteFloatAsync(slaveId, ProtocolConfig.BmsRegisters.AvgTemperature, (float)bmsData.AvgTemperature);

                // 其他参数
                await WriteFloatAsync(slaveId, ProtocolConfig.BmsRegisters.InsulationResistance, (float)bmsData.InsulationResistance);
                await WriteFloatAsync(slaveId, ProtocolConfig.BmsRegisters.AvailableEnergy, (float)bmsData.AvailableEnergy);
                await WriteSingleRegisterAsync(slaveId, ProtocolConfig.BmsRegisters.Status, (ushort)bmsData.Status);
                await WriteSingleRegisterAsync(slaveId, ProtocolConfig.BmsRegisters.AlarmLevel, (ushort)bmsData.AlarmLevel);
                await WriteSingleRegisterAsync(slaveId, ProtocolConfig.BmsRegisters.OnlineClusterCount, (ushort)bmsData.OnlineClusterCount);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 发送空调数据到工控机
        /// </summary>
        /// <param name="acData">空调数据</param>
        /// <param name="slaveId">从站 ID</param>
        /// <returns>发送成功返回 true，否则返回 false</returns>
        public async Task<bool> SendAcDataAsync(AirConditionerData acData, byte slaveId)
        {
            if (!IsConnected || _modbusMaster == null)
                return false;

            try
            {
                // 温度信息
                await WriteFloatAsync(slaveId, ProtocolConfig.AcRegisters.IndoorTemperature, (float)acData.IndoorTemperature);
                await WriteFloatAsync(slaveId, ProtocolConfig.AcRegisters.OutdoorTemperature, (float)acData.OutdoorTemperature);
                await WriteFloatAsync(slaveId, ProtocolConfig.AcRegisters.SetTemperature, (float)acData.SetTemperature);

                // 湿度信息
                await WriteFloatAsync(slaveId, ProtocolConfig.AcRegisters.IndoorHumidity, (float)acData.IndoorHumidity);
                await WriteFloatAsync(slaveId, ProtocolConfig.AcRegisters.OutdoorHumidity, (float)acData.OutdoorHumidity);

                // 压缩机和风机
                await WriteFloatAsync(slaveId, ProtocolConfig.AcRegisters.CompressorFrequency, (float)acData.CompressorFrequency);
                await WriteSingleRegisterAsync(slaveId, ProtocolConfig.AcRegisters.IndoorFanSpeed, (ushort)acData.IndoorFanSpeed);
                await WriteSingleRegisterAsync(slaveId, ProtocolConfig.AcRegisters.OutdoorFanSpeed, (ushort)acData.OutdoorFanSpeed);

                // 制冷剂参数
                await WriteFloatAsync(slaveId, ProtocolConfig.AcRegisters.EvaporatorTemperature, (float)acData.EvaporatorTemperature);
                await WriteFloatAsync(slaveId, ProtocolConfig.AcRegisters.CondenserTemperature, (float)acData.CondenserTemperature);
                await WriteFloatAsync(slaveId, ProtocolConfig.AcRegisters.SuctionPressure, (float)acData.SuctionPressure);
                await WriteFloatAsync(slaveId, ProtocolConfig.AcRegisters.DischargePressure, (float)acData.DischargePressure);

                // 功率信息
                await WriteFloatAsync(slaveId, ProtocolConfig.AcRegisters.Power, (float)acData.Power);
                await WriteFloatAsync(slaveId, ProtocolConfig.AcRegisters.CoolingCapacity, (float)acData.CoolingCapacity);

                // 状态和模式
                await WriteSingleRegisterAsync(slaveId, ProtocolConfig.AcRegisters.Mode, (ushort)acData.Mode);
                await WriteSingleRegisterAsync(slaveId, ProtocolConfig.AcRegisters.Status, (ushort)acData.Status);
                await WriteSingleRegisterAsync(slaveId, ProtocolConfig.AcRegisters.FanSpeedLevel, (ushort)acData.FanSpeedLevel);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 写入浮点数（占用2个寄存器）
        /// </summary>
        private async Task WriteFloatAsync(byte slaveId, ushort startAddress, float value)
        {
            if (_modbusMaster == null)
                return;

            var registers = ConvertFloatToRegisters(value);
            await _modbusMaster.WriteMultipleRegistersAsync(slaveId, startAddress, registers);
        }

        /// <summary>
        /// 写入单个寄存器
        /// </summary>
        private async Task WriteSingleRegisterAsync(byte slaveId, ushort address, ushort value)
        {
            if (_modbusMaster == null)
                return;

            await _modbusMaster.WriteSingleRegisterAsync(slaveId, address, value);
        }

        /// <summary>
        /// 将浮点数转换为两个16位寄存器
        /// </summary>
        private ushort[] ConvertFloatToRegisters(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return new ushort[]
            {
                BitConverter.ToUInt16(bytes, 0),
                BitConverter.ToUInt16(bytes, 2)
            };
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                _modbusMaster?.Dispose();
                _tcpClient?.Close();
                _tcpClient?.Dispose();
            }
            catch
            {
                // 忽略释放时的异常
            }
            finally
            {
                _modbusMaster = null;
                _tcpClient = null;
            }
        }
    }
}
