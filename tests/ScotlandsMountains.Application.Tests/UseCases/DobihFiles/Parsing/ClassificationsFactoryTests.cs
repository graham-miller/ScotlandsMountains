using ScotlandsMountains.Application.UseCases.DobihFiles.Parsing;

namespace ScotlandsMountains.Application.Tests.UseCases.DobihFiles.Parsing;

[TestFixture]
public class ClassificationsFactoryTests
{
    [Test]
    public void Build_ReturnsClassifications()
    {
        // arrange
        var sut = new ClassificationsFactory();
        
        // act
        var actual = sut.Build();

        // assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.Not.Empty);
            Assert.That(actual.Count, Is.EqualTo(65));

            var first = actual.First();
            Assert.That(first.Name, Is.EqualTo("Munros"));
            Assert.That(first.NameSingular, Is.EqualTo("Munro"));
            Assert.That(first.DisplayOrder, Is.EqualTo(1));
            Assert.That(first.Description, Does.StartWith("Scottish hills at least 3,000ft in height"));
            Assert.That(first.DobihCode, Is.EqualTo("M"));
        }
    }
}
