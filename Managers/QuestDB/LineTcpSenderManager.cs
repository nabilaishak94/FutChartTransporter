using System;
using System.Collections.Generic;
using System.Text;

using System.Globalization;
using System.Net.Sockets;
using FutChartTransporter_DotCore.Common;

namespace FutChartTransporter_DotCore.Managers.QuestDB
{
    public class LineTcpSenderManager : IDisposable
    {
        private static readonly long EpochTicks = new DateTime(1970, 1, 1).Ticks;
        private static Socket _clientSocket;
        private static byte[] _sendBuffer;
        private int _position;
        private bool _hasMetric;
        private bool _quoted;
        private bool _noFields = true;
        private string cachedAddress = "";
        private int cachedPort = 0;
        private int cachedBufferSize = 0;

        public LineTcpSenderManager()
        {

        }
        
        public LineTcpSenderManager(String address, int port, int bufferSize = 4096)
        {
            cachedAddress = address;
            cachedPort = port;
            cachedBufferSize = bufferSize;

            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clientSocket.NoDelay = true;
            _clientSocket.Blocking = true;
            _clientSocket.Connect(address, port);            
            _sendBuffer = new byte[bufferSize];

            while(_clientSocket.Connected)
            {
                return;
            }
        }

        public void ValidateSocketConnected()
        {
            if (_clientSocket.Connected)
                return;
                        
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clientSocket.NoDelay = true;
            _clientSocket.Blocking = true;
            _clientSocket.Connect(cachedAddress, cachedPort);
            _sendBuffer = new byte[cachedBufferSize];

            while (_clientSocket.Connected)
            {
                return;
            }
        }

        public void StopSocket()
        {
            _clientSocket.Disconnect(false);
            _clientSocket.Dispose();
        }

        public LineTcpSenderManager Table(ReadOnlySpan<char> name)
        {
            if (_hasMetric)
            {
                throw new InvalidOperationException("duplicate metric");
            }

            _quoted = false;
            _hasMetric = true;
            EncodeUtf8(name);
            return this;
        }

        public LineTcpSenderManager Symbol(ReadOnlySpan<char> tag, ReadOnlySpan<char> value)
        {
            if (_hasMetric && _noFields)
            {
                Put(',').EncodeUtf8(tag).Put('=').EncodeUtf8(value);
                return this;
            }
            throw new InvalidOperationException("metric expected");
        }

        private LineTcpSenderManager Column(ReadOnlySpan<char> name)
        {
            if (_hasMetric)
            {
                if (_noFields)
                {
                    Put(' ');
                    _noFields = false;
                }
                else
                {
                    Put(',');
                }

                return EncodeUtf8(name).Put('=');
            }
            throw new InvalidOperationException("metric expected");
        }

        public LineTcpSenderManager Column(ReadOnlySpan<char> name, ReadOnlySpan<char> value)
        {
            Column(name).Put('\"');
            _quoted = true;
            EncodeUtf8(value);
            _quoted = false;
            Put('\"');
            return this;
        }

        public LineTcpSenderManager Column(ReadOnlySpan<char> name, long value)
        {
            Column(name).Put(value).Put('i');
            return this;
        }

        public LineTcpSenderManager Column(ReadOnlySpan<char> name, double value)
        {
            Column(name).Put(value.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        private LineTcpSenderManager Put(long value)
        {
            if (value == long.MinValue)
            {
                // Special case, long.MinValue cannot be handled by QuestDB
                throw new ArgumentOutOfRangeException();
            }

            Span<byte> num = stackalloc byte[20];
            int pos = num.Length;
            long remaining = Math.Abs(value);
            do
            {
                long digit = remaining % 10;
                num[--pos] = (byte)('0' + digit);
                remaining /= 10;
            } while (remaining != 0);

            if (value < 0)
            {
                num[--pos] = (byte)'-';
            }

            int len = num.Length - pos;
            if (_position + len >= _sendBuffer.Length)
            {
                Flush();
            }
            num.Slice(pos, len).CopyTo(_sendBuffer.AsSpan(_position));
            _position += len;

            return this;
        }

        private LineTcpSenderManager EncodeUtf8(ReadOnlySpan<char> name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                var c = name[i];
                if (c < 128)
                {
                    PutSpecial(c);
                }
                else
                {
                    PutUtf8(c);
                }
            }

            return this;
        }

        private void PutUtf8(char c)
        {
            if (_position + 4 >= _sendBuffer.Length)
            {
                Flush();
            }

            Span<byte> bytes = _sendBuffer.AsSpan(_position);
            Span<char> chars = stackalloc char[1] { c };
            _position += Encoding.UTF8.GetBytes(chars, bytes);
        }

        private void PutSpecial(char c)
        {
            switch (c)
            {
                case ' ':
                case ',':
                case '=':
                    if (!_quoted)
                    {
                        Put('\\');
                    }
                    goto default;
                default:
                    Put(c);
                    break;
                case '\n':
                case '\r':
                    Put('\\').Put(c);
                    break;
                case '"':
                    if (_quoted)
                    {
                        Put('\\');
                    }

                    Put(c);
                    break;
                case '\\':
                    Put('\\').Put('\\');
                    break;
            }
        }

        private LineTcpSenderManager Put(ReadOnlySpan<char> chars)
        {
            foreach (var c in chars)
            {
                Put(c);
            }

            return this;
        }

        private LineTcpSenderManager Put(char c)
        {
            if (_position + 1 >= _sendBuffer.Length)
            {
                Flush();
            }

            _sendBuffer[_position++] = (byte)c;
            return this;
        }

        public void Flush()
        {
            int sent = _clientSocket.Send(_sendBuffer, 0, _position, SocketFlags.None);
            _position -= sent;
        }

        public void Dispose()
        {
            try
            {
                if (_position > 0)
                {
                    Flush();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error on disposing LineTcpClient: {0}", ex);
            }
            finally
            {
                _clientSocket.Dispose();
            }
        }

        public void AtNow()
        {
            Put('\n');
            _hasMetric = false;
            _noFields = true;
        }

        public void At(DateTime timestamp)
        {
            long epoch = timestamp.Ticks - EpochTicks;
            Put(' ').Put(epoch).Put('0').Put('0').AtNow();
        }

        public void At(long epochNano)
        {
            Put(' ').Put(epochNano).AtNow();
        }
    }
}
