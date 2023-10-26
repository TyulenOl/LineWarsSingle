using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

public class TestGrouperSelectors
{
    // [Test]
    // public void NewTestScriptSimplePasses()
    // {
    //     var defaultGrouper = new DefaultTargetActionGrouper();
    //     var actions = new[]
    //     {
    //         new FakeTargetAction()
    //         {
    //             TargetType = typeof(int),
    //             Name = "1"
    //         },
    //         new FakeTargetAction()
    //         {
    //             TargetType = typeof(int),
    //             Name = "2"
    //         },
    //         new FakeTargetAction()
    //         {
    //             TargetType = typeof(Object),
    //             Name = "3"
    //         },
    //         new FakeTargetAction()
    //         {
    //             TargetType = typeof(object),
    //             Name = "4"
    //         },
    //         new FakeTargetAction()
    //         {
    //             TargetType = typeof(MonoBehaviour),
    //             Name = "5"
    //         }
    //     };
    //
    //     var group = defaultGrouper.GroupByType(actions);
    //     var expected = new Dictionary<Type, string[]>()
    //     {
    //         {typeof(int), new[] {"1", "2"}},
    //         {typeof(Object), new[] {"3"}},
    //         {typeof(object), new[] {"4"}},
    //         {typeof(MonoBehaviour), new[] {"5"}},
    //     };
    //     var actual = group.ToDictionary(
    //         x => x.Key,
    //         x => x.Value
    //             .Select(y => ((FakeTargetAction) y).Name)
    //             .ToArray()
    //     );
    //
    //     CollectionAssert.AreEquivalent(expected, actual);
    // }
    //
    // class FakeTargetAction : ITargetedAction
    // {
    //     public string Name;
    //     public Type TargetType { get; set; }
    //
    //     public bool IsMyTarget(ITarget target)
    //     {
    //         throw new NotImplementedException();
    //     }
    //
    //     public ICommandWithCommandType GenerateCommand(ITarget target)
    //     {
    //         throw new NotImplementedException();
    //     }
    // }
}