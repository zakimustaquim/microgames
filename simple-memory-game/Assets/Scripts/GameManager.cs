using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // GameManager class implementation
    // This class manages the game state, handles events, and coordinates between different game components.
    private IList<IList<GameUnit>> gameUnits;
    private PatternGenerator patternGenerator;
    public Text text;
    public QuadrantFinder finder;

    void Start()
    {
        gameUnits = new List<IList<GameUnit>>();
        patternGenerator = new PatternGenerator(4);
        Restart();
    }

    void Update()
    {
        text.text = $"{patternGenerator.CurrLength} | {gameUnits.Count}";

        // Handle game logic
        var state = GetState();
        switch (state)
        {
            case GameState.Animating:
                // Handle animating state
                foreach (var unit in gameUnits.Last())
                {
                    if (unit.status == QuadrantStatus.AnimationNotStarted)
                    {
                        var quadrant = finder
                            .FindQuadrants()
                            .FirstOrDefault(q => q.GetComponent<QuadrantScript>().quadrantIndex == unit.gameEvent.quadrant);

                        if (quadrant != null)
                        {
                            quadrant.GetComponent<QuadrantScript>().Animate(unit.gameEvent.GetUnityColor());
                            unit.status = QuadrantStatus.CurrentlyAnimating;
                        }
                    }
                }
                break;

            case GameState.AllDone:
                // Handle all done state
                GenerateNextPattern();
                break;
        }
    }

    void Restart()
    {
        gameUnits = new List<IList<GameUnit>>();
        GenerateNextPattern();
    }

    private void GenerateNextPattern()
    {
        IList<GameUnit> nextPattern = patternGenerator.GenerateNext()
            .Select(gameEvent => new GameUnit(gameEvent, QuadrantStatus.AnimationNotStarted))
            .ToList();

        gameUnits.Add(nextPattern);
    }

    private class GameUnit
    {
        public GameEvent gameEvent;
        public QuadrantStatus status;

        public GameUnit(GameEvent gameEvent, QuadrantStatus status)
        {
            this.gameEvent = gameEvent;
            this.status = status;
        }
    }

    private enum GameState
    {
        Animating,
        WaitingForInput,
        AllDone,
    }

    private GameState GetState()
    {
        if (gameUnits.Any(unit => unit.Any(u => u.status == QuadrantStatus.AnimationNotStarted)))
        {
            Debug.Log("Current State: Animating");
            return GameState.Animating;
        }

        if (gameUnits.All(unit => unit.All(u => u.status == QuadrantStatus.Completed)))
        {
            Debug.Log("Current State: AllDone");
            return GameState.AllDone;
        }

        Debug.Log("Current State: WaitingForInput");
        return GameState.WaitingForInput;
    }
}
