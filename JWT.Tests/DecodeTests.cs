using System;
using System.Collections.Generic;
using FluentAssertions;
using JWT.exceptions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JWT.Tests
{
    [TestFixture]
    public class DecodeTests
    {
        private const string Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.cr0xw8c_HKzhFBMQrseSPGoJ0NPlRp_3BKzP96jwBdY";
        readonly Customer _customer = new Customer() { FirstName = "Bob", Age = 37 };

        private readonly Dictionary<string, object> _dictionaryPayload = new Dictionary<string, object>() { 
            { "FirstName", "Bob" },
            { "Age", 37 }
        };

        [Test]
        public void Should_Decode_Token_To_Json_Encoded_String()
        {
            var expectedPayload = JsonConvert.SerializeObject(_customer);

            string decodedPayload = JsonWebToken.Decode(Token, "ABC", false);

            Assert.AreEqual(expectedPayload, decodedPayload);
        }

        [Test]
        public void Should_Decode_Token_To_Dictionary()
        {
            object decodedPayload = JsonWebToken.DecodeToObject(Token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(_dictionaryPayload, options=>options.IncludingAllRuntimeProperties());
        }

        [Test]
        public void Should_Decode_Token_To_Dictionary_With_ServiceStack()
        {
            JsonWebToken.JsonSerializer = new ServiceStackJsonSerializer();

            object decodedPayload = JsonWebToken.DecodeToObject(Token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(_dictionaryPayload, options => options.IncludingAllRuntimeProperties());
        }

        [Test]
        public void Should_Decode_Token_To_Dictionary_With_Newtonsoft() {
            JsonWebToken.JsonSerializer = new NewtonJsonSerializer();

            object decodedPayload = JsonWebToken.DecodeToObject(Token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(_dictionaryPayload, options => options.IncludingAllRuntimeProperties());
        }

        [Test]
        public void Should_Decode_Token_To_Generic_Type()
        {
            Customer decodedPayload = JsonWebToken.DecodeToObject<Customer>(Token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(_customer);
        }

        [Test]
        public void Should_Decode_Token_To_Generic_Type_With_ServiceStack() {
            JsonWebToken.JsonSerializer = new ServiceStackJsonSerializer();

            Customer decodedPayload = JsonWebToken.DecodeToObject<Customer>(Token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(_customer);
        }

        [Test]
        public void Should_Decode_Token_To_Generic_Type_With_Newtonsoft() {
            JsonWebToken.JsonSerializer = new NewtonJsonSerializer();

            Customer decodedPayload = JsonWebToken.DecodeToObject<Customer>(Token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(_customer);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_Throw_On_Malformed_Token()
        {
            const string malformedtoken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.cr0xw8c_HKzhFBMQrseSPGoJ0NPlRp_3BKzP96jwBdY";

            JsonWebToken.DecodeToObject<Customer>(malformedtoken, "ABC", false);
        }

        [Test]
        [ExpectedException(typeof(SignatureVerificationException))]
        public void Should_Throw_On_Invalid_Key()
        {
            const string invalidkey = "XYZ";

            JsonWebToken.DecodeToObject<Customer>(Token, invalidkey, true);
        }

        [Test]
        [ExpectedException(typeof(SignatureVerificationException))]
        public void Should_Throw_On_Invalid_Expiration_Claim()
        {
            var invalidexptoken = JsonWebToken.Encode(new { exp = "asdsad" }, "ABC", JwtHashAlgorithm.HS256);

            JsonWebToken.DecodeToObject<Customer>(invalidexptoken, "ABC", true);
        }

        [Test]
        [ExpectedException(typeof(SignatureVerificationException))]
        public void Should_Throw_On_Expired_Token()
        {
            var anHourAgoUtc = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0));
            Int32 unixTimestamp = (Int32)(anHourAgoUtc.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var invalidexptoken = JsonWebToken.Encode(new { exp=unixTimestamp }, "ABC", JwtHashAlgorithm.HS256);

            JsonWebToken.DecodeToObject<Customer>(invalidexptoken, "ABC", true);
        }
    }

    public class Customer {
        public string FirstName {get;set;}
        public int Age {get;set;}
    }
}
