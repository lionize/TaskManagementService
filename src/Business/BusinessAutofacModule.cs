using Autofac;
using Lionize.IntegrationMessages;
using MathNet.Numerics.Random;
using System;
using TIKSN.Lionize.Messaging.Handlers;
using TIKSN.Lionize.TaskManagementService.Business.MessageHandlers;
using TIKSN.Serialization.Numerics;

namespace TIKSN.Lionize.TaskManagementService.Business
{
    public class BusinessAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CryptoRandomSource>()
                .As<Random>()
                .SingleInstance();

            builder.RegisterType<UnsignedBigIntegerBinaryDeserializer>()
                .SingleInstance();

            builder
                .RegisterType<TaskUpsertedConsumerMessageHandler>()
                .As<IConsumerMessageHandler<TaskUpserted>>()
                .SingleInstance();
        }
    }
}