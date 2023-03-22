using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachineManager<T> where T : BaseState
{
    private Stack<T> Stack { get; }

    public T CurrentState => Stack.Count > 0 ? Stack.Peek() : null;

    public BaseStateMachineManager()
    {
        Stack = new Stack<T>();
    }

    public void PushState(T newState)
    {
        CurrentState?.Exit();
        Stack.Push(newState);
        newState.Init();
    }

    public T PopState()
    {
        var state = Stack.Pop();
        state.Exit();
        CurrentState?.Resume();
        return state;
    }

    public void PopTo<U>() where U : T
    {
        PopState();
        for (int i = Stack.Count; i >= 0; i--)
        {
            var state = Stack.Pop();

            if (state is U)
            {
                PushState(state);
                return;
            }
        }
        throw new InvalidOperationException("O estado requisitado não está presente na pilha");
    }

    public bool Contains<U>() where U : T
    {
        foreach (var item in Stack)
        {
            if (item is U)
                return true;
        }
        return false;
    }

    public void ClearStack(T newState)
    {
        while (Stack.Count > 1) PopState();

        SwapState(newState);
    }

    public void SwapState(T newState)
    {
        PopState();
        PushState(newState);
    }
}

public class BaseState
{
    public virtual void Init() { }
    public virtual void Exit() { }
    public virtual void Resume() { }
}
