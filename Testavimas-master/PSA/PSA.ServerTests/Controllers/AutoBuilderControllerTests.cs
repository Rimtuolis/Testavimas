using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.X509;
using PSA.Server.Controllers;
using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA.Server.Controllers.Tests
{
    [TestClass()]
    public class AutoBuilderControllerTests
    {
        public static Fixture _fixture = new();
        public static IEnumerable<object[]> GetData()
        {
            yield return new object[] {
                _fixture.Build<AutoGenerator>().Create(),
                _fixture.Build<Product>().With(x => x.Category, 2).CreateMany().ToList()
            };
            yield return new object[] {
                _fixture.Build<AutoGenerator>().Create(),
                _fixture.Build<Product>().With(x => x.Category, 3).CreateMany().ToList(),
            };
            yield return new object[] {
                _fixture.Build<AutoGenerator>().Create(),
                _fixture.Build<Product>().With(x => x.Category, 4).CreateMany().ToList(),
            };
            yield return new object[] {
                _fixture.Build<AutoGenerator>().Create(),
                _fixture.Build<Product>().With(x => x.Category, 5).CreateMany().ToList(),
            };
            yield return new object[] {
                _fixture.Build<AutoGenerator>().Create(),
                _fixture.Build<Product>().With(x => x.Category, 6).CreateMany().ToList()
            };
            yield return new object[] {
                _fixture.Build<AutoGenerator>().Create(),
                _fixture.Build<Product>().With(x => x.Category, 7).CreateMany().ToList()
            };

        }
        public static IEnumerable<object[]> Zodiac15FightingStyle()
        {
            var generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 1).With(x => x.Fighting_Style, 1).Create();
            yield return new object[] {
                generator,
                _fixture.Build<Product>().With(x=> x.Attack, 7).With(x=> x.Material, generator.Material).CreateMany(6).ToList()
            };
            yield return new object[] {
                _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 5).With( x => x.Fighting_Style,2).Create(),
                _fixture.Build<Product>().CreateMany(6).ToList()
            };
            yield return new object[] {
                _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 1).With( x => x.Fighting_Style,3).Create(),
                _fixture.Build<Product>().CreateMany(6).ToList()
            };
            generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 4).With(x => x.Fighting_Style, 1).With(x => x.Budget, 1).Create();
           yield return new object[] {
                generator,
                 _fixture.Build<Product>().With(x=> x.Price, 150).With(x=> x.Attack, 7).CreateMany(6).ToList()

            };
            generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 9).With(x => x.Fighting_Style, 2).With(x => x.Budget, 2).Create();
            yield return new object[] {
                generator,
                _fixture.Build<Product>().With(x=> x.Price, 423).With(x=> x.Attack, 7).With(x=> x.Defense,7).CreateMany(6).ToList()
            };
            generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 4).With(x => x.Fighting_Style, 3).With(x => x.Budget, 3).Create();
            yield return new object[] {
                generator,
                _fixture.Build<Product>().With(x=> x.Price, 666).With(x=> x.Attack, 7).With(x=> x.Defense,7).CreateMany(6).ToList()
            };
            generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 2).With(x => x.Material, 1).With(x => x.Budget, 1).Create();
            yield return new object[] {
                generator,
                _fixture.Build<Product>().With(x=> x.Price, 240).With(x=> x.Material, generator.Material).CreateMany(6).ToList()

            };
            yield return new object[] {
                _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 8).With( x => x.Material,2).Create(),
                _fixture.Build<Product>().CreateMany(6).ToList()
            };
            yield return new object[] {
                _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 2).With( x => x.Material,3).Create(),
                _fixture.Build<Product>().CreateMany(6).ToList()
            };
                generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 2).With(x => x.Material, 4).With(x => x.Budget, 2).Create();
            yield return new object[] {
                generator,
                 _fixture.Build<Product>().With(x=> x.Price, 455).With(x=> x.Material, generator.Material).CreateMany(6).ToList()

            };
            yield return new object[] {
                _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 8).With( x => x.Material,5).Create(),
                _fixture.Build<Product>().CreateMany(6).ToList()
            };
            generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 2).With(x => x.Material, 6).With(x => x.Budget, 3).Create();
            yield return new object[] {
                 generator,
                 _fixture.Build<Product>().With(x=> x.Price, 750).With(x=> x.Material, generator.Material).CreateMany(6).ToList()
            };
            generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 10).With(x => x.Material, 1).With(x => x.Fighting_Style, 1).Create();
            yield return new object[] {
                generator,
               _fixture.Build<Product>().With(x=> x.Attack, 5).With(x=> x.Material, generator.Material).CreateMany(6).ToList()

            };
            generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 12).With(x => x.Material, 2).With(x => x.Fighting_Style, 2).Create();
            yield return new object[] {
                generator,
                _fixture.Build<Product>().With(x=> x.Attack, 5).With(x=> x.Defense, 5).With(x=> x.Material, generator.Material).CreateMany(6).ToList()
            };
            generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 12).With(x => x.Material, 3).With(x => x.Fighting_Style, 3).Create();
            yield return new object[] {
                generator,
                _fixture.Build<Product>().With(x=> x.Defense, 8).With(x=> x.Material, generator.Material).CreateMany(6).ToList()
            };
            yield return new object[] {
                _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 12).With( x => x.Material,4).Create(),
                _fixture.Build<Product>().CreateMany(6).ToList()

            };
            yield return new object[] {
                _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 10).With( x => x.Material,5).Create(),
                _fixture.Build<Product>().CreateMany(6).ToList()
            };
            yield return new object[] {
                _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 12).With( x => x.Material,6).Create(),
                _fixture.Build<Product>().CreateMany(6).ToList()
            };
            generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 3).With(x => x.Budget, 1).With(x => x.Fighting_Style, 1).Create();
            yield return new object[] {
                generator,
                _fixture.Build<Product>().With(x=> x.Price, 200).With(x=> x.Attack, 7).CreateMany(6).ToList()

            };
            generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 11).With(x => x.Budget, 2).With(x => x.Fighting_Style, 2).Create();
            yield return new object[] {
                generator,
                _fixture.Build<Product>().With(x=> x.Price, 400).With(x=> x.Attack, 3).With(x=> x.Defense, 3).CreateMany(6).ToList()

            };
            generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 3).With(x => x.Budget, 3).With(x => x.Fighting_Style, 3).Create();
            yield return new object[] {
                generator,
                _fixture.Build<Product>().With(x=> x.Price, 900).With(x=> x.Defense, 7).CreateMany(6).ToList()

            };
            generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 6).With(x => x.Budget, 1).Create();
            yield return new object[] {
                generator,
                _fixture.Build<Product>().With(x=> x.Price, 199).With(x=> x.Material, generator.Material).CreateMany(6).ToList()

            };
            generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 7).With(x => x.Budget, 2).Create();
            yield return new object[] {
                generator,
                _fixture.Build<Product>().With(x=> x.Price, 333).With(x=> x.Material, generator.Material).CreateMany(6).ToList()
            };
            generator = _fixture.Build<AutoGenerator>().With(x => x.Zodiac, 6).With(x => x.Budget, 3).Create();
            yield return new object[] {
                generator,
                 _fixture.Build<Product>().With(x=> x.Price, 666).With(x=> x.Material, generator.Material).CreateMany(6).ToList()
            };


        }



        private readonly Mock<IDatabaseOperationsService> _databaseOperationsServiceMock;
        private readonly Mock<IAutoBuildingService> _autoBuildingServiceMock;

        public AutoBuilderControllerTests()
        {
            _databaseOperationsServiceMock = new();
            _autoBuildingServiceMock = new();

        }

        [TestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
        public void RobotIsDoomed_Without_EssentialPart(AutoGenerator userInput, List<Product> productsFromDbService)
        {
            //Arrange
            _databaseOperationsServiceMock.Setup(x => x.ReadListAsync<Product>(It.IsAny<string>())).ReturnsAsync(productsFromDbService);
            var sut = new AutoBuilderController(_autoBuildingServiceMock.Object, _databaseOperationsServiceMock.Object);

            //Act
            var output = sut.GenerationParameters(userInput);

            //Assert

        }

        [TestMethod]
        public void RobotWithAlltheParts()
        {
            //Arrange
            var input = _fixture.Create<AutoGenerator>();
            var productsFromDbService = _fixture.Build<Product>().CreateMany(6).ToList();
            productsFromDbService[0].Category = 2;
            productsFromDbService[1].Category = 3;
            productsFromDbService[2].Category = 4;
            productsFromDbService[3].Category = 5;
            productsFromDbService[4].Category = 6;
            productsFromDbService[5].Category = 7;
            _databaseOperationsServiceMock.Setup(x => x.ReadListAsync<Product>(It.IsAny<string>())).ReturnsAsync(productsFromDbService);
            var sut = new AutoBuilderController(_autoBuildingServiceMock.Object, _databaseOperationsServiceMock.Object);

            //Act
            var output = sut.GenerationParameters(input);

            //Assert

        }
        [TestMethod]
        [DynamicData(nameof(Zodiac15FightingStyle), DynamicDataSourceType.Method)]
        public void RobotWithAllthePartsZodiac(AutoGenerator input,List<Product> testData)
        {
            //Arrange
            testData[0].Category = 2;
            testData[1].Category = 3;
            testData[2].Category = 4;
            testData[3].Category = 5;
            testData[4].Category = 6;
            testData[5].Category = 7;
            testData[1].Connection = testData[0].Connection;
            testData[2].Connection = testData[0].Connection;
            testData[3].Connection = testData[0].Connection;
            testData[4].Connection = testData[0].Connection;
            testData[5].Connection = testData[0].Connection;
            _databaseOperationsServiceMock.Setup(x => x.ReadListAsync<Product>(It.IsAny<string>())).ReturnsAsync(testData);
            var sut = new AutoBuilderController(_autoBuildingServiceMock.Object, _databaseOperationsServiceMock.Object);

            //Act
            var output = sut.GenerationParameters(input);

            //Assert

        }



    }
}