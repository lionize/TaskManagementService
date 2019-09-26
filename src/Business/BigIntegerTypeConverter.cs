using AutoMapper;
using Lionize.IntegrationMessages;
using System;
using System.Globalization;
using TIKSN.Serialization;

namespace TIKSN.Lionize.TaskManagementService.Business
{
    public class BigIntegerTypeConverter : ITypeConverter<BigInteger, string>, ITypeConverter<string, BigInteger>
    {
        private readonly ICustomSerializer<byte[], System.Numerics.BigInteger> _serializer;
        private readonly ICustomDeserializer<byte[], System.Numerics.BigInteger> _deserializer;

        public BigIntegerTypeConverter(ICustomSerializer<byte[], System.Numerics.BigInteger> serializer, ICustomDeserializer<byte[], System.Numerics.BigInteger> deserializer)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        }

        public string Convert(BigInteger source, string destination, ResolutionContext context)
        {
            return _deserializer.Deserialize(source.Value.ToArray()).ToString();
        }

        public BigInteger Convert(string source, BigInteger destination, ResolutionContext context)
        {
            return new BigInteger { Value = _serializer.Serialize(System.Numerics.BigInteger.Parse(source, CultureInfo.InvariantCulture)) };
        }
    }
}
