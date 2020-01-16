using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NetworkConveyer.Objects
{
    public class ConveyPackage
    {
        public ConveyEndPoint Source { get; set; }
        public ConveyEndPoint Target { get; set; }
        
        public byte[] Raw { get; private set; }

        public ConveyPackage()
        {
            // nothing to do.
        }

        public ConveyPackage(byte[] raw)
        {
            Raw = raw;
        }

        public string ToHexmap()
        {
            var sb = new StringBuilder();
            if (Raw != null)
            {
                foreach (var bin in Raw)
                {
                    var text = bin.ToString("X2");
                    sb.Append(text + " ");
                }
            }
            return sb.ToString().TrimEnd();
        }

        public void ParseHexMap(string hexmap)
        {
            var invalidCharRegex = new Regex(@"[^0-9A-Fa-f ]");
            var pairedRegex = new Regex(@"^(?:(?:(?:[0-9A-Fa-f]{2})(?: {0,1}))*)$");
            var binList = new List<byte>();

            if (invalidCharRegex.IsMatch(hexmap))
            {
                return;
            }
            if (!pairedRegex.IsMatch(hexmap))
            {
                return;
            }
            var parts = hexmap.Split(' ');
            foreach (var bstr in parts)
            {
                if (bstr.Length != 2)
                {
                    return;
                }
                int bin;
                try
                {
                    bin = int.Parse(bstr, System.Globalization.NumberStyles.HexNumber);
                    binList.Add((byte)bin);
                }
                catch
                {
                    return;
                }
            }

            this.Raw = binList.ToArray();
        }
    }
}
