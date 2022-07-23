using System;
using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    /// <summary>
    /// 状態管理機
    /// </summary>
    public interface IStateMachine
    {
        /// <summary> デフォルート状態 </summary>
        IState DefaultState { set; get; }
        /// <summary> 前状態 </summary>
        IState BeforeState { get; }
        /// <summary> 現在状態 </summary>
        IState CurrentState { get; }

        /// <summary>
        /// 状態増加
        /// </summary>
        /// <param name="state"></param>
        void AddState(IState state);
        /// <summary>
        /// 状態削除
        /// </summary>
        /// <param name="state"></param>
        void RemoveState(IState state);
    }
}