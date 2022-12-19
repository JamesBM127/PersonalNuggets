namespace DeserializeJsonNUnitTest
{
    public class JsonToObjectTests
    {
        public IConfiguration Configuration { get; set; }
        public DummyModel DummyModel { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json", false)
                                    .Build();
        }

        [Test]
        public void ToObject_ConvertJsonToObject_Sucess()
        {
            #region Arrange
            DummyModel = Configuration.ToObject<DummyModel>("DummyModelSection");
            #endregion

            #region Act
            #endregion

            #region Assert
            Assert.IsTrue(DummyModel.GuidType != Guid.Empty);
            Assert.Greater(DummyModel.IntType, 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(DummyModel.StringType));
            Assert.IsTrue(DummyModel.BooleanType);
            Assert.Greater(DummyModel.DoubleType, 0);
            Assert.Greater(DummyModel.DecimalType, 0);
            Assert.IsTrue(DummyModel.DateTimeTypeDateOnly != new DateTime());
            Assert.IsTrue(DummyModel.DateTimeTypeFullTime != new DateTime());
            #endregion
        }
    }
}