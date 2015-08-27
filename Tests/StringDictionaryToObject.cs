﻿namespace ToObject.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using ToObject.Exceptions;

    [TestClass]
    public class StringDictionaryToObject
    {
        [TestMethod]
        public void LengthyTest()
        {
            var expected =
                new SomeData
                {
                    Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                    Date = new DateTime(1999, 1, 1),
                    Text = "level 1",
                    Nested =
                        new SomeData
                        {
                            Id = new Guid("e591be31-289f-4a99-ba67-288ea24b7d7e"),
                            Date = new DateTime(1999, 2, 2),
                            Text = "level 2",
                            Nested =
                                new SomeData
                                {
                                    Id = null,
                                    Date = null,
                                    Text = null,
                                    Nested =
                                        new SomeData
                                        {
                                            Id = new Guid("3bfdd62f-8b31-4aa2-931d-46535f291b0e"),
                                            Date = new DateTime(1999, 4, 4),
                                            Text = "level 4",
                                            Nested = null,
                                        },
                                },
                        },
                };

            var dict =
                new Dictionary<string, string>()
                {
                    // 1st level
                    { "Id", expected.Id.ToString() },
                    { "Date", expected.Date.ToString() },
                    { "Text", expected.Text },

                    // 2nd level
                    { "Nested.Id", expected.Nested.Id.ToString() },
                    { "Nested.Date", expected.Nested.Date.ToString() },
                    { "Nested.Text", expected.Nested.Text },

                    // 3rd level
                    { "Nested.Nested.Id", expected.Nested.Nested.Id.ToString() },
                    { "Nested.Nested.Date", expected.Nested.Nested.Date.ToString() },
                    { "Nested.Nested.Text", expected.Nested.Nested.Text },

                    // 4th level
                    { "Nested.Nested.Nested.Id", expected.Nested.Nested.Nested.Id.ToString() },
                    { "Nested.Nested.Nested.Date", expected.Nested.Nested.Nested.Date.ToString() },
                    { "Nested.Nested.Nested.Text", expected.Nested.Nested.Nested.Text },

                    // 5th level
                    { "Nested.Nested.Nested.Nested.Id", null },
                    { "Nested.Nested.Nested.Nested.Date", null },
                    { "Nested.Nested.Nested.Nested.Text", null },
                };
            
            var actual = dict.ToObject<SomeData>();

            // 1st level
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Text, actual.Text);

            // 2nd level
            Assert.AreEqual(expected.Nested.Id, actual.Nested.Id);
            Assert.AreEqual(expected.Nested.Date, actual.Nested.Date);
            Assert.AreEqual(expected.Nested.Text, actual.Nested.Text);

            // 3rd level
            Assert.AreEqual(expected.Nested.Nested.Id, actual.Nested.Nested.Id);
            Assert.AreEqual(expected.Nested.Nested.Date, actual.Nested.Nested.Date);
            Assert.AreEqual(expected.Nested.Nested.Text, actual.Nested.Nested.Text);

            // 4th level
            Assert.AreEqual(expected.Nested.Nested.Nested.Id, actual.Nested.Nested.Nested.Id);
            Assert.AreEqual(expected.Nested.Nested.Nested.Date, actual.Nested.Nested.Nested.Date);
            Assert.AreEqual(expected.Nested.Nested.Nested.Text, actual.Nested.Nested.Nested.Text);

            // 5th level
            Assert.AreEqual(null, actual.Nested.Nested.Nested.Nested);
        }

        [TestMethod]
        [ExpectedException(typeof(CouldntParseException))]
        public void DifferentTypes()
        {
            // Arrange
            var dict =
                new Dictionary<string, string>()
                {
                    { "Id", "11" }, // Should be a Guid
                    { "Date", "1999-01-01" },
                    { "Text", "eleven" },
                };

            // Act
            dict.ToObject<SomeData>();
        }

        [TestMethod]
        [ExpectedException(typeof(PropertyNotFoundException))]
        public void MissingProperty()
        {
            // Arrange
            var dict =
                new Dictionary<string, string>()
                {
                    { "Id", "366f4bd3-6717-4b14-9c79-70515296df7e" },
                    { "Date", "1999-01-01" },
                    { "Text", "eleven" },
                    { "Integer", "11" }, // There's no Integer
                };

            // Act
            dict.ToObject<SomeData>();
        }

        [TestMethod]
        public void MissingPropertySafe()
        {
            // Arrange
            var dict =
                new Dictionary<string, string>()
                {
                    { "Id", "366f4bd3-6717-4b14-9c79-70515296df7e" },
                    { "Date", "1999-01-01" },
                    { "Text", "eleven" },
                    { "Integer", "11" }, // There's no Integer
                };

            // Act
            dict.ToObjectSafe<SomeData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null()
        {
            // Arrange
            IDictionary<string, string> dict = null;

            // Act
            dict.ToObject<SomeData>();
        }
    }
}
