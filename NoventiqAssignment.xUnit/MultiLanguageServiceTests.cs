using Microsoft.AspNetCore.Http;
using Moq;
using NoventiqAssignment.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoventiqAssignment.xUnit
{
    public class MultiLanguageServiceTests
    {
        private readonly Mock<IHttpContextAccessor> httpContextAccessorMock;
        private readonly MultiLanguageService multiLanguageService;

        public MultiLanguageServiceTests()
        {
            httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            multiLanguageService = new MultiLanguageService(httpContextAccessorMock.Object);
        }
        private void SetupHttpContext(IHeaderDictionary headers=null)
        {
            var httpContextMock = new Mock<HttpContext>();
            var requestMock = new Mock<HttpRequest>();

            requestMock.Setup(r => r.Headers).Returns(headers ?? new HeaderDictionary());
            httpContextMock.Setup(c => c.Request).Returns(requestMock.Object);
            httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);
        }

        [Fact]
        public void GetCurrentLanguage_WhenHttpContextIsNull_ReturnsDefaultLanguage()
        {
            // Arrange
            httpContextAccessorMock.Setup(a => a.HttpContext).Returns((HttpContext?)null);

            // Act
            var result = multiLanguageService.GetCurrentLanguage();

            // Assert
            Assert.Equal("en", result);
        }

        [Fact]
        public void GetCurrentLanguage_WhenNoAcceptLanguageHeader_ReturnsDefaultLanguage()
        {
            SetupHttpContext();
            Assert.Equal("en", multiLanguageService.GetCurrentLanguage());
        }

        [Fact]
        public void GetCurrentLanguage_WithSingleLanguage_ReturnsPrimaryLanguage()
        {
            var headers = new HeaderDictionary { { "Accept-Language", "fr-FR" } };
            SetupHttpContext(headers);
            Assert.Equal("fr", multiLanguageService.GetCurrentLanguage());
        }
    }
}
