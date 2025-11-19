using ScotlandsMountains.Domain.Entities;

namespace ScotlandsMountains.Domain.Tests.Entities;

[TestFixture]
public class EntityTests
{
    private class EntityIntIdTypeA : Entity<int>
    {
        public EntityIntIdTypeA() { }
        public EntityIntIdTypeA(int id) => Id = id;
    }

    private class EntityIntIdTypeB : Entity<int>
    {
        public EntityIntIdTypeB() { }
        public EntityIntIdTypeB(int id) => Id = id;
    }

    private class EntityGuidId : Entity<Guid>
    {
        public EntityGuidId(Guid id) => Id = id;
    }

    [Test]
    public void Equals_Null_ReturnsFalse()
    {
        // arrange
        var entityA = new EntityIntIdTypeA(1);

        // act & assert
        using (var scope = Assert.EnterMultipleScope())
        {
            Assert.That(entityA.Equals(null), Is.False);
            Assert.That(entityA == null, Is.False);
            Assert.That(entityA == null, Is.False);
            Assert.That(null == entityA, Is.False);
            Assert.That(entityA != null, Is.True);
            Assert.That(null != entityA, Is.True);
        }
    }

    [Test]
    public void Equals_SameReference_ReturnsTrue()
    {
        // arrange
        var entity = new EntityIntIdTypeA();

        // act & assert
        Assert.That(entity.Equals(entity), Is.True);
    }

    [Test]
    public void Equals_DifferentTransientInstance_ReturnsFalse()
    {
        // arrange
        var entityA1 = new EntityIntIdTypeA();
        var entityA2 = new EntityIntIdTypeA();

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
        var entityA1 = new EntityIntIdTypeA(1);
        var entityA2 = new EntityIntIdTypeA(1);

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
        var entityA1 = new EntityIntIdTypeA(1);
        var entityA2 = new EntityIntIdTypeA(2);

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
        var entityA = new EntityIntIdTypeA(SameId);
        var entityB = new EntityIntIdTypeB(SameId);

        // act & assert
        using (var scope = Assert.EnterMultipleScope())
        {
            Assert.That(entityA.Equals(entityB), Is.False);
            Assert.That(entityA != entityB, Is.True);
            Assert.That(entityA.GetHashCode(), Is.Not.EqualTo(entityB.GetHashCode()));
        }
    }

    [Test]
    public void Equals_SameGuidIdAndType_ReturnsTrue()
    {
        // arrange
        var sameId = Guid.NewGuid();
        var entityC1 = new EntityGuidId(sameId);
        var entityC2 = new EntityGuidId(sameId);

        // act & assert
        using (var scope = Assert.EnterMultipleScope())
        {
            Assert.That(entityC1.Equals(entityC2), Is.True);
            Assert.That(entityC1 == entityC2, Is.True);
            Assert.That(entityC1.GetHashCode(), Is.EqualTo(entityC2.GetHashCode()));
        }
    }

    [Test]
    public void Equals_DifferentGuidId_ReturnsFalse()
    {
        // arrange
        var entityC1 = new EntityGuidId(Guid.NewGuid());
        var entityC2 = new EntityGuidId(Guid.NewGuid());

        // act & assert
        using (var scope = Assert.EnterMultipleScope())
        {
            Assert.That(entityC1.Equals(entityC2), Is.False);
            Assert.That(entityC1 != entityC2, Is.True);
            Assert.That(entityC1.GetHashCode(), Is.Not.EqualTo(entityC2.GetHashCode()));
        }
    }

    [Test]
    public void GetHashCode_DifferentTransientInstances_AreNotEqual()
    {
        // arrange
        var entityA1 = new EntityIntIdTypeA();
        var entityA2 = new EntityIntIdTypeA();

        // act & assert
        Assert.That(entityA1.GetHashCode(), Is.Not.EqualTo(entityA2.GetHashCode()));
    }
}