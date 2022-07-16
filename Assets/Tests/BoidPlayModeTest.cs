using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BoidPlayModeTest
{
    const int actorCount = 100;

    // A Test behaves as an ordinary method
    [Test]
    public void BoidJobTestSimplePasses()
    {


        // 하나의 테스트에 대해서 테스트 답을 적어놓고 틀리면 에러처리, expect
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator BoidPlayModeTestWithEnumeratorPasses()
    {

        BoidJob boidJob = new BoidJob();
        boidJob.InitActors(actorCount);


        Debug.Break();

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
