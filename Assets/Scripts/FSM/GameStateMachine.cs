using System;
using System.Collections;
using System.Collections.Generic;

namespace FSM{
    /// <summary>
    /// 状態管理機
    /// </summary>
    public class GameStateMachine : BaseState, IStateMachine
    {
        public GameStateMachine(string name, IState defaultState) : base(name){
            _states = new List<IState>();
            _defaultState = defaultState;
        }

        #region Data Define
        private IState _defaultState;
        private IState _beforeState;
        private IState _currentState;
        private List<IState> _states; //状態リスト
        private bool _isTransition = false; //遷移中
        private ITransition _transition;    //実行中の遷移
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public IState DefaultState{
            get { return _defaultState; }
            set{
                AddState(value);
                _defaultState = value;
            }
        }

        public IState BeforeState{
            get { return _beforeState; }
        }

        /// <summary>
        /// 現在状態
        /// </summary>
        public IState CurrentState{
            get { return _currentState; }
        }
        #endregion

        #region Core Function
        /// <summary>
        /// 状態追加
        /// 最初追加の状態はデフォルート状態
        /// </summary>
        /// <param name="state"></param>
        public void AddState(IState state){
            if(state != null && !_states.Contains(state)){
                _states.Add(state);
                state.Parent = this;
                if(_defaultState == null){
                    _defaultState = state;
                }
            }
        }

        /// <summary>
        /// 状態削除
        /// </summary>
        /// <param name="state"></param>
        public void RemoveState(IState state){
            //実行中の状態は削除不可
            if(_currentState == state){
                return;
            }

            if(state != null && _states.Contains(state)){
                _states.Remove(state);
                state.Parent = null;
                //もし削除されたのはデフォルート状態の場合
                //リスト一番目の状態は新しいデフォルート状態になる
                if(_defaultState == state){
                    _defaultState = (_states.Count >= 1) ? _states[0] : null;
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void OnUpdate(float deltaTime)
        {
            //状態遷移中は更新停止
            if(_isTransition){
                if(_transition.TransitionUpdate()){ 
                    //遷移終了、現在状態を更新する
                    DoTransiton(_transition);
                    _isTransition = false;
                }
                return;
            }

            base.OnUpdate(deltaTime);

            if(_currentState == null){
                _currentState = _defaultState;
            }

            //次の状態への遷移確認
            List<ITransition> transitions = _currentState.Transitions;
            for (int i = 0; i < transitions.Count; ++i){
                ITransition t = transitions[i];
                if(t.CheckStartCondition()){
                    _isTransition = true;
                    _transition = t; //遷移
                    return;
                }
            }

            _currentState.OnUpdate(deltaTime);
        }

        public override void OnLateUpdate(float deltaTime)
        {
            base.OnLateUpdate(deltaTime);
            _currentState.OnLateUpdate(deltaTime);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            _currentState.OnFixedUpdate();
        }
        #endregion

        /// <summary>
        /// 状態更新
        /// </summary>
        private void DoTransiton(ITransition transition){
            _currentState.OnExit(transition.To); //現在状態を離脱する
            _beforeState = _currentState; //現在状態を記録する
            _currentState = transition.To; //新しい状態に変更
            _currentState.OnEnter(transition.From); //新しい状態の初期化
        }
    }
}