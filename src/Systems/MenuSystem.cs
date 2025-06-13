using System;

public class MenuSystem
{
    public string Line;
    public string[] Options;
    public int choice = 0;

    private InputSystem _inputSystem;
    private SleepSystem _sleepSystem;

    public MenuSystem(InputSystem inputSystem, SleepSystem sleepSystem)
    {
        _inputSystem = inputSystem;
        _sleepSystem = sleepSystem;

        InitText(
            "Sleep for the night?",
            [
                "Yes",
                "No"
            ]
        );
    }

    public void InitText(string line, string[] options)
    {
        Line = line;
        Options = options;
    }

    public void Update()
    {
        var inputs = _inputSystem.GetInputState();
        if (inputs.MoveDown)
            choice = choice < Options.Length - 1 ? ++choice : choice;
        else if (inputs.MoveUp)
            choice = choice > 0 ? --choice : choice;
        else if (inputs.Enter)
            ComputeChoice();
    }

    public void ComputeChoice()
    {
        if (choice == 0)
        {
            // Sleep
            _sleepSystem.StartSleepCycle();
            GameStateManager.SetState(GameState.Playing);
        }
        else
        {
            // Default exit back to gameplay option
            GameStateManager.SetState(GameState.Playing);
        }
    }



}