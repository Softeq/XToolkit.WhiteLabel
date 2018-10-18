// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Plugin.Permissions.Abstractions;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Permissions
{
    public class PermissionsTests
    {
        private readonly Mock<IPermissions> _permissionsMock;
        private readonly Mock<IPermissionsDialogService> _dialogServiceMock;

        public PermissionsTests()
        {
            _permissionsMock = new Mock<IPermissions>();
            _dialogServiceMock = new Mock<IPermissionsDialogService>();
        }

        [Fact]
        public async void Granted_Permission_Should_Be_Returned()
        {
            //arrange
            _permissionsMock.Setup(_ => _.CheckPermissionStatusAsync(Permission.Camera)).Returns(() => Task.FromResult(PermissionStatus.Granted));
            var permissionManager = new PermissionManager(_permissionsMock.Object, _dialogServiceMock.Object);

            //act
            var result = await permissionManager.CheckPermissionAsync(Permission.Camera);

            //assert
            Assert.Equal(PermissionStatus.Granted, result);
        }

        [Fact]
        public async void Disabled_Permission_Should_Be_Returned()
        {
            //arrange
            _permissionsMock.Setup(_ => _.CheckPermissionStatusAsync(Permission.Camera)).Returns(() => Task.FromResult(PermissionStatus.Disabled));
            var permissionManager = new PermissionManager(_permissionsMock.Object, _dialogServiceMock.Object);

            //act
            var result = await permissionManager.CheckPermissionAsync(Permission.Camera);

            //assert
            Assert.Equal(PermissionStatus.Disabled, result);
        }

        [Fact]
        public async void Restrcited_Permission_Should_Be_Returned()
        {
            //arrange
            _permissionsMock.Setup(_ => _.CheckPermissionStatusAsync(Permission.Camera)).Returns(() => Task.FromResult(PermissionStatus.Restricted));
            var permissionManager = new PermissionManager(_permissionsMock.Object, _dialogServiceMock.Object);

            //act
            var result = await permissionManager.CheckPermissionAsync(Permission.Camera);
            //assert
            Assert.Equal(PermissionStatus.Restricted, result);
        }

        [Fact]
        public async void Denied_Should_Show_Dialog_And_Return_Denied()
        {
            //arrange
            _permissionsMock.Setup(_ => _.CheckPermissionStatusAsync(Permission.Camera)).Returns(() => Task.FromResult(PermissionStatus.Denied));
            _dialogServiceMock.Setup(_ => _.ShowDialogAsync(It.IsAny<Permission>(), It.IsAny<PermissionStatus>())).Returns(() => Task.FromResult(false));

            var permissionManager = new PermissionManager(_permissionsMock.Object, _dialogServiceMock.Object);

            //act
            var result = await permissionManager.CheckPermissionAsync(Permission.Camera);

            //assert
            _dialogServiceMock.Verify(mock => mock.ShowDialogAsync(It.IsAny<Permission>(), It.IsAny<PermissionStatus>()), Times.Once);
            _permissionsMock.Verify(mock => mock.OpenAppSettings(), Times.Never);

            Assert.Equal(PermissionStatus.Denied, result);
        }

        [Fact]
        public async void Denied_Should_Show_Dialog_Open_Settings_And_Return_Denied()
        {
            //arrange
            _permissionsMock.Setup(_ => _.CheckPermissionStatusAsync(Permission.Camera)).Returns(() => Task.FromResult(PermissionStatus.Denied));
            _dialogServiceMock.Setup(_ => _.ShowDialogAsync(It.IsAny<Permission>(), It.IsAny<PermissionStatus>())).Returns(() => Task.FromResult(true));

            var permissionManager = new PermissionManager(_permissionsMock.Object, _dialogServiceMock.Object);

            //act
            var result = await permissionManager.CheckPermissionAsync(Permission.Camera);

            //assert
            _dialogServiceMock.Verify(mock => mock.ShowDialogAsync(Permission.Camera, PermissionStatus.Denied), Times.Once);
            _permissionsMock.Verify(mock => mock.OpenAppSettings(), Times.Once);

            Assert.Equal(PermissionStatus.Denied, result);
        }

        [Fact]
        public async void Unknown_Should_Show_Dialog_And_Return_Unknown()
        {
            //arrange
            _permissionsMock.Setup(_ => _.CheckPermissionStatusAsync(Permission.Camera)).Returns(() => Task.FromResult(PermissionStatus.Unknown));
            _dialogServiceMock.Setup(_ => _.ShowDialogAsync(It.IsAny<Permission>(), It.IsAny<PermissionStatus>())).Returns(() => Task.FromResult(false));

            var permissionManager = new PermissionManager(_permissionsMock.Object, _dialogServiceMock.Object);

            //act
            var result = await permissionManager.CheckPermissionAsync(Permission.Camera);

            //assert
            _dialogServiceMock.Verify(mock => mock.ShowDialogAsync(It.IsAny<Permission>(), It.IsAny<PermissionStatus>()), Times.Once);
            _permissionsMock.Verify(mock => mock.RequestPermissionsAsync(Permission.Camera), Times.Never);

            Assert.Equal(PermissionStatus.Unknown, result);
        }

        [Fact]
        public async void Unknown_Should_Show_Dialog_Request_Permission_And_Return_Unknown()
        {
            //arrange
            _permissionsMock.Setup(_ => _.CheckPermissionStatusAsync(Permission.Camera)).Returns(() => Task.FromResult(PermissionStatus.Unknown));
            _permissionsMock.Setup(_ => _.RequestPermissionsAsync(Permission.Camera)).Returns(() => Task.FromResult(new Dictionary<Permission, PermissionStatus>()));
            _dialogServiceMock.Setup(_ => _.ShowDialogAsync(It.IsAny<Permission>(), It.IsAny<PermissionStatus>())).Returns(() => Task.FromResult(true));

            var permissionManager = new PermissionManager(_permissionsMock.Object, _dialogServiceMock.Object);

            //act
            var result = await permissionManager.CheckPermissionAsync(Permission.Camera);

            //assert
            _dialogServiceMock.Verify(mock => mock.ShowDialogAsync(It.IsAny<Permission>(), It.IsAny<PermissionStatus>()), Times.Once);
            _permissionsMock.Verify(mock => mock.RequestPermissionsAsync(Permission.Camera), Times.Once);

            Assert.Equal(PermissionStatus.Unknown, result);
        }

        [Fact]
        public async void Unknown_Should_Show_Dialog_Request_Permission_And_Return_Granted()
        {
            //arrange
            _permissionsMock.Setup(_ => _.CheckPermissionStatusAsync(Permission.Camera)).Returns(() => Task.FromResult(PermissionStatus.Unknown));
            _permissionsMock.Setup(_ => _.RequestPermissionsAsync(Permission.Camera)).Returns(() => Task.FromResult(new Dictionary<Permission, PermissionStatus> { { Permission.Camera, PermissionStatus.Granted } }));
            _dialogServiceMock.Setup(_ => _.ShowDialogAsync(It.IsAny<Permission>(), It.IsAny<PermissionStatus>())).Returns(() => Task.FromResult(true));

            var permissionManager = new PermissionManager(_permissionsMock.Object, _dialogServiceMock.Object);

            //act
            var result = await permissionManager.CheckPermissionAsync(Permission.Camera);

            //assert
            _dialogServiceMock.Verify(mock => mock.ShowDialogAsync(It.IsAny<Permission>(), It.IsAny<PermissionStatus>()), Times.Once);
            _permissionsMock.Verify(mock => mock.RequestPermissionsAsync(Permission.Camera), Times.Once);

            Assert.Equal(PermissionStatus.Granted, result);
        }
    }
}