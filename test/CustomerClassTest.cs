using Connect.Core.Entities;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Connect.Application.DTOs;
using Connect.Application.Services;
using Connect.Application.Settings;
using Microsoft.AspNetCore.Http;
using Assert = Xunit.Assert;

namespace Tests.CustomerService
{
    public class RegisterNullUserDtoTests
    {
        private readonly Mock<UserManager<Customer>> _userManagerMock;
        private readonly Mock<IUserHelpers>  _userHelpersMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IMailingService> _mailingServiceMock;
        private readonly Mock<HttpContextAccessor>  _contextAccessorMock;

        private readonly Connect.Application.Services.CustomerService _customerService;

        public RegisterNullUserDtoTests()
        {
            _userManagerMock = new Mock<UserManager<Customer>>(
                new Mock<IUserStore<Customer>>().Object, 
                null, null, null, null, null, null, null, null
            );
            _userHelpersMock = new Mock<IUserHelpers>();
            _mapperMock = new Mock<IMapper>();
            _mailingServiceMock = new Mock<IMailingService>();
            
            _customerService = new Connect.Application.Services.CustomerService(
                null,
                _userManagerMock.Object,
                null, null, null,_userHelpersMock.Object,_mailingServiceMock.Object, null, null
            );
        }
        [Fact]
        public async Task Register_ShouldThrowArgumentNullException_WhenUserDtoIsNull()
        {
            // Act & Assert (Verify exception is thrown with the correct parameter name)
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _customerService.Register(null));
            Assert.Equal("userDto", exception.ParamName);
        }

        [Fact]
        public async Task Register_ShouldReturnFailedResult_WhenUserAlreadyExists()
        {
            var userDto = new RegisterUserDto { Email = "existinguser@example.com", Password = "Password123" };
            
            // Setup the mock to simulate that a user already exists
            _userManagerMock.Setup(x => x.FindByEmailAsync(userDto.Email))
                            .ReturnsAsync(new Customer());

            // Act
            var result = await _customerService.CreateUserAsync(userDto);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description == "User with this email already exists.");
        }
        
        
        
        
        
        
        
        
        
        [Fact]
        public async Task ConfirmEmail_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            string email = "nonexistentuser@example.com";
            string token = "dummyToken";

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                            .ReturnsAsync((Customer)null); // Simulate user not found

            // Act
            var result = await _customerService.ConfirmEmail(email, token);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ConfirmEmail_ShouldReturnTrue_WhenEmailConfirmationSucceeds()
        {
            // Arrange
            string email = "existinguser@example.com";
            string token = "validToken";
            var user = new Customer { Email = email };

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                            .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.ConfirmEmailAsync(user, token))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _customerService.ConfirmEmail(email, token);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ConfirmEmail_ShouldReturnFalse_WhenEmailConfirmationFails()
        {
            // Arrange
            string email = "existinguser@example.com";
            string token = "invalidToken";
            var user = new Customer { Email = email };

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                            .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.ConfirmEmailAsync(user, token))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid token." }));

            // Act
            var result = await _customerService.ConfirmEmail(email, token);

            // Assert
            Assert.False(result);
        }
        
        
        
        
        
        
        
     
        
        
        
        
        
        
        
        
        
        
        
        [Fact]
        public async Task ForgotPasswordAsync_ShouldReturnUserNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            string email = "nonexistentuser@example.com";
            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _customerService.ForgotPasswordAsync(email);

            // Assert
            Assert.Equal("User Not Found", result);
        }
        
        
        
        [Fact]
        public async Task ForgotPasswordAsync_ShouldReturnOTPSentSuccessfully_WhenMailIsSent()
        {
            // Arrange
            string email = "existinguser@example.com";
            var user = new Customer { Email = email };

            // Mock user retrieval
            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

            // Mock mailing service
            _mailingServiceMock.Setup(x => x.SendMail(It.IsAny<MailMessage>()));

            // Act
            var result = await _customerService.ForgotPasswordAsync(email);

            // Assert
            Assert.Equal("OTP Sent Successfully", result);
            _mailingServiceMock.Verify(x => x.SendMail(It.IsAny<MailMessage>()), Times.Once);
        }

        
        
        
        [Fact]
        public async Task ForgotPasswordAsync_ShouldReturnErrorMessage_WhenMailSendingFails()
        {
            // Arrange
            string email = "existinguser@example.com";
            var user = new Customer { Email = email };

            // Mock finding the user by email
            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(user);

 
            // Mock the mailing service to throw an exception when attempting to send the mail
            _mailingServiceMock.Setup(x => x.SendMail(It.IsAny<MailMessage>()))
                .Throws(new Exception("SMTP Error"));

            // Act
            var result = await _customerService.ForgotPasswordAsync(email);

            // Assert
            Assert.StartsWith("An Error Occurred,", result);
            Assert.Contains("SMTP Error", result);
        }

        
        [Fact]
        public void GenerateOTP_ShouldGenerateAndStoreOTP_WhenCalled()
        {
            // Arrange
            string email = "test@example.com";

            // Act
            var otp = _customerService.GenerateOTP(email);

            // Use reflection to access the private _otpCache
            var otpCacheField = typeof(Connect.Application.Services.CustomerService)
                .GetField("_otpCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            var otpCache = (Dictionary<string, (string OTP, DateTime Expiry)>)otpCacheField.GetValue(null);

            // Assert
            Assert.NotNull(otp);
            Assert.Equal(6, otp.Length);
            Assert.True(int.TryParse(otp, out _));
            Assert.True(otpCache.ContainsKey(email));
            Assert.Equal(otp, otpCache[email].OTP);

            var expirationTime = otpCache[email].Expiry;
            Assert.InRange(expirationTime, DateTime.UtcNow.AddMinutes(4.5), DateTime.UtcNow.AddMinutes(5.5));
        }




    }
}
