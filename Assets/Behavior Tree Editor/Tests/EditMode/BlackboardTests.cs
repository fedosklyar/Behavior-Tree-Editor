using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BlackboardTests
{

    private Blackboard blackboard;

    [SetUp]
    public void SetUp()
    {
        blackboard = ScriptableObject.CreateInstance<Blackboard>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(blackboard);
    }

    [Test]
    public void PopulateFromEntries_Int_ParsedCorrectly()
    {
        blackboard.AddValues(new List<BlackboardEntry>
        {
            new BlackboardEntry { key = "score", valueType = BlackboardValueType.Int, primitiveValue = "42" }
        });
        blackboard.PopulateFromEntries();

        Assert.AreEqual(42, blackboard.TryGetValue<int>("score"));
    }

    [Test]
    public void PopulateFromEntries_Int_InvalidString_ReturnsFallback()
    {
        blackboard.AddValues(new List<BlackboardEntry>
        {
            new BlackboardEntry { key = "score", valueType = BlackboardValueType.Int, primitiveValue = "notanint" }
        });
        blackboard.PopulateFromEntries();

        Assert.AreEqual(0, blackboard.TryGetValue<int>("score"));
    }

    [Test]
    public void PopulateFromEntries_Float_ParsedCorrectly()
    {
        blackboard.AddValues(new List<BlackboardEntry>
        {
            new BlackboardEntry { key = "speed", valueType = BlackboardValueType.Float, primitiveValue = "3.14" }
        });
        blackboard.PopulateFromEntries();

        Assert.AreEqual(3.14f, blackboard.TryGetValue<float>("speed"), 0.001f);
    }

    [TestCase("true", true)]
    [TestCase("True", true)]
    [TestCase("TRUE", true)]
    [TestCase("1", true)]
    [TestCase("false", false)]
    [TestCase("False", false)]
    [TestCase("0", false)]
    [TestCase("garbage", false)]
    public void PopulateFromEntries_Bool_VariousInputs(string input, bool expected)
    {
        blackboard.AddValues(new List<BlackboardEntry>
        {
            new BlackboardEntry { key = "flag", valueType = BlackboardValueType.Bool, primitiveValue = input }
        });
        blackboard.PopulateFromEntries();

        Assert.AreEqual(expected, blackboard.TryGetValue<bool>("flag"));
    }

    [Test]
    public void PopulateFromEntries_String_StoredCorrectly()
    {
        blackboard.AddValues(new List<BlackboardEntry>
        {
            new BlackboardEntry { key = "tag", valueType = BlackboardValueType.String, primitiveValue = "Enemy" }
        });
        blackboard.PopulateFromEntries();

        Assert.AreEqual("Enemy", blackboard.TryGetValue<string>("tag"));
    }

    // --- Indexer and TryGetValue ---

    [Test]
    public void Indexer_SetAndGet_ReturnsCorrectValue()
    {
        blackboard.PopulateFromEntries();
        blackboard["health"] = 100;

        Assert.AreEqual(100, blackboard["health"]);
    }

    [Test]
    public void Indexer_MissingKey_ReturnsNull()
    {
        blackboard.PopulateFromEntries();

        Assert.IsNull(blackboard["nonexistent"]);
    }

    [Test]
    public void TryGetValue_MissingKey_ReturnsFallback()
    {
        blackboard.PopulateFromEntries();

        Assert.AreEqual(-1, blackboard.TryGetValue<int>("missing", -1));
    }

    [Test]
    public void ContainsKey_ExistingKey_ReturnsTrue()
    {
        blackboard.PopulateFromEntries();
        blackboard["key"] = "value";

        Assert.IsTrue(blackboard.ContainsKey("key"));
    }

    [Test]
    public void ContainsKey_MissingKey_ReturnsFalse()
    {
        blackboard.PopulateFromEntries();

        Assert.IsFalse(blackboard.ContainsKey("missing"));
    }

    // --- Clone ---

    [Test]
    public void Clone_ProducesIndependentCopy()
    {
        blackboard.PopulateFromEntries();
        blackboard["shared"] = 1;

        Blackboard clone = blackboard.Clone();
        clone["shared"] = 99;

        // original should be unaffected
        Assert.AreEqual(1, blackboard["shared"]);
        Object.DestroyImmediate(clone);
    }
}
