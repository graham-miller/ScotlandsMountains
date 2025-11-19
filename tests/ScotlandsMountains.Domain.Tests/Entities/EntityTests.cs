using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Domain.Tests.Entities;

[TestFixture]
public class EntityTests
{
    private class EntityTypeA : Entity
    {
        public EntityTypeA() { }
        public EntityTypeA(int id) => Id = id;
    }

    private class EntityTypeB : Entity
    {
        public EntityTypeB() { }
        public EntityTypeB(int id) => Id = id;
    }

    [Test]
    public void Equals_Null_ReturnsFalse()
    {
        // arrange
        var entityA = new EntityTypeA(1);

        // act & assert
        using (var scope = Assert.EnterMultipleScope())
        {
            Assert.That(entityA.Equals(null), Is.False);
            Assert.That(entityA! == null, Is.False);
            Assert.That(entityA! == null, Is.False);
            Assert.That(null == entityA!, Is.False);
            Assert.That(entityA! != null, Is.True);
            Assert.That(null != entityA!, Is.True);
        }
    }

    [Test]
    public void Equals_SameReference_ReturnsTrue()
    {
        // arrange
        var entity = new EntityTypeA();

        // act & assert
        Assert.That(entity.Equals(entity), Is.True);
    }

    [Test]
    public void Equals_DifferentTransientInstance_ReturnsFalse()
    {
        // arrange
        var entityA1 = new EntityTypeA();
        var entityA2 = new EntityTypeA();

        // act & assert
        using (var scope = Assert.EnterMultipleScope())
        {
            Assert.That(entityA1.Equals(entityA2), Is.False);
            Assert.That(entityA1 == entityA2, Is.False);
            Assert.That(entityA1.Equals((object)entityA2), Is.False);
        }
    }

    [Test]
    public void Equals_SameIntIdAndType_ReturnsTrue()
    {
        // arrange
        var entityA1 = new EntityTypeA(1);
        var entityA2 = new EntityTypeA(1);

        // act & assert
        using (var scope = Assert.EnterMultipleScope())
        {
            Assert.That(entityA1.Equals(entityA2), Is.True);
            Assert.That(entityA1 == entityA2, Is.True);
            Assert.That(entityA1.GetHashCode(), Is.EqualTo(entityA2.GetHashCode()));
        }
    }

    [Test]
    public void Equals_DifferentIntId_ReturnsFalse()
    {
        // arrange
        var entityA1 = new EntityTypeA(1);
        var entityA2 = new EntityTypeA(2);

        // act & assert
        using (var scope = Assert.EnterMultipleScope())
        {
            Assert.That(entityA1.Equals(entityA2), Is.False);
            Assert.That(entityA1 != entityA2, Is.True);
            Assert.That(entityA1.GetHashCode(), Is.Not.EqualTo(entityA2.GetHashCode()));
        }
    }

    [Test]
    public void Equals_DifferentType_ReturnsFalse()
    {
        // arrange
        const int SameId = 42;
        var entityA = new EntityTypeA(SameId);
        var entityB = new EntityTypeB(SameId);

        // act & assert
        using (var scope = Assert.EnterMultipleScope())
        {
            Assert.That(entityA.Equals(entityB), Is.False);
            Assert.That(entityA != entityB, Is.True);
            Assert.That(entityA.GetHashCode(), Is.Not.EqualTo(entityB.GetHashCode()));
        }
    }

    [Test]
    public void GetHashCode_DifferentTransientInstances_AreNotEqual()
    {
        // arrange
        var entityA1 = new EntityTypeA();
        var entityA2 = new EntityTypeA();

        // act & assert
        Assert.That(entityA1.GetHashCode(), Is.Not.EqualTo(entityA2.GetHashCode()));
    }
}