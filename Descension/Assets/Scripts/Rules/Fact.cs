using System;
using Util.Enums;

namespace Rules
{
    [Serializable]
    public class Fact
    {
        public FactKey FactKey;
        public string Key => FactKey.ToString();
        public int Value;
    }

}