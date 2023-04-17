using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class ComplexSceneTests
    {
        [OneTimeSetUp]
        public void LoadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("ComplexDemoScene");
        }

        [UnityTest]
        public IEnumerator ComplexDemoScene_RunsFor100FramesWithoutErrors()
        {
            for (var i = 0; i < 100; i++)
            {
                yield return null;
            }
            
            LogAssert.NoUnexpectedReceived();
        }
    }
}
