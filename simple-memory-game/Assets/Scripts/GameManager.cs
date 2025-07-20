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
    public static GameManager Instance { get; private set; }

    void Start()
    {
        Instance = this;
        gameUnits = new List<IList<GameUnit>>();
        patternGenerator = new PatternGenerator();
        finder = new QuadrantFinder();
        Restart();
    }

    void Update()
    {
        Debug.Log("State: " + gameUnits.Last().Select(u => u.ToString()).Aggregate((a, b) => a + ", " + b));
        text.text = $"{patternGenerator.CurrLength} | {gameUnits.Count}";

        // Handle game logic
        var state = GetState();
        switch (state)
        {
            case GameState.WaitingForNextAnimation:
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
                            quadrant.GetComponent<QuadrantScript>().Animate(unit);
                            break;
                        }
                    }
                }
                break;

            case GameState.AllDone:
                // Handle all done state
                GenerateNextPattern();
                break;

            case GameState.WaitingForInput:
                finder.ResetQuadrants(true); // Allow input on quadrants
                break;
        }
    }

    void Restart()
    {
        finder.ResetQuadrants(false);
        gameUnits = new List<IList<GameUnit>>();
        patternGenerator = new PatternGenerator();
        GenerateNextPattern();
    }

    private void GenerateNextPattern()
    {
        finder.ResetQuadrants(false); // Reset quadrants to not accept input
        IList<GameUnit> nextPattern = patternGenerator.GenerateNext()
            .Select(gameEvent => new GameUnit(gameEvent, QuadrantStatus.AnimationNotStarted))
            .ToList();

        gameUnits.Add(nextPattern);
    }

    public class GameUnit
    {
        public GameEvent gameEvent;
        public QuadrantStatus status;

        public GameUnit(GameEvent gameEvent, QuadrantStatus status)
        {
            this.gameEvent = gameEvent;
            this.status = status;
        }

        public override string ToString()
        {
            return gameEvent.ToString() + $" | Status: {status}";
        }
    }

    private enum GameState
    {
        WaitingForNextAnimation,
        Animating,
        WaitingForInput,
        AllDone,
    }

    private GameState GetState()
    {
        if (gameUnits.Last().Any(u => u.status == QuadrantStatus.CurrentlyAnimating))
        {
            Debug.Log("Current State: Animating");
            return GameState.Animating;
        }

        if (gameUnits.Last().Any(u => u.status == QuadrantStatus.AnimationNotStarted))
        {
            Debug.Log("Current State: WaitingForNextAnimation");
            return GameState.WaitingForNextAnimation;
        }

        if (gameUnits.Last().All(u => u.status == QuadrantStatus.Completed))
        {
            Debug.Log("Current State: AllDone");
            return GameState.AllDone;
        }

        Debug.Log("Current State: WaitingForInput");
        return GameState.WaitingForInput;
    }

    public void HandleNextInput(int quadrant, Colors color)
    {
        var currentUnit = gameUnits.Last().FirstOrDefault(u => u.status == QuadrantStatus.InProgress);
        if (currentUnit.gameEvent.quadrant == quadrant && currentUnit.gameEvent.color == color)
        {
            Debug.Log($"Correct input for quadrant {quadrant} with color {color}");
            currentUnit.status = QuadrantStatus.Completed; // Mark as completed
        }
        else
        {
            Debug.Log($"Incorrect input for quadrant {quadrant} with color {color}");
            // Handle incorrect input (e.g., reset game, show error, etc.)
            Restart();
        }
    }
}
