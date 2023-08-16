using System;
using System.Collections.Generic;

public class StateMachine
{
   public State CurrentState => _currentState;
   protected State _currentState;
   private Dictionary<State, List<Transition>> _transitions = new Dictionary<State,List<Transition>>();
   private List<Transition> _currentTransitions = new List<Transition>();
   private List<Transition> _anyTransitions = new List<Transition>();

   public event Action<State, State> StateChanged;
   
   private static List<Transition> EmptyTransitions = new List<Transition>(0);

   public void OnLogic()
   {
      var transition = GetTransition();
      if (transition != null)
         SetState(transition.To);
      
      _currentState?.OnLogic();
   }

   public void OnPhysics()
   {
      _currentState?.OnPhysics();
   }

   public void SetState(State state)
   {
      if (state == _currentState)
         return;
      
      var previousState = _currentState;
      _currentState?.OnExit();
      _currentState = state;
      
      _transitions.TryGetValue(_currentState, out _currentTransitions);
      if (_currentTransitions == null)
         _currentTransitions = EmptyTransitions;
      
      _currentState.OnEnter();
      StateChanged?.Invoke(previousState, _currentState);
   }

   public void AddTransition(State from, State to, Func<bool> predicate)
   {
      if (_transitions.TryGetValue(from, out var transitions) == false)
      {
         transitions = new List<Transition>();
         _transitions[from] = transitions;
      }
      
      transitions.Add(new Transition(to, predicate));
   }

   public void AddAnyTransition(State state, Func<bool> predicate)
   {
      _anyTransitions.Add(new Transition(state, predicate));
   }

   private class Transition
   {
      public Func<bool> Condition {get; }
      public State To { get; }

      public Transition(State to, Func<bool> condition)
      {
         To = to;
         Condition = condition;
      }
   }

   private Transition GetTransition()
   {
      foreach(var transition in _anyTransitions)
         if (transition.Condition())
            return transition;
      
      foreach (var transition in _currentTransitions)
         if (transition.Condition())
            return transition;

      return null;
   }
}