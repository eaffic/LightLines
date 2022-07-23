using System;
using System.Collections;
using System.Collections.Generic;


namespace FSM{
    public delegate bool GameTransitionDelegate();

    /// <summary>
    /// 状態遷移用ベースクラス
    /// </summary>
    public class BaseTransition : ITransition
    {
        public BaseTransition(string name, IState fromState, IState toState)
        {
            _name = name;
            _from = fromState;
            _to = toState;
        }

        #region Data Define
        /// <summary>
        /// 遷移更新
        /// </summary>
        public event GameTransitionDelegate OnTransitionCallBack;
        /// <summary>
        /// 遷移確認
        /// </summary>
        public event GameTransitionDelegate OnCheckCallBack; 

        private IState _from;
        private IState _to;
        private string _name;
        #endregion

        #region Properties
        public IState From{
            get { return _from; }
            set { _from = value; }
        }

        public IState To{
            get { return _to; }
            set { _to = value; }
        }

        public string Name{
            get { return _name; }
        }
        #endregion

        #region Core Function
        /// <summary>
        /// 遷移更新中状態
        /// </summary>
        /// <returns>true/false 遷移終了/遷移継続</returns>
        public bool TransitionUpdate(){
            if(OnTransitionCallBack != null){
                return OnTransitionCallBack();
            }
            return true;
        }

        /// <summary>
        /// 遷移確認
        /// </summary>
        /// <returns>true/false 遷移必要/遷移なし</returns>
        public bool CheckStartCondition(){
            if(OnCheckCallBack != null){
                return OnCheckCallBack();
            }
            return false;
        }
        #endregion
    }
}