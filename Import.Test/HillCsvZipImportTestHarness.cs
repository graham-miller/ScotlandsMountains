using NUnit.Framework;

namespace ScotlandsMountains.Import.Test
{
    public class HillCsvZipImportTestHarness
    {
        [Test]
        public void TestHarness()
        {
            new HillCsvZipImporter().Import();
        }
    }
}