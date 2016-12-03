using System;

namespace Altidude.Contracts
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class EqualsIgnoreAttribute : System.Attribute
    {
        private readonly string _orsak;
        public const string DefaultOrsakVärde = "ej angivet";

        public EqualsIgnoreAttribute(string orsak = DefaultOrsakVärde)
        {
            if (string.IsNullOrEmpty(orsak))
                throw new ArgumentException("orsak");

            _orsak = orsak;
        }

        public string Orsak
        {
            get { return _orsak; }
        }
    }
}
