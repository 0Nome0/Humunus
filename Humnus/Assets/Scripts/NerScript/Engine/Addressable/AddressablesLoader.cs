using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NerScript.Resource
{
    public static class AddressablesLoader
    {
        public static IObservable<T> Load<T>(string address)
            => Observable.FromCoroutine<T>(observer => _Load(observer, address));

        private static IEnumerator _Load<T>(IObserver<T> observer, string address)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            handle.Completed += sp =>
            {
                observer.OnNext(sp.Result);
                observer.OnCompleted();
            };
            return handle;
        }




        public static IObservable<long> GetSize(string address)
            => Observable.FromCoroutine<long>(observer => _GetSize(observer, address));

        private static IEnumerator _GetSize(IObserver<long> observer, string address)
        {
            AsyncOperationHandle<long> handle = Addressables.GetDownloadSizeAsync(address);
            handle.Completed += sp =>
            {
                observer.OnNext(sp.Result);
                observer.OnCompleted();
            };
            return handle;
        }
    }
}
