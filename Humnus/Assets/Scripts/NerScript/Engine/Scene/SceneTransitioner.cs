using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NerScript.Attribute;
using NerScript.Resource;
using UniRx;
using UniRx.Async;
using System;
using System.Threading.Tasks;

namespace NerScript.Games
{
    public class SceneTransitioner : MonoBehaviour
    {
        [SerializeField, SceneName] private string nextScene = "";



        private static readonly Subject<Unit> onSceneChangeStatic = new Subject<Unit>();
        public static IOptimizedObservable<Unit> OnSceneChangeStartStatic => onSceneChangeStatic;

        private readonly Subject<Unit> onSceneChange = new Subject<Unit>();
        public IOptimizedObservable<Unit> OnSceneChangeStart => onSceneChange;

        /// <summary>
        /// 次のシーンへ
        /// </summary>
        [Obsolete]
        public void Next()
        {
            ChangeSceneStart();
            SceneManager.LoadScene(nextScene);
        }

        [Obsolete]
        public void NextWithFade(int frame)
        {
            ChangeSceneStart();
            ScreenManager.FadeSystem.Instance.FadeStart(frame, frame, () => { SceneManager.LoadScene(nextScene); });
        }


        public void NextAsync()
        {
            NextAsync(null);
        }

        public void NextAsync(Action onLoaded)
        {
            ChangeSceneStart();
            NextSceneAsync(onLoaded);
            GC.Collect();
        }

        private async void NextSceneAsync(Action onLoaded)
        {
            Subject<float> progress = new Subject<float>();
            ScreenManager.LoadSceneScreenSystem.Instance.Show(progress);

            await
                SceneManager
                   .LoadSceneAsync(nextScene)
                   .ConfigureAwait(Progress.Create<float>(progress.OnNext));

            progress.OnCompleted();
            onLoaded?.Invoke();
        }


        private void ChangeSceneStart()
        {
            SceneManagerAgent.PreviousSceneName = SceneManager.GetActiveScene().name;
            onSceneChange.OnNext(Unit.Default);
            onSceneChangeStatic.OnNext(Unit.Default);
        }


        private void OnDestroy()
        {
            onSceneChange.OnCompleted();
        }
    }
}
