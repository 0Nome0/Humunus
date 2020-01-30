using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace NerScript.Input
{
    /// <summary>
    /// システム操作の入力を通知するインターフェース
    /// </summary>
    public interface IInputSystemProvider : IInput
    {
        /// <summary>
        /// 決定ボタン入力が押し離しされたときの通知
        /// </summary>
        IOptimizedObservable<bool> OnSubmit { get; }
        /// <summary>
        /// キャンセルボタン入力が押し離しされたときの通知
        /// </summary>
        IOptimizedObservable<bool> OnCancel { get; }
        /// <summary>
        /// ポーズボタン入力が押し離しされたときの通知
        /// </summary>
        IOptimizedObservable<bool> OnPause { get; }

    }

}