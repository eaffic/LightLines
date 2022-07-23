using System;
using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    public interface ITransition
    {
        /// <summary> 遷移前の状態 </summary>
        IState From { get; set; }

        /// <summary> 遷移先の状態 </summary>
        IState To { get; set; }

        /// <summary> この遷移過程の名前 </summary>
        string Name { get; }

        /// <summary>
        /// 遷移更新
        /// </summary>
        /// <returns>true/false 遷移終了/遷移中 </returns>
        bool TransitionUpdate();

        /// <summary>
        /// 遷移条件確認
        /// </summary>
        /// <returns>true/false 遷移使用/遷移不使用</returns>
        bool CheckStartCondition();
    }
}