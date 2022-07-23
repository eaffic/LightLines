using System;
using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    //各イベンド対応のデリゲート
    public delegate void GameDelegate();
    public delegate void GameDelegateState(IState state);
    public delegate void GameDelegateFloat(float f); //f フレームタイム

    /// <summary>
    /// 状態ベースクラス
    /// </summary>
    public abstract class BaseState : IState
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">状態の名前</param>
        public BaseState(string name)
        {
            _name = name;
            _transitions = new List<ITransition>();
        }

        #region Data Define
        //状態更新用イベンド
        public event GameDelegateState OnEnterCallBack;
        public event GameDelegateState OnExitCallBack;
        public event GameDelegateFloat OnUpdateCallBack;
        public event GameDelegateFloat OnLateUpdateCallBack;
        public event GameDelegate OnFixedUpdateCallBack;

        private string _name; //state名
        private string _tag; //stateのタグ
        private IStateMachine _parent; //この状態の所属管理マシン
        private float _timer; //フレーム計算
        private List<ITransition> _transitions; //遷移先のリスト
        #endregion

        #region Properties
        public string Name {
            get { return _name; }
        }

        public string Tag{
            get { return _tag; }
            set { _tag = value; }
        }

        public IStateMachine Parent{
            get { return _parent; }
            set { _parent = value; }
        }

        public float Timer{
            get { return _timer; }
        }

        public List<ITransition> Transitions{
            get { return _transitions; }
        }
        #endregion

        #region Core Function
        /// <summary>
        /// 遷移先追加
        /// </summary>
        /// <param name="t"></param>
        public void AddTransition(ITransition t){
            if(t != null && !_transitions.Contains(t)){
                _transitions.Add(t);
            }
        }

        /// <summary>
        /// 状態初期化
        /// </summary>
        /// <param name="previewState"></param>
        public virtual void OnEnter(IState previewState){
            _timer = 0; //タイムリセット
            if(OnEnterCallBack != null){
                OnEnterCallBack(previewState); //イベンドを呼び出す
            }
        }

        /// <summary>
        /// 状態終了
        /// </summary>
        /// <param name="nextState"></param>
        public virtual void OnExit(IState nextState){
            if(OnExitCallBack != null){
                OnExitCallBack(nextState); //イベンドを呼び出す
            }
        }

        /// <summary>
        /// 状態更新
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void OnUpdate(float deltaTime){
            _timer += deltaTime;
            if(OnUpdateCallBack != null){
                OnUpdateCallBack(deltaTime); //イベンドを呼び出す
            }
        }

        /// <summary>
        /// 状態更新
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void OnLateUpdate(float deltaTime){
            _timer += deltaTime;
            if(OnLateUpdateCallBack != null){
                OnLateUpdateCallBack(deltaTime); //イベンドを呼び出す
            }
        }

        /// <summary>
        /// 状態更新
        /// </summary>
        public virtual void OnFixedUpdate(){
            if(OnFixedUpdateCallBack != null){
                OnFixedUpdateCallBack(); //イベンドを呼び出す
            }
        }
        #endregion
    }
}