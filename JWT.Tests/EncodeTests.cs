using System.Collections.Generic;
using NUnit.Framework;

namespace JWT.Tests
{
    [TestFixture]
    public class EncodeTests
    {
        private const string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.cr0xw8c_HKzhFBMQrseSPGoJ0NPlRp_3BKzP96jwBdY";
        private const string Extraheaderstoken = "eyJmb28iOiJiYXIiLCJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.slrbXF9VSrlX7LKsV-Umb_zEzWLxQjCfUOjNTbvyr1g";
        readonly Customer _customer = new Customer() { FirstName = "Bob", Age = 37 };

        [Test]
        public void Should_Encode_Type()
        {
            string result = JsonWebToken.Encode(_customer, "ABC", JwtHashAlgorithm.HS256);

            Assert.AreEqual(Token, result);
        }

        [Test]
        public void Should_Encode_Type_With_Extra_Headers()
        {
            var extraheaders = new Dictionary<string, object>() { {"foo", "bar"} };
            
            string result = JsonWebToken.Encode(extraheaders, _customer, "ABC", JwtHashAlgorithm.HS256);

            Assert.AreEqual(Extraheaderstoken, result);
        }

        [Test]
        public void Should_Encode_Type_With_ServiceStack()
        {
            JsonWebToken.JsonSerializer = new ServiceStackJsonSerializer();
            string result = JsonWebToken.Encode(_customer, "ABC", JwtHashAlgorithm.HS256);

            Assert.AreEqual(Token, result);
        }

        [Test]
        public void Should_Encode_Type_With_ServiceStack_And_Extra_Headers() {
            JsonWebToken.JsonSerializer = new ServiceStackJsonSerializer();
            
            var extraheaders = new Dictionary<string, object>() { { "foo", "bar" } };
            string result = JsonWebToken.Encode(extraheaders, _customer, "ABC", JwtHashAlgorithm.HS256);

            Assert.AreEqual(Extraheaderstoken, result);
        }

        [Test]
        public void Should_Encode_Type_With_Newtonsoft_Serializer() {
            JsonWebToken.JsonSerializer = new NewtonJsonSerializer();
            string result = JsonWebToken.Encode(_customer, "ABC", JwtHashAlgorithm.HS256);

            Assert.AreEqual(Token, result);
        }

        [Test]
        public void Should_Encode_Type_With_Newtonsoft_Serializer_And_Extra_Headers() {
            JsonWebToken.JsonSerializer = new NewtonJsonSerializer();

            var extraheaders = new Dictionary<string, object>() { { "foo", "bar" } };
            string result = JsonWebToken.Encode(extraheaders, _customer, "ABC", JwtHashAlgorithm.HS256);

            Assert.AreEqual(Extraheaderstoken, result);
        }
    }
}
