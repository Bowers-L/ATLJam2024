using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class MarkovStateManager : MonoBehaviour
{
    [SerializeField] private float stateTimerDuration;
    private float[][] _stateProbabilities = new float[3][];
    private MarkovState _currState;

    private float _stateTimer;

    public UnityEvent<float> OnTimerChanged;
    public UnityEvent<MarkovState> OnStateChanged;

    private void Awake()
    {
        _stateProbabilities = new float[3][];
        for (int i =0; i < 3; i++)
        {
            _stateProbabilities[i] = new float[3];
        }
    }

    private void Start()
    {
        _currState = MarkovState.Yellow;
        _stateTimer = stateTimerDuration;
        InitProbabilities();
    }

    private void Update()
    {
        if (_stateTimer <= 0.0f)
        {
            //Change States
            _currState = GetNextState();
            OnStateChanged?.Invoke(_currState);
            _stateTimer = stateTimerDuration;
        } else
        {
            _stateTimer -= Time.deltaTime;
        }
        OnTimerChanged?.Invoke(_stateTimer);
    }

    public void InitProbabilities()
    {
        for (int curr_state = 0; curr_state < _stateProbabilities.Length; curr_state++)
        {
            int numNextStates = _stateProbabilities[curr_state].Length;
            for (int next_state = 0; next_state < numNextStates; next_state++)
            {
                _stateProbabilities[curr_state][next_state] = 1.0f / numNextStates;
            }
        }
    }

    public MarkovState GetNextState()
    {
        float[] next_probabilities = _stateProbabilities[(int) _currState];

        float rand = UnityEngine.Random.value;  //roll for random state

        MarkovState nextState = MarkovState.Yellow;
        float threshold = 0;
        for (int i = 0; i < next_probabilities.Length; i++)
        {
            threshold += next_probabilities[i];
            if (rand < threshold)
            {
                nextState = (MarkovState) i;
                break;
            }
        }

        Debug.Log($"Next State Is: {nextState}");
        return nextState;
    }
}