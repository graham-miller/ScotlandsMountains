using ScotlandsMountains.Application.UseCases.DobihFiles.Parsing;

namespace ScotlandsMountains.Application.Tests.UseCases.DobihFiles.Parsing;

[TestFixture]
public class CountiesFactoryTests
{
    [Test]
    public void BuildFrom_ReturnsDistinctOrderedCounties()
    {
        // arrange
        var records = new DobihRecords("Any file name", new List<DobihRecord>
        {
            new() { County = "CountyB" },
            new() { County = "CountyA/CountyB" },
            new() { County = "CountyC" },
            new() { County = " CountyC " },
            new() { County = " CountyD / CountyB " },
            new() { County = "" },
            new() { County = " " },
            new() { County = null },
            new() { County = "CountyA" }
        });

        var sut = new CountiesFactory();
        
        // act
        var actual = sut.BuildFrom(records);
        
        // assert
        Assert.That(
            actual.Select(c =>c.Name),
            Is.EquivalentTo(["CountyA", "CountyB", "CountyC", "CountyD"]));
    }
}
