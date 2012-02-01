using FluentAssertions;
using NUnit.Framework;

namespace DeskMetrics.Test
{
    public class HardwareFixture
    {

        EnviromentInformation _hardware = new EnviromentInformation();

        [Test]
        public void ProcessorName()
        {
            _hardware.ProcessorName.Should().Contain("Intel");
        }

        [Test]
        public void ProcessorBrand()
        {
            _hardware.ProcessorBrand.Should().Contain("Intel");
        }

        [Test]
        public void ProcessorCores()
        {
            _hardware.ProcessorCores.Should().BeInRange(1, 8);
        }

        [Test]
        public void ProcessorFrequency()
        {
            _hardware.ProcessorFrequency.Should().BeInRange(900, 4000);
        }

        [Test]
        public void ProcessorArchicteture()
        {
            _hardware.ProcessorArchicteture.Should().Be(64);
        }

        [Test]
        public void MemoryFree()
        {
            _hardware.MemoryFree.Should().BeInRange(100, 160000000000);
        }

        [Test]
        public void MemoryTotal()
        {
            _hardware.MemoryTotal.Should().BeInRange(1000000000, 12000000000); //1gb-12gb
        }

        [Test]
        public void OsVersion()
        {
            _hardware.OsVersion.Should().NotContain("Microsoft");
            _hardware.OsVersion.Should().Contain("Windows");
        }

        [Test]
        public void OsServicePack()
        {
            _hardware.OsServicePack.Should().NotBeNull();
        }

        [Test]
        public void OsArchitecture()
        {
            _hardware.OsArchitecture.Should().Be(64);
        }

        [Test]
        public void FrameworkVersion()
        {
            _hardware.FrameworkVersion.Should().Contain("4.");
        }
    }
}
