﻿using System;
using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;

namespace AppConfigFacility.Tests.Unit
{
    [TestFixture]
    public class AggregateSettingsProviderTests
    {
        [Test]
        public void GetSetting_CanGetSettingFromAppSettings()
        {
            // Arrange
            const string key = "StringSetting";
            var expectedValue = ConfigurationManager.AppSettings[key];

            var provider = new AggregateSettingsProvider(new[] {new AppSettingsProvider()});

            // Act
            var value = provider.GetSetting(key, typeof(string));

            // Assert
            Assert.AreEqual(expectedValue, value);
        }

        [Test]
        public void GetSetting_CanGetSettingFromEnvironmentInsteadOfAppSettings()
        {
            // Arrange
            const string key = "StringSetting";
            const string expectedValue = "EnvironmentValue";

            Environment.SetEnvironmentVariable(key, expectedValue);

            var provider = new AggregateSettingsProvider(new List<ISettingsProvider>
                {new EnvironmentSettingsProvider(), new AppSettingsProvider()});

            // Act
            var value = provider.GetSetting(key, typeof(string));

            // Assert
            Assert.AreEqual(expectedValue, value);
        }

        [Test]
        public void GetSetting_FallsBackToNextProviderIfFirstProviderHasNoValue()
        {
            // Arrange
            const string key = "StringSetting";
            var expectedValue = ConfigurationManager.AppSettings[key];

            Environment.SetEnvironmentVariable(key, null);

            var provider = new AggregateSettingsProvider(new List<ISettingsProvider>
                {new EnvironmentSettingsProvider(), new AppSettingsProvider()});

            // Act
            var value = provider.GetSetting(key, typeof(string));

            // Assert
            Assert.AreEqual(expectedValue, value);
        }

        [Test]
        public void Constructor_ThrowsExceptionIfNoProvidersAreSpecified()
        {
            Assert.Throws<ArgumentException>(() => new AggregateSettingsProvider(new List<ISettingsProvider>()));
            Assert.Throws<ArgumentException>(() => new AggregateSettingsProvider(null));
        }
    }
}
