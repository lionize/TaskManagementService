using Autofac;
using MathNet.Numerics.Random;
using System;
using TIKSN.Serialization.Numerics;

namespace TIKSN.Lionize.TaskManagementService.Business
{
    public class BusinessAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EndpointAddressProvider>()
                .As<IEndpointAddressProvider>()
                .SingleInstance();

            builder.RegisterType<CryptoRandomSource>()
                .As<Random>()
                .SingleInstance();

            builder.RegisterType<UnsignedBigIntegerBinaryDeserializer>()
                .SingleInstance();
        }
    }
}