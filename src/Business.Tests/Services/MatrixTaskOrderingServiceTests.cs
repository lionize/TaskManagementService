using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TIKSN.Lionize.TaskManagementService.Data.Entities;
using TIKSN.Lionize.TaskManagementService.Data.Repositories;
using Xunit;

namespace TIKSN.Lionize.TaskManagementService.Business.Services.Tests
{
    public class MatrixTaskOrderingServiceTests
    {
        [Theory, AutoData]
        public async Task MoveToMatrix_ForDebug(
            MatrixTaskEntity[] quadrantTasks,
            string taskId,
            bool important,
            bool urgent,
            int order,
            Guid userId)
        {
            var matrixTaskRepository = Substitute.For<IMatrixTaskRepository>();

            matrixTaskRepository
                .GetMatrixQuadrantAsync(Arg.Any<Guid>(), Arg.Any<bool>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
                .Returns(quadrantTasks);

            matrixTaskRepository
                .GetAsync(taskId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new MatrixTaskEntity { ID = taskId, Order = order }));

            var sut = new MatrixTaskOrderingService(matrixTaskRepository);
            await sut.MoveToMatrix(taskId, important, urgent, order, userId, default).ConfigureAwait(false);
        }

        [Fact]
        public async Task MoveToMatrix_InsertInBetween()
        {
            Fixture fixture = new Fixture();
            var matrixTaskRepository = Substitute.For<IMatrixTaskRepository>();

            MatrixTaskEntity[] quadrantTasks = new[]
            {
                new MatrixTaskEntity { ID = "1827481110", Order = 1 },
                new MatrixTaskEntity { ID = "1047090038", Order = 2 },
                new MatrixTaskEntity { ID = "789557273", Order = 4 },
                new MatrixTaskEntity { ID = "1597491702", Order = 6 },
                new MatrixTaskEntity { ID = "1190555372", Order = 8 }
            };

            matrixTaskRepository
                .GetMatrixQuadrantAsync(Arg.Any<Guid>(), Arg.Any<bool>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
                .Returns(quadrantTasks);

            matrixTaskRepository
                .GetAsync("1463984414", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new MatrixTaskEntity { ID = "1463984414", Order = 4 }));

            var sut = new MatrixTaskOrderingService(matrixTaskRepository);
            await sut.MoveToMatrix(
                "1463984414",
                fixture.Create<bool>(),
                fixture.Create<bool>(),
                order: 4,
                fixture.Create<Guid>(),
                default).ConfigureAwait(false);

            var matrixTask = await matrixTaskRepository.GetAsync("1463984414", default).ConfigureAwait(false);
            matrixTask.Order.Should().Be(4);

            quadrantTasks.Single(x => x.ID == "1827481110").Order.Should().Be(1);
            quadrantTasks.Single(x => x.ID == "1047090038").Order.Should().Be(2);
            quadrantTasks.Single(x => x.ID == "789557273").Order.Should().Be(5);
            quadrantTasks.Single(x => x.ID == "1597491702").Order.Should().Be(6);
            quadrantTasks.Single(x => x.ID == "1190555372").Order.Should().Be(8);
        }
    }
}