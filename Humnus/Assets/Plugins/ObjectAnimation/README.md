# ObjectAnimation
UniRxをつかったオブジェクトの主にTransformの制御




















v2.3.0 曲線移動
		-BezierMoveAnimの追加

Version:2.3 =====Bezier=====

v2.2.2 FloatToFloatAnimがint型で値が来てしまうのを修正

v2.2.1 内部的処理の微調整

v2.2.0 バグ修正
		-PlayActionAnim終了後、次のアニメに遷移しないバグの修正。

Version:2.2 =====バグ修正=====

v2.1.2 一部アニメのExitの挙動の修正。

v2.1.1 RepeatのExitの改良
		-現在のアニメから実行される。

v2.1.0 停止機構の改修
		-同時多数アニメの再帰的な停止、再開、破壊、終了。
		-停止時の処理の変更。
		-直にDestroy時、OnAnimEndの呼び出しを無効。

Version:2.1 =====停止機構の調整=====

v2.0.2 Inspector上で操作をできないように。

v2.0.1 現在のAnimeの名前の確認
		-Inspectorで確認できるように。

v2.0.0 AnimationPlatDetail.アニメの進捗確認、デバッグ
		-ObjectAnimationはInspector上で進捗を確認できる。
		-残りアニメ数の確認
		-アニメオプションの確認
		-X.Xアップデートごとにコンセプトを設定

Version:2.0 =====デバッグアップデート=====

v1.11.2 Simultaneousで開始したアニメをControllerでも操作可能に。

v1.11.1 NerScript導入
		 -NerScriptに依存。内部的な処理を各種調整。

v1.11.0 新停止機構"Destroy"追加
		 -Dispose後にOnAnimEndを実行せずに終了。更にObjectAnimationを破壊。

v1.10.3 FixedLocalMove,FixedScaleの追加

v1.10.2 EndlessをRepeatの設定とし、EndlessAnimを廃止。

v1.10.1 前回の変更点をIObjectAnimModule全体に適応

v1.10.0 PlayActionAnimが連続した場合、1フレームずつ遅れて実行される挙動を同じフレーム内で実行されるように変更。

v1.9.3 一部UI修正

v1.9.2.1 全体的な機能修正2

v1.9.2 全体的な機能修正

v1.9.1 original側から全ての参照にapplyできるようにコマンドを追加
		-ApplyAnimation

v1.9.0 Scene上の他のアニメーションと同じ動きを定義する、ReferenceAnimationの追加
		-ReferenceAnimation

v1.8.1 AnimationReorderWindowのUI調整

v1.8.0 ReorderableListを使用した並び替え機能を追加
		-AnimationReorderWindow

v1.7.1 新しいUnityEditorバージョンに対応したUIの調整

v1.7.0.1 全体的なUIの調整2

v1.7.0 全体的なUIの調整

v1.6.1 ビルダーの終了処理修正

v1.6.0 ビルダーの終了処理作成
		-onEnd

v1.5.0 ビルダーの複数アニメ対応
		-AnimeName

v1.4.0 Animeのenable設定
		-enable

v1.3.1 ビルダーのUIの調整
		-foldout

v1.3.0 ビルダーに各種設定の追加
		-settings,Copy,Paste,sort,

v1.2.0 ビルダーに同時アニメの追加２
		-AsSoonAs

v1.1.5 ビルダーに同時アニメの追加
		-Simultaneous

v1.1.4 ビルダーにWait追加
		-wait

v1.1.3 ビルダーにローカル系統の追加
		-localmove,localrotate

v1.1.2 ビルダーにEasingの設定
		-easing

v1.1.1 ビルダーに回転、スケールの追加
		-rotate,scale

v1.1.0 Builderの追加
		-ObjectAnimationBuilder

v1.0.0 本リリース。全体的な調整。

v0.9.1 Tripアニメを、メモリ機能で再生に変更

v0.9.0 メモリー機能
		-AnimMemory,MemoryUser,Memorize

v0.8.0 アニメ再生オプション
		-RejectOthers,RejectOthersForever,RemoveOthers,RunThisOnly

v0.7.0 一部のアニメのFix化
		-FixMove,FixRotate,FixMove...etc

v0.6.3 新規アクセサリ
		-SimpleMover,SimpleRotater

v0.6.2 Accessoryの追加
		-SimpleLookAter

v0.6.1 他のObjectのアニメ制御
		-OtherObject

v0.6.0 全体的な調整、停止機構
		-Stop,Continue,Dispose

v0.5.5 Moduleの概念,対象に回転
		-Modele,LookAter

v0.5.4 フェード
		-ImageFade

v0.5.3 絶対移動的でないアニメ
		-AddAxis,AddPosition

v0.5.2 数値のみのアニメ
		-Leap,IntToInt

v0.5.1 アニメにEasing導入
		-EasingTypes

v0.5.0 Easing単体の追加
		-Easing

v0.4.1 同時実行の修正

v0.4.0 処理の実行、同時実行
		-PlayActionAnim,Simultaneous

v0.3.1 ループ、エンドレスの処理の変更。それに合わせるPlannerの調整

v0.3.0 アニメのループ、エンドレス
		-Repeat,Endless

v0.2.2 アニメの追加
		-Wait,各種のLocal版

v0.2.1 アニメの追加
		-各種TripAnim

v0.2.0 アニメの追加
		-RotateAbs,RotateRel,ScaleAbs,SclaRel

v0.1.0 メソッドチェーンでアニメを作成
		-MoveAbs,MoveRel
