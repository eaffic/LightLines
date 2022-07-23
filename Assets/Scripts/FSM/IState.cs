using System;
using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    public interface IState
    {
        /// <summary> 状態の名前 </summary>
        string Name { get; }

        /// <summary> 状態のタグ </summary>
        string Tag { set; get; }

        /// <summary> 現状態の所属マシン </summary>
        IStateMachine Parent { set; get; }

        /// <summary> 経過時間 </summary>
        float Timer { get; }

        /// <summary> 遷移可能のリスト </summary>
        List<ITransition> Transitions { get; }


        /// <summary> 遷移情報を登録する </summary>
        void AddTransition(ITransition transition);

        /// <summary> 状態に入った時 </summary>
        void OnEnter(IState previewState);

        /// <summary> 次の状態に移行する時 </summary>
        void OnExit(IState nextState);

        /// <summary> 状態を継続している時 </summary>
        void OnUpdate(float deltaTime);
        void OnLateUpdate(float deltaTime);
        void OnFixedUpdate();
    }
}