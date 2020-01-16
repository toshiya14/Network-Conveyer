using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NetworkConveyer.Objects
{
    public class ConveyEndPoint
    {
        public EndPointType Type { get; private set; }
        public string Raw { get; private set; }
        private IPEndPoint ParsedIEP;
        private string httplink;

        public ConveyEndPoint(string raw, EndPointType type = EndPointType.HttpAddress)
        {
            this.Raw = raw;

            if(type == EndPointType.HttpAddress)
            {
                if (raw.StartsWith("http://") || raw.StartsWith("https://"))
                {
                    httplink = raw;
                }
                else if (raw.StartsWith("/"))
                {
                    httplink = "http://localhost" + raw;
                }
                else
                {
                    httplink = "http://" + raw;
                }
            }

            if(type== EndPointType.IP)
            {
                ParsedIEP = ParseIEP(raw);
            }
        }

        public ConveyEndPoint(IPEndPoint iep)
        {
            this.Type = EndPointType.IP;
            this.ParsedIEP = iep;
            this.Raw = iep.ToString();
        }

        public string ToHttpAddress()
        {
            if (Type != EndPointType.HttpAddress)
            {
                return null;
            }
            return httplink;
        }

        public IPEndPoint ToIPEndPoint()
        {
            if(Type != EndPointType.IP)
            {
                return null;
            }
            return ParsedIEP;
        }
        private IPEndPoint ParseIEP(string input)
        {
            if (input.Contains(":"))
            {
                var parts = input.Split(':');
                if (parts.Length != 2)
                {
                    return null;
                }
                var ipadr = IPAddress.Parse(parts[0]);
                var port = int.Parse(parts[1]);
                if (port < 1 || port > 65535)
                {
                    return null;
                }
                return new IPEndPoint(ipadr, port);
            }
            else
            {
                return null;
            }
        }
    }
}
