using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class SimpleSceneTests
    {
        [OneTimeSetUp]
        public void LoadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SimpleDemoScene");
        }

        [UnityTest]
        public IEnumerator SimpleDemoScene_RunsFor100FramesWithoutErrors()
        {
            for (var i = 0; i < 100; i++)
            {
                yield return null;
            }
            
            LogAssert.NoUnexpectedReceived();
        }
    }
}
